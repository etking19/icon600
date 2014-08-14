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

        public Server(string serverExePath)
        {
            // Prepare the process to run
            process = new ProcessStartInfo();
            process.FileName = serverExePath;
            process.WindowStyle = ProcessWindowStyle.Hidden;
            process.UseShellExecute = false;
            process.CreateNoWindow = true;
        }

        public void StartVncServer()
        {
            if (isVncServerStarted())
            {
                return;
            }

            try
            {
                // run as service
                process.Arguments = "-start";
                Process.Start(process);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            
        }

        public void StopVncServer()
        {
            if (isVncServerStarted() == false)
            {
                return;
            }

            try
            {
                process.Arguments = "-controlservice -disconnectall";
                Process.Start(process);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            
        }

        public bool isVncServerStarted()
        {
            foreach (Process innerProcess in Process.GetProcessesByName("tvnserver"))
            {
                return true;
            }

            return false;
        }

        public void refreshVncServer()
        {
            if (isVncServerStarted() == false)
            {
                return;
            }

            try
            {
                process.Arguments = "-controlservice -reload";
                Process.Start(process);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            
        }
    }
}
