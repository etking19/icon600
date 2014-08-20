using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ClientMessageBoxCmd : BaseCmd
    {
        public string Message { get; set; }
        public System.Drawing.Font TextFont { get; set; }
        public System.Drawing.Color TextColor { get; set; }
        public int Duration { get; set; }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
