using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;
using WindowsFormClient.Server;

namespace WindowsFormClient.Telnet.Command
{
    class LaunchInputSource : TelnetCommand
    {
        public const string COMMAND = "LaunchInputSource";

        /// <summary>
        /// launch input source base in db index
        /// </summary>
        /// <param name="command">
        /// command[0] = "command pattern"
        /// command[1] = "db index"
        /// command[2] = "username"
        /// command[3] = "password"
        /// </param>
        /// <returns></returns>
        public override string executeCommand(string[] command)
        {
            if (command.Count() != 4)
            {
                throw new Exception();
            }

            List<UserData> userDataList = new List<UserData>(Server.ServerDbHelper.GetInstance().GetAllUsers());
            UserData userData = userDataList.Find(user
                =>
                (user.username.CompareTo(command[2]) == 0 &&
                user.password.CompareTo(command[3]) == 0));
            if (userData == null)
            {
                // no matched 
                return "Credential verification failed. Please check login info.";
            }

            int dbIndex = 0;
            if (int.TryParse(command[1], out dbIndex) == false)
            {
                throw new Exception();
            }

            try
            {
                int result = ServerVisionHelper.getInstance().LaunchVisionWindow(dbIndex);
                Server.LaunchedSourcesHelper.GetInstance().AddLaunchedApp(userData.id, result, dbIndex);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            

            return "Input source launched successfully";
        }

        public override string getCommandPattern()
        {
            return "LaunchInputSource [input source id] [username] [password]";
        }
    }
}
