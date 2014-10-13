using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data.SubData
{
    public class PresetsEntry
    {
        /// <summary>
        /// preset name table id
        /// </summary>
        public int Identifier { get; set; }

        /// <summary>
        /// preset name
        /// </summary>
        public string Name { get; set; }

        public List<ApplicationEntry> ApplicationList { get; set; }

        public List<VncEntry> VncList { get; set; }

        public List<InputAttributes> InputList { get; set; }
    }
}
