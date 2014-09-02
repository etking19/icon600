using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Data
{
    public class ServerScreenInfo : BaseCmd
    {
        public int MatrixRow { get; set; }
        public int MatrixCol { get; set; }

        public List<MonitorInfo> ServerMonitorsList { get; set; }
    }
}
