using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;

namespace WindowsFormClient.Telnet.Command
{
    class GetRemoteList : TelnetCommand
    {
        public const string COMMAND = "GetRemoteList";

        /// <summary>
        /// get the remote vnc list
        /// </summary>
        /// <param name="command">
        /// command[0] = "command pattern"
        /// </param>
        /// <returns></returns>
        public override string executeCommand(string[] command)
        {
            RemoteVncData[] visionData = Server.ServerDbHelper.GetInstance().GetRemoteVncList();
            string reply = "";
            foreach (RemoteVncData data in visionData)
            {
                reply += string.Format("id:{0}, displayName:{1}, remoteIp:{2}, remotePort:{3}",
                    data.id,
                    data.name,
                    data.remoteIp,
                    data.remotePort);

                reply += Environment.NewLine;
            }

            return reply;
        }

        public override string getCommandPattern()
        {
            return "GetRemoteList";
        }
    }
}
