using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Settings
{
    class ApplicationSettings
    {
        private static ApplicationSettings sInstance;

        public List<PresetsEntry> PresetList { get; set; }
        public List<ApplicationEntry> ApplicationList { get; set; }
        public List<VncEntry> VncList { get; set; }
        public List<InputAttributes> InputList { get; set; }

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
