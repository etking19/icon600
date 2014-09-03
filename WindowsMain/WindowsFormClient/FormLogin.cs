using Session.Connection;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Utils.Windows;

namespace WindowsFormClient
{
    public partial class FormLogin : Form
    {
        private ConnectionManager connectionManager;
        private string vncServerPath = String.Empty;

        public FormLogin()
        {
            InitializeComponent();

            // create round edge region
            Region = System.Drawing.Region.FromHrgn(Utils.Windows.NativeMethods.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            connectionManager = new ConnectionManager();
            connectionManager.EvtConnected += connectionManager_EvtConnected;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.AcceptButton = buttonLogin;
            this.CancelButton = buttonClose;

            textBoxServerIp.Text = Properties.Settings.Default.ServerIp;
            textBoxServerPort.Text = Properties.Settings.Default.ServerPort;

            if (Properties.Settings.Default.TightVncServerPath == String.Empty)
            {
                // auto search the tight vnc server
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    foreach (String vncPath in Utils.Files.DirSearch(d.RootDirectory.FullName + "Program Files", "tvnserver.exe"))
                    {
                        vncServerPath = vncPath;
                        break;
                    }

                    if (vncServerPath != String.Empty)
                    {
                        break;
                    }
                }

                if (vncServerPath == String.Empty)
                {
                    MessageBox.Show("Tight VNC executable path not found." + Environment.NewLine + "Please install Tight VNC application to use VNC feature.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
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
            Properties.Settings.Default.TightVncServerPath = vncServerPath;

            Properties.Settings.Default.Save();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constant.WM_NCHITTEST)
            {
                // to allow move by clicking the window's body
                base.WndProc(ref m);

                if (m.Result.ToInt32() == (int)Constant.HitTest.Client)
                {
                    m.Result = new IntPtr((int)Constant.HitTest.Caption);
                }

                return;
            }
            base.WndProc(ref m);
        }
    }
}
