using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;
using WindowsFormClient.Command;
using WindowsFormClient.Server;

namespace WindowsFormClient.Telnet.Command
{
    class ClearWall : TelnetCommand
    {
        public const string COMMAND = "ClearWall";

        /// <summary>
        /// clear the wall base on user's login
        /// </summary>
        /// <param name="command">
        /// command[0] = "command pattern"
        /// command[1] = "username"
        /// command[2] = "password"
        /// </param>
        /// <returns>Invalid string if wrong login info</returns>
        public override string executeCommand(string[] command)
        {
            if (command.Count() != 3)
            {
                throw new Exception();
            }

            List<UserData> userDataList = new List<UserData>(Server.ServerDbHelper.GetInstance().GetAllUsers());
            UserData userData = userDataList.Find(user
                =>
                (user.username.CompareTo(command[1]) == 0 &&
                user.password.CompareTo(command[2]) == 0));
            if (userData == null)
            {
                // no matched 
                return "Credential verification failed. Please check login info.";
            }

            // clean the wall based on user login id
            // close all lauched applications
            new ClientPresetCmdImpl().ClearWall(userData.id);

            return "Clean wall successfully.";
        }

        public override string getCommandPattern()
        {
            return "ClearWall [username] [password]";
        }
    }
}
