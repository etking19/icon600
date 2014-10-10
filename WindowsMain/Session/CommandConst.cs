using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class CommandConst
    {
        /// <summary>
        /// used by Server send to Client
        /// </summary>
        public enum MainCommandServer 
        { 
            WindowsInfo = 1,
            ScreenInfo = 2,
            UserPriviledge = 3,
            Presents = 4,
        }

        public enum SubCommandServer
        {
            // WindowsInfo
            WindowsList = 1,

            // ScreenInfo
            DisplayInfo = 100,

            // UserPriviledge
            ApplicationList = 201,
            Maintenance = 202,
            ViewingArea = 203,
            
            // User Presents
            PresetList = 300,
            VncList = 301,
            VisionInput = 302,
        }

        /// <summary>
        /// use by Client to send to Server
        /// </summary>
        public enum MainCommandClient
        {
            LoginInfo = 1000,
            ControlInfo = 1001,
            Functionality = 1002,
        }

        public enum SubCommandClient
        {
            // LoginInfo
            Credential = 1,

            // ControlInfo
            WindowsAttributes = 100,
            Mouse = 101,
            Keyboard = 102,

            // Functionality
            Vnc = 200,
            Preset = 201,
            Maintenance = 202,
            MessageBox = 203,
            Application = 204,
            VisionInput = 205,
        }
    }
}
