using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Client.Model
{
    public class VncModel
    {
        public string DisplayName { get; set; }

        public int DisplayCount { get; set; }
        public string VncServerIp { get; set; }
        public int VncServerPort { get; set; }
    }
}
