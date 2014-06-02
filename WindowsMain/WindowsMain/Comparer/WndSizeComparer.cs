using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsMain.Comparer
{
    public class WndSizeComparer : EqualityComparer<WndPos>
    {
        public override bool Equals(WndPos wnd1, WndPos wnd2)
        {
            if (wnd1.width == wnd2.width &&
                wnd1.height == wnd2.height)
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
