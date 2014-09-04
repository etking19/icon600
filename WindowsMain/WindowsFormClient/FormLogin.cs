using Session.Connection;
using System;
using System.ComponentModel;
using System.Diagnostics;
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

        private delegate void DelegateUI();
        private delegate void DelegateUIForm(FormClient sender);
        private FormProgress formProgress;

        private FormClient formClient;

        public FormLogin()
        {
            InitializeComponent();

            // create round edge region
            Region = System.Drawing.Region.FromHrgn(Utils.Windows.NativeMethods.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            connectionManager = new ConnectionManager();
            connectionManager.EvtConnected += connectionManager_EvtConnected;
            connectionManager.EvtDisconnected += connectionManager_EvtDisconnected;
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
            formProgress = new FormProgress();
            formProgress.Show(this);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // try login to server
            if(false == connectionManager.StartClient(textBoxServerIp.Text, int.Parse(textBoxServerPort.Text)))
            {
                // failed
                showMessage();
            }
        }

        void showMessage()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateUI(showMessage));
                return;
            }

            formProgress.Close();
            MessageBox.Show(this, "Failed to connect. Please check server IP and/or port number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        void connectionManager_EvtConnected()
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateUI(connectionManager_EvtConnected));
                return;
            }

            // send credential to server
            formClient = new FormClient(connectionManager, textBoxUsername.Text, textBoxPassword.Text);
            formClient.FormClosed += formClient_FormClosed;
            formClient.EvtServerReply += formClient_EvtServerReply;
            formClient.WindowState = FormWindowState.Minimized;
            formClient.Show(this);
        }

        void formClient_EvtServerReply(FormClient sender)
        {
            // hide itself
            if(this.InvokeRequired)
            {
                this.Invoke(new DelegateUIForm(formClient_EvtServerReply), sender);
                return;
            }

            formProgress.Close();
            this.Hide();

            formClient.WindowState = FormWindowState.Normal;
        }

        void connectionManager_EvtDisconnected()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateUI(connectionManager_EvtDisconnected));
                return;
            }

            if(this.Visible)
            {
                // invalid username password
                formProgress.Close();
                MessageBox.Show(this, "Invalid username and/or passowrd.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (m.Msg == Constant.WM_SYSCOMMAND)
            {
                switch ((UInt32)m.WParam)
                {
                    case Constant.SC_MINIMIZE:
                    case Constant.SC_MAXIMIZE:
                    case Constant.SC_RESTORE:
                    case Constant.SC_MAXIMIZE2:
                        // do not handle sizing
                        return;
                    default:
                        break;
                }
            }
            else if (m.Msg == Constant.WM_NCHITTEST)
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
