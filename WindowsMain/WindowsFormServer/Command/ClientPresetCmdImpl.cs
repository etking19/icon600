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

                server.GetConnectionMgr().SendData(
                    (int)CommandConst.MainCommandServer.UserPriviledge,
                    (int)CommandConst.SubCommandServer.PresetList,
                    serverPresetStatus,
                    new List<string>() { userId });
            }
        }

        private void AddPreset(string socketId, int dbUserId, ClientPresetsCmd presetData)
        {
            // Get the current latest position of all running apps
            IList<WindowsHelper.ApplicationInfo> appInfoList = Utils.Windows.WindowsHelper.GetRunningApplicationInfo();

            // get the user data associate with this user
            var userData = Server.ConnectedClientHelper.GetInstance().GetAllUsers().First(t => t.SocketUserId.CompareTo(socketId) == 0);
            if (userData == null)
            {
                return;
            }

            // get the application triggered by uer
            Dictionary<int, WindowsRect> appDic = new Dictionary<int, WindowsRect>();
            Dictionary<int, int> currentApps = new Dictionary<int,int>(userData.LaunchedAppList);
            for(int i = 0; i < currentApps.Count(); i++)
            {
                int wndIdentifier = currentApps.ElementAt(i).Key;
                int dbIndex = currentApps.ElementAt(i).Value;

                try
                {
                    var latestInfo = appInfoList.First(t => t.id == wndIdentifier);

                    appDic.Add(dbIndex, new WindowsRect()
                    {
                        Left = latestInfo.posX,
                        Top = latestInfo.posY,
                        Right = latestInfo.posX + latestInfo.width,
                        Bottom = latestInfo.posY + latestInfo.height,
                    });
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
            }

            // get the vnc triggered by user
            Dictionary<int, WindowsRect> vncDic = new Dictionary<int, WindowsRect>();
            Dictionary<int, int> currentVncs = new Dictionary<int,int>(userData.LaunchedVncList);
            for (int i = 0; i < currentVncs.Count(); i++)
            {
                int wndIdentifier = currentVncs.ElementAt(i).Key;
                int dbIndex = currentVncs.ElementAt(i).Value;

                try
                {
                    var latestInfo = appInfoList.First(t => t.id == wndIdentifier);

                    vncDic.Add(dbIndex, new WindowsRect()
                    {
                        Left = latestInfo.posX,
                        Top = latestInfo.posY,
                        Right = latestInfo.posX + latestInfo.width,
                        Bottom = latestInfo.posY + latestInfo.height,
                    });
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
                
            }

            // TODO: get latest position of sources
            // get source input triggerred by user
            Dictionary<int, WindowsRect> inputDic = new Dictionary<int, WindowsRect>();
            Dictionary<uint, int> currentSources = new Dictionary<uint,int>(userData.LaunchedSourceList);
            for (int i = 0; i < currentSources.Count(); i++ )
            {
                uint processId = currentSources.ElementAt(i).Key;
                int dbIndex = currentSources.ElementAt(i).Value;

                try
                {
                    //var latestInfo = appInfoList.First(t => t.processId == processId);

                    inputDic.Add(dbIndex, new WindowsRect()
                    {
                       // Left = latestInfo.posX,
                       // Top = latestInfo.posY,
                       // Right = latestInfo.posX + latestInfo.width,
                       // Bottom = latestInfo.posY + latestInfo.height,
                    });
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }

            }

            Server.ServerDbHelper.GetInstance().AddPreset(
                presetData.PresetEntry.Name,
                dbUserId,
                appDic,
                vncDic,
                inputDic);
        }

        private void RemovePreset(ClientPresetsCmd presetData)
        {
            // remove preset from database
            Server.ServerDbHelper.GetInstance().RemovePreset(presetData.PresetEntry.Identifier);
        }

        private void ModifyPreset(int userId, ClientPresetsCmd presetData)
        {
            /*
            List<int> applicationIds = new List<int>();
            foreach (ApplicationEntry entry in presetData.PresetEntry.ApplicationList)
            {
                applicationIds.Add(entry.Identifier);
            }

            List<int> vncIds = new List<int>();
            foreach (VncEntry vnc in presetData.PresetEntry.VncList)
            {
                vncIds.Add(vnc.Identifier);
            }

            List<int> inputIds = new List<int>();
            foreach (InputAttributes input in presetData.PresetEntry.InputList)
            {
                inputIds.Add(input.InputId);
            }


            Server.ServerDbHelper.GetInstance().EditPreset(presetData.PresetEntry.Identifier, presetData.PresetEntry.Name, userId, applicationIds, vncIds, inputIds);
             * */
        }

        private void LaunchPreset(string clientId, int dbUserId, ClientPresetsCmd presetData)
        {
            // 1. Close all existing running applications
            foreach(Utils.Windows.WindowsHelper.ApplicationInfo info in Utils.Windows.WindowsHelper.GetRunningApplicationInfo())
            {
                if (info.name.Contains("Vistrol"))
                {
                    // minimize Vistrol control application
                    Utils.Windows.NativeMethods.ShowWindow(new IntPtr(info.id), Constant.SW_FORCEMINIMIZE);
                }
                else
                {
                    // close other running application
                    Utils.Windows.NativeMethods.SendMessage(new IntPtr(info.id), Utils.Windows.Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
            }

            // 2. trigger the apps in the preset by giving preset's id
            // get the rect from the preset table
            PresetData preset = Server.ServerDbHelper.GetInstance().GetPresetByUserId(dbUserId).First(PresetData => PresetData.Id == presetData.PresetEntry.Identifier);
            foreach (ApplicationData appData in preset.AppDataList)
            {
                ProcessStartInfo info = new ProcessStartInfo()
                {
                    FileName = appData.applicationPath,
                    Arguments = appData.arguments
                };
                using(Process process = Process.Start(info))
                {
                    int tryMax = 1000;
                    while ((process.MainWindowHandle == IntPtr.Zero) || !NativeMethods.IsWindowVisible(process.MainWindowHandle))
                    {
                        System.Threading.Thread.Sleep(10);
                        process.Refresh();
                        if (tryMax-- <= 0)
                        {
                            break;
                        }
                    }
                    process.WaitForInputIdle(1000);
                    NativeMethods.MoveWindow(process.MainWindowHandle,
                            appData.rect.Left,
                            appData.rect.Top,
                            appData.rect.Right - appData.rect.Left,
                            appData.rect.Bottom - appData.rect.Top,
                            true);

                    // add to the connected client info
                    ConnectedClientHelper.GetInstance().AddLaunchedApp(clientId, process.MainWindowHandle.ToInt32(), appData.id);
                }
            }

            // start vnc
            foreach (RemoteVncData remoteData in preset.VncDataList)
            {
                int id = vncClient.StartClient(
                    remoteData.remoteIp, 
                    remoteData.remotePort, 
                    remoteData.rect.Left,
                    remoteData.rect.Top,
                    remoteData.rect.Right - remoteData.rect.Left,
                    remoteData.rect.Bottom - remoteData.rect.Top);

                // add to the connected client info
                ConnectedClientHelper.GetInstance().AddLaunchedVnc(clientId, id, remoteData.id);
            }

            // start source input
            foreach (VisionData inputData in preset.InputDataList)
            {
                uint result = (uint)ServerVisionHelper.getInstance().LaunchVisionWindow(
                    inputData.id, 
                    inputData.rect.Left,
                    inputData.rect.Top,
                    inputData.rect.Right - inputData.rect.Left,
                    inputData.rect.Bottom - inputData.rect.Top);

                // add to the connected client info
                ConnectedClientHelper.GetInstance().AddLaunchedInputSource(clientId, result, inputData.id);
            }
        }
    }
}
