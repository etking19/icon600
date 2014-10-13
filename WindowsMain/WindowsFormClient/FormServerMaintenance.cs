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
            if(MessageBox.Show(this, 
                "Vistrol server PC will shutdown immediately, do you want to proceed?", 
                "Warning",
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }

        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this,
                "Vistrol server PC will restart immediately, do you want to proceed?",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Retry;
                this.Close();
            }
        }

        private void FormServerMaintenance_Load(object sender, EventArgs e)
        {
        }
    }
}
