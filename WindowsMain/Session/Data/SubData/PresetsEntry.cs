﻿using System;
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

        public IList<ApplicationEntry> ApplicationList { get; set; }
    }
}
