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

            int result = LaunchApplication(appData);
            Trace.WriteLine("Launched application id: " + result);
            //Server.ConnectedClientHelper.GetInstance().AddLaunchedApp(userId, process.MainWindowHandle.ToInt32(), appData.id);
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
                    Trace.WriteLine(String.Format("Launched application: {0}, pos: {1},{2},{3},{4}",
                        appData.name, appData.rect.Left, appData.rect.Top,
                                appData.rect.Right - appData.rect.Left,
                                appData.rect.Bottom - appData.rect.Top));
                    if (appData.rect.Left != 0 ||
                            appData.rect.Top != 0 ||
                            appData.rect.Right != 0 ||
                            appData.rect.Bottom != 0)
                    {
                        // wait the process to stable
                        //if (process.WaitForInputIdle())
                        {
                            Thread.Sleep(800);
                            appIdentifier = Utils.Windows.NativeMethods.GetForegroundWindow().ToInt32();
                            NativeMethods.MoveWindow(new IntPtr(appIdentifier),
                                appData.rect.Left,
                                appData.rect.Top,
                                appData.rect.Right - appData.rect.Left,
                                appData.rect.Bottom - appData.rect.Top,
                                true);
                        }

                        //int tryMax = 100;
                        //while ((process.MainWindowHandle == IntPtr.Zero))
                        //{
                        //    System.Threading.Thread.Sleep(10);
                        //    process.Refresh();
                        //    if (tryMax-- <= 0)
                        //    {
                        //        break;
                        //    }
                        //}
                        //appIdentifier = process.MainWindowHandle;

                        //while (!NativeMethods.IsWindowVisible(appIdentifier))
                        //{
                        //    System.Threading.Thread.Sleep(10);
                        //    process.Refresh();
                        //    if (tryMax-- <= 0)
                        //    {
                        //        break;
                        //    }
                        //}
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
