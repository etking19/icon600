using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Telnet.Command
{
    class RemovePreset : TelnetCommand
    {
        public const string COMMAND = "CleanPreset";

        /// <summary>
        /// remove preset from db
        /// </summary>
        /// <param name="command">
        /// command[0] = "command pattern"
        /// command[1] = "db index"
        /// </param>
        /// <returns></returns>
        public override string executeCommand(string[] command)
        {
            if (command.Count() != 2)
            {
                throw new Exception();
            }

            int presetId = 0;
            if(int.TryParse(command[1], out presetId) == false)
            {
                throw new Exception();
            }

            Server.ServerDbHelper.GetInstance().RemovePreset(presetId);
            return "Preset removed.";
        }

        public override string getCommandPattern()
        {
            return "CleanPreset [preset id]";
        }
    }
}
