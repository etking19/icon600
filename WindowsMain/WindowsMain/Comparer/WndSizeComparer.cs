using System.Collections.Generic;

namespace WindowsMain.Comparer
{
    public class WndSizeComparer : EqualityComparer<Client.Model.WindowsModel>
    {
        public override bool Equals(Client.Model.WindowsModel wnd1, Client.Model.WindowsModel wnd2)
        {
            if (wnd1.Width == wnd2.Width &&
                wnd1.Height == wnd2.Height)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode(Client.Model.WindowsModel wndPos)
        {
            return wndPos.WindowsId.GetHashCode();
        }
    }
}
