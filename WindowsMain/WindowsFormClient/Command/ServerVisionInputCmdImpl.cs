using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            client.RefreshVisionInputStatus(visionInputStatusData.InputAttributesList);
        }
    }
}
