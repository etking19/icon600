using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Utils.Hooks
{
    public class MouseHook : BaseHook
    {
        public delegate void MouseHookEventHandler(object sender, MouseHookEventArgs arg);
        public event MouseHookEventHandler HookInvoked;

        private HProc hookProc;

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseHookStruct
        {
            public Point pt;
            public UInt32 mouseData;
            public UInt32 flags;
            public UInt32 time;
            public UIntPtr dwExtraInfo;
        }

        public class MouseHookEventArgs : EventArgs
        {
            public Int32 code;
            public IntPtr wParam;
            public MouseHookStruct lParam;
        }

        public override bool StartHook(int threadId)
        {
            // Create an instance of HookProc.
            hookProc = new HProc(MouseHookProc);

            hHook = SetWindowsHookEx((int)HookId.WH_MOUSE_LL, hookProc, IntPtr.Zero, threadId);
            return IsHooking();
        }

        public override bool StopHook()
        {
            if(UnhookWindowsHookEx(hHook))
            {
                hHook = 0;
            }

            return !IsHooking();
        }

        public override bool IsHooking()
        {
            return (hHook != 0);
        }

        public int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            MouseHookStruct messageStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

            if (nCode < 0)
            {
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                if (HookInvoked != null)
                {
                    MouseHookEventArgs eventArg = new MouseHookEventArgs
                    {
                        code = nCode,
                        wParam = wParam,
                        lParam = messageStruct
                    };

                    HookInvoked.BeginInvoke(this, eventArg, null, null);
                }

                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }
    }
}
