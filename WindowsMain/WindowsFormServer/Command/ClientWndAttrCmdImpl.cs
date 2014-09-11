using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Windows;

namespace WindowsFormClient.Command
{
    class ClientWndAttrCmdImpl : BaseImplementer
    {
        public override void ExecuteCommand(string userId, string command)
        {
            ClientWndCmd data = deserialize.Deserialize<ClientWndCmd>(command);
            if (data == null)
            {
                return;
            }

            switch (data.CommandType)
            {
                case ClientWndCmd.CommandId.EMinimize:
                    NativeMethods.ShowWindow(new IntPtr(data.Id), Constant.SW_SHOWMINIMIZED);
                    break;
                case ClientWndCmd.CommandId.EMaximize:
                    NativeMethods.ShowWindow(new IntPtr(data.Id), Constant.SW_SHOWMAXIMIZED);
                    NativeMethods.SetForegroundWindow(new IntPtr(data.Id));
                    break;
                case ClientWndCmd.CommandId.ERelocation:
                    NativeMethods.SetWindowPos(new IntPtr(data.Id), Constant.HWND_TOP, data.PositionX, data.PositionY, 0, 0, (Int32)(Constant.SWP_NOSIZE));
                    break;
                case ClientWndCmd.CommandId.EResize:
                    NativeMethods.SetWindowPos(new IntPtr(data.Id), Constant.HWND_TOP, 0, 0, data.Width, data.Height, (Int32)Constant.SWP_NOMOVE);
                    break;
                case ClientWndCmd.CommandId.ERestore:
                    NativeMethods.ShowWindow(new IntPtr(data.Id), Constant.SW_SHOWNORMAL);
                    break;
                case ClientWndCmd.CommandId.EClose:
                    NativeMethods.SendMessage(new IntPtr(data.Id), Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    break;
                case ClientWndCmd.CommandId.ESetForeground:
                    NativeMethods.SetWindowPos(new IntPtr(data.Id), Constant.HWND_TOPMOST, 0, 0, 0, 0, (Int32)(Constant.SWP_NOMOVE | Constant.SWP_NOSIZE | Constant.SWP_SHOWWINDOW));
                    NativeMethods.SetWindowPos(new IntPtr(data.Id), Constant.HWND_NOTOPMOST, 0, 0, 0, 0, (Int32)(Constant.SWP_NOMOVE | Constant.SWP_NOSIZE | Constant.SWP_SHOWWINDOW));
                    break;
                default:
                    break;
            }
        }
    }
}
