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
    public partial class FormServerMaintenance : Form
    {
        public FormServerMaintenance()
        {
            InitializeComponent();
        }

        private void buttonShutdown_Click(object sender, EventArgs e)
        {

        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {

        }

        private void buttonStandby_Click(object sender, EventArgs e)
        {

        }

        private void FormServerMaintenance_Load(object sender, EventArgs e)
        {
            buttonRestart.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonShutdown.DialogResult = System.Windows.Forms.DialogResult.Retry;
            buttonStandby.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }
    }
}
