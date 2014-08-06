using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ServerVncStatus : BaseCmd
    {
        public List<VncEntry> UserVncList { get; set; }
    }
}
