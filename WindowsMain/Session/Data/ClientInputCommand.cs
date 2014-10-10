using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ClientInputCommand : BaseCmd
    {
        public enum EAction
        {
            Launch = 1,
        }

        public EAction Action { get; set; }
        public InputAttributes Attribute { get; set; }
    }
}
