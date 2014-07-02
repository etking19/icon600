using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class CommandConst
    {
        public enum MainCommandServer 
        { 
            ServerWindowsInfo = 1,
            ServerMonitorsInfo = 2,
        }

        public enum MainCommandClient
        {
            ClientLoginInfo = 1000,
            ClientControlInfo = 1001,
            ClientVncInfo = 1002,
        }

        public enum SubCmdServerWindowsInfo
        {
            WindowsList = 0,
        }

        public enum SubCmdServerMonitorsInfo
        {
            MonitorList = 0,
        }

        public enum SubCmdClientLoginInfo
        {
            LoginInfo = 0,
        }

        public enum SubCmdClientControlInfo
        {
            WindowsAttributes = 0,
            Mouse,
            Keyboard,
            Maintenance,
        }

        public enum SubCmdVncInfo
        {
            Start = 0,
            Stop,
        }
    }
}
