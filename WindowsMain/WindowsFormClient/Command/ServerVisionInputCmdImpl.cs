using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Settings;

namespace WindowsFormClient.Command
{
    class ServerVisionInputCmdImpl : BaseImplementer
    {
        private IClient client = null;
        public ServerVisionInputCmdImpl(IClient client)
        {
            this.client = client;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ServerInputStatus visionInputStatusData = deserialize.Deserialize<ServerInputStatus>(command);
            if (visionInputStatusData == null)
            {
                return;
            }

            ApplicationSettings.GetInstance().InputList = visionInputStatusData.InputAttributesList;
            client.RefreshVisionInputStatus(visionInputStatusData.InputAttributesList);
        }
    }
}
