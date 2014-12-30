using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ServerMaintenanceStatus : BaseCmd
    {
        public bool AllowMaintenance { get; set; }
        public bool AllowRemoteControl { get; set; }
    }
}
