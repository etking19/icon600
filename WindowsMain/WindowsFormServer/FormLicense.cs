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
    public partial class FormLicense : Form
    {
        private delegate void DelegateUI(int counter);
        private int mCounter;
        private BackgroundWorker worker;

        public FormLicense()
        {
            InitializeComponent();
        }

        private void FormLicense_Load(object sender, EventArgs e)
        {
            this.FormClosed += FormLicense_FormClosed;

            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AcceptButton = buttonOK;

            mCounter = 60;
            labelLicense.Text = "No license found. Please make sure you plug in the license dongle.";
            labelCounter.Text = "Application will close in 60 seconds";

            // create a counter
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();
        }

        void FormLicense_FormClosed(object sender, FormClosedEventArgs e)
        {
            worker.CancelAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                // change the counter
                SetMessage(mCounter--);
                Thread.Sleep(1000);
            }
            
        }

        public void SetMessage(int counter)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new DelegateUI(SetMessage), counter);
                return;
            }

            labelCounter.Text = String.Format("Application will close in {0} seconds", counter);
            if (counter <= 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
    }
}
