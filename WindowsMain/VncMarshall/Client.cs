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
                process.Arguments = String.Format("-viewonly=yes -mouselocal=normal -showcontrols=no -scale=auto {0}::{1}", vncServerIp, vncServerPort);
                using(Process clientProcess = Process.Start(process))
                {
                    return clientProcess.Id;
                }
            }
            catch(Exception)
            {
            }

            return -1;
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
