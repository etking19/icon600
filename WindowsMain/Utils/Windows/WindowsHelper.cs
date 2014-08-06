using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Utils.Windows
{
    public class WindowsHelper
    {
        public struct ApplicationInfo
        {
            public int id { get; set; }
            public string name { get; set; }

            public int posX { get; set; }
            public int posY { get; set; }
            public int width { get; set; }
            public int height { get; set; }

            public int style { get; set; }
        }

        public struct MonitorInfo
        {
            public string Availability { get; set; }
            public int ScreenHeight { get; set; }
            public int ScreenWidth { get; set; }
            public Utils.Windows.NativeMethods.Rect MonitorArea { get; set; }
            public Utils.Windows.NativeMethods.Rect WorkArea { get; set; }
        }

        public static IList<ApplicationInfo> GetRunningApplicationInfo()
        {
            // get the visible applications
            var collection = new List<ApplicationInfo>();
            NativeMethods.EnumDelegate filter = delegate(IntPtr hWnd, int lParam)
            {
                if (IsAltTabWindow(hWnd) == false)
                {
                    return true;
                }

                StringBuilder strbTitle = new StringBuilder(255);
                int nLength = NativeMethods.GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                string strTitle = strbTitle.ToString();

                if (string.IsNullOrEmpty(strTitle) == false)
                {
                    NativeMethods.Rect wndRect = new NativeMethods.Rect();
                    NativeMethods.GetWindowRect(hWnd, ref wndRect);

                    int style = NativeMethods.GetWindowLong(hWnd, Constant.GWL_STYLE);
                    collection.Add(new ApplicationInfo { id = hWnd.ToInt32(), name = strTitle, posX = wndRect.Left, posY = wndRect.Top, width = wndRect.Right - wndRect.Left, height = wndRect.Bottom - wndRect.Top, style = style });
                }
                return true;
            };

            if (NativeMethods.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
            {
                Trace.WriteLine("enum desktop failed");
                return collection.AsReadOnly();
            }

            return collection.AsReadOnly();
        }

        public static bool IsAltTabWindow(IntPtr hwnd)
        {
            NativeMethods.TITLEBARINFO ti = new NativeMethods.TITLEBARINFO();
            IntPtr hwndTry, hwndWalk = IntPtr.Zero;

            if (!NativeMethods.IsWindowVisible(hwnd))
                return false;

            hwndTry = NativeMethods.GetAncestor(hwnd, Constant.GA_ROOTOWNER);
            while (hwndTry != hwndWalk)
            {
                hwndWalk = hwndTry;
                hwndTry = NativeMethods.GetLastActivePopup(hwndWalk);
                if (NativeMethods.IsWindowVisible(hwndTry))
                    break;
            }
            if (hwndWalk != hwnd)
                return false;

            // the following removes some task tray programs and "Program Manager"
            NativeMethods.GetTitleBarInfo(hwnd, ref ti);
            if ((ti.rgstate[0] & Constant.STATE_SYSTEM_INVISIBLE) != 0)
                return false;

            // Tool windows should not be displayed either, these do not appear in the
            // task bar.
            if ((NativeMethods.GetWindowLong(hwnd, Constant.GWL_EXSTYLE) & Constant.WS_EX_TOOLWINDOW) != 0)
                return false;

            return true;
        }

        public static IList<MonitorInfo> GetMonitorList()
        {
            List<MonitorInfo> col = new List<MonitorInfo>();
            NativeMethods.MonitorEnumProc testDelegate = new NativeMethods.MonitorEnumProc(
                delegate(IntPtr hMonitor, IntPtr hdcMonitor, ref NativeMethods.Rect lprcMonitor, IntPtr dwData)
                {
                    NativeMethods.MonitorInfo mi = new NativeMethods.MonitorInfo();
                    mi.size = (uint)Marshal.SizeOf(mi);
                    bool success = NativeMethods.GetMonitorInfo(hMonitor, ref mi);
                    if (success)
                    {
                        MonitorInfo di = new MonitorInfo();
                        di.ScreenWidth = (mi.monitor.Right - mi.monitor.Left);
                        di.ScreenHeight = (mi.monitor.Bottom - mi.monitor.Top);
                        di.MonitorArea = mi.monitor;
                        di.WorkArea = mi.work;
                        di.Availability = mi.flags.ToString();
                        col.Add(di);
                    }
                    return true;
                });

            NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, testDelegate, IntPtr.Zero);
            return col.AsReadOnly();
        }
    }
}
