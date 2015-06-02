using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Utils.Windows;

namespace VncMarshall
{
    public class Client
    {
        private string _vncClientExePath;

        public Client(string vncClientExePath)
        {
            // Prepare the process to run
            _vncClientExePath = vncClientExePath;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int StartClient(string vncServerIp, int vncServerPort)
        {
            int appIdentifier = 0;
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(_vncClientExePath);

                processInfo.Arguments = String.Format("-connect {0}::{1} -notoolbar -nostatus", vncServerIp, vncServerPort);
                //processInfo.Arguments = String.Format("-viewonly=yes -mouselocal=normal -scale=auto {0}::{1}", vncServerIp, vncServerPort);
                var previous = WindowsHelper.GetRunningApplicationInfo();
                using (Process clientProcess = Process.Start(processInfo))
                {
                    clientProcess.WaitForInputIdle(3000);

                    // this is assuming the program created a new window
                    int max_tries = 30;
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

                    if (diff.Count() > 0)
                    {
                        Trace.WriteLine("VNC identifier: " + diff.ElementAt(0).id);
                        appIdentifier = diff.ElementAt(0).id;

                        //int style = NativeMethods.GetWindowLong(new IntPtr(appIdentifier), Constant.GWL_STYLE);
                        //style &= ~(int)(Constant.WS_BORDER | Constant.WS_CAPTION | Constant.WS_THICKFRAME);
                        //NativeMethods.SetWindowLong(new IntPtr(appIdentifier), Constant.GWL_STYLE, style);
                        //NativeMethods.SetWindowPos(new IntPtr(appIdentifier), 0, 
                        //    0, 0, 0, 0, 
                        //    (int)(Constant.SWP_NOSIZE | Constant.SWP_NOMOVE | Constant.SWP_NOZORDER | Constant.SWP_NOACTIVATE | Constant.SWP_FRAMECHANGED));
                    }
                    else
                    {
                        Trace.WriteLine("VNC identifier not found!");
                    }
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return appIdentifier;
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

        public int StartClient(string vncServerIp, int vncServerPort, int left, int top, int width, int height)
        {
            int result = StartClient(vncServerIp, vncServerPort);

            // add delay - the VNC will resize itself after receive content from peer site, this to make sure VNC resized first then we set the desire size.
            Thread.Sleep(1500);

            // set to desired location
            NativeMethods.MoveWindow(new IntPtr(result),
                            left,
                            top,
                            width,
                            height,
                            true);

            return result;
        }

        public void StopClient(int processId)
        {
            try
            {
                Process.GetProcessById(processId).Kill();
                processId = 0;
            }
            catch (Exception)
            {
            }
        }

        public int GetRunningClientsCount()
        {
            int count = 0;
            foreach (Process innerProcess in Process.GetProcessesByName("tvnviewer"))
            {
                count++;
            }

            return count;
        }

        public void StopAllClients()
        {
            foreach (Process innerProcess in Process.GetProcessesByName("tvnviewer"))
            {
                innerProcess.Kill();
            }
        }
    }
}
