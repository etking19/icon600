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

namespace CustomMessageBox
{
    public partial class FormMsgBox : Form
    {
        private string message;
        private Font messageFont;
        private Color messageColor;
        private Color backgroundColor;
        private int duration;
        private Rectangle messageBoxRect;
        private bool animationEnabled;

        private BackgroundWorker workerFlying = null;
        private BackgroundWorker workerClose = null;
        private delegate void DelegateUI();

        // parameter:
        // 1. string: message to display
        // 2. string: font to use
        // 3. string: text color to use
        // 4. string: background color to use
        // 5. int: duration in second (-1 indicate infinite)
        // 6. int: pos left
        // 7. int: pos top
        // 8. int: pos width
        // 9. int: pos height
        // 10. bool: animation enable
        public FormMsgBox(string[] args)
        {
            if (args.Count() != 10)
            {
                Application.Exit();
                return;
            }

            InitializeComponent();

            message = args[0];
            messageFont = new Session.Common.SerializableFont() { SerializeFontAttribute = args[1] }.FontValue;
            messageColor = System.Drawing.ColorTranslator.FromHtml(args[2]);
            backgroundColor = System.Drawing.ColorTranslator.FromHtml(args[3]);
            duration = Convert.ToInt32(args[4]);
            messageBoxRect = new Rectangle()
            {
                X = Convert.ToInt32(args[5]),
                Y = Convert.ToInt32(args[6]),
                Width = Convert.ToInt32(args[7]),
                Height = Convert.ToInt32(args[8]),
            };

            animationEnabled = Convert.ToBoolean(args[9]);
        }

        private void FormMsgBox_Load(object sender, EventArgs e)
        {
            this.FormClosing += FormMsgBox_FormClosing;

            // set the background color
            this.BackColor = backgroundColor;

            // set the message
            SizeF size = labelMessage.CreateGraphics().MeasureString(message, messageFont);
            this.Size = new Size((int)size.Width + 10, (int)size.Height + 10);
            this.TopMost = true;
            this.Left = messageBoxRect.X;
            this.Top = messageBoxRect.Y;

            // visible message
            labelMessage.Text = message;
            labelMessage.Font = messageFont;
            labelMessage.ForeColor = messageColor;
            labelMessage.BackColor = Color.Transparent;

            int offsetX = 5;
            int locationX = offsetX;
            int locationY = offsetX;
            labelMessage.Location = new Point(locationX, locationY);
            labelMessage.LocationChanged += labelMessage_LocationChanged;

            if(duration != -1)
            {
                workerClose = new BackgroundWorker();
                workerClose.DoWork += workerClose_DoWork;
                workerClose.WorkerSupportsCancellation = true;
                workerClose.RunWorkerAsync();
            }

            labelMessageFollow.Text = "";
            if (animationEnabled)
            {
                // following message
                labelMessageFollow.Text = message;
                labelMessageFollow.Font = messageFont;
                labelMessageFollow.ForeColor = messageColor;
                labelMessageFollow.BackColor = Color.Transparent;
                labelMessageFollow.Location = new Point(labelMessage.Location.X + labelMessage.Size.Width + 5, labelMessage.Location.Y);

                workerFlying = new BackgroundWorker();
                workerFlying.DoWork += workerFlying_DoWork;
                workerFlying.WorkerSupportsCancellation = true;
                workerFlying.RunWorkerAsync();
            }
        }

        void labelMessage_LocationChanged(object sender, EventArgs e)
        {
            labelMessageFollow.Location = new Point(labelMessage.Location.X + labelMessage.Size.Width + 5, labelMessage.Location.Y);
        }

        void FormMsgBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (workerClose != null)
                {
                    workerClose.CancelAsync();
                }
            }
            catch (Exception)
            {
            }

            try
            {
                if (workerFlying != null)
                {
                    workerFlying.CancelAsync();
                }
            }
            catch (Exception)
            {
            }
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
            while (true)
            {
                changeLocation();
                Thread.Sleep(1000 / (int)messageFont.Size);
            }
        }

        void workerClose_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(duration * 1000);

            closeDialog();
        }

        private void closeDialog()
        {
            if (this.InvokeRequired)
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
                latestPost.X = labelMessageFollow.Location.X;
            }
            labelMessage.Location = latestPost;
        }
    }
}
