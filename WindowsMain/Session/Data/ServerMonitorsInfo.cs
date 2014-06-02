using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Data
{
    public class ServerMonitorsInfo : BaseCmd
    {
        public List<MonitorInfo> monitorAttributes { get; set; }

        public override string getCommandString()
        {
            return serializer.Serialize(this);
        }
    }
}
