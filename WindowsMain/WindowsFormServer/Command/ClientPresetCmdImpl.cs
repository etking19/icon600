using Session;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utils.Windows;
using WcfServiceLibrary1;
using WindowsFormClient.Server;

namespace WindowsFormClient.Command
{
    class ClientPresetCmdImpl : BaseImplementer
    {
        private int clientId;
        private IServer server;
        private VncMarshall.Client vncClient;

        /// <summary>
        /// constructor for external class used (telnet)
        /// </summary>
        public ClientPresetCmdImpl()
        {

        }

        public ClientPresetCmdImpl(IServer server, int userId, VncMarshall.Client vncClient)
        {
            this.clientId = userId;
            this.server = server;
            this.vncClient = vncClient;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientPresetsCmd presetData = deserialize.Deserialize<ClientPresetsCmd>(command);
            if (presetData == null)
            {
                return;
            }

            bool broadcastChanges = false;
            switch (presetData.ControlType)
            {
                case ClientPresetsCmd.EControlType.Add:
                    AddPreset(userId, clientId, presetData);
                    broadcastChanges = true;
                    break;
                case ClientPresetsCmd.EControlType.Delete:
                    RemovePreset(presetData);
                    broadcastChanges = true;
                    break;
                case ClientPresetsCmd.EControlType.Launch:
                    LaunchPreset(userId, clientId, presetData);
                    break;
                case ClientPresetsCmd.EControlType.Modify:
                    ModifyPreset(clientId, presetData);
                    break;
                default:
                    break;
            }

            if (broadcastChanges)
            {
                // TODO: broadcast changes to user who login using the owner's account
                // get user's preset list
                ServerPresetsStatus serverPresetStatus = new ServerPresetsStatus();
                serverPresetStatus.UserPresetList = new List<PresetsEntry>();
                foreach (PresetData data in Server.ServerDbHelper.GetInstance().GetPresetByUserId(clientId))
                {
                    List<ApplicationEntry> presetAppEntries = new List<ApplicationEntry>();
                    foreach (ApplicationData appData in data.AppDataList)
                    {
                        presetAppEntries.Add(new ApplicationEntry()
                        {
                            Identifier = appData.id,
                            Name = appData.name
                        });
                    }

                    List<VncEntry> presetVncEntries = new List<VncEntry>();
                    foreach (RemoteVncData vncData in data.VncDataList)
                    {
                        presetVncEntries.Add(new VncEntry()
                        {
                            Identifier = vncData.id,
                            DisplayName = vncData.name,
                            IpAddress = vncData.remoteIp,
                            Port = vncData.remotePort,
                        });
                    }

                    List<InputAttributes> presetInputEntries = new List<InputAttributes>();
                    foreach (VisionData inputData in data.InputDataList)
                    {
                        presetInputEntries.Add(
                            ServerVisionHelper.getInstance().GetAllVisionInputsAttributes().First(InputAttributes 
                                => InputAttributes.InputId == inputData.id));
                    }

                    serverPresetStatus.UserPresetList.Add(new PresetsEntry()
                    {
                        Identifier = data.Id,
                        Name = data.Name,
                        ApplicationList = presetAppEntries,
                        VncList = presetVncEntries,
                        InputList = presetInputEntries,
                    });
                }

                // should get all connected client with same login
                List<string> connectedClientSocketList;
                if (ConnectedClientHelper.GetInstance().GetConnectedUsersGroupByDB().TryGetValue(clientId, out connectedClientSocketList))
                {
                    server.GetConnectionMgr().SendData(
                    (int)CommandConst.MainCommandServer.UserPriviledge,
                    (int)CommandConst.SubCommandServer.PresetList,
                    serverPresetStatus,
                    connectedClientSocketList);
                }
                else 
                {
                    // Should not happen, just in case
                    Trace.WriteLine("ERROR: cannot get connected user socket list by user db id: " + clientId);
                    server.GetConnectionMgr().SendData(
                        (int)CommandConst.MainCommandServer.UserPriviledge,
                        (int)CommandConst.SubCommandServer.PresetList,
                        serverPresetStatus,
                        new List<string>() { userId });
                } 
            }
        }


        public void AddPresetExternal(int dbUserId, string presetName)
        {
            List<KeyValuePair<int, WindowsRect>> appList = new List<KeyValuePair<int, WindowsRect>>();
            List<KeyValuePair<int, WindowsRect>> vncList = new List<KeyValuePair<int, WindowsRect>>();
            List<KeyValuePair<int, WindowsRect>> visionList = new List<KeyValuePair<int, WindowsRect>>();

            // Get the current latest position of all running apps
            IList<WindowsHelper.ApplicationInfo> appInfoList = Utils.Windows.WindowsHelper.GetRunningApplicationInfo();

            // key - window unique identifier
            // value - application DB id
            Dictionary<int, List<int>> launcedAppDic = Server.LaunchedWndHelper.GetInstance().GetLaunchedApps(dbUserId);

            for (int i = 0; i < launcedAppDic.Count(); i++)
            {
                int wndIdentifier = launcedAppDic.ElementAt(i).Key;
                List<int> appDBIndexes = launcedAppDic.ElementAt(i).Value;

                WindowsRect rect = new WindowsRect();
                try
                {
                    var latestInfo = appInfoList.Single(t => t.id == wndIdentifier);

                    if (latestInfo.posX != -32000)
                    {
                        rect.Left = latestInfo.posX;
                        rect.Top = latestInfo.posY;
                        rect.Right = latestInfo.posX + latestInfo.width;
                        rect.Bottom = latestInfo.posY + latestInfo.height;
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }

                foreach (int appDBIndex in appDBIndexes)
                {
                    appList.Add(new KeyValuePair<int, WindowsRect>(appDBIndex, rect));
                }

            }


            // key - window unique identifier
            // value - application DB id
            Dictionary<int, int> launcedVncDic = Server.LaunchedVncHelper.GetInstance().GetLaunchedApps(dbUserId);

            for (int i = 0; i < launcedVncDic.Count(); i++)
            {
                int wndIdentifier = launcedVncDic.ElementAt(i).Key;
                int vncDBIndex = launcedVncDic.ElementAt(i).Value;

                WindowsRect rect = new WindowsRect();
                try
                {
                    var latestInfo = appInfoList.Single(t => t.id == wndIdentifier);

                    if (latestInfo.posX != -32000)
                    {
                        rect.Left = latestInfo.posX;
                        rect.Top = latestInfo.posY;
                        rect.Right = latestInfo.posX + latestInfo.width;
                        rect.Bottom = latestInfo.posY + latestInfo.height;
                    }

                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }

                vncList.Add(new KeyValuePair<int, WindowsRect>(vncDBIndex, rect));
            }


            // key - window unique identifier
            // value - application DB id
            Dictionary<int, int> launcedSourcesDic = Server.LaunchedSourcesHelper.GetInstance().GetLaunchedApps(dbUserId);

            for (int i = 0; i < launcedSourcesDic.Count(); i++)
            {
                int wndIdentifier = launcedSourcesDic.ElementAt(i).Key;
                int sourceDBIndex = launcedSourcesDic.ElementAt(i).Value;

                WindowsRect rect = new WindowsRect();
                try
                {
                    var latestInfo = appInfoList.Single(t => t.id == wndIdentifier);

                    if (latestInfo.posX != -32000)
                    {
                        rect.Left = latestInfo.posX;
                        rect.Top = latestInfo.posY;
                        rect.Right = latestInfo.posX + latestInfo.width;
                        rect.Bottom = latestInfo.posY + latestInfo.height;
                    }

                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }

                visionList.Add(new KeyValuePair<int, WindowsRect>(sourceDBIndex, rect));
            }

            Server.ServerDbHelper.GetInstance().AddPreset(
               presetName,
               dbUserId,
               appList,
               vncList,
               visionList);
        }

        private void AddPreset(string socketId, int dbUserId, ClientPresetsCmd presetData)
        {
            AddPresetExternal(dbUserId, presetData.PresetDataEntry.Name);
        }

        private void RemovePreset(ClientPresetsCmd presetData)
        {
            // remove preset from database
            Server.ServerDbHelper.GetInstance().RemovePreset(presetData.PresetDataEntry.Identifier);
        }

        private void ModifyPreset(int userId, ClientPresetsCmd presetData)
        {
            // no implementation yet
        }

        public void LaunchPresetExternal(int dbUserId, int presetDbId)
        {
            // get the rect from the preset table
            PresetData preset = Server.ServerDbHelper.GetInstance().GetPresetByUserId(dbUserId).First(PresetData => PresetData.Id == presetDbId);
            ClientAppCmdImpl clientImpl = new ClientAppCmdImpl();
            foreach (ApplicationData appData in preset.AppDataList)
            {
                int result = clientImpl.LaunchApplication(appData);
                LaunchedWndHelper.GetInstance().AddLaunchedApp(dbUserId, result, appData.id);
            }

            // start vnc
            foreach (RemoteVncData remoteData in preset.VncDataList)
            {
                int result = vncClient.StartClient(
                    remoteData.remoteIp,
                    remoteData.remotePort,
                    remoteData.rect.Left,
                    remoteData.rect.Top,
                    remoteData.rect.Right - remoteData.rect.Left,
                    remoteData.rect.Bottom - remoteData.rect.Top);

                // add to the connected client info
                LaunchedVncHelper.GetInstance().AddLaunchedApp(dbUserId, result, remoteData.id);
            }

            // start source input
            foreach (VisionData inputData in preset.InputDataList)
            {
                int result = ServerVisionHelper.getInstance().LaunchVisionWindow(
                    inputData.id,
                    inputData.rect.Left,
                    inputData.rect.Top,
                    inputData.rect.Right - inputData.rect.Left,
                    inputData.rect.Bottom - inputData.rect.Top);

                // add to the connected client info
                LaunchedSourcesHelper.GetInstance().AddLaunchedApp(dbUserId, result, inputData.id);
            }
        }

        public void ClearWall(int dbUserId)
        {
            // close all lauched applications
            var launchedApp = LaunchedWndHelper.GetInstance().GetLaunchedApps(dbUserId);
            foreach (int wndIdentifier in launchedApp.Keys)
            {
                if (Properties.Settings.Default.Debug)
                {
                    MessageBox.Show("before clear window identifier: " + wndIdentifier);
                }
                Utils.Windows.NativeMethods.PostMessage(new IntPtr(wndIdentifier), Utils.Windows.Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }

            // reset the launched list
            LaunchedWndHelper.GetInstance().ClearAll(dbUserId);

            var launchedVnc = LaunchedVncHelper.GetInstance().GetLaunchedApps(dbUserId);
            foreach (int wndIdentifier in launchedVnc.Keys)
            {
                if (Properties.Settings.Default.Debug)
                {
                    MessageBox.Show("before clear vnc identifier: " + wndIdentifier);
                }
                Utils.Windows.NativeMethods.PostMessage(new IntPtr(wndIdentifier), Utils.Windows.Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }

            // reset the launched list
            LaunchedVncHelper.GetInstance().ClearAll(dbUserId);

            var launchedSources = LaunchedSourcesHelper.GetInstance().GetLaunchedApps(dbUserId);
            foreach (int wndIdentifier in launchedSources.Keys)
            {
                if (Properties.Settings.Default.Debug)
                {
                    MessageBox.Show("before clear source identifier: " + wndIdentifier);
                }
                Utils.Windows.NativeMethods.PostMessage(new IntPtr(wndIdentifier), Utils.Windows.Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }

            // reset the launched list
            LaunchedSourcesHelper.GetInstance().ClearAll(dbUserId);
        }

        private void LaunchPreset(string clientId, int dbUserId, ClientPresetsCmd presetData)
        {
            if (Properties.Settings.Default.Debug)
            {
                MessageBox.Show("before clear wall");
            }

            // 1. Clean all launched applications
            ClearWall(dbUserId);

            if (Properties.Settings.Default.Debug)
            {
                MessageBox.Show("after clear wall");
            }

            // 2. trigger the apps in the preset by giving preset's id
            LaunchPresetExternal(dbUserId, presetData.PresetDataEntry.Identifier);
        }
    }
}
