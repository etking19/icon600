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
        public delegate void OnWndAttribute(List<Windows.WindowsMgr.WndAttributes> wndAttributes);
        public event OnWndAttribute EvtWndAttributes;

        private volatile bool _shouldStop;
        private List<Windows.WindowsMgr.WndAttributes> wndList = new List<Windows.WindowsMgr.WndAttributes>();
        
        public void DoWork()
        {
            while (!_shouldStop)
            {
                wndList.Clear();

                // get the visible applications
                var collection = new List<Windows.WindowsMgr.WndAttributes>();
                User32.EnumDelegate filter = delegate(IntPtr hWnd, int lParam)
                {
                    if (IsAltTabWindow(hWnd) == false)
                    {
                        return true;
                    }

                    StringBuilder strbTitle = new StringBuilder(255);
                    int nLength = User32.GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                    string strTitle = strbTitle.ToString();

                    if (string.IsNullOrEmpty(strTitle) == false)
                    {
                        User32.Rect wndRect = new User32.Rect();
                        User32.GetWindowRect(hWnd, ref wndRect);

                        int style = User32.GetWindowLong(hWnd, Constant.GWL_STYLE);
                        collection.Add(new Windows.WindowsMgr.WndAttributes { id = hWnd.ToInt32(), name = strTitle, posX = wndRect.Left, posY = wndRect.Top, width = wndRect.Right - wndRect.Left, height = wndRect.Bottom - wndRect.Top, style = style });
                    }
                    return true;
                };

                if (User32.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
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
            User32.TITLEBARINFO ti = new User32.TITLEBARINFO();
            IntPtr hwndTry, hwndWalk = IntPtr.Zero;

            if (!User32.IsWindowVisible(hwnd))
                return false;

            hwndTry = User32.GetAncestor(hwnd, Constant.GA_ROOTOWNER);
            while (hwndTry != hwndWalk)
            {
                hwndWalk = hwndTry;
                hwndTry = User32.GetLastActivePopup(hwndWalk);
                if (User32.IsWindowVisible(hwndTry))
                    break;
            }
            if (hwndWalk != hwnd)
                return false;

            // the following removes some task tray programs and "Program Manager"
            User32.GetTitleBarInfo(hwnd, ref ti);
            if ((ti.rgstate[0] & Constant.STATE_SYSTEM_INVISIBLE) != 0)
                return false;

            // Tool windows should not be displayed either, these do not appear in the
            // task bar.
            if ((User32.GetWindowLong(hwnd, Constant.GWL_EXSTYLE) & Constant.WS_EX_TOOLWINDOW) != 0)
                return false;

            return true;
        }
    }
}
