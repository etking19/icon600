using Session.Data.SubData;
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
    public partial class FormAddPreset : Form
    {
        private string presetName;

        public string PresetName
        {
            get
            {
                return presetName;
            }
        }

        public FormAddPreset()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            presetName = textBoxName.Text;
            if (presetName.Length == 0)
            {
                textBoxName.BackColor = Color.Red;
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        private void FormAddPreset_Load(object sender, EventArgs e)
        {
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
        }
    }
}
