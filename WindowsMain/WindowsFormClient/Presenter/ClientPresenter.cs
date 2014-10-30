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
    class ClientPresenter
    {
        public enum ServerMaintenanceMode
        {
            Shutdown = 1,
            Standby,
            Restart,
        }

        private IClient client;
        private ConnectionManager connectionMgr;
        private Client.ClientCmdMgr clientCmdMgr;

        public ClientPresenter(IClient client, ConnectionManager connectionMgr, string username, string password)
        {
            // initialize helper
            this.client = client;
            clientCmdMgr = new Client.ClientCmdMgr(client);

            this.connectionMgr = connectionMgr;
            connectionMgr.EvtDisconnected += connectionMgr_EvtDisconnected;
            connectionMgr.EvtServerDataReceived += connectionMgr_EvtServerDataReceived;

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

            connectionMgr.BroadcastMessage((int)CommandConst.MainCommandClient.LoginInfo, (int)CommandConst.SubCommandClient.Credential, loginCmd);
        }

        public void ShowMessage(string text, Font font, Color color, Color backgroundColor, int duration, int left, int top, int width, int height, bool animation)
        {
            ClientMessageBoxCmd msgBoxCmd = new ClientMessageBoxCmd()
            {
                Message = text,
                TextFont = new Session.Common.SerializableFont(font).SerializeFontAttribute,
                TextColor = System.Drawing.ColorTranslator.ToHtml(color),
                BackgroundColor = System.Drawing.ColorTranslator.ToHtml(backgroundColor),
                Duration = duration,
                Left = left,
                Top = top,
                Width = width,
                Height = height,
                AnimationEnabled = animation,
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

        public void AddPreset(string name)
        {
            PresetsEntry presetEntry = new PresetsEntry()
            {
                Name = name,
            };

            ClientPresetsCmd presetCmd = new ClientPresetsCmd()
            {
                ControlType = ClientPresetsCmd.EControlType.Add,
                PresetEntry = presetEntry,
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.Preset,
                presetCmd);
        }

        public void RemovePreset(Client.Model.PresetModel presetModel)
        {
            ClientPresetsCmd presetCmd = new ClientPresetsCmd()
            {
                ControlType = ClientPresetsCmd.EControlType.Delete,
                PresetEntry = new PresetsEntry { Identifier = presetModel.PresetId }
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.Preset,
                presetCmd);
        }

        public void TriggerPreset(Client.Model.PresetModel presetModel)
        {
            ClientPresetsCmd presetCmd = new ClientPresetsCmd()
            {
                ControlType = ClientPresetsCmd.EControlType.Launch,
                PresetEntry = new PresetsEntry()
                {
                    Identifier = presetModel.PresetId,
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
                    Identifier = model.Identifier,
                    IpAddress = model.VncServerIp,
                    Port = model.VncServerPort,
                    DisplayName = model.DisplayName,
                }
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.Vnc,
                vncCommand);
        }

        public void TriggerApplication(Client.Model.ApplicationModel model)
        {
            ClientApplicationCmd appCommand = new ClientApplicationCmd()
            {
                ApplicationEntry = new ApplicationEntry()
                {
                    Identifier = model.AppliationId,
                    Name = model.ApplicationName,
                }
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.Application,
                appCommand);
        }

        public void TriggerVisionInput(InputAttributes attributes)
        {
            ClientInputCommand visionCmd = new ClientInputCommand()
            {
                Action = ClientInputCommand.EAction.Launch,
                Attribute = attributes,
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.Functionality,
                (int)CommandConst.SubCommandClient.VisionInput,
                visionCmd);
        }

        public void EditUserSettings(int newGridX, int newGridY, bool newSnap)
        {
            ClientUserSettingCmd clientUserSettingCmd = new ClientUserSettingCmd()
            {
                UserSetting = new UserSetting()
                {
                    gridX = newGridX,
                    gridY = newGridY,
                    isSnap = newSnap,
                }
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.LoginInfo,
                (int)CommandConst.SubCommandClient.UserSetting,
                clientUserSettingCmd);
        }
    }
}
