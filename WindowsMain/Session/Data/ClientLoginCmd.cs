
using Session.Data.SubData;
using System.Collections.Generic;
namespace Session.Data
{
    public class ClientLoginCmd : BaseCmd
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public IList<VncEntry> VncList { get; set; }

        //public string VncServerIp { get; set; }
        //public int VncServerPort { get; set; }

        //public IList<MonitorInfo> MonitorsInfo { get; set; }
    }
}
