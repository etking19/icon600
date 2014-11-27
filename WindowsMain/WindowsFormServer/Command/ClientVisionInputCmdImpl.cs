using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Server;

namespace WindowsFormClient.Command
{
    class ClientVisionInputCmdImpl : BaseImplementer
    {
        public ClientVisionInputCmdImpl()
        {

        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientInputCommand visionData = deserialize.Deserialize<ClientInputCommand>(command);
            if (visionData == null)
            {
                return;
            }

            uint processId = (uint)ServerVisionHelper.getInstance().LaunchVisionWindow(visionData.Attribute.InputId);
            //Server.ConnectedClientHelper.GetInstance().AddLaunchedInputSource(userId, processId, visionData.Attribute.InputId);
        }
    }
}
