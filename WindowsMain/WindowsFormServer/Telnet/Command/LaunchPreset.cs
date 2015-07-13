using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;
using WindowsFormClient.Command;

namespace WindowsFormClient.Telnet.Command
{
    class LaunchPreset : TelnetCommand
    {
        public const string COMMAND = "LaunchPreset";

        private VncMarshall.Client vncClient;

        public LaunchPreset(VncMarshall.Client vncClient)
        {
            this.vncClient = vncClient;
        }

        /// <summary>
        /// launched preset 
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
                new ClientPresetCmdImpl(vncClient).ClearWall(userData.id);
                new ClientPresetCmdImpl(vncClient).LaunchPresetExternal(userData.id, dbIndex);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            
            return "Preset launched successfully";
        }

        public override string getCommandPattern()
        {
            return "LaunchPreset [preset db id] [username] [password]";
        }
    }
}
