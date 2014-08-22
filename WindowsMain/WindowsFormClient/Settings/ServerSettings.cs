using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Settings
{
    class ServerSettings
    {
        private static ServerSettings sInstance;

        public int DesktopRow { get; set; }
        public int DesktopColumn { get; set; }

        public int DesktopLeft { get; set; }
        public int DesktopTop { get; set; }
        public int DesktopWidth { get; set; }
        public int DesktopHeight { get; set; }

        public int ViewingAreaLeft { get; set; }
        public int ViewingAreaTop { get; set; }
        public int ViewingAreaWidth { get; set; }
        public int ViewingAreaHeight { get; set; }


        private ServerSettings()
        {

        }

        public static ServerSettings GetInstance()
        {
            if(sInstance == null)
            {
                sInstance = new ServerSettings();
            }

            return sInstance;
        }
    }
}
