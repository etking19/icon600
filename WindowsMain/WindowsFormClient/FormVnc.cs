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
    public partial class FormVnc : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public FormVnc()
        {
            InitializeComponent();
        }

        private void FormVnc_Load(object sender, EventArgs e)
        {
            listBoxVnc.MouseDown += listBoxVnc_MouseDown;
        }

        void listBoxVnc_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBoxVnc.Items.Count == 0)
                return;

            int index = listBoxVnc.IndexFromPoint(e.X, e.Y);
            if(index != -1)
            {
                DragDropEffects dde1 = DoDragDrop(listBoxVnc.Items[index], DragDropEffects.All);
            }
        }

        public void SetVNCList(IList<Client.Model.VncModel> vncList)
        {
            listBoxVnc.DataSource = new BindingSource(vncList, null);
            listBoxVnc.DisplayMember = "DisplayName";
            listBoxVnc.ValueMember = "DisplayCount";
        }
    }
}
