using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ClientMaintenanceCmd : BaseCmd
    {
        public enum CommandId
        {
            EShutdown = 1,
            ELogOff,
            EReboot,
        }

        public CommandId CommandType { get; set; }
    }
}
