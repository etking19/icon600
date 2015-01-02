using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Telnet.Command
{
    class LaunchPreset : TelnetCommand
    {
        public override string executeCommand(string[] command)
        {
            return "";
        }

        public override string getCommandPattern()
        {
            throw new NotImplementedException();
        }
    }
}
