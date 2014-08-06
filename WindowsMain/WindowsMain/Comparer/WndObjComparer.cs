using System.Collections.Generic;

namespace WindowsMain.Comparer
{
    public class WndObjComparer : EqualityComparer<Client.Model.WindowsModel>
    {
        public override bool Equals(Client.Model.WindowsModel wnd1, Client.Model.WindowsModel wnd2)
        {
            if (wnd1.WindowsId == wnd2.WindowsId)
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
