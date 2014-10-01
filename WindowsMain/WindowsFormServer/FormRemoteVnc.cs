using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormClient
{
    public partial class FormRemoteVnc : Form
    {
        public string DisplayName { get; set; }
        public string RemoteIp { get; set; }
        public int RemotePort { get; set; }

        public bool IsDirty { get; set; }

        public FormRemoteVnc()
        {
            InitializeComponent();
        }

        private void FormRemoteVnc_Load(object sender, EventArgs e)
        {
            this.textBoxDisplayName.Text = DisplayName;
            this.textBoxRemoteIp.Text = RemoteIp;
            this.textBoxRemotePort.Text = RemotePort.ToString();

            this.textBoxDisplayName.TextChanged += textBoxDisplayName_TextChanged;
            this.textBoxRemoteIp.TextChanged += textBoxRemoteIp_TextChanged;
            this.textBoxRemotePort.TextChanged += textBoxRemotePort_TextChanged;
            
            
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;

            IsDirty = false;
        }

        void textBoxRemoteIp_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        void textBoxRemotePort_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        void textBoxDisplayName_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool error = false;
            DisplayName = textBoxDisplayName.Text;
            if (DisplayName == String.Empty)
            {
                textBoxDisplayName.BackColor = Color.Red;
                error = true;
            }

            RemoteIp = textBoxRemoteIp.Text;
            IPAddress tempAdd;
            if (RemoteIp == String.Empty ||
                IPAddress.TryParse(RemoteIp, out tempAdd) == false)
            {
                textBoxRemoteIp.BackColor = Color.Red;
                error = true;
            }

            int tempPort;
            if (textBoxRemotePort.Text == String.Empty ||
                int.TryParse(textBoxRemotePort.Text, out tempPort) == false)
            {
                textBoxRemotePort.BackColor = Color.Red;
                error = true;
            }
            else
            {
                RemotePort = tempPort;
            }

            if (error)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
        }
    }
}
