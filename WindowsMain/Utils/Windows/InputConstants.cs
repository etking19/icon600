﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Utils.Windows
{
    public class InputConstants
    {
        // Input types
        public const int MOUSE = 0;
        public const int KEYBOARD = 1;
        public const int HARDWARE = 2;

        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;
        public const uint WM_SYSKEYDOWN = 0x0104;
        public const uint WM_SYSKEYUP = 0x0105;

        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_UNICODE = 0x0004;
        public const uint KEYEVENTF_SCANCODE = 0x0008;

        public const uint XBUTTON1 = 0x0001;
        public const uint XBUTTON2 = 0x0002;

        public const uint MOUSEEVENTF_MOVE = 0x0001;
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const uint MOUSEEVENTF_XDOWN = 0x0080;
        public const uint MOUSEEVENTF_XUP = 0x0100;
        public const uint MOUSEEVENTF_WHEEL = 0x0800;
        public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
        public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;
        public const uint WM_MOUSEMOVE = 0x0200;
        public const uint WM_MOUSEWHEEL = 0x020A;
        public const uint WM_MOUSEHWHEEL = 0x020E;
        public const uint WM_RBUTTONDOWN = 0x0204;
        public const uint WM_RBUTTONUP = 0x0205;

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public Int32 dx;
            public Int32 dy;
            public UInt32 mouseData;
            public UInt32 dwFlags;
            public UInt32 time;
            public UIntPtr dwExtraInfo;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBOARDINPUT
        {
            public UInt16 wVk;
            public UInt16 wScan;
            public UInt32 dwFlags;
            public UInt32 time;
            public IntPtr dwExtraInfo;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public UInt32 uMsg;
            public UInt16 wParamL;
            public UInt16 wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public int type;

            [FieldOffset(4)]
            public MOUSEINPUT mi;

            [FieldOffset(4)]
            public KEYBOARDINPUT ki;

            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }
    }
}
