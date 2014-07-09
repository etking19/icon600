using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace VncMarshall
{
    public class Client
    {
        public struct WindowsAttributes
        {
            public int PosX { get; set; }
            public int PosY { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        private ProcessStartInfo process;
        private int processId = 0;

        public Client()
        {
            // Prepare the process to run
            process = new ProcessStartInfo();
            process.FileName = "tvnviewer.exe";
            process.WindowStyle = ProcessWindowStyle.Normal;
            process.CreateNoWindow = true;
        }

        public bool StartClient(string vncServerIp, int vncServerPort, WindowsAttributes windowsAttr)
        {
            try
            {
                process.Arguments = String.Format("-viewonly=yes -mouselocal=normal -showcontrols=no -scale=auto {0}::{1}", vncServerIp, vncServerPort);
                using (Process proc = Process.Start(process))
                {
                    processId = proc.Id;

                    // modify the client viewer window
                    // class name getting from main.cpp file, when creating TvnViewer class
                    IntPtr hwnd = Utils.Windows.NativeMethods.FindWindow("TvnWindowClass", null);

                    int retryCount = 10;
                    while ((hwnd == IntPtr.Zero) && (retryCount >0))
                    {
                        // waiting to deploy the window
                        Thread.Sleep(1000);

                        hwnd = Utils.Windows.NativeMethods.FindWindow("TvnWindowClass", null);
                        retryCount--;
                    }

                    if (hwnd != IntPtr.Zero)
                    {
                        Thread.Sleep(1000);
                        Trace.WriteLine("modify windows");
                        Utils.Windows.NativeMethods.MoveWindow(hwnd, windowsAttr.PosX, windowsAttr.PosY, windowsAttr.Width, windowsAttr.Height, true);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public bool StopClient()
        {
            if (processId == 0)
            {
                ForceClose();
                return true;
            }

            try
            {
                Process.GetProcessById(processId).Kill();
                processId = 0;
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        private void ForceClose()
        {
            foreach (Process innerProcess in Process.GetProcessesByName("tvnclient"))
            {
                innerProcess.Kill();
            }
        }
    }
}
