﻿using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsMain.Comparer
{
    public class WndNameComparer : EqualityComparer<WndPos>
    {
        public override bool Equals(WndPos wnd1, WndPos wnd2)
        {
            if (wnd1.name == wnd2.name)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode(WndPos wndPos)
        {
            return wndPos.id.GetHashCode();
        }
    }
}
