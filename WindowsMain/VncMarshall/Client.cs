using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VncMarshall
{
    public class Client
    {
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

        public bool StartClient(string vncServerIp, int vncServerPort)
        {
            try
            {
                process.Arguments = String.Format("{0}::{1}", vncServerIp, vncServerPort);
                using (Process proc = Process.Start(process))
                {
                    processId = proc.Id;
                }
            }
            catch (Exception)
            {
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
