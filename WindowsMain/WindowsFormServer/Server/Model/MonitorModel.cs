using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Server.Model
{
    public class MonitorModel
    {
        public string Label { get; set; }

        public int WorkAreaLeft { get; set; }
        public int WorkAreaTop { get; set; }
        public int WorkAreaRight { get; set; }
        public int WorkAreaBottom { get; set; }
    }
}
