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
        private ConnectionManager connectionManager;

        public FormLogin()
        {
            InitializeComponent();
            connectionManager = new ConnectionManager();
            connectionManager.EvtConnected += connectionManager_EvtConnected;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            textBoxServerIp.Text = Properties.Settings.Default.ServerIp;
            textBoxServerPort.Text = Properties.Settings.Default.ServerPort;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // try login to server
            connectionManager.StartClient(textBoxServerIp.Text, int.Parse(textBoxServerPort.Text));
        }

        void connectionManager_EvtConnected()
        {
            this.Hide();

            FormClient formClient = new FormClient(connectionManager, textBoxUsername.Text, textBoxPassword.Text);
            formClient.FormClosed += formClient_FormClosed;
            formClient.Show(this);
        }

        void formClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            textBoxUsername.Text = String.Empty;
            textBoxPassword.Text = String.Empty;
            this.Show();
        }

        private void FormLogin_Closing(object sender, FormClosingEventArgs e)
        {
            // save current server's ip into properties file
            Properties.Settings.Default.ServerIp = textBoxServerIp.Text;
            Properties.Settings.Default.ServerPort = textBoxServerPort.Text;

            Properties.Settings.Default.Save();
        }
    }
}
