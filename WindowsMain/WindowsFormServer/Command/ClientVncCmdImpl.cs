using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Server;
using WindowsFormClient.Server.Model;

namespace WindowsFormClient.Command
{
    class ClientVncCmdImpl : BaseImplementer
    {
        private VncMarshall.Client vncClientImpl = null;
        private IServer server;

        public ClientVncCmdImpl(IServer server, VncMarshall.Client vncClient)
        {
            this.vncClientImpl = vncClient;
            this.server = server;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientVncCmd data = deserialize.Deserialize<ClientVncCmd>(command);
            if (data == null)
            {
                return;
            }

            switch (data.CommandId)
            {
                case ClientVncCmd.ECommandId.Start:
                    StartVnc(userId, data.UserVncData);
                    break;
                default:
                    break;
            }
        }

        private void StartVnc(string userId, VncEntry data)
        {
            int result = vncClientImpl.StartClient(data.IpAddress, data.Port);

            // save to user list
            int userDBid = ConnectedClientHelper.GetInstance().GetClientInfo(userId).DbUserId;
            Server.LaunchedVncHelper.GetInstance().AddLaunchedApp(userDBid, result, data.Identifier);
        }
    }
}
