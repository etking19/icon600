using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Telnet.Command;

namespace WindowsFormClient.Telnet
{
    /// <summary>
    /// class to parse the command sent by telnet
    /// </summary>
    public class CommandParser
    {
        private GetAppList _getAppListCmd;

        public CommandParser()
        {
            _getAppListCmd = new GetAppList();
        }

        public string parseCommand(string command)
        {
            string[] cmdList = command.Split(' ');

            string errorMsg = "Invalid command. For help please type '?'." + Environment.NewLine;
            if(cmdList.Length == 0)
            {
                return errorMsg;
            }

            switch(cmdList[0])
            {
                case "?":
                    return "List of supported commands:" + Environment.NewLine
                        + _getAppListCmd.getCommandPattern()
                        + Environment.NewLine;

                case GetAppList.COMMAND:
                    return _getAppListCmd.executeCommand(cmdList);

                default:
                    return errorMsg;

            }
        }
    }
}
