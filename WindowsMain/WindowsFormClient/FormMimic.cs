using CustomWinForm;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private List<Panel> panelList = new List<Panel>();

        /// <summary>
        /// actual layout of full desktop view
        /// </summary>
        private Rectangle referenceLayout;

        /// <summary>
        /// a list to store current column grid line position 
        /// represents X position
        /// </summary>
        private List<int> columnGridList = new List<int>();
        public IList<int> ColumnGrid
        {
            get
            {
                return columnGridList.AsReadOnly();
            }
        }

        /// <summary>
        /// a list to store current row grid line position
        /// represents Y position
        /// </summary>
        private List<int> rowGridList = new List<int>();
        public IList<int> RowGrid
        {
            get
            {
                return rowGridList.AsReadOnly();
            }
        }

        public FormMimic()
        {
            InitializeComponent();

            referenceLayout = new Rectangle();
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
            float scaleX = (float)this.Width / (float)FullSize.Width;
            float scaleY = (float)this.Height / (float)FullSize.Height;

            referenceLayout.Location = new Point(-(int)((float)ReferenceLeft * scaleX), -(int)((float)ReferenceTop * scaleY));

            referenceLayout.Width = (int)(FullSize.Width * this.Width / VisibleSize.Width);
            referenceLayout.Height = (int)(FullSize.Height * this.Height / VisibleSize.Height);

            // update the column and row list data
            columnGridList.Clear();
            rowGridList.Clear();

            for (int i = 0; i <= Row; i++ )
            {
                // create the panel to fake the boundary line
                int yLinePos = i * (referenceLayout.Height-1) / Row;

                Panel panel = new Panel();
                panel.Name = String.Format("Row{0}", i);
                panel.BackColor = Color.White;
                panel.Location = new Point(0, yLinePos);
                panel.Width = referenceLayout.Width;
                panel.Height = 1;
                this.Controls.Add(panel);
                panelList.Add(panel);

                rowGridList.Add(yLinePos);
            }

            for (int j = 0; j <= Column; j++)
            {
                int xLinePos = j * (referenceLayout.Width-1) / Column;

                Panel panel = new Panel();
                panel.Name = String.Format("Column{0}", j);
                panel.BackColor = Color.White;
                panel.Location = new Point(xLinePos, 0);
                panel.Width = 1;
                panel.Height = referenceLayout.Height;
                this.Controls.Add(panel);
                panelList.Add(panel);

                columnGridList.Add(xLinePos);
            }

            mHolder.SendToBack();
            mHolder.SetSnapGrid(columnGridList, rowGridList);
        }
    }
}
