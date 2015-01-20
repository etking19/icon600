using RemoteFormServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using Utils.Windows;
using WcfServiceLibrary1;

namespace RemoteFormServer
{
    public partial class FormConnect : Form
    {
        private string mUsername;
        private string mPassword;

        public string ServerIp { get; set; }
        public int ServerPort { get; set; }

        public FormConnect(string correctUsername, string correctPassword)
        {
            InitializeComponent();

            Region = System.Drawing.Region.FromHrgn(Utils.Windows.NativeMethods.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            mUsername = correctUsername;
            mPassword = correctPassword;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.AcceptButton = buttonLogin;

            // load default
            textBoxServerIp.Text = Properties.Settings.Default.lastServerIp;
            numericUpDownServerPort.Value = Properties.Settings.Default.lastServerPort;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            ServerIp = textBoxServerIp.Text;
            ServerPort = Convert.ToInt32(numericUpDownServerPort.Value);

            IPAddress tempAddress;
            if (IPAddress.TryParse(ServerIp, out tempAddress) == false)            
            {
                MessageBox.Show("Please insert proper server IP address!");
                return;
            }

            // save the setting
            Properties.Settings.Default.lastServerIp = ServerIp;
            Properties.Settings.Default.lastServerPort = ServerPort;
            Properties.Settings.Default.Save();

            if (mUsername.CompareTo(textBoxUsername.Text) == 0 &&
                mPassword.CompareTo(textBoxPassword.Text) == 0)
            {
                FormRemoteConfigure formRemote = new FormRemoteConfigure();
                try
                {
                    InstanceContext instanceContext = new InstanceContext(formRemote);
                    EndpointAddress address = new EndpointAddress(new Uri(string.Format("net.tcp://{0}:{1}/Service1", ServerIp, ServerPort)));

                    NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                    tcpBinding.ReceiveTimeout = new TimeSpan(24, 20, 31, 23);
                    tcpBinding.MaxBufferSize = 2147483647;
                    tcpBinding.MaxReceivedMessageSize = 2147483647;

                    DuplexChannelFactory<IService1> dupFactory = new DuplexChannelFactory<IService1>(instanceContext, tcpBinding, address);
                    dupFactory.Open();

                    IService1 wcfService = dupFactory.CreateChannel();
                    wcfService.RegisterCallback();

                    formRemote.Initialize(wcfService);
                }
                catch (Exception)
                {
                    MessageBox.Show("Cant connect to specify server IP and/or port!");
                    return;
                }

                this.Hide();
                formRemote.ShowDialog();
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
