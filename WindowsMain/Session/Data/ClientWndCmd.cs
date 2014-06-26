using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Data
{
    public class ClientWndCmd : BaseCmd
    {
        public enum CommandId
        {
            EClose = 1,
            EMinimize,
            EMaximize,
            ERestore,
            ERelocation,
            EResize,
            ESetForeground,
        }

        public CommandId CommandType { get; set; }

        public int Id { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
