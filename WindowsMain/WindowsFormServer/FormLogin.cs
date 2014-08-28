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
        private string mUsername;
        private string mPassword;

        private ConnectionManager connectionMgr = new ConnectionManager();

        public FormLogin(string correctUsername, string correctPassword)
        {
            InitializeComponent();

            mUsername = correctUsername;
            mPassword = correctPassword;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.AcceptButton = buttonLogin;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (mUsername.CompareTo(textBoxUsername.Text) == 0 &&
                mPassword.CompareTo(textBoxPassword.Text) == 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect username and/or password!");
            }
        }

        private void FormLogin_Closed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
