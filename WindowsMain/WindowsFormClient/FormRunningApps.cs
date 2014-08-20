using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormClient
{
    public partial class FormRunningApps : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public FormRunningApps()
        {
            InitializeComponent();
        }

        public void SetApplicationListData(IList<Client.Model.WindowsModel> appList)
        {
            listBoxApps.DataSource = new BindingSource(appList, null);
            listBoxApps.DisplayMember = "DisplayName";
            listBoxApps.ValueMember = "WindowsId";
        }

        private void FormRunningApps_Load(object sender, EventArgs e)
        {
            listBoxApps.MouseDown += listBoxApps_MouseDown;
        }

        void listBoxApps_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBoxApps.Items.Count == 0)
                return;

            int index = listBoxApps.IndexFromPoint(e.X, e.Y);
            if(index != -1)
            {
                DragDropEffects dde1 = DoDragDrop(listBoxApps.Items[index], DragDropEffects.All);
            }
        }
    }
}
