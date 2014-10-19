using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Data.SubData
{
    public class WndPos
    {
        public int id { get; set; }
        public string name { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public Int32 style { get; set; }

        public int ZOrder { get; set; }
        public uint ProcessId { get; set; }
    }
}
