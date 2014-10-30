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
    public partial class FormUserSetting : Form
    {

        public int GridX 
        { 
            get
            {
                return (int)numericUpDownGridX.Value;
            }
            set
            {
                numericUpDownGridX.Value = value;
            }
        }

        public int GridY 
        {
            get
            {
                return (int)numericUpDownGridY.Value;
            }
            set
            {
                numericUpDownGridY.Value = value;
            }
        }
        public bool ApplySnap 
        { 
            get
            {
                return checkBoxApplySnap.Checked;
            }
            set
            {
                checkBoxApplySnap.Checked = value;
            }
        }

        public FormUserSetting()
        {
            InitializeComponent();
        }

        private void FormUserSetting_Load(object sender, EventArgs e)
        {
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
        }

    }
}
