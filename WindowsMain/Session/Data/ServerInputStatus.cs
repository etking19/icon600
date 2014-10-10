using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ServerInputStatus : BaseCmd
    {
        public List<InputAttributes> InputAttributesList { get; set; }
    }
}
