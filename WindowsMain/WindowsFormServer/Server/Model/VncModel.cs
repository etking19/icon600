using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Server.Model
{
    public class VncModel
    {
        public string OwnerPCName { get; set; }
        public int MonitorCount { get; set; }
        public string IpAddress { get; set; }
        public int ListeningPort { get; set; }
    }
}
