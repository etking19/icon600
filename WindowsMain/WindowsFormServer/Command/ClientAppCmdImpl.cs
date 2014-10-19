using Session.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
                int tryMax = 1000;
                while ((process.MainWindowHandle == IntPtr.Zero) || !NativeMethods.IsWindowVisible(process.MainWindowHandle))
                {
                    System.Threading.Thread.Sleep(10);
                    process.Refresh();
                    if (tryMax-- <= 0)
                    {
                        break;
                    }
                }
                process.WaitForInputIdle(1000);
                NativeMethods.MoveWindow(process.MainWindowHandle,
                        appData.rect.Left,
                        appData.rect.Top,
                        appData.rect.Right - appData.rect.Left,
                        appData.rect.Bottom - appData.rect.Top,
                        true);

                // save to user list
                Server.ConnectedClientHelper.GetInstance().AddLaunchedApp(userId, process.MainWindowHandle.ToInt32(), appData.id);
            }
        }
    }
}
