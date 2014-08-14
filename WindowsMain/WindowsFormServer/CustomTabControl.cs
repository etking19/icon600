using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormClient
{
    /// <summary>
    /// Summary description for TabControl.
    /// </summary>
    public partial class CustomTabControl : System.Windows.Forms.TabControl
    {

        public delegate void SelectedTabPageChangeEventHandler(Object sender, TabPageChangeEventArgs e);

        [Description("Occurs as a tab is being changed.")]
        public event SelectedTabPageChangeEventHandler SelectedIndexChanging;

        public CustomTabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region Interop

        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr HWND;
            public uint idFrom;
            public int code;
            public override String ToString()
            {
                return String.Format("Hwnd: {0}, ControlID: {1}, Code: {2}", HWND, idFrom, code);
            }
        }

        private const int TCN_FIRST = 0 - 550;
        private const int TCN_SELCHANGING = (TCN_FIRST - 2);

        private const int WM_USER = 0x400;
        private const int WM_NOTIFY = 0x4E;
        private const int WM_REFLECT = WM_USER + 0x1C00;

        #endregion

        #region BackColor Manipulation

        //As well as exposing the property to the Designer we want it to behave just like any other 
        //controls BackColor property so we need some clever manipulation.

        private Color m_Backcolor = Color.Empty;
        [Browsable(true), Description("The background color used to display text and graphics in a control.")]
        public override Color BackColor
        {
            get
            {
                if (m_Backcolor.Equals(Color.Empty))
                {
                    if (Parent == null)
                        return Control.DefaultBackColor;
                    else
                        return Parent.BackColor;
                }
                return m_Backcolor;
            }
            set
            {
                if (m_Backcolor.Equals(value)) return;
                m_Backcolor = value;
                Invalidate();
                //Let the Tabpages know that the backcolor has changed.
                base.OnBackColorChanged(EventArgs.Empty);
            }
        }
        public bool ShouldSerializeBackColor()
        {
            return !m_Backcolor.Equals(Color.Empty);
        }
        public override void ResetBackColor()
        {
            m_Backcolor = Color.Empty;
            Invalidate();
        }

        #endregion

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }


        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (TabCount <= 0)
            {
                return;
            }

            // clear the initial color
            e.Graphics.Clear(BackColor);

            //Draw a custom background for Transparent TabPages
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Far;

            Rectangle r = ClientRectangle;
            TabPage tp;
            SolidBrush PaintBrush = new SolidBrush(BackColor);
            SolidBrush SelectedBrush = new SolidBrush(Color.White);
            Pen BorderPen = new Pen(Color.Black);

            //Draw the Tabs
            for (int index = 0; index <= TabCount - 1; index++)
            {
                tp = TabPages[index];
                r = GetTabRect(index);

                ButtonBorderStyle bs = ButtonBorderStyle.None;

                // draw the border to seemless
                ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, bs);

                if(index == SelectedIndex)
                {
                    // draw different color for the selected tab
                    int x = 2;
                    int y = 0;
                    if(index == 0)
                    {
                        y = -2;
                    }
                    r.Inflate(x, y);
                    e.Graphics.FillRectangle(SelectedBrush, r);
                    r.Inflate(-x, -y);
                }

                // draw the bottom line of the tab
                if (index != 0)
                {
                    e.Graphics.DrawLine(BorderPen, new Point(r.Left, r.Top), new Point(r.Right, r.Top));
                }
                
                e.Graphics.DrawLine(BorderPen, new Point(r.Left, r.Bottom), new Point(r.Right, r.Bottom));

                PaintBrush.Color = tp.ForeColor;

                //Set up rotation for left and right aligned tabs
                if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right)
                {
                    PointF cp = new PointF(r.Left + (r.Width >> 1), r.Top + (r.Height >> 1));
                    e.Graphics.TranslateTransform(cp.X, cp.Y);
                    r = new Rectangle(-(r.Height >> 1), -(r.Width >> 1), r.Height, r.Width);
                }

                //Draw the Tab Text
                if (tp.Enabled)
                {
                    e.Graphics.DrawString(tp.Text, Font, PaintBrush, (RectangleF)r, sf);
                } 
                else
                {
                    ControlPaint.DrawStringDisabled(e.Graphics, tp.Text, Font, tp.BackColor, (RectangleF)r, sf);
                }

                // draw the tab image
                Image tabImage = null;
                if (this.ImageList != null)
                {
                    TabPage page = this.TabPages[index];
                    if (page.ImageIndex > -1 && page.ImageIndex < this.ImageList.Images.Count)
                    {
                        tabImage = this.ImageList.Images[page.ImageIndex];
                    }

                    if (page.ImageKey.Length > 0 && this.ImageList.Images.ContainsKey(page.ImageKey))
                    {
                        tabImage = this.ImageList.Images[page.ImageKey];
                    }

                    using (Bitmap bm = new Bitmap(r.Width, r.Height))
                    {
                        using (Graphics bmGraphics = Graphics.FromImage(bm))
                        {
                            if (tabImage != null)
                            {
                                Rectangle imageRect = new Rectangle(0, 0, tabImage.Width, tabImage.Height);
                                imageRect.Offset((r.Width - imageRect.Width)/ 2, (r.Height - imageRect.Height)/ 2);
                                bmGraphics.DrawImage(tabImage, imageRect);
                            }
                        }

                        e.Graphics.DrawImage(bm, r);
                    }
                }

                e.Graphics.ResetTransform();
            }

            BorderPen.Dispose();
            PaintBrush.Dispose();
            SelectedBrush.Dispose();
        }
      
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (WM_REFLECT + WM_NOTIFY))
            {
                NMHDR hdr = (NMHDR)(Marshal.PtrToStructure(m.LParam, typeof(NMHDR)));
                if (hdr.code == TCN_SELCHANGING)
                {
                    TabPage tp = TestTab(PointToClient(Cursor.Position));
                    if (tp != null)
                    {
                        TabPageChangeEventArgs e = new TabPageChangeEventArgs(SelectedTab, tp);
                        if (SelectedIndexChanging != null)
                            SelectedIndexChanging(this, e);
                        if (e.Cancel || tp.Enabled == false)
                        {
                            m.Result = new IntPtr(1);
                            return;
                        }
                    }
                }
            }
            base.WndProc(ref m);
        }

        private TabPage TestTab(Point pt)
        {
            for (int index = 0; index <= TabCount - 1; index++)
            {
                if (GetTabRect(index).Contains(pt.X, pt.Y))
                    return TabPages[index];
            }
            return null;
        }

    }

    public class TabPageChangeEventArgs : EventArgs
    {
        private TabPage _Selected = null;
        private TabPage _PreSelected = null;
        public bool Cancel = false;

        public TabPage CurrentTab
        {
            get
            {
                return _Selected;
            }
        }


        public TabPage NextTab
        {
            get
            {
                return _PreSelected;
            }
        }


        public TabPageChangeEventArgs(TabPage CurrentTab, TabPage NextTab)
        {
            _Selected = CurrentTab;
            _PreSelected = NextTab;
        }


    }
}
