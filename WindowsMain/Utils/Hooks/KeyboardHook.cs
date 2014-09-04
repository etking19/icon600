using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Utils.Hooks
{
    public class KeyboardHook : BaseHook
    {
        public delegate void KeyboardHookEventHandler(object sender, KeyboardHookEventArgs data);
        public event KeyboardHookEventHandler HookInvoked;

        private HProc hookProc;
        private static int hHook = 0;

        public class KeyboardHookEventArgs : EventArgs
        {
            public Int32 code;
            public Int32 wParam;
            public KeyboardHookStruct lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardHookStruct
        {
            public UInt32 vkCode;
            public UInt32 scanCode;
            public UInt32 flags;
            public UInt32 time;
            public UIntPtr dwExtraInfo;
        }

        public override bool StartHook(int threadId)
        {
            // Create an instance of HookProc.
            hookProc = new HProc(KeyboardHookProc);

            hHook = SetWindowsHookEx((int)HookId.WH_KEYBOARD_LL, hookProc, IntPtr.Zero, threadId);
            return IsHooking();
        }

        public override bool StopHook()
        {
            if (UnhookWindowsHookEx(hHook))
            {
                hHook = 0;
            }

            return !IsHooking();
        }

        public override bool IsHooking()
        {
            return (hHook != 0);
        }

        public int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            
            if (nCode >= 0)
            {
                if (HookInvoked != null)
                {
                    KeyboardHookStruct messageStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                    KeyboardHookEventArgs eventArg = new KeyboardHookEventArgs
                    {
                        code = nCode,
                        wParam = wParam.ToInt32(),
                        lParam = messageStruct
                    };

                    HookInvoked.BeginInvoke(this, eventArg, null, null);
                }

                return 1;
            }

            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }
    }
}
