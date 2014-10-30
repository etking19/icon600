using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Command
{
    class ServerUserSettingCmdImpl : BaseImplementer
    {
        private IClient client = null;

        public ServerUserSettingCmdImpl(IClient client)
        {
            this.client = client;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ServerUserSetting settingData = deserialize.Deserialize<ServerUserSetting>(command);
            if (settingData == null)
            {
                return;
            }

            client.RefreshUserGridLayout(settingData.UserSetting);
        }
    }
}
