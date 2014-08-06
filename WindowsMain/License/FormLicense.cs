using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace License
{
    public partial class FormLicense : Form
    {
        public FormLicense()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // encode the entry
            byte[] data = Encryptor.GetInstance().EncodeContent(textBoxIdentifier.Text);

            // generate a file from the string
            if(Utils.WriteFile(@"VostrolLicense.dat", data))
            {
                btnGenerateLicence.BackColor = Color.Green;
            }
            else
            {
                btnGenerateLicence.BackColor = Color.Red;
            }
        }

        private void btnGetIndetifier_Click(object sender, EventArgs e)
        {
            textBoxIdentifier.Text = Utils.GetMachineIdentifier();
        }
    }
}
