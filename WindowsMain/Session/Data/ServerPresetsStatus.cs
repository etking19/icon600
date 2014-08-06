using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ServerPresetsStatus : BaseCmd
    {
        public List<PresetsEntry> UserPresetList { get; set; }
    }
}
