using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsMain.Client
{
    public class MinimizeComboBox
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public WndPos Data { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
