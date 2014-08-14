using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Windows;

namespace WindowsFormClient.Command
{
    class ClientMaintenanceCmdImpl : BaseImplementer
    {
        public override void ExecuteCommand(string userId, string command)
        {
            ClientMaintenanceCmd maintenaceCmd = deserialize.Deserialize<ClientMaintenanceCmd>(command);
            if (maintenaceCmd == null)
            {
                return;
            }

            switch (maintenaceCmd.CommandType)
            {
                case ClientMaintenanceCmd.CommandId.EShutdown:
                    DoExitWindow(Constant.EWX_SHUTDOWN);
                    break;
                case ClientMaintenanceCmd.CommandId.EReboot:
                    DoExitWindow(Constant.EWX_REBOOT);
                    break;
                case ClientMaintenanceCmd.CommandId.ELogOff:
                    DoExitWindow(Constant.EWX_LOGOFF);
                    break;
                default:
                    break;
            }
        }

        private bool DoExitWindow(int exitCode)
        {
            bool result;
            Utils.Windows.NativeMethods.TokPriv1Luid tp;
            IntPtr hproc = Utils.Windows.NativeMethods.GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            result = Utils.Windows.NativeMethods.OpenProcessToken(hproc, Constant.TOKEN_ADJUST_PRIVILEGES | Constant.TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = Constant.SE_PRIVILEGE_ENABLED;
            result &= Utils.Windows.NativeMethods.LookupPrivilegeValue(null, Constant.SE_SHUTDOWN_NAME, ref tp.Luid);
            result &= Utils.Windows.NativeMethods.AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            result &= Utils.Windows.NativeMethods.ExitWindowsEx(exitCode, 0);

            return result;
        }
    }
}
