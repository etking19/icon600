using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data.SubData
{
    public class VncEntry
    {
        public int Identifier { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// Used when client login to server
        /// </summary>
        public string IpAddress { get; set; }
        public int Port { get; set; }
    }
}
