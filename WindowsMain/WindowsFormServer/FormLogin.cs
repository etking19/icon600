using Session.Connection;
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
    public partial class FormLogin : Form
    {
        private const string USERNAME = "username";
        private const string PASSWORD = "password";

        private ConnectionManager connectionMgr = new ConnectionManager();

        public FormLogin()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (USERNAME.CompareTo(textBoxUsername.Text) == 0 &&
                PASSWORD.CompareTo(textBoxPassword.Text) == 0)
            {
                this.Hide();

                FormServer formServer = new FormServer(connectionMgr);
                formServer.ShowDialog(this);

                // reset the text field
                textBoxUsername.Text = String.Empty;
                textBoxPassword.Text = String.Empty;
                this.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // initialize database
            Server.ServerDbHelper.GetInstance().Initialize();
        }

        private void FormLogin_Closed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
