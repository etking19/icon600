using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utils.Windows;

namespace WindowsFormClient
{
    public partial class FormMessage : Form
    {
        public string Message
        {
            get;
            set;
        }

        public Font MessageFont
        {
            get;
            set;
        }

        public Color MessageColor
        {
            get;
            set;
        }

        public int MessageDuration
        {
            get;
            set;
        }

        public Size MessageBoxSize
        {
            get;
            set;
        }

        private BackgroundWorker workerFlying;
        private BackgroundWorker workerClose;
        private delegate void DelegateUI();

        public FormMessage()
        {
            InitializeComponent();
        }

        private void FormMessage_Load(object sender, EventArgs e)
        {
            this.FormClosing += FormMessage_FormClosing;

            SizeF size = labelMessage.CreateGraphics().MeasureString(Message, MessageFont);
            this.BackColor = Color.White;
            this.Size = new Size((int)size.Width + 10, (int)size.Height + 10);
            this.TopMost = true;

            // set the attributes for message
            labelMessage.Text = Message;
            labelMessage.Font = MessageFont;
            labelMessage.ForeColor = MessageColor;

            // put the text on the middle vertical, left most
            int offsetX = 5;
            int locationX = offsetX;
            int locationY = offsetX;
            labelMessage.Location = new Point(locationX, locationY);

            workerClose = new BackgroundWorker();
            workerClose.DoWork += workerClose_DoWork;
            workerClose.WorkerSupportsCancellation = true;
            workerClose.RunWorkerAsync();

            workerFlying = new BackgroundWorker();
            workerFlying.DoWork += workerFlying_DoWork;
            workerFlying.WorkerSupportsCancellation = true;
            workerFlying.RunWorkerAsync();
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

        void workerFlying_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                changeLocation();
                Thread.Sleep(1000/(int)MessageFont.Size);
            }
        }

        void workerClose_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(MessageDuration*1000);

            closeDialog();
        }

        void FormMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                workerClose.CancelAsync();
            }
            catch (Exception)
            {
            }

            try
            {
                workerFlying.CancelAsync();
            }
            catch (Exception)
            {
            }
            
        }

        private void closeDialog()
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateUI(closeDialog));
                return;
            }

            this.Close();
        }

        private void changeLocation()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateUI(changeLocation));
                return;
            }

            Point currentLocation = labelMessage.Location;
            Point latestPost = new Point(currentLocation.X - 1, currentLocation.Y);
            if (latestPost.X <= (-labelMessage.Width))
            {
                latestPost.X = this.Width;
            }
            labelMessage.Location = latestPost;
        }
    }
}
