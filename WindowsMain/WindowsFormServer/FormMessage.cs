using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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

            this.BackColor = Color.Black;
            this.Size = MessageBoxSize;

            // set the attributes for message
            labelMessage.Text = Message;
            labelMessage.Font = MessageFont;
            labelMessage.ForeColor = MessageColor;

            labelMessageDuplicate.Text = Message;
            labelMessageDuplicate.Font = MessageFont;
            labelMessageDuplicate.ForeColor = MessageColor;

            // put the text on the middle vertical, left most
            int offsetX = 5;
            int locationX = offsetX;
            int locationY = MessageBoxSize.Height / 2;
            labelMessage.Location = new Point(locationX, locationY);

            labelMessageDuplicate.Location = new Point(this.Width - offsetX, locationY);

            workerClose = new BackgroundWorker();
            workerClose.DoWork += workerClose_DoWork;
            workerClose.WorkerSupportsCancellation = true;
            workerClose.RunWorkerAsync();

            workerFlying = new BackgroundWorker();
            workerFlying.DoWork += workerFlying_DoWork;
            workerFlying.WorkerSupportsCancellation = true;
            workerFlying.RunWorkerAsync();
        }

        void workerFlying_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                changeLocation();
                Thread.Sleep(1000);
            }
        }

        void workerClose_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(MessageDuration);

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
                this.BeginInvoke(new DelegateUI(changeLocation));
                return;
            }

            Point currentLocation = labelMessage.Location;
            currentLocation.X -= 1;
            labelMessage.Location = currentLocation;

            Point currentDuplicateLocation = labelMessageDuplicate.Location;
            currentDuplicateLocation.X -= 1;
            labelMessageDuplicate.Location = currentDuplicateLocation;
        }
    }
}
