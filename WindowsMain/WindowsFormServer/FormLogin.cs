using Session.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils.Windows;

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

            Region = System.Drawing.Region.FromHrgn(Utils.Windows.NativeMethods.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            mUsername = correctUsername;
            mPassword = correctPassword;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.AcceptButton = buttonLogin;
            this.TopMost = true;
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
