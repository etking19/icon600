using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using Utils.Windows;
using WindowsFormClient.Client.Model;

namespace WindowsFormClient.Presenter
{
    class ClientPresenter : IDisposable
    {
        public enum ServerMaintenanceMode
        {
            Shutdown = 1,
            Standby,
            Restart,
        }

        private IClient client;
        private ConnectionManager connectionMgr;
        private VncMarshall.Server vncServer;
        private Client.ClientCmdMgr clientCmdMgr;

        public ClientPresenter(IClient client, ConnectionManager connectionMgr, string username, string password)
        {
            // initialize helper
            this.client = client;
            clientCmdMgr = new Client.ClientCmdMgr(client);

            this.connectionMgr = connectionMgr;
            connectionMgr.EvtDisconnected += connectionMgr_EvtDisconnected;
            connectionMgr.EvtServerDataReceived += connectionMgr_EvtServerDataReceived;

            // create new vnc instance
            vncServer = new VncMarshall.Server(Properties.Settings.Default.TightVncServerPath);
            vncServer.StartVncServer();

            // send credential to server
            SendCredential(username, password);
        }

        void connectionMgr_EvtServerDataReceived(int mainId, int subId, string commandData)
        {
            clientCmdMgr.ExeCommand("0", mainId, subId, commandData);
        }


        void connectionMgr_EvtDisconnected()
        {
            client.CloseApplication();
        }

        private void SendCredential(string username, string password)
        {
            ClientLoginCmd loginCmd = new ClientLoginCmd();
            loginCmd.Username = username;
            loginCmd.Password = password;

            // get the VNC status, if there is VNC server installed, get the ip address and port shared
            List<VncEntry> vncList = new List<VncEntry>();
            int port;
            if ((port = VncMarshall.VncRegistryHelper.GetListeningPort()) != -1)
            {
                // clean up current extra ports on vnc server
                VncMarshall.VncRegistryHelper.RemoveExtrasListeningPort();

                // add the current monitors as extra listening port on vnc server
                int monitorCount = 1;
                foreach (WindowsHelper.MonitorInfo info in Utils.Windows.WindowsHelper.GetMonitorList())
                {
                    int extraPort = ++port;
                    VncMarshall.VncRegistryHelper.AddExtraListeningPorts(
                        extraPort, 
                        info.WorkArea.Left,
                        info.WorkArea.Top,
                        info.WorkArea.Right - info.WorkArea.Left,
                        info.WorkArea.Bottom - info.WorkArea.Top);

                    vncList.Add(new VncEntry() 
                    { 
                        IpAddress = Utils.Socket.LocalIPAddress(),
                        Port = extraPort,
                        OwnerPCName = Dns.GetHostName(),
                        MonitorCount = monitorCount,
                    });

                    monitorCount++;
                }
            }

            // reload the vnc server
            vncServer.refreshVncServer();

            loginCmd.VncList = vncList;
            connectionMgr.BroadcastMessage((int)CommandConst.MainCommandClient.LoginInfo, (int)CommandConst.SubCommandClient.Credential, loginCmd);
        }

        public void ShowMessage(string text, Font font, Color color, Color backgroundColor, int duration, int left, int top, int width, int height)
        {
            ClientMessageBoxCmd msgBoxCmd = new ClientMessageBoxCmd()
            {
                Message = text,
                TextFont = new SerializableFont(font).SerializeFontAttribute,
                TextColor = System.Drawing.ColorTranslator.ToHtml(color),
                BackgroundColor = System.Drawing.ColorTranslator.ToHtml(backgroundColor),
                Duration = duration,
                Left = left,
                Top = top,
                Width = width,
                Height = height
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.MessageBox,
                msgBoxCmd);
        }

        public void ServerMaintenance(ServerMaintenanceMode mode)
        {
            ClientMaintenanceCmd maintenanceCmd = new ClientMaintenanceCmd();
            switch (mode)
            {
                case ServerMaintenanceMode.Shutdown:
                    maintenanceCmd.CommandType = ClientMaintenanceCmd.CommandId.EShutdown;
                    break;
                case ServerMaintenanceMode.Restart:
                    maintenanceCmd.CommandType = ClientMaintenanceCmd.CommandId.EReboot;
                    break;
                case ServerMaintenanceMode.Standby:
                    maintenanceCmd.CommandType = ClientMaintenanceCmd.CommandId.ELogOff;
                    break;
            }

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.Maintenance,
                maintenanceCmd);
        }

        public void ControlServerMouse(int x, int y, uint mouseData, uint flag)
        {
            ClientMouseCmd mouseCmd = new ClientMouseCmd();
            mouseCmd.data = new ClientMouseCmd.MouseData
            {
                dx = x,
                dy = y,
                mouseData = mouseData,
                dwFlags = flag,
                time = 0,
                dwExtraInfo = 0
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.Mouse,
                mouseCmd);
        }

        public void ControlServerKeyboard(UInt16 vkCode, UInt16 scanCode, uint time, UInt32 flags)
        {
            ClientKeyboardCmd keyboardCmd = new ClientKeyboardCmd();
            keyboardCmd.data = new ClientKeyboardCmd.KeyboardData
            {
                wVk = vkCode,
                wScan = scanCode,
                time = time,
                dwFlags = flags,
                dwExtraInfo = 0
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.Keyboard,
                keyboardCmd);
        }

        public void AddPreset(string name, List<ApplicationEntry> appList)
        {
            PresetsEntry presetEntry = new PresetsEntry()
            {
                Name = name,
                ApplicationList = appList,
            };

            ClientPresetsControl presetCmd = new ClientPresetsControl()
            {
                ControlType = ClientPresetsControl.EControlType.Add,
                PresetEntry = presetEntry,
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.Preset,
                presetCmd);
        }

        public void RemovePreset(Client.Model.PresetModel presetModel)
        {
            List<ApplicationEntry> appEntriesList = new List<ApplicationEntry>();
            foreach (ApplicationModel model in presetModel.ApplicationList)
            {
                appEntriesList.Add(new ApplicationEntry() {
                    Identifier = model.AppliationId,
                    Name = model.ApplicationName,
                });
            }

            ClientPresetsControl presetCmd = new ClientPresetsControl()
            {
                ControlType = ClientPresetsControl.EControlType.Delete,
                PresetEntry = new PresetsEntry { Identifier = presetModel.PresetId, Name = presetModel.PresetName, ApplicationList = appEntriesList }
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.Preset,
                presetCmd);
        }

        public void TriggerPreset(Client.Model.PresetModel presetModel)
        {
            ClientPresetsControl presetCmd = new ClientPresetsControl()
            {
                ControlType = ClientPresetsControl.EControlType.Launch,
                PresetEntry = new PresetsEntry()
                {
                    Identifier = presetModel.PresetId,
                    Name = presetModel.PresetName
                }
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.Preset,
                presetCmd);
        }

        public void SetApplicationForeground(int wndId)
        {
            ClientWndCmd wndCommand = new ClientWndCmd();
            wndCommand.CommandType = ClientWndCmd.CommandId.ESetForeground;
            wndCommand.Id = wndId;

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.WindowsAttributes,
                wndCommand);  
        }

        public void SetApplicationMinimize(int wndId)
        {
            ClientWndCmd wndCommand = new ClientWndCmd();
            wndCommand.CommandType = ClientWndCmd.CommandId.EMinimize;
            wndCommand.Id = wndId;

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.WindowsAttributes,
                wndCommand);
        }

        public void SetApplicationSize(int wndId, Size newSize)
        {
            ClientWndCmd wndCommand = new ClientWndCmd();
            wndCommand.CommandType = ClientWndCmd.CommandId.EResize;
            wndCommand.Id = wndId;
            wndCommand.Width = newSize.Width;
            wndCommand.Height = newSize.Height;

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.WindowsAttributes,
                wndCommand);
        }

        public void SetApplicationRestore(int wndId)
        {
            ClientWndCmd wndCommand = new ClientWndCmd();
            wndCommand.CommandType = ClientWndCmd.CommandId.ERestore;
            wndCommand.Id = wndId;

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.WindowsAttributes,
                wndCommand);
        }

        public void SetApplicationMaximize(int wndId)
        {
            ClientWndCmd wndCommand = new ClientWndCmd();
            wndCommand.CommandType = ClientWndCmd.CommandId.EMaximize;
            wndCommand.Id = wndId;

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.WindowsAttributes,
                wndCommand);
        }

        public void SetApplicationPos(int wndId, int xPos, int yPos)
        {
            ClientWndCmd wndCommand = new ClientWndCmd();
            wndCommand.CommandType = ClientWndCmd.CommandId.ERelocation;
            wndCommand.Id = wndId;
            wndCommand.PositionX = xPos;
            wndCommand.PositionY = yPos;

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.WindowsAttributes,
                wndCommand);
        }

        public void SetApplicationClose(int wndId)
        {
            ClientWndCmd wndCommand = new ClientWndCmd();
            wndCommand.CommandType = ClientWndCmd.CommandId.EClose;
            wndCommand.Id = wndId;

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.WindowsAttributes,
                wndCommand);
        }

        public void TriggerVnc(Client.Model.VncModel model)
        {
            ClientVncCmd vncCommand = new ClientVncCmd()
            {
                CommandId = ClientVncCmd.ECommandId.Start,
                UserVncData = new VncEntry() 
                { 
                    IpAddress = model.VncServerIp,
                    Port = model.VncServerPort,
                    OwnerPCName = model.DisplayName,
                    MonitorCount = model.DisplayCount
                }
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.Vnc,
                vncCommand);
        }

        public void Dispose()
        {
            vncServer.StopVncServer();
        }
    }
}
