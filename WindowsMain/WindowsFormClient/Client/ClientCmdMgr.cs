﻿using Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Command;

namespace WindowsFormClient.Client
{
    class ClientCmdMgr
    {
        private IClient client = null;

        public ClientCmdMgr(IClient client)
        {
            this.client = client;
        }

        public void ExeCommand(string userId, int mainId, int subId, string command)
        {
            ICmdImplementer implementer = null;
            if ((implementer = GetImplementer(mainId, subId)) != null)
            {
                implementer.ExecuteCommand(userId, command);
            }
        }

        private ICmdImplementer GetImplementer(int mainId, int subId)
        {
            ICmdImplementer implementor = null;
            switch (subId)
            {
                case (int)CommandConst.SubCommandServer.ApplicationList:
                    implementor = new Command.ServerAppStatusCmdImpl(client);
                    break;
                case (int)CommandConst.SubCommandServer.DisplayInfo:
                    implementor = new Command.ServerLoginReplyCmdImpl(client);
                    break;
                case (int)CommandConst.SubCommandServer.Maintenance:
                    implementor = new Command.ServerMaintenanceCmdImpl(client);
                    break;
                case (int)CommandConst.SubCommandServer.PresetList:
                    implementor = new Command.ServerPresetCmdImpl(client);
                    break;
                case (int)CommandConst.SubCommandServer.VncList:
                    implementor = new Command.ServerVncStatusCmdImpl(client);
                    break;
                case (int)CommandConst.SubCommandServer.WindowsList:
                    implementor = new Command.ServerWndsAttrCmdImpl(client);
                    break;
                case (int)CommandConst.SubCommandServer.ViewingArea:
                    implementor = new Command.ServerViewingAreaCmdImpl(client);
                    break;
                case (int)CommandConst.SubCommandServer.VisionInput:
                    implementor = new Command.ServerVisionInputCmdImpl(client);
                    break;
                case (int)CommandConst.SubCommandServer.UserSetting:
                    implementor = new Command.ServerUserSettingCmdImpl(client);
                    break;
                default:
                    break;
            }

            return implementor;
        }
    }
}
