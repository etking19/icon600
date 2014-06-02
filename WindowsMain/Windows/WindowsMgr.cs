using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Utils.Windows;

namespace Windows
{
    public class WindowsMgr
    {
        public delegate void OnApplicationWndChanged(List<WndAttributes> wndAttributes);
        public event OnApplicationWndChanged EvtApplicationWndChanged;

        private List<WndAttributes> _CurrentActiveWnds = new List<WndAttributes>();
        private MonitorWorker worker = new MonitorWorker();
        private Thread workerThread;

        public struct WndAttributes
        {
            public int id { get; set; }
            public string name { get; set; }

            public int posX { get; set; }
            public int posY { get; set; }
            public int width { get; set; }
            public int height { get; set; }

            public int style { get; set; }
        }

        public class DisplayInfo
        {
            public string Availability { get; set; }
            public int ScreenHeight { get; set; }
            public int ScreenWidth { get; set; }
            public Utils.Windows.User32.Rect MonitorArea { get; set; }
            public Utils.Windows.User32.Rect WorkArea { get; set; }
        }

        public WindowsMgr()
        {
            worker = new MonitorWorker();
            workerThread = new Thread(worker.DoWork);

            worker.EvtWndAttributes += new MonitorWorker.OnWndAttribute(MonitorWorker_onWndAttributes);
        }

        ~WindowsMgr()
        {
            if (IsMonitoring())
            {
                StopMonitor();
            }
        }

        public void StartMonitor()
        {
            if (IsMonitoring())
            {
                return;
            }

            workerThread.Start();
            while (!workerThread.IsAlive) ;
        }

        public void StopMonitor()
        {
            if(IsMonitoring() == false)
            {
                return;
            }
            worker.RequestStop();
            workerThread.Join();
        }

        public bool IsMonitoring()
        {
            return workerThread.IsAlive;
        }

        public List<WndAttributes> getAllVisibleApps()
        {
            return _CurrentActiveWnds;
        }

        void MonitorWorker_onWndAttributes(List<WndAttributes> appList)
        {
            _CurrentActiveWnds.Clear();
            _CurrentActiveWnds.AddRange(appList);

            if (EvtApplicationWndChanged != null)
            {
                EvtApplicationWndChanged(appList);
            }
        }

        public List<DisplayInfo> GetScreens()
        {
            List<DisplayInfo> col = new List<DisplayInfo>();
            User32.MonitorEnumProc testDelegate = new User32.MonitorEnumProc(
                delegate(IntPtr hMonitor, IntPtr hdcMonitor, ref User32.Rect lprcMonitor, IntPtr dwData)
                {
                    User32.MonitorInfo mi = new User32.MonitorInfo();
                    mi.size = (uint)Marshal.SizeOf(mi);
                    bool success = User32.GetMonitorInfo(hMonitor, ref mi);
                    if (success)
                    {
                        DisplayInfo di = new DisplayInfo();
                        di.ScreenWidth = (mi.monitor.Right - mi.monitor.Left);
                        di.ScreenHeight = (mi.monitor.Bottom - mi.monitor.Top);
                        di.MonitorArea = mi.monitor;
                        di.WorkArea = mi.work;
                        di.Availability = mi.flags.ToString();
                        col.Add(di);
                    }
                    return true;
                });
            User32.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, testDelegate, IntPtr.Zero);
            return col;
        }

    }
}
