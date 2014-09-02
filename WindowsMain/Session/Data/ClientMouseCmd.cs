using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Data
{
    public class ClientMouseCmd : BaseCmd
    {
        public struct MouseData
        {
            public Int32 dx;
            public Int32 dy;
            public UInt32 mouseData;
            public UInt32 dwFlags;
            public UInt32 time;
            public Int32 dwExtraInfo;
        }

        public MouseData data { get; set; }
    }
}
