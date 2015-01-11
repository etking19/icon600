using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;

namespace WindowsFormClient.Telnet.Command
{
    class LaunchRemote : TelnetCommand
    {
        public const string COMMAND = "LaunchRemote";

        private VncMarshall.Client _vncClient;

        public LaunchRemote(VncMarshall.Client vncClient)
        {
            this._vncClient = vncClient;
        }

        /// <summary>
        /// launch remote stored base in db index
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

            // get the info from database
            RemoteVncData vncInfo = Server.ServerDbHelper.GetInstance().GetRemoteVncList().ToList().Find(
                vncData => vncData.id == dbIndex);
            if (vncInfo == null)
            {
                // no matched 
                return "No matched id found.";
            }

            // launch and save the data
            try
            {
                int result = _vncClient.StartClient(vncInfo.remoteIp, vncInfo.remotePort);
                Server.LaunchedVncHelper.GetInstance().AddLaunchedApp(userData.id, result, dbIndex);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            

            return "Remote launched successfully";
        }

        public override string getCommandPattern()
        {
            return "LaunchRemote [remote db index] [username] [password]";
        }
    }
}
