using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Utils.Windows;

namespace Windows
{
    public class MonitorWorker
    {
        public delegate void OnWndAttribute(List<Windows.WindowsAppMgr.WndAttributes> wndAttributes);
        public event OnWndAttribute EvtWndAttributes;

        private volatile bool _shouldStop = false;
        private List<Windows.WindowsAppMgr.WndAttributes> wndList = new List<Windows.WindowsAppMgr.WndAttributes>();
        
        public void DoWork()
        {
            while (!_shouldStop)
            {
                wndList.Clear();

                // get the visible applications
                var collection = new List<Windows.WindowsAppMgr.WndAttributes>();
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
                        collection.Add(new Windows.WindowsAppMgr.WndAttributes { id = hWnd.ToInt32(), name = strTitle, posX = wndRect.Left, posY = wndRect.Top, width = wndRect.Right - wndRect.Left, height = wndRect.Bottom - wndRect.Top, style = style });
                    }
                    return true;
                };

                if (NativeMethods.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
                {
                    foreach (var item in collection)
                    {
                        wndList.Add(item);
                    }
                }

                if (EvtWndAttributes != null)
                {
                    EvtWndAttributes(wndList);
                }

                Thread.Sleep(100);
            }
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }

        private bool IsAltTabWindow(IntPtr hwnd)
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
    }
}
