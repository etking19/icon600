using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Utils.Windows;

namespace VncMarshall
{
    public class Client
    {
        private ProcessStartInfo process;

        public Client(string vncClientExePath)
        {
            // Prepare the process to run
            process = new ProcessStartInfo();
            process.FileName = vncClientExePath;
        }

        public int StartClient(string vncServerIp, int vncServerPort)
        {
            try
            {
                process.Arguments = String.Format("-viewonly=yes -mouselocal=normal -scale=auto {0}::{1}", vncServerIp, vncServerPort);
                using(Process clientProcess = Process.Start(process))
                {
                    int tryMax = 1000;
                    while ((clientProcess.MainWindowHandle == IntPtr.Zero) || !NativeMethods.IsWindowVisible(clientProcess.MainWindowHandle))
                    {
                        System.Threading.Thread.Sleep(10);
                        clientProcess.Refresh();
                        if (tryMax-- <= 0)
                        {
                            break;
                        }
                    }
                    clientProcess.WaitForInputIdle(1000);
                    return clientProcess.MainWindowHandle.ToInt32();
                }
            }
            catch(Exception)
            {
            }

            return -1;
        }

        public int StartClient(string vncServerIp, int vncServerPort, int left, int top, int width, int height)
        {
            int result = StartClient(vncServerIp, vncServerPort);

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
