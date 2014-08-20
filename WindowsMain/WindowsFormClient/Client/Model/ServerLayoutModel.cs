using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Client.Model
{
    public class ServerLayoutModel
    {
        public int LayoutRow { get; set; }
        public int LayoutColumn { get; set; }

        public WindowsModel DesktopLayout { get; set; }

        public WindowsModel ViewingArea { get; set; }
    }
}
