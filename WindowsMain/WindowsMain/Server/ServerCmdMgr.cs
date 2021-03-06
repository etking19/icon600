﻿using Session;
using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormServer.Command;

namespace WindowsFormServer.Server
{
    class ServerCmdMgr
    {
        private IServer server;

        public ServerCmdMgr(IServer server)
        {
            this.server = server;
        }

        public void ExeCommand(string userId, int mainId, int subId, string command)
        {
            ICmdImplementer implementer = null;
            if ((implementer = GetImplementer(userId, mainId, subId)) != null)
            {
                implementer.ExecuteCommand(userId, command);
            }
        }

        private ICmdImplementer GetImplementer(string userId, int mainId, int subId)
        {
            ICmdImplementer implementor = null;
            switch (subId)
            {
                case (int)CommandConst.SubCommandClient.Credential:
                    implementor = new Command.ClientLoginImpl(server);
                    break;
                case (int)CommandConst.SubCommandClient.Keyboard:
                    implementor = new Command.ClientKeyboardCmdImpl();
                    break;
                case (int)CommandConst.SubCommandClient.Maintenance:
                    implementor = new Command.ClientMaintenanceCmdImpl();
                    break;
                case (int)CommandConst.SubCommandClient.Mouse:
                    implementor = new Command.ClientMouseCmdImpl();
                    break;
                case (int)CommandConst.SubCommandClient.Preset:
                    // get user table primary key from user id string
                    implementor = new Command.ClientPresetCmdImpl(server.GetClientInfo(userId).DbUserId);
                    break;
                case (int)CommandConst.SubCommandClient.Vnc:
                    implementor = new Command.ClientVncCmdImpl(server, server.GetVncClient());
                    break;
                case (int)CommandConst.SubCommandClient.WindowsAttributes:
                    implementor = new Command.ClientWndAttrCmdImpl();
                    break;
            }

            return implementor;
        }

    }
}
