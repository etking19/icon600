using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Utils.Windows;

namespace Windows
{
    public class WindowsAppMgr
    {
        public delegate void OnApplicationWndChanged(List<WndAttributes> wndAttributes);
        public event OnApplicationWndChanged EvtApplicationWndChanged;

        private List<WndAttributes> _CurrentActiveWnds = new List<WndAttributes>();
        private MonitorWorker worker = new MonitorWorker();
        private Thread workerThread = null;

        public struct WndAttributes
        {
            public int id { get; set; }
            public string name { get; set; }

            public int posX { get; set; }
            public int posY { get; set; }
            public int width { get; set; }
            public int height { get; set; }

            public int style { get; set; }

            public uint processId { get; set; }
        }

        ~WindowsAppMgr()
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

            worker = new MonitorWorker();
            worker.EvtWndAttributes += new MonitorWorker.OnWndAttribute(MonitorWorker_onWndAttributes);

            workerThread = new Thread(worker.DoWork);
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
            workerThread = null;
        }

        public bool IsMonitoring()
        {
            return (workerThread != null && workerThread.IsAlive);
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
    }
}
