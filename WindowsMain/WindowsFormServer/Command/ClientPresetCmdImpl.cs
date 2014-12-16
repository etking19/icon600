using Session;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

                // TODO: should get all connected client with same login
                server.GetConnectionMgr().SendData(
                    (int)CommandConst.MainCommandServer.UserPriviledge,
                    (int)CommandConst.SubCommandServer.PresetList,
                    serverPresetStatus,
                    new List<string>() { userId });
            }
        }

        private void AddPreset(string socketId, int dbUserId, ClientPresetsCmd presetData)
        {
            List<KeyValuePair<int, WindowsRect>> appList = new List<KeyValuePair<int, WindowsRect>>();
            List<KeyValuePair<int, WindowsRect>> vncList = new List<KeyValuePair<int, WindowsRect>>();
            List<KeyValuePair<int, WindowsRect>> visionList = new List<KeyValuePair<int, WindowsRect>>();
            
            /*
            List<ApplicationEntry> appEntryList = presetData.PresetDataEntry.PresetAppList;
            List<WndPos> appWndPosList = presetData.PresetDataEntry.PresetAppPos;
            for (int count = 0; count < appEntryList.Count; count++ )
            {
                appList.Add(new KeyValuePair<int, WindowsRect>(appEntryList.ElementAt(count).Identifier, new WindowsRect()
                {
                    Left = appWndPosList.ElementAt(count).posX,
                    Top = appWndPosList.ElementAt(count).posY,
                    Right = appWndPosList.ElementAt(count).posX + appWndPosList.ElementAt(count).width,
                    Bottom = appWndPosList.ElementAt(count).posY + appWndPosList.ElementAt(count).height,
                }));

                Trace.WriteLine(String.Format("Add preset application: {0}, pos: {1},{2},{3},{4}",
                    appEntryList.ElementAt(count).Name,
                    appWndPosList.ElementAt(count).posX,
                    appWndPosList.ElementAt(count).posY,
                    appWndPosList.ElementAt(count).width,
                    appWndPosList.ElementAt(count).height));
            }

            
            List<VncEntry> vncEntryList = presetData.PresetDataEntry.PresetVncList;
            List<WndPos> vncWndPosList = presetData.PresetDataEntry.PresetVncPos;
            for (int count = 0; count < vncEntryList.Count; count++)
            {
                vncList.Add(new KeyValuePair<int, WindowsRect>(vncEntryList.ElementAt(count).Identifier, new WindowsRect()
                {
                    Left = vncWndPosList.ElementAt(count).posX,
                    Top = vncWndPosList.ElementAt(count).posY,
                    Right = vncWndPosList.ElementAt(count).posX + vncWndPosList.ElementAt(count).width,
                    Bottom = vncWndPosList.ElementAt(count).posY + vncWndPosList.ElementAt(count).height,
                }));
            }

            
            List<InputAttributes> visionEntryList = presetData.PresetDataEntry.PresetVisionInputList;
            List<WndPos> visionWndPosList = presetData.PresetDataEntry.PresetVisionInputPos;
            for (int count = 0; count < visionEntryList.Count; count++)
            {
                visionList.Add(new KeyValuePair<int, WindowsRect>(visionEntryList.ElementAt(count).InputId, new WindowsRect()
                {
                    Left = visionWndPosList.ElementAt(count).posX,
                    Top = visionWndPosList.ElementAt(count).posY,
                    Right = visionWndPosList.ElementAt(count).posX + visionWndPosList.ElementAt(count).width,
                    Bottom = visionWndPosList.ElementAt(count).posY + visionWndPosList.ElementAt(count).height,
                }));
            }
            */
            
            // cater for previously launched preset

            // Get the current latest position of all running apps
            IList<WindowsHelper.ApplicationInfo> appInfoList = Utils.Windows.WindowsHelper.GetRunningApplicationInfo();
            //foreach (WindowsHelper.ApplicationInfo info in appInfoList)
            //{
            //    Trace.WriteLine(info.name + ", id: " + info.id);
            //}
            
            // get the user data associate with this user
            var userData = Server.ConnectedClientHelper.GetInstance().GetAllUsers().First(t => t.SocketUserId.CompareTo(socketId) == 0);
            if (userData == null)
            {
                return;
            }

            var currentApps = new List<KeyValuePair<int, int>>(userData.LaunchedAppList);
            for (int i = 0; i < currentApps.Count(); i++)
            {
                int wndIdentifier = currentApps.ElementAt(i).Key;
                int dbIndex = currentApps.ElementAt(i).Value;

                //Trace.WriteLine("Win identifier: " + wndIdentifier);

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

                Trace.WriteLine("Save preset: " + dbIndex + String.Format(" rect: {0} {1} {2} {3}", rect.Left, rect.Top, rect.Right, rect.Bottom));
                appList.Add(new KeyValuePair<int, WindowsRect>(dbIndex, rect));
            }

            var currentVncs = new List<KeyValuePair<int, int>>(userData.LaunchedVncList);
            for (int i = 0; i < currentVncs.Count(); i++)
            {
                int wndIdentifier = currentVncs.ElementAt(i).Key;
                int dbIndex = currentVncs.ElementAt(i).Value;

                WindowsRect rect = new WindowsRect();
                try
                {
                    var latestInfo = appInfoList.First(t => t.id == wndIdentifier);

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

                vncList.Add(new KeyValuePair<int, WindowsRect>(dbIndex, rect));
            }

            var currentSources = new List<KeyValuePair<int, int>>(userData.LaunchedSourceList);
            for (int i = 0; i < currentSources.Count(); i++)
            {
                int wndIdentifier = currentSources.ElementAt(i).Key;
                int dbIndex = currentSources.ElementAt(i).Value;

                WindowsRect rect = new WindowsRect();
                try
                {
                    var latestInfo = appInfoList.First(t => t.id == wndIdentifier);

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

                visionList.Add(new KeyValuePair<int, WindowsRect>(dbIndex, rect));
            }

            Server.ServerDbHelper.GetInstance().AddPreset(
                presetData.PresetDataEntry.Name,
                dbUserId,
                appList,
                vncList,
                visionList);




            /* ---------------------------------
             * OLD IMPLEMENTATION WHERE SERVER KEEP REFERENCE TO APP TRIGGER
             * ---------------------------------
             * 
            // Get the current latest position of all running apps
            IList<WindowsHelper.ApplicationInfo> appInfoList = Utils.Windows.WindowsHelper.GetRunningApplicationInfo();

            // get the user data associate with this user
            var userData = Server.ConnectedClientHelper.GetInstance().GetAllUsers().First(t => t.SocketUserId.CompareTo(socketId) == 0);
            if (userData == null)
            {
                return;
            }

            // get the application triggered by uer
            List<KeyValuePair<int, WindowsRect>> appDic = new List<KeyValuePair<int, WindowsRect>>();
            Dictionary<int, int> currentApps = new Dictionary<int,int>(userData.LaunchedAppList);
            for(int i = 0; i < currentApps.Count(); i++)
            {
                int wndIdentifier = currentApps.ElementAt(i).Key;
                int dbIndex = currentApps.ElementAt(i).Value;

                try
                {
                    var latestInfo = appInfoList.First(t => t.id == wndIdentifier);

                    WindowsRect rect = new WindowsRect()
                    {
                        Left = latestInfo.posX,
                        Top = latestInfo.posY,
                        Right = latestInfo.posX + latestInfo.width,
                        Bottom = latestInfo.posY + latestInfo.height,
                    };
                    appDic.Add(new KeyValuePair<int, WindowsRect>(dbIndex, rect));

                    Trace.WriteLine(string.Format("add preset app: {0},{1},{2},{3}", rect.Left, rect.Top, rect.Right, rect.Bottom));
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
            }

            // get the vnc triggered by user
            List<KeyValuePair<int, WindowsRect>> vncDic = new List<KeyValuePair<int, WindowsRect>>();
            Dictionary<int, int> currentVncs = new Dictionary<int,int>(userData.LaunchedVncList);
            for (int i = 0; i < currentVncs.Count(); i++)
            {
                int wndIdentifier = currentVncs.ElementAt(i).Key;
                int dbIndex = currentVncs.ElementAt(i).Value;

                try
                {
                    var latestInfo = appInfoList.First(t => t.id == wndIdentifier);

                    vncDic.Add(new KeyValuePair<int, WindowsRect>(dbIndex, new WindowsRect()
                    {
                        Left = latestInfo.posX,
                        Top = latestInfo.posY,
                        Right = latestInfo.posX + latestInfo.width,
                        Bottom = latestInfo.posY + latestInfo.height,
                    }));
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
                
            }

            // TODO: get latest position of sources
            // get source input triggerred by user
            List<KeyValuePair<int, WindowsRect>> inputDic = new List<KeyValuePair<int, WindowsRect>>();
            Dictionary<uint, int> currentSources = new Dictionary<uint,int>(userData.LaunchedSourceList);
            for (int i = 0; i < currentSources.Count(); i++ )
            {
                uint processId = currentSources.ElementAt(i).Key;
                int dbIndex = currentSources.ElementAt(i).Value;

                try
                {
                    //var latestInfo = appInfoList.First(t => t.processId == processId);

                    inputDic.Add(new KeyValuePair<int, WindowsRect>(dbIndex, new WindowsRect()
                    {
                       // Left = latestInfo.posX,
                       // Top = latestInfo.posY,
                       // Right = latestInfo.posX + latestInfo.width,
                       // Bottom = latestInfo.posY + latestInfo.height,
                    }));
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }

            }

            Server.ServerDbHelper.GetInstance().AddPreset(
                presetData.PresetDataEntry.Name,
                dbUserId,
                appDic,
                vncDic,
                inputDic);
             */
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

        private void LaunchPreset(string clientId, int dbUserId, ClientPresetsCmd presetData)
        {
            // 1. Close all existing running applications
            foreach(Utils.Windows.WindowsHelper.ApplicationInfo info in Utils.Windows.WindowsHelper.GetRunningApplicationInfo())
            {
                if (info.name.Contains("Vistrol"))
                {
                    // minimize Vistrol control application
                    Utils.Windows.NativeMethods.ShowWindow(new IntPtr(info.id), Constant.SW_MINIMIZE);
                }
                else
                {
                    // close other running application
                    Utils.Windows.NativeMethods.SendMessage(new IntPtr(info.id), Utils.Windows.Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
            }

            // reset the launched list
            ConnectedClientHelper.GetInstance().ClearLaunchedData(clientId);

            // 2. trigger the apps in the preset by giving preset's id
            // get the rect from the preset table
            PresetData preset = Server.ServerDbHelper.GetInstance().GetPresetByUserId(dbUserId).First(PresetData => PresetData.Id == presetData.PresetDataEntry.Identifier);
            ClientAppCmdImpl clientImpl = new ClientAppCmdImpl();
            foreach (ApplicationData appData in preset.AppDataList)
            {
                int result = clientImpl.LaunchApplication(appData);
                Trace.WriteLine("Launched preset application id: " + result);
                ConnectedClientHelper.GetInstance().AddLaunchedApp(clientId, result, appData.id);
            }

            // start vnc
            foreach (RemoteVncData remoteData in preset.VncDataList)
            {
                Trace.WriteLine(String.Format("lauched vnc: {0},{1},{2},{3}", 
                    remoteData.rect.Left,
                    remoteData.rect.Top,
                    remoteData.rect.Right,
                    remoteData.rect.Bottom));

                int id = vncClient.StartClient(
                    remoteData.remoteIp, 
                    remoteData.remotePort, 
                    remoteData.rect.Left,
                    remoteData.rect.Top,
                    remoteData.rect.Right - remoteData.rect.Left,
                    remoteData.rect.Bottom - remoteData.rect.Top);

                // add to the connected client info
                Trace.WriteLine("Launched preset remote data id: " + id);
                ConnectedClientHelper.GetInstance().AddLaunchedVnc(clientId, id, remoteData.id);
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
                Trace.WriteLine("Launched preset input source id: " + result);
                ConnectedClientHelper.GetInstance().AddLaunchedInputSource(clientId, result, inputData.id);
            }
        }
    }
}
