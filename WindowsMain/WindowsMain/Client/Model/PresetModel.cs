using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormServer.Client.Model
{
    public class PresetModel
    {
        public int PresetId { get; set; }

        public string PresetName { get; set; }

        public IList<ApplicationModel> ApplicationList { get; set; }
    }
}
