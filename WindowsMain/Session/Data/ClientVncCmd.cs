﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Session.Data
{
    public class ClientVncCmd : BaseCmd
    {
        public string IpAddress { get; set; }
        public int PortNumber { get; set; }
    }
}
