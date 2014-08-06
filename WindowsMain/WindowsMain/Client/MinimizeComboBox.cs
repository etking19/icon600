using System;

namespace WindowsMain.Client
{
    public class MinimizeComboBox
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public Client.Model.WindowsModel Data { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
