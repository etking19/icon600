using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Settings
{
    class ApplicationSettings
    {
        private static ApplicationSettings sInstance;

        public List<Session.Data.SubData.PresetsEntry> PresetList { get; set; }
        public List<Session.Data.SubData.ApplicationEntry> ApplicationList { get; set; }

        private ApplicationSettings()
        {

        }

        public static ApplicationSettings GetInstance()
        {
            if (sInstance == null)
            {
                sInstance = new ApplicationSettings();
            }
            return sInstance;
        }
    }
}
