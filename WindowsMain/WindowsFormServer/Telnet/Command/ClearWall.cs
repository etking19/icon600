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
        private Windows.WindowsAppMgr wndMgr;

        public ClearWall(Windows.WindowsAppMgr wndMgr)
        {
            this.wndMgr = wndMgr;
        }

        /// <summary>
        /// clear the entire wall
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

            // close all application running on screen
            List<Windows.WindowsAppMgr.WndAttributes> activeApps = wndMgr.getAllVisibleApps();
            foreach (Windows.WindowsAppMgr.WndAttributes wndAttr in activeApps) 
            {
                Utils.Windows.NativeMethods.PostMessage(new IntPtr(wndAttr.id), Utils.Windows.Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }

            // remove the records
            LaunchedWndHelper.GetInstance().Reset();
            LaunchedVncHelper.GetInstance().Reset();
            LaunchedSourcesHelper.GetInstance().Reset();

            // clear base on id
            //new ClientPresetCmdImpl().ClearWall(userData.id);

            return "Clear wall successfully.";
        }

        public override string getCommandPattern()
        {
            return "ClearWall [username] [password]";
        }
    }
}
