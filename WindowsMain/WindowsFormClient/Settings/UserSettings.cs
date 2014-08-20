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
