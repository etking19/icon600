using System.Collections.Generic;

namespace WindowsFormClient.Comparer
{
    public class WndNameComparer : EqualityComparer<Client.Model.WindowsModel>
    {
        public override bool Equals(Client.Model.WindowsModel wnd1, Client.Model.WindowsModel wnd2)
        {
            if (wnd1.DisplayName.CompareTo(wnd2.DisplayName) == 0)
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
