﻿using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsMain.Client.Model;

namespace WindowsMain.Command
{
    class ServerMaintenanceCmdImpl : BaseImplementer
    {
        private IClient client = null;

        public ServerMaintenanceCmdImpl(IClient client)
        {
            this.client = client;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ServerMaintenanceStatus maintenanceData = deserialize.Deserialize<ServerMaintenanceStatus>(command);
            if (maintenanceData == null)
            {
                return;
            }

            UserPriviledgeModel model = new UserPriviledgeModel()
            {
                AllowMaintenance = maintenanceData.AllowMaintenance,
            };

            client.RefreshMaintenanceStatus(model);
        }
    }
}
