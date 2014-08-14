using System.Collections.Generic;

namespace WindowsFormServer.Comparer
{
    public class WndPosComparer : EqualityComparer<Client.Model.WindowsModel>
    {
        public override bool Equals(Client.Model.WindowsModel wnd1, Client.Model.WindowsModel wnd2)
        {
            if (wnd1.PosLeft == wnd2.PosLeft &&
                wnd1.PosTop == wnd2.PosTop)
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
