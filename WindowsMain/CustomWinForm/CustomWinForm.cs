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

        private Int32 style;
        public Int32 Style 
        { 
            get { return style; }
            set 
            { 
                style = value;
                UpdateStyles();
            } 
        }

        private IList<int> columnSnapGrid = null;
        private IList<int> rowSnapGrid = null;

        /// <summary>
        /// timer to keep track user activity for resizing
        /// </summary>
        private Timer _resizingTimer = null;

        public CustomWinForm(int id, Int32 style)
        {
            InitializeComponent();
            this.Id = id;
            this.Style = style;
        }

        private void CustomWinForm_Load(object sender, EventArgs e)
        {
            // handle grid capturing
            this.LocationChanged += CustomWinForm_LocationChanged;
            this.Resize += CustomWinForm_Resize;

            // timer to cater resizing event where fire size changed event after certain time idle
            _resizingTimer = new Timer();
            _resizingTimer.Interval = 500;
            _resizingTimer.Tick += (senderTimer, evt) => ResizeFinished();
        }

        void CustomWinForm_LocationChanged(object sender, EventArgs e)
        {
            int outLocationX;
            int outLocationY;
            if (performLocationSnap(this.Location.X, this.Location.Y, out outLocationX, out outLocationY))
            {
                this.Location = new Point(outLocationX, outLocationY);
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

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if(this.Size.Width == 0 ||
                this.Size.Height == 0)
            {
                return;
            }

            if (_resizingTimer != null)
            {
                _resizingTimer.Start();
            }
        }

        private void ResizeFinished()
        {
            _resizingTimer.Stop();

            onDelegateSizeChangedEvt(this, Size);
        }

        public void SetWindowName(string name)
        {
            // change the caption of the window
            NativeMethods.SendMessage(this.Handle, Constant.WM_SETTEXT, (IntPtr)name.Length, name);
        }

        public void SetWindowSize(Size newSize)
        {
            Trace.WriteLine("new size: " + newSize.Width + "," + newSize.Height);
            NativeMethods.SetWindowPos(this.Handle, 0, 0, 0, newSize.Width, newSize.Height, (Int32)(Constant.SWP_NOMOVE));
        }

        public void SetWindowLocation(int x, int y)
        {
            NativeMethods.SetWindowPos(this.Handle, 0, x, y, 0, 0, (Int32)(Constant.SWP_NOSIZE));
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
                base.WndProc(ref m);

                if (m.Result.ToInt32() == (int)Constant.HitTest.Client)
                {
                    // allow to move by clicking client area
                    m.Result = new IntPtr((int)Constant.HitTest.Caption);
                }

                return;
            }
            else if (m.Msg == Constant.WM_EXITSIZEMOVE)
            {
                // notify when user finished move the position
                onDelegatePosChangedEvt(this, Location.X, Location.Y);
            }

            base.WndProc(ref m);
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
            return delta >= -15 && delta <= 15;     // within 10 pixels
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
                    Trace.WriteLine("snapWidth " + snapWidth);
                    break;
                }
            }

            foreach (int row in rowSnapGrid)
            {
                if (doSnap(yPos + height, row))
                {
                    snap = true;
                    snapHeight = row - yPos;
                    Trace.WriteLine("snapHeight " + snapHeight);
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
