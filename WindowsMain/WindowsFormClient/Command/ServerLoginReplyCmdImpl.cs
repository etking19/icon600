using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Client.Model;
using WindowsFormClient.Settings;

namespace WindowsFormClient.Command
{
    class ServerLoginReplyCmdImpl : BaseImplementer
    {
        private IClient client = null;

        public ServerLoginReplyCmdImpl(IClient client)
        {
            this.client = client;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ServerLoginReply loginData = deserialize.Deserialize<ServerLoginReply>(command);
            if (loginData == null)
            {
                return;
            }

            // calculate the actual size of server desktop (might have multiple monitors)
            int minPosX = 0;
            int minPosY = 0;
            int maxPosX = 0;
            int maxPosY = 0;
            foreach (MonitorInfo monitor in loginData.ServerLayout.ServerMonitorsList)
            {
                minPosX = Math.Min(monitor.LeftPos, minPosX);
                minPosY = Math.Min(monitor.TopPos, minPosY);

                maxPosX = Math.Max(monitor.RightPos, maxPosX);
                maxPosY = Math.Max(monitor.BottomPos, maxPosY);
            }

            UserInfoModel userInfo = new UserInfoModel() { UserId = loginData.UserId, DisplayName = loginData.LoginName };
            ServerLayoutModel layoutInfo = new ServerLayoutModel()
            {
                DesktopLayout = new WindowsModel() 
                {
                    PosLeft=minPosX,
                    PosTop=minPosY, 
                    Width=maxPosX-minPosX, 
                    Height=maxPosY-minPosY
                },
                
                LayoutColumn = loginData.ServerLayout.MatrixCol,
                LayoutRow = loginData.ServerLayout.MatrixRow,
            };

            WindowsModel viewingArea = new WindowsModel()
            {
                PosLeft = loginData.ViewingArea.LeftPos,
                PosTop = loginData.ViewingArea.TopPos,
                Width = loginData.ViewingArea.RightPos - loginData.ViewingArea.LeftPos,
                Height = loginData.ViewingArea.BottomPos - loginData.ViewingArea.TopPos
            };

            // save to settings
            ServerSettings.GetInstance().DesktopLeft = layoutInfo.DesktopLayout.PosLeft;
            ServerSettings.GetInstance().DesktopTop = layoutInfo.DesktopLayout.PosTop;
            ServerSettings.GetInstance().DesktopWidth = layoutInfo.DesktopLayout.Width;
            ServerSettings.GetInstance().DesktopHeight = layoutInfo.DesktopLayout.Height;

            ServerSettings.GetInstance().ViewingAreaLeft = viewingArea.PosLeft;
            ServerSettings.GetInstance().ViewingAreaTop = viewingArea.PosTop;
            ServerSettings.GetInstance().ViewingAreaWidth = viewingArea.Width;
            ServerSettings.GetInstance().ViewingAreaHeight = viewingArea.Height;

            ServerSettings.GetInstance().DesktopRow = layoutInfo.LayoutRow;
            ServerSettings.GetInstance().DesktopColumn = layoutInfo.LayoutColumn;

            UserSettings.GetInstance().UserId = userInfo.UserId;
            UserSettings.GetInstance().DisplayName = userInfo.DisplayName;

            // update the gui
            client.RefreshLayout(userInfo, layoutInfo, viewingArea);

            // update the application priviledge
            ServerAppStatusCmdImpl appCmdImpl = new ServerAppStatusCmdImpl(client);
            appCmdImpl.ExecuteCommand(userId, loginData.UserApplications.getCommandString());

            // update the maintenance priviledge
            ServerMaintenanceCmdImpl maintenanceCmdImpl = new ServerMaintenanceCmdImpl(client);
            maintenanceCmdImpl.ExecuteCommand(userId, loginData.UserMaintenance.getCommandString());

            // update presets saved
            ServerPresetCmdImpl presetCmdImpl = new ServerPresetCmdImpl(client);
            presetCmdImpl.ExecuteCommand(userId, loginData.UserPresets.getCommandString());

            // update vnc saved
            ServerVncStatusCmdImpl vncCmdImp = new ServerVncStatusCmdImpl(client);
            vncCmdImp.ExecuteCommand(userId, loginData.VncStatus.getCommandString());
        }
    }
}
