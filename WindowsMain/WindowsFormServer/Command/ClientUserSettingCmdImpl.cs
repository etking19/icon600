using Session;
using Session.Data;
using System.Collections.Generic;

namespace WindowsFormClient.Command
{
    class ClientUserSettingCmdImpl : BaseImplementer
    {
        private IServer server;
        private int dbUserId;

        public ClientUserSettingCmdImpl(IServer server, int dbUserId)
        {
            this.dbUserId = dbUserId;
            this.server = server;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientUserSettingCmd settingData = deserialize.Deserialize<ClientUserSettingCmd>(command);
            if (settingData == null)
            {
                return;
            }

            if(Server.ServerDbHelper.GetInstance().EditUserSetting(dbUserId, settingData.UserSetting.gridX, settingData.UserSetting.gridY, settingData.UserSetting.isSnap))
            {
                ServerUserSetting userSetting = new ServerUserSetting()
                {
                    UserSetting = new Session.Data.SubData.UserSetting()
                    {
                        gridX = settingData.UserSetting.gridX,
                        gridY = settingData.UserSetting.gridY,
                        isSnap = settingData.UserSetting.isSnap,
                    }
                };

                // TODO: should notify all connected login user with same id
                // notify user
                server.GetConnectionMgr().SendData(
                    (int)CommandConst.MainCommandServer.UserPriviledge,
                    (int)CommandConst.SubCommandServer.UserSetting,
                    userSetting,
                    new List<string>() { userId });
            }

        }
    }
}
