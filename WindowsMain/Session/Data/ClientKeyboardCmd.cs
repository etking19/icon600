using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Data
{
    public class ClientKeyboardCmd : BaseCmd
    {
        public struct KeyboardData
        {
            public UInt16 wVk;
            public UInt16 wScan;
            public UInt32 dwFlags;
            public UInt32 time;
            public UInt64 dwExtraInfo;
        }

        public KeyboardData data { get; set; }
    }
}
