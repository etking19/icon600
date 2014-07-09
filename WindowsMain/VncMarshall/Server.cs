using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VncMarshall
{
    public class Server
    {
        public enum SharingMode
        {
            ShareFull = 0,
            SharePrimary,
            ShareDisplay,
            ShareRect,
        }

        public enum SharingColor
        {
            Bits8 = 0,
            Bits16,
            Bits32,
        }

        public struct SharingAttributes
        {
            public SharingMode ShareMode { get; set; }

            /// <summary>
            /// Only use if ShareDisplay mode
            /// </summary>
            public int ShareMonitorNum { get; set; }

            /// <summary>
            /// Only use if ShareRect mode
            /// </summary>
            public int PosLeft { get; set; }
            public int PosTop { get; set; }
            public int PosRight { get; set; }
            public int PosBottom { get; set; }


        }

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

        public bool StartServer(Int32 portNumber, SharingAttributes attributes)
        {
            VncRegistryHelper.SetServerPort(portNumber);
            VncRegistryHelper.RemoveWallpaper(false);
            VncRegistryHelper.EnableMirrorDriver(true);

            //// run the application first
            Process procInitial = null;
            process.Arguments = "-run";
            using (procInitial = Process.Start(process))
            {
                processId = procInitial.Id;
            }

            switch (attributes.ShareMode)
            {
                case SharingMode.ShareFull:
                    process.Arguments = "-controlapp -sharefull";
                    break;
                case SharingMode.SharePrimary:
                    process.Arguments = "-controlapp -shareprimary";
                    break;
                case SharingMode.ShareDisplay:
                    process.Arguments = String.Format("-controlapp -sharedisplay {0}", attributes.ShareMonitorNum);
                    break;
                case SharingMode.ShareRect:
                    process.Arguments = String.Format("-controlapp -sharerect {0}x{1}+{2}+{3}",
                        attributes.PosRight - attributes.PosLeft,
                        attributes.PosBottom - attributes.PosTop,
                        attributes.PosLeft,
                        attributes.PosTop);
                    break;
            }

            try
            {
                using (Process proc = Process.Start(process))
                {
                    processId = proc.Id;
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
                process.Arguments = "-controlapp -shutdown";
                Process.Start(process);
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
