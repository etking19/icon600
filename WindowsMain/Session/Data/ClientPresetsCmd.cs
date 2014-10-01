using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ClientPresetsCmd : BaseCmd
    {
        public enum EControlType
        {
            Add = 0,
            Modify,
            Delete,
            Launch,
        }

        public EControlType ControlType { get; set; }

        public PresetsEntry PresetEntry { get; set; }
    }
}
