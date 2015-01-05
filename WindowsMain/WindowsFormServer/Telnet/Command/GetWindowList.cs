using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;

namespace WindowsFormClient.Telnet.Command
{
    class GetWindowList : TelnetCommand
    {
        public const string COMMAND = "GetWindowList";

        public override string executeCommand(string[] command)
        {
            ApplicationData[] appDataList = Server.ServerDbHelper.GetInstance().GetAllApplications().ToArray();

            string reply = "";
            foreach (ApplicationData data in appDataList)
            {
                reply += string.Format("id:{0}, displayName:{1}, exePath:{2}, arguments:{3}, rect:{4}, {5}, {6}, {7}",
                    data.id,
                    data.name,
                    data.applicationPath,
                    data.arguments,
                    data.rect.Left,
                    data.rect.Top,
                    data.rect.Right,
                    data.rect.Bottom);

                reply += Environment.NewLine;
            }

            return reply;
        }

        public override string getCommandPattern()
        {
            return "GetWindowList";
        }
    }
}
