using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Utils.Windows;

namespace CustomWinForm
{
    public partial class CustomControlHolder: UserControl
    {

        public delegate void OnControlSizeChanged(int id, Size newSize);
        public delegate void OnControlPosChanged(int id, int xPos, int yPos);
        public delegate void OnControlMinimize(int id);
        public delegate void OnControlMaximize(int id);
        public delegate void OnControlRestore(int id);
        public delegate void OnControlClose(int id);

        public event OnControlSizeChanged onDelegateSizeChangedEvt;
        public event OnControlPosChanged onDelegatePosChangedEvt;
        public event OnControlMinimize onDelegateMaximizedEvt;
        public event OnControlMaximize onDelegateMinimizedEvt;
        public event OnControlRestore onDelegateRestoredEvt;
        public event OnControlClose onDelegateClosedEvt;

        private Size mMaxSize;
        /// <summary>
        /// The total resolution of the control
        /// </summary>
        public Size MaxSize 
        {
            get { return mMaxSize; }
            set { mMaxSize = value; HandleSizing(); }
        }

        /// <summary>
        /// the reference point of the origin of the control
        /// </summary>
        public int ReferenceXPos { get; set; }
        public int ReferenceYPos { get; set; }
 
        public struct ControlAttributes
        {
            public string WindowName { get; set; }
            public int Xpos { get; set; }
            public int Ypos { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Style { get; set; }
            public Size MinimumSize { get; set; }
            public int ZOrder { get; set; }
        }

        private float mScaleX = 1.0f;
        private float mScaleY = 1.0f;

        private Dictionary<int, CustomWinForm> mControlsDic = new System.Collections.Generic.Dictionary<int, CustomWinForm>();

        public CustomControlHolder(Size maxSize, int relativeXpos, int relativeYPos)
        {
            InitializeComponent();

            this.MaxSize = maxSize;
            this.ReferenceXPos = relativeXpos;
            this.ReferenceYPos = relativeYPos;
        }

        Point getActualPoint(int x, int y)
        {
            int actualX = (int)Math.Round((float)x / mScaleX) + ReferenceXPos;
            int actualY = (int)Math.Round((float)y / mScaleY) + ReferenceYPos;

            return new Point(actualX, actualY);
        }

        Point getRelativePoint(int x, int y)
        {
            double referenceX = x - ReferenceXPos;
            double referenceY = y - ReferenceYPos;
            int actualX = (int)Math.Round(referenceX * mScaleX);
            int actualY = (int)Math.Round(referenceY * mScaleY);

            return new Point(actualX, actualY);
        }

        public int AddControl(ControlAttributes controlAttr)
        {
            int id = Guid.NewGuid().GetHashCode();
            CustomWinForm winForm = new CustomWinForm(id, controlAttr.Style);

            winForm.SetWindowName(controlAttr.WindowName);            
            this.Controls.Add(winForm);
            this.Controls.SetChildIndex(winForm, controlAttr.ZOrder);
            mControlsDic.Add(id, winForm);

            winForm.Style = controlAttr.Style;

            // set size and pos after add
            winForm.Location = getRelativePoint(controlAttr.Xpos, controlAttr.Ypos);

            winForm.Size = new Size((int)controlAttr.Width, (int)controlAttr.Height);
            winForm.Scale(new SizeF(mScaleX, mScaleY));

            winForm.MinimumSize = new System.Drawing.Size(1, 1);

            // register the event callback
            winForm.onDelegateClosedEvt += winForm_onDelegateClosedEvt;
            winForm.onDelegateMaximizedEvt += winForm_onDelegateMaximizedEvt;
            winForm.onDelegateMinimizedEvt += winForm_onDelegateMinimizedEvt;
            winForm.onDelegatePosChangedEvt += winForm_onDelegatePosChangedEvt;
            winForm.onDelegateRestoredEvt += winForm_onDelegateRestoredEvt;
            winForm.onDelegateSizeChangedEvt += winForm_onDelegateSizeChangedEvt;

            return id;
        }

        void winForm_onDelegateSizeChangedEvt(CustomWinForm winForm, Size size)
        {
            if (onDelegateSizeChangedEvt != null)
            {
                Size actualSize = new Size((int)Math.Round((float)size.Width / mScaleX), (int)Math.Round((float)size.Height / mScaleY));
                onDelegateSizeChangedEvt(winForm.Id, actualSize);
            }
        }

        void winForm_onDelegateRestoredEvt(CustomWinForm winForm)
        {
            if (onDelegateRestoredEvt != null)
            {
                onDelegateRestoredEvt(winForm.Id);
            }
        }

        void winForm_onDelegatePosChangedEvt(CustomWinForm winForm, int xPos, int yPos)
        {
            if (onDelegatePosChangedEvt != null)
            {
                Point actual = getActualPoint(xPos, yPos);
                onDelegatePosChangedEvt(winForm.Id, actual.X, actual.Y);
            }
        }

        void winForm_onDelegateMinimizedEvt(CustomWinForm winForm)
        {
            if (onDelegateMinimizedEvt != null)
            {
                onDelegateMinimizedEvt(winForm.Id);
            }
        }

        void winForm_onDelegateMaximizedEvt(CustomWinForm winForm)
        {
            if (onDelegateMaximizedEvt != null)
            {
                onDelegateMaximizedEvt(winForm.Id);
            }
        }

        void winForm_onDelegateClosedEvt(CustomWinForm winForm)
        {
            if (onDelegateClosedEvt != null)
            {
                onDelegateClosedEvt(winForm.Id);
            }
        }

        public void RemoveControl(int id)
        {
            CustomWinForm control;
            if(mControlsDic.TryGetValue(id, out control))
            {
                this.Controls.Remove(control);
            }
        }

        public void RemoveAllControls()
        {
            foreach (KeyValuePair<int, CustomWinForm> keyValuePair in mControlsDic)
            {
                this.Controls.Remove(keyValuePair.Value);
            }

            mControlsDic.Clear();
        }

        public void ChangeControlSize(int id, Size newSize)
        {
            CustomWinForm control;
            if (mControlsDic.TryGetValue(id, out control))
            {
                Size ratioSize = new Size((int)Math.Round((float)newSize.Width * mScaleX), 
                    (int)Math.Round((float)newSize.Height * mScaleY));

                control.Size = ratioSize;
            }
        }

        public void ChangeControlPos(int id, Point newPos)
        {
             CustomWinForm control;
             if (mControlsDic.TryGetValue(id, out control))
             {
                 Point ratioPoint = new Point((int)Math.Round((float)(newPos.X - ReferenceXPos) * mScaleX),
                    (int)Math.Round((float)(newPos.Y - ReferenceYPos) * mScaleY));
                 control.Location = ratioPoint;
             }
        }

        public void ChangeControlName(int id, String name)
        {
            CustomWinForm control;
            if (mControlsDic.TryGetValue(id, out control))
            {
                control.SetWindowName(name);
            }
        }

        public void ChangeControlStyle(int id, int Style)
        {
            CustomWinForm control;
            if (mControlsDic.TryGetValue(id, out control))
            {
                control.Style = Style;
            }
        }

        public void ChangeControlZOrder(int id, int newZOrder)
        {
            CustomWinForm control;
            if (mControlsDic.TryGetValue(id, out control))
            {
                this.Controls.SetChildIndex(control, newZOrder);
            }
        }

        private void onSizeChanged(object sender, EventArgs e)
        {
            HandleSizing();
        }

        private void HandleSizing()
        {
            if (MaxSize.Width != 0)
            {
                mScaleX = (float)this.Width / (float)MaxSize.Width;
            }

            if (MaxSize.Height != 0)
            {
                mScaleY = (float)this.Height / (float)MaxSize.Height;
            }

            // update all controls
            foreach (KeyValuePair<int, CustomWinForm> map in mControlsDic)
            {
                map.Value.Scale(new SizeF(mScaleX, mScaleY));
            }
        }
    }
}
