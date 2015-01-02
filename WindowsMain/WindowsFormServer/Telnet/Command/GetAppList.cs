using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Telnet.Command
{
    class GetAppList : TelnetCommand
    {
        public const string COMMAND = "GetAppList";

        public override string executeCommand(string[] command)
        {
            return Server.ServerDbHelper.GetInstance().GetAllApplications().ToArray().ToString() + Environment.NewLine;
        }

        public override string getCommandPattern()
        {
            return "GetAppList";
        }
    }
}
