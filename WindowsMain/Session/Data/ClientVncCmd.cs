using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ClientVncCmd : BaseCmd
    {
        public enum ECommandId
        {
            Start = 1,
        }

        public ECommandId CommandId { get; set; }
        public VncEntry UserVncData { get; set; }
    }
}
