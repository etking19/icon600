using CustomWinForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormClient
{
    public partial class FormMimic : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public Size FullSize { get; set; }
        public Size VisibleSize { get; set; }
        public int ReferenceLeft { get; set; }
        public int ReferenceTop { get; set; }

        private delegate void delegateUI();
        private CustomControlHolder mHolder = null;

        public FormMimic()
        {
            InitializeComponent();
        }

        private void FormMimic_Load(object sender, EventArgs e)
        {
            this.SizeChanged += FormMimic_SizeChanged;
        }

        void FormMimic_SizeChanged(object sender, EventArgs e)
        {
            RefreshMatrixLayout();
        }

        public void AddMimicHolder(CustomControlHolder holder)
        {
            mHolder = holder;
            mHolder.Dock = DockStyle.Fill;
            this.Controls.Add(mHolder);
            mHolder.SendToBack();
        }

        private List<Panel> panelList = new List<Panel>();

        public void RefreshMatrixLayout()
        {
            if (VisibleSize.Width == 0 ||
                VisibleSize.Height == 0 ||
                FullSize.Width == 0 ||
                FullSize.Height == 0 ||
                Row == 0 ||
                Column == 0)
            {
                return;
            }

            if(this.InvokeRequired)
            {
                this.Invoke(new delegateUI(RefreshMatrixLayout));
                return;
            }

            foreach (Panel panel in panelList)
            {
                this.Controls.Remove(panel);
            }

            // modify the reference layout
            Rectangle referenceLayout = new Rectangle();

            float scaleX = (float)this.Width / (float)FullSize.Width;
            float scaleY = (float)this.Height / (float)FullSize.Height;

            referenceLayout.Location = new Point(-(int)((float)ReferenceLeft * scaleX), -(int)((float)ReferenceTop * scaleY));

            referenceLayout.Width = (int)(FullSize.Width * this.Width / VisibleSize.Width);
            referenceLayout.Height = (int)(FullSize.Height * this.Height / VisibleSize.Height);

            for (int i = 0; i <= Row; i++ )
            {
                // create the panel to fake the boundary line
                Panel panel = new Panel();
                panel.Name = String.Format("Row{0}", i);
                panel.BackColor = Color.White;
                panel.Location = new Point(0, i * (referenceLayout.Height-1) / Row);
                panel.Width = referenceLayout.Width;
                panel.Height = 1;
                this.Controls.Add(panel);

                panelList.Add(panel);
            }

            for (int j = 0; j <= Column; j++)
            {
                Panel panel = new Panel();
                panel.Name = String.Format("Column{0}", j);
                panel.BackColor = Color.White;
                panel.Location = new Point(j * (referenceLayout.Width-1) / Column, 0);
                panel.Width = 1;
                panel.Height = referenceLayout.Height;
                this.Controls.Add(panel);

                panelList.Add(panel);
            }

            mHolder.SendToBack();
        }
    }
}
