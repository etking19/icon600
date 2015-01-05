using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;
using WindowsFormClient.Command;

namespace WindowsFormClient.Telnet.Command
{
    class CreatePreset : TelnetCommand
    {
        public const string COMMAND = "CreatePreset";

        /// <summary>
        /// Create preset based on user's login
        /// </summary>
        /// <param name="command">
        /// command[0] = "command pattern"
        /// command[1] = "username"
        /// command[2] = "password"
        /// command[3] = "preset name with no space"
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
                (user.username.CompareTo(command[1]) == 0 &&
                user.password.CompareTo(command[2]) == 0));
            if (userData == null)
            {
                // no matched 
                return "Credential verification failed. Please check login info.";
            }

            // add preset code from ClientPresetCmdImpl
            int dbUserId = userData.id;
            new ClientPresetCmdImpl().AddPresetExternal(dbUserId, command[3]);

            return "Add preset successfully.";
        }

        public override string getCommandPattern()
        {
            return "CreatePreset [username] [password] [preset name with no space in between]";
        }
    }
}
