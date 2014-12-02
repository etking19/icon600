using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using Utils.Windows;

namespace CustomWinForm
{
    public partial class CustomWinForm : UserControl
    {
        public delegate void OnControlSizeChanged(CustomWinForm winForm, Size size);
        public delegate void OnPosChanged(CustomWinForm winForm, int xPos, int yPos);
        public delegate void OnMinimized(CustomWinForm winForm);
        public delegate void OnMaximized(CustomWinForm winForm);
        public delegate void OnRestored(CustomWinForm winForm);
        public delegate void OnClosed(CustomWinForm winForm);

        public event OnControlSizeChanged onDelegateSizeChangedEvt;
        public event OnPosChanged onDelegatePosChangedEvt;
        public event OnMaximized onDelegateMaximizedEvt;
        public event OnMinimized onDelegateMinimizedEvt;
        public event OnRestored onDelegateRestoredEvt;
        public event OnClosed onDelegateClosedEvt;

        public int Id { get; private set;}

        // used to control the actual size and relative size scaling issue
        public Size LatestSize { get; set; }
        public Point LatestPos { get; set; }

        /// <summary>
        /// keep the previous position before sent to server
        /// set when wanted to send command to server about changed pos
        /// reset when server notify position does not match this pos or actual pos
        /// </summary>
        public List<Point> DelegatePos 
        { 
            get
            {
                return delegatePosList;
            }
        }
        private List<Point> delegatePosList = new List<Point>();

        /// <summary>
        /// used to control server sending position update while the actual window already moved, pending send update to server
        /// </summary>
        public List<Point> ForwardPos
        {
            get
            {
                return forwardPosList;
            }
        }
        private List<Point> forwardPosList = new List<Point>();

        private Int32 style;
        public Int32 Style 
        { 
            get { return style; }
            set 
            { 
                style = value;
                //UpdateStyles();
            } 
        }

        private Size currentSize { get; set; }

        private IList<int> columnSnapGrid = null;
        private IList<int> rowSnapGrid = null;

        public CustomWinForm(int id, Int32 style)
        {
            InitializeComponent();
            this.Id = id;
            this.Style = style;            
        }

        public void SetWindowName(string name)
        {
            // change the caption of the window
            NativeMethods.SendMessage(this.Handle, Constant.WM_SETTEXT, (IntPtr)name.Length, name);
        }

        public void SetWindowSize(Size newSize)
        {
            currentSize = newSize;
            NativeMethods.SetWindowPos(this.Handle, 0, 0, 0, newSize.Width, newSize.Height, (Int32)(Constant.SWP_NOMOVE));
        }

        public void SetWindowLocation(int x, int y)
        {
            NativeMethods.SetWindowPos(this.Handle, 0, x, y, 0, 0, (Int32)(Constant.SWP_NOSIZE));
        }

        private void onLocationChanged(object sender, EventArgs e)
        {
            int outLocationX;
            int outLocationY;
            if (performLocationSnap(this.Location.X, this.Location.Y, out outLocationX, out outLocationY))
            {
                this.Location = new Point(outLocationX, outLocationY);
            }
            else
            {
                if (onDelegatePosChangedEvt != null)
                {
                    delegatePosList.Add(LatestPos);
                    onDelegatePosChangedEvt(this, this.Location.X, this.Location.Y);
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();

                var cp = base.CreateParams;
                cp.Style |= (int)(this.Style & 0x7FFFFFFF);     // Avoid 0x8000000 (WS_POPUP windows style)
                
                return cp;
            }
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if ((UInt32)m.Msg == Constant.WM_SYSCOMMAND)
            {
                switch((UInt32)m.WParam)
                {
                    case Constant.SC_CLOSE:
                        if(onDelegateClosedEvt != null)
                        {
                            onDelegateClosedEvt(this);
                        }
                        return;
                    case Constant.SC_MINIMIZE:
                        if (onDelegateMinimizedEvt != null)
                        {
                            onDelegateMinimizedEvt(this);
                        }
                        return;
                    case Constant.SC_MAXIMIZE:
                        if (onDelegateMaximizedEvt != null)
                        {
                            onDelegateMaximizedEvt(this);
                        }
                        return;
                    case Constant.SC_RESTORE:
                        if (onDelegateRestoredEvt != null)
                        {
                            onDelegateRestoredEvt(this);
                        }
                        return;
                    default:
                        break;
                }
            }
            else if (m.Msg == Constant.WM_NCHITTEST)
            {
                // to allow move by clicking the window's body
                base.WndProc(ref m);
                //m.Result = new IntPtr(-1);        // past wndproc to parent

                if (m.Result.ToInt32() == (int)Constant.HitTest.Border)
                {
                    //m.Result = new IntPtr((int)Constant.HitTest.Caption);
                    currentSize = this.Size;
                }

                if (currentSize != this.Size)
                {
                    onDelegateSizeChangedEvt(this, this.Size);
                }

                return;
            }

            //Trace.WriteLine(String.Format("message: {0}", (UInt32)m.Msg));
            base.WndProc(ref m);
        }

        private void CustomWinForm_Load(object sender, EventArgs e)
        {
            this.Resize += CustomWinForm_Resize;
            this.MouseDown += CustomWinForm_MouseDown;
            this.MouseUp += CustomWinForm_MouseUp;
        }

        void CustomWinForm_MouseUp(object sender, MouseEventArgs e)
        {
            if(currentSize != this.Size)
            {
                onDelegateSizeChangedEvt(this, this.Size);
            }
        }

        void CustomWinForm_MouseDown(object sender, MouseEventArgs e)
        {
            if(this.Cursor != DefaultCursor)
            {
                currentSize = this.Size;
            }
            else if (e.Button == MouseButtons.Left)
            {
                Utils.Windows.NativeMethods.ReleaseCapture();
                Utils.Windows.NativeMethods.SendMessage(this.Handle, Constant.WM_NCLBUTTONDOWN, new IntPtr((int)Constant.HitTest.Caption), IntPtr.Zero);
            }
        }


        void CustomWinForm_Resize(object sender, EventArgs e)
        {
            // check if the size snap
            int snapWidth;
            int snapHeight;
            if (performSizeSnap(this.Location.X, this.Location.Y, this.Width, this.Height, out snapWidth, out snapHeight))
            {
                this.Width = snapWidth;
                this.Height = snapHeight;
            }
        }

        public void SetColumnSnapGrid(IList<int> columnGrid)
        {
            this.columnSnapGrid = columnGrid;
        }

        public void SetRowSnapGrid(IList<int> rowGrid)
        {
            this.rowSnapGrid = rowGrid;
        }

        private bool doSnap(int pos, int edge)
        {
            int delta = pos - edge;
            return delta > 0 && delta <= 15;     // within 10 pixels
        }

        private bool performSizeSnap(int xPos, int yPos, int width, int height, out int snapWidth, out int snapHeight)
        {
            snapWidth = width;
            snapHeight = height;

            if (columnSnapGrid == null ||
                rowSnapGrid == null)
            {
                return false;
            }

            bool snap = false;
            foreach (int column in columnSnapGrid)
            {
                if (doSnap(xPos + width, column))
                {
                    snap = true;
                    snapWidth = column - xPos;
                    break;
                }
            }

            foreach (int row in rowSnapGrid)
            {
                if (doSnap(yPos + height, row))
                {
                    snap = true;
                    snapHeight = row - yPos;
                    break;
                }
            }

            return snap;
        }

        private bool performLocationSnap(int xPos, int yPos, out int snapX, out int snapY)
        {
            snapX = xPos;
            snapY = yPos;

            if (columnSnapGrid == null ||
                rowSnapGrid == null)
            {
                return false;
            }

            bool snap = false;
            foreach (int column in columnSnapGrid)
            {
                if (doSnap(xPos, column))
                {
                    snap = true;
                    snapX = column;
                    break;
                }
            }

            foreach (int row in rowSnapGrid)
            {
                if (doSnap(yPos, row))
                {
                    snap = true;
                    snapY = row;
                    break;
                }
            }

            return snap;
        }
    }
}
