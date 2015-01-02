using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Telnet.Command
{
    abstract class TelnetCommand
    {
        public abstract string executeCommand(string[] command);
        public abstract string getCommandPattern();
    }
}
