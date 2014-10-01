using Session.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Utils.Windows;
using WcfServiceLibrary1;

namespace WindowsFormClient.Command
{
    class ClientAppCmdImpl : BaseImplementer
    {
        public ClientAppCmdImpl()
        {

        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientApplicationCmd presetData = deserialize.Deserialize<ClientApplicationCmd>(command);
            if (presetData == null)
            {
                return;
            }

            ApplicationData appData = Server.ServerDbHelper.GetInstance().GetAllApplications().First(AppData 
                => AppData.id == presetData.ApplicationEntry.Identifier);

            if (appData == null)
            {
                Trace.WriteLine("unable to find matched application id: " + presetData.ApplicationEntry.Identifier);
                return;
            }

            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = appData.applicationPath,
                Arguments = appData.arguments
            };

            using (Process process = Process.Start(info))
            {
                NativeMethods.SetWindowPos(new IntPtr(process.Id), Constant.HWND_TOP, appData.rect.Left, appData.rect.Top, 0, 0, (Int32)(Constant.SWP_NOSIZE));
                NativeMethods.SetWindowPos(new IntPtr(process.Id), Constant.HWND_TOP, 0, 0, appData.rect.Right - appData.rect.Left, appData.rect.Bottom - appData.rect.Top, (Int32)Constant.SWP_NOMOVE);
            }
        }
    }
}
