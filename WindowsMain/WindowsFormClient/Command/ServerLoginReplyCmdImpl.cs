using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Client.Model;

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
            foreach (MonitorInfo monitor in loginData.ServerLayout.MonitorAttributes)
            {
                minPosX = Math.Min(monitor.LeftPos, minPosX);
                minPosY = Math.Min(monitor.TopPos, minPosY);

                maxPosX = Math.Max(monitor.RightPos, maxPosX);
                maxPosY = Math.Max(monitor.BottomPos, maxPosY);
            }

            UserInfoModel userInfo = new UserInfoModel() { UserId = loginData.UserId, DisplayName = loginData.LoginName };
            ServerLayoutModel layoutInfo = new ServerLayoutModel()
            {
                DesktopLayout = new WindowsModel() {PosLeft=minPosX, PosTop=minPosY, Width=maxPosX-minPosX, Height=maxPosY-minPosY},
                LayoutColumn = loginData.ServerLayout.MatrixCol,
                LayoutRow = loginData.ServerLayout.MatrixRow,
            };

            // update the gui
            client.RefreshLayout(userInfo, layoutInfo);

            // update the application priviledge
            ServerAppStatusCmdImpl appCmdImpl = new ServerAppStatusCmdImpl(client);
            appCmdImpl.ExecuteCommand(userId, loginData.UserApplications.getCommandString());

            // update the maintenance priviledge
            ServerMaintenanceCmdImpl maintenanceCmdImpl = new ServerMaintenanceCmdImpl(client);
            maintenanceCmdImpl.ExecuteCommand(userId, loginData.UserMaintenance.getCommandString());

            // update presets saved
            ServerPresetCmdImpl presetCmdImpl = new ServerPresetCmdImpl(client);
            presetCmdImpl.ExecuteCommand(userId, loginData.UserPresets.getCommandString());
        }
    }
}
