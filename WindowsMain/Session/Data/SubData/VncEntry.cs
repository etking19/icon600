using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data.SubData
{
    public class VncEntry
    {
        public string OwnerPCName { get; set; }
        public int MonitorCount { get; set; }

        /// <summary>
        /// Used when client login to server
        /// </summary>
        public string IpAddress { get; set; }
        public int Port { get; set; }
    }
}
