using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;

namespace WindowsFormClient.Telnet.Command
{
    class GetInputSourceList : TelnetCommand
    {
        public const string COMMAND = "GetInputSourcesList";

        /// <summary>
        /// get the input sources athorized by server
        /// </summary>
        /// <param name="command">
        /// command[0] = "command pattern"
        /// </param>
        /// <returns></returns>
        public override string executeCommand(string[] command)
        {
            VisionData[] visionData = Server.ServerDbHelper.GetInstance().GetAllVisionInputs();
            string reply = "";
            foreach (VisionData data in visionData)
            {
                reply += string.Format("id:{0}, inputSourceDisplayName:{1}", 
                    data.id, 
                    data.windowStr);

                reply += Environment.NewLine;
            }

            return reply;
        }

        public override string getCommandPattern()
        {
            return "GetInputSourcesList";
        }
    }
}
