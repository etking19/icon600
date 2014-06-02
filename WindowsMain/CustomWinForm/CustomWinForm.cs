using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Text;
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

        public CustomWinForm(int id, Int32 style)
        {
            InitializeComponent();
            this.Id = id;
            this.Style = style;
        }

        public void SetWindowName(string name)
        {
            // change the caption of the window
            User32.SendMessage(this.Handle, Constant.WM_SETTEXT, (IntPtr)name.Length, name);
        }

        private IntPtr MakeLParam(int LoWord, int HiWord)
        {
            int i = (HiWord << 16) | (LoWord & 0xffff);
            return new IntPtr(i);
        }

        private void onSizeChangedEvt(object sender, EventArgs e)
        {
            if (onDelegateSizeChangedEvt != null)
            {
                onDelegateSizeChangedEvt(this, this.Size);
                Trace.WriteLine(String.Format("Size changed: {0},{1}", this.Size.Width, this.Size.Height));
            }        
        }

        private void onLocationChanged(object sender, EventArgs e)
        {
            if (onDelegatePosChangedEvt != null)
            {
                onDelegatePosChangedEvt(this, this.Location.X, this.Location.Y);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();

                var cp = base.CreateParams;
                cp.Style |= this.Style;
                
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
            else if ((UInt32)m.Msg == Constant.WM_ACTIVATEAPP)
            {
                bool activated = ((Int32)m.WParam != 0);
                Trace.WriteLine(String.Format("activation {0}", activated));
            }
            else if (m.Msg == Constant.WM_NCHITTEST)
            {
                base.WndProc(ref m);
                if ((int)m.Result == 1)
                {
                    m.Result = (IntPtr)2;
                }

                return;
            }

            //Trace.WriteLine(String.Format("message: {0}", m.Msg));
            base.WndProc(ref m);
        }
    }
}
