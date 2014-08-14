using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormServer.Client.Model
{
    public class WindowsModel
    {
        public int WindowsId { get; set; }
        public string DisplayName { get; set; }

        public int PosLeft { get; set; }

        public int PosTop { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int Style { get; set; }
        public int ZOrder { get; set; }
    }
}
