using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data.SubData
{
    public class PresetDataEntry
    {
        /// <summary>
        /// preset name table id
        /// </summary>
        public int Identifier { get; set; }

        /// <summary>
        /// preset name
        /// </summary>
        public string Name { get; set; }

        public List<KeyValuePair<ApplicationEntry, WndPos>> PresetAppList { get; set; }
        public List<KeyValuePair<VncEntry, WndPos>> PresetVncList { get; set; }
        public List<KeyValuePair<InputAttributes, WndPos>> PresetVisionInputList { get; set; }
    }
}
