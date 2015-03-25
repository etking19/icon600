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
        public Size LatestRelativeSize { get; set; }

        public Point LatestPos { get; set; }
        public Point LatestRelativePos { get; set; }

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

        private ICustomHolder _customHolder;

        public CustomWinForm(int id, Int32 style, ICustomHolder reference)
        {
            InitializeComponent();
            this.Id = id;
            this.Style = style;
            this._customHolder = reference;
        }

        private void CustomWinForm_Load(object sender, EventArgs e)
        {
            // handle grid capturing
            this.LocationChanged += CustomWinForm_LocationChanged;
            this.Resize += CustomWinForm_Resize;
        }

        void CustomWinForm_LocationChanged(object sender, EventArgs e)
        {
            int outLocationX;
            int outLocationY;
            if (performLocationSnap(this.Location.X, this.Location.Y, out outLocationX, out outLocationY))
            {
                if (this.Location.X != outLocationX ||
                    this.Location.Y != outLocationY)
                {
                    // only handle the same snap one time
                    this.Location = new Point(outLocationX, outLocationY);
                }
            }
        }

        void CustomWinForm_Resize(object sender, EventArgs e)
        {
            // check if the size snap
            int snapX, snapY;
            int snapWidth;
            int snapHeight;
            if (performSizeSnap(this.Location.X, this.Location.Y, this.Width, this.Height, 
                out snapX, out snapY, out snapWidth, out snapHeight))
            {
                // only handle width and height, locationChange event should handle the X and Y coordinate change
                if (this.Width != snapWidth ||
                    this.Height != snapHeight)
                {
                    this.Width = snapWidth;
                    this.Height = snapHeight;
                }

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
                _resizingTimer.Stop();
                _resizingTimer.Start();
            }
            else
            {
                // timer to cater resizing event where fire size changed event after certain time idle
                _resizingTimer = new Timer();
                _resizingTimer.Interval = 500;
                _resizingTimer.Tick += (senderTimer, evt) => ResizeFinished();
            }
        }

        private void ResizeFinished()
        {
            _resizingTimer.Stop();

            onDelegateSizeChangedEvt(this, this.Size);
        }

        public void SetWindowName(string name)
        {
            // change the caption of the window
            NativeMethods.SendMessage(this.Handle, Constant.WM_SETTEXT, (IntPtr)name.Length, name);
        }

        public void SetWindowSize(Size newSize)
        {
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

        private bool performSizeSnap(int xPos, int yPos, int width, int height, 
            out int snapX, out int snapY, out int snapWidth, out int snapHeight)
        {
            return _customHolder.performSizeSnapCheck(xPos, yPos, width, height, out snapX, out snapY, out snapWidth, out snapHeight);
        }

        private bool performLocationSnap(int xPos, int yPos, out int snapX, out int snapY)
        {
            return _customHolder.performLocationSnapCheck(xPos, yPos, out snapX, out snapY);
        }
    }
}
