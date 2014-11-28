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

        public List<ApplicationEntry> PresetAppList { get; set; }
        public List<WndPos> PresetAppPos { get; set; }

        public List<VncEntry> PresetVncList { get; set; }
        public List<WndPos> PresetVncPos { get; set; }
        
        public List<InputAttributes> PresetVisionInputList { get; set; }
        public List<WndPos> PresetVisionInputPos { get; set; }
    }
}
