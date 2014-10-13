using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Client.Model
{
    public class PresetModel
    {
        public int PresetId { get; set; }

        public string PresetName { get; set; }

        public IList<ApplicationModel> ApplicationList { get; set; }

        public IList<VncModel> VncList { get; set; }

        public IList<InputAttributes> VisionInputList { get; set; }
    }
}
