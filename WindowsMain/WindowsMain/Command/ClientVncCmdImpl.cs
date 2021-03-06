﻿using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormServer.Server;
using WindowsFormServer.Server.Model;

namespace WindowsFormServer.Command
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
                    StartVnc(data.UserVncData);
                    break;
                default:
                    break;
            }
        }

        private void StartVnc(VncEntry data)
        {
            ClientInfoModel clientInfo = server.GetClientInfo(data.Identifier);
            vncClientImpl.StartClient(clientInfo.VncInfo.IpAdress, clientInfo.VncInfo.ListeningPort);
        }
    }
}
