using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WindowsFormClient
{
    public partial class FormApplications : DockContent
    {
        public FormApplications()
        {
            InitializeComponent();
        }

        private void FormApplications_Load(object sender, EventArgs e)
        {
            listBoxApplications.MouseDown += listBoxApplications_MouseDown;
        }

        void listBoxApplications_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBoxApplications.Items.Count == 0)
                return;

            int index = listBoxApplications.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                DragDropEffects dde1 = DoDragDrop(listBoxApplications.Items[index], DragDropEffects.All);
            }
        }

        public void SetApplicationsListData(IList<Client.Model.ApplicationModel> appList)
        {
            listBoxApplications.DataSource = new BindingSource(appList, null);
            listBoxApplications.DisplayMember = "ApplicationName";
            listBoxApplications.ValueMember = "AppliationId";
        }
    }
}
