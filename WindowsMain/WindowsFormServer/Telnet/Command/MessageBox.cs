using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Telnet.Command
{
    class MessageBox : TelnetCommand
    {
        public const string COMMAND = "MessageBox";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">
        /// command[0] = "command pattern" 
        /// command[1] = "message" 
        /// command[2] = "font attributes"
        /// command[3] = "word color attributes" 
        /// command[4] = "background color attributes"
        /// command[5] = "duration"
        /// command[6] = "rect left"
        /// command[7] = "rect top"
        /// command[8] = "allow animation" (true/false)
        /// </param>
        /// <returns></returns>
        public override string executeCommand(string[] command)
        {
            if (command.Count() != 9)
            {
                throw new Exception();
            }

            int duration = 0;
            if (int.TryParse(command[5], out duration) == false)
            {
                throw new Exception();
            }

            int left = 0;
            if (int.TryParse(command[6], out left) == false)
            {
                throw new Exception();
            }

            int top = 0;
            if (int.TryParse(command[7], out top) == false)
            {
                throw new Exception();
            }

            bool animation = false;
            if (bool.TryParse(command[8], out animation) == false)
            {
                throw new Exception();
            }

            string arg = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" {4} {5} {6} {7} {8} {9}",
                command[1],
                command[2],
                command[3],
                command[4],
                command[5],
                left,
                top,
                0,
                0,
                animation);

            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = "CustomMessageBox.exe",
                Arguments = arg
            };

            try
            {
                Process.Start(info);
            }
            catch (Exception)
            {
                return "failed to start message box";
            }

            return "Message box started successfully";
        }

        public override string getCommandPattern()
        {
            /// command[0] = "command pattern" 
            /// command[1] = "message" 
            /// command[2] = "font attributes"
            /// command[3] = "word color attributes" 
            /// command[4] = "background color attributes"
            /// command[5] = "duration"
            /// command[6] = "rect left"
            /// command[7] = "rect top"
            /// command[8] = "allow animation"
            return "MessageBox [message] [font] [word color] [background color] [duration, -1 for infinite] [pos left] [pos top] [0=no animation, 1=allow animation]";
        }
    }
}
