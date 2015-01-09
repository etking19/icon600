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
        public delegate void OnAppBringToFront(FormRunningApps form, Client.Model.WindowsModel model);
        public event OnAppBringToFront EvtAppBringToFront;

        public delegate void OnAppMinimize(FormRunningApps form, Client.Model.WindowsModel model);
        public event OnAppMinimize EvtAppMinimize;

        public delegate void OnAppMaximize(FormRunningApps form, Client.Model.WindowsModel model);
        public event OnAppMaximize EvtAppMaximize;

        public delegate void OnAppRestore(FormRunningApps form, Client.Model.WindowsModel model);
        public event OnAppRestore EvtAppRestore;

        public delegate void OnAppClose(FormRunningApps form, Client.Model.WindowsModel model);
        public event OnAppClose EvtAppClose;

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

        private void bringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxApps.SelectedItems)
            {
                if (EvtAppBringToFront != null)
                {
                    EvtAppBringToFront.BeginInvoke(this, (Client.Model.WindowsModel)item, null, null);
                }
            }
        }

        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxApps.SelectedItems)
            {
                if (EvtAppMinimize != null)
                {
                    EvtAppMinimize.BeginInvoke(this, (Client.Model.WindowsModel)item, null, null);
                }
            }
        }

        private void maximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxApps.SelectedItems)
            {
                if (EvtAppMaximize != null)
                {
                    EvtAppMaximize.BeginInvoke(this, (Client.Model.WindowsModel)item, null, null);
                }
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxApps.SelectedItems)
            {
                if (EvtAppRestore != null)
                {
                    EvtAppRestore.BeginInvoke(this, (Client.Model.WindowsModel)item, null, null);
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxApps.SelectedItems)
            {
                if (EvtAppClose != null)
                {
                    EvtAppClose.BeginInvoke(this, (Client.Model.WindowsModel)item, null, null);
                }
            }
        }

        private void closeAllApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if( MessageBox.Show(this,
                "This will close all running applications on server." + Environment.NewLine + "Are you sure want to continue?",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (object item in listBoxApps.Items)
                {
                    if (EvtAppClose != null)
                    {
                        EvtAppClose.BeginInvoke(this, (Client.Model.WindowsModel)item, null, null);
                    }
                }
            }
        }
    }
}
