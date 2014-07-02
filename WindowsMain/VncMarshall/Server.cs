using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VncMarshall
{
    public class Server
    {
        private ProcessStartInfo process;
        private int processId = 0;

        public Server()
        {
            // Prepare the process to run
            process = new ProcessStartInfo();
            process.FileName = "tvnserver.exe";
            process.WindowStyle = ProcessWindowStyle.Hidden;
            process.CreateNoWindow = true;
        }

        public bool StartServer(Int32 portNumber)
        {
            try
            {
                using (Process proc = Process.Start(process))
                {
                    processId = proc.Id;

                    ProcessStartInfo process2 = new ProcessStartInfo();
                    process2.FileName = "tvnserver.exe";
                    process2.Arguments = "-controlapp";
                    process2.WindowStyle = ProcessWindowStyle.Hidden;
                    process2.CreateNoWindow = true;
                }
            }
            catch(Exception)
            {
                return false;
            }
            
            return true;
        }

        public bool StopServer()
        {
            if (processId == 0)
            {
                forceClose();
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

        private void forceClose()
        {
            foreach (Process innerProcess in Process.GetProcessesByName("tvnserver"))
            {
                innerProcess.Kill();
            }
        }
    }
}
