using Session.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Utils.Windows;
using WcfServiceLibrary1;
using WindowsFormClient.Server;

namespace WindowsFormClient.Command
{
    class ClientAppCmdImpl : BaseImplementer
    {
        public ClientAppCmdImpl()
        {

        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientApplicationCmd clientAppData = deserialize.Deserialize<ClientApplicationCmd>(command);
            if (clientAppData == null)
            {
                return;
            }

            ApplicationData appData = Server.ServerDbHelper.GetInstance().GetAllApplications().First(AppData 
                => AppData.id == clientAppData.ApplicationEntry.Identifier);

            if (appData == null)
            {
                Trace.WriteLine("unable to find matched application id: " + clientAppData.ApplicationEntry.Identifier);
                return;
            }

            int result = LaunchApplication(appData);
            //Server.ConnectedClientHelper.GetInstance().AddLaunchedApp(userId, result, appData.id);
            int userDBid = ConnectedClientHelper.GetInstance().GetClientInfo(userId).DbUserId;
            Server.LaunchedWndHelper.GetInstance().AddLaunchedApp(userDBid, result, appData.id);
        }

        public int LaunchApplication(ApplicationData appData)
        {
            int appIdentifier = -1;
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = appData.applicationPath,
                Arguments = appData.arguments
            };

            try
            {
                using (Process process = Process.Start(info))
                {
                    Thread.Sleep(800);
                    appIdentifier = Utils.Windows.NativeMethods.GetForegroundWindow().ToInt32();

                    if (appData.rect.Left != 0 ||
                            appData.rect.Top != 0 ||
                            appData.rect.Right != 0 ||
                            appData.rect.Bottom != 0)
                    {
                        NativeMethods.MoveWindow(new IntPtr(appIdentifier),
                            appData.rect.Left,
                            appData.rect.Top,
                            appData.rect.Right - appData.rect.Left,
                            appData.rect.Bottom - appData.rect.Top,
                            true);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("trigger application: " + e);
            }

            return appIdentifier;
        }

    }
}
