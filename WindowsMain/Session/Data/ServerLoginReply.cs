using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ServerLoginReply : BaseCmd
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }

        public ServerScreenInfo ServerLayout { get; set; }

        public MonitorInfo ViewingArea { get; set; }

        public ServerPresetsStatus UserPresets { get; set; }

        public ServerMaintenanceStatus UserMaintenance { get; set; }

        public ServerApplicationStatus UserApplications { get; set; }

        public ServerVncStatus VncStatus { get; set; }

        public ServerUserSetting UserSetting { get; set; }
    }
}
