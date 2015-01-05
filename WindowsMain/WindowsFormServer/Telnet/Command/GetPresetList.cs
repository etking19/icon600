using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;

namespace WindowsFormClient.Telnet.Command
{
    class GetPresetList : TelnetCommand
    {
        public const string COMMAND = "GetPresetList";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">
        /// command[0] = "command pattern"
        /// command[1] = "username"
        /// command[2] = "password"
        /// </param>
        /// <returns></returns>
        public override string executeCommand(string[] command)
        {
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

            int dbUserId = userData.id;
            PresetData[] presetData = Server.ServerDbHelper.GetInstance().GetPresetByUserId(dbUserId).ToArray();

            string reply = "";
            foreach (PresetData data in presetData)
            {
                reply += string.Format("id:{0}, displayName:{1}",
                    data.Id,
                    data.Name);

                reply += Environment.NewLine;
            }

            return reply;
        }

        public override string getCommandPattern()
        {
            return "GetPresetList [username] [password]";
        }
    }
}
