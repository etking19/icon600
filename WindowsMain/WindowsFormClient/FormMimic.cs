using CustomWinForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormClient
{
    public partial class FormMimic : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        /// <summary>
        /// server's matrix number
        /// </summary>
        public int Row { get; set; }
        public int Column { get; set; }

        /// <summary>
        /// client matrix number
        /// </summary>
        public int ClientRow { get; set; }
        public int ClientColumn { get; set; }
        public bool ApplySnap { get; set; }

        /// <summary>
        /// Server total screen size
        /// </summary>
        public Size FullSize { get; set; }

        /// <summary>
        /// client allowed visible size
        /// </summary>
        public Size VisibleSize { get; set; }

        /// <summary>
        /// client allow visible left/top position
        /// </summary>
        public int ReferenceLeft { get; set; }
        public int ReferenceTop { get; set; }

        private delegate void delegateUI();
        private CustomControlHolder mHolder = null;

        private List<Panel> panelList = new List<Panel>();          // matrix server
        private List<Panel> userPanelList = new List<Panel>();      // user matrix setting

        /// <summary>
        /// actual layout of full desktop view
        /// </summary>
        private Rectangle referenceLayout;

        /// <summary>
        /// a list to store current column grid line position 
        /// </summary>
        private List<int> columnGridList = new List<int>();
        private List<int> rowGridList = new List<int>();

        /// <summary>
        /// a list to store user grid/colum line position
        /// </summary>
        private List<int> userColumnGridList = new List<int>();
        private List<int> userRowGridList = new List<int>();

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
            RefreshUserMatrixLayout();
        }

        public void AddMimicHolder(CustomControlHolder holder)
        {
            mHolder = holder;
            mHolder.Dock = DockStyle.Fill;
            this.Controls.Add(mHolder);
            mHolder.SendToBack();
        }

        public void RefreshUserMatrixLayout()
        {
            if (ClientRow == 0 ||
                ClientColumn == 0)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.Invoke(new delegateUI(RefreshUserMatrixLayout));
                return;
            }

            foreach (Panel panel in userPanelList)
            {
                this.Controls.Remove(panel);
            }

            // update the column and row list data
            userColumnGridList.Clear();
            userRowGridList.Clear();

            for (int i = 0; i <= ClientRow; i++)
            {
                // create the panel to fake the boundary line
                int yLinePos = i * (this.Size.Height - 1) / ClientRow;

                Panel panel = new Panel();
                panel.Name = String.Format("RowUser{0}", i);
                panel.BackColor = Color.Blue;
                panel.Location = new Point(0, yLinePos);
                panel.Width = this.Size.Width;
                panel.Height = 1;
                this.Controls.Add(panel);
                userPanelList.Add(panel);

                userRowGridList.Add(yLinePos);
            }

            for (int j = 0; j <= ClientColumn; j++)
            {
                int xLinePos = j * (this.Size.Width - 1) / ClientColumn;

                Panel panel = new Panel();
                panel.Name = String.Format("ColumnUser{0}", j);
                panel.BackColor = Color.Blue;
                panel.Location = new Point(xLinePos, 0);
                panel.Width = 1;
                panel.Height = this.Size.Height;
                this.Controls.Add(panel);
                userPanelList.Add(panel);

                userColumnGridList.Add(xLinePos);
            }

            if(ApplySnap)
            {
                List<int> combinedColGridList = new List<int>(userColumnGridList);
                combinedColGridList.AddRange(columnGridList);

                List<int> combinedRowGridList = new List<int>(userRowGridList);
                combinedRowGridList.AddRange(rowGridList);

                mHolder.SendToBack();
                mHolder.SetSnapGrid(combinedColGridList, combinedRowGridList);
            }
            else
            {
                mHolder.SendToBack();
                mHolder.SetSnapGrid(columnGridList, rowGridList);
            }
            
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

            List<int> combinedColGridList = new List<int>(columnGridList);
            combinedColGridList.AddRange(userColumnGridList);

            List<int> combinedRowGridList = new List<int>(rowGridList);
            combinedRowGridList.AddRange(userRowGridList);

            mHolder.SendToBack();
            mHolder.SetSnapGrid(combinedColGridList, combinedRowGridList);
        }
    }
}
