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
using System.Management;
using System.IO;
using System.Runtime.CompilerServices;

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

        private int getProcessFromExePath(string path)
        {
            var wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process";
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            {
                var query = from p in Process.GetProcesses()
                            join mo in results.Cast<ManagementObject>()
                            on p.Id equals (int)(uint)mo["ProcessId"]
                            select new
                            {
                                Process = p,
                                Path = (string)mo["ExecutablePath"],
                            };
                foreach (var item in query)
                {
                    try 
                    {
                        if (item.Path.CompareTo(path) == 0)
                        {
                            return item.Process.MainWindowHandle.ToInt32();
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int LaunchApplication(ApplicationData appData)
        {
            int appIdentifier = 0;

            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = appData.applicationPath,
                Arguments = appData.arguments,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = false,
                UseShellExecute = false,
            };

            try
            {
                var previous = WindowsHelper.GetRunningApplicationInfo();
                using (Process process = Process.Start(info))
                {
                    process.WaitForInputIdle(3000);

                    // this is assuming the program created a new window
                    int max_tries = 5;
                    var current = WindowsHelper.GetRunningApplicationInfo();
                    var diff = current.Except(previous, new ProcessComparer());
                    while (diff.Count() == 0)
                    {
                        if (max_tries <= 0)
                        {
                            break;
                        }
                        max_tries--;
                        Thread.Sleep(100);
                        current = WindowsHelper.GetRunningApplicationInfo();
                        diff = current.Except(previous, new ProcessComparer());
                    }

                    // this cater when no new window formed
                    if (diff.Count() == 0)
                    {
                        appIdentifier = getProcessFromExePath(appData.applicationPath);
                    }
                    else
                    {
                        appIdentifier = diff.ElementAt(0).id;
                    }
                    
                    Trace.WriteLine("Launched process identifier: " + appIdentifier);
                    //appIdentifier = startProcess.MainWindowHandle.ToInt32();

                    if (appData.rect.Left != 0 ||
                        appData.rect.Top != 0 ||
                        appData.rect.Right != 0 ||
                        appData.rect.Bottom != 0)
                    {
                        NativeMethods.SetWindowPos(new IntPtr(appIdentifier),
                            0,
                            appData.rect.Left,
                            appData.rect.Top,
                            appData.rect.Right - appData.rect.Left,
                            appData.rect.Bottom - appData.rect.Top,
                            0);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("trigger application: " + e);
            }

            return appIdentifier;
        }

        void previousProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Trace.WriteLine(e.Data);
        }

    }

    class ProcessComparer : EqualityComparer<Utils.Windows.WindowsHelper.ApplicationInfo>
    {
        public override bool Equals(Utils.Windows.WindowsHelper.ApplicationInfo wnd1, Utils.Windows.WindowsHelper.ApplicationInfo wnd2)
        {
            return (wnd1.id == wnd2.id);
        }

        public override int GetHashCode(Utils.Windows.WindowsHelper.ApplicationInfo obj)
        {
            return obj.GetHashCode();
        }
    }
}


