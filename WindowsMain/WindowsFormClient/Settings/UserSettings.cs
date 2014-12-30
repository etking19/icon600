using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Settings
{
    class UserSettings
    {
        private static UserSettings sInstance;

        public int UserId {get;set;}
        public string DisplayName { get; set; }
        public bool AllowMaintenance { get; set; }
        public bool AllowRemoteControl { get; set; }

        public int GridX { get; set; }
        public int GridY { get; set; }
        public bool ApplySnap { get; set; }
        
        private UserSettings()
        {

        }

        public static UserSettings GetInstance()
        {
            if(sInstance == null)
            {
                sInstance = new UserSettings();
            }
            return sInstance;
        }
    }
}
