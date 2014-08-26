﻿using System;
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
        
        /// <summary>
        /// The total resolution of the control which allow to view set be server
        /// </summary>
        public Size VirtualSize 
        {
            get { return mVirtualSize; }
            set 
            { 
                mVirtualSize = value;

                mScaleX = (float)this.Width / (float)VirtualSize.Width;
                mScaleY = (float)this.Height / (float)VirtualSize.Height;
            }
        }

        /// <summary>
        /// the reference point of the origin of the control
        /// </summary>
        public int ReferenceXPos { get; set; }
        public int ReferenceYPos { get; set; }

        /// <summary>
        /// keep the size of the control if parent maximized
        /// </summary>
        public Size MaximizedSize { get; set; }

        /// <summary>
        /// keep the current control size, used when restore called
        /// </summary>
        public Size CurrentSize { get; set; }
        private bool isMaximized = false;

        public int MatrixRow { get; set; }
        public int MatrixColumn { get; set; }
        
        public struct ControlAttributes
        {
            public int Id { get; set; }
            public string WindowName { get; set; }
            public int Xpos { get; set; }
            public int Ypos { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Style { get; set; }
            public Size MinimumSize { get; set; }
            public int ZOrder { get; set; }
        }

        private Size mVirtualSize;

        private float mScaleX = 1.0f;
        private float mScaleY = 1.0f;

        /// <summary>
        /// store the child controls
        /// int: window's unique id from server
        /// CustomWinForm: mimic window created
        /// </summary>
        private Dictionary<int, CustomWinForm> mControlsDic;

        private Size mInvalidSize = new Size(-int.MaxValue, -int.MaxValue);
        private Point mInvalidPoint = new Point(-int.MaxValue, -int.MaxValue);

        public CustomControlHolder(Size maxSize, int relativeXpos, int relativeYPos)
        {
            InitializeComponent();

            mControlsDic = new System.Collections.Generic.Dictionary<int, CustomWinForm>();

            this.VirtualSize = maxSize;
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

        public void AddControl(ControlAttributes controlAttr)
        {
            CustomWinForm winForm = new CustomWinForm(controlAttr.Id, controlAttr.Style);
            Trace.WriteLine(String.Format("New form added: id:{5} {0} pos:{1},{2} zorder:{3} style:{4} size:{6},{7}",
                controlAttr.WindowName, controlAttr.Xpos, controlAttr.Ypos, controlAttr.ZOrder, controlAttr.Style, controlAttr.Id, controlAttr.Width, controlAttr.Height));

            this.Controls.Add(winForm);
            this.Controls.SetChildIndex(winForm, controlAttr.ZOrder);
            mControlsDic.Add(controlAttr.Id, winForm);

            winForm.SetWindowName(controlAttr.WindowName);
            winForm.Style = controlAttr.Style;

            if ((controlAttr.Style & Constant.WS_MINIMIZE) != 0)
            {
                // application in minimize mode
                // the value useful to cater the application restore from minimize state later
                winForm.ActualSize = mInvalidSize;
                winForm.SetWindowSize(mInvalidSize);

                winForm.ActualPos = mInvalidPoint;
                winForm.SetWindowLocation(mInvalidPoint.X, mInvalidPoint.Y);
            }
            else
            {
                winForm.ActualSize = new Size((int)controlAttr.Width, (int)controlAttr.Height);
                winForm.SetWindowSize(new Size((int)Math.Round((float)controlAttr.Width * mScaleX), (int)Math.Round((float)controlAttr.Height * mScaleY)));

                // set size and pos after add
                winForm.ActualPos = new Point(controlAttr.Xpos, controlAttr.Ypos);
                winForm.Location = getRelativePoint(controlAttr.Xpos, controlAttr.Ypos);
            }

            // register the event callback
            winForm.onDelegateClosedEvt += winForm_onDelegateClosedEvt;
            winForm.onDelegateMaximizedEvt += winForm_onDelegateMaximizedEvt;
            winForm.onDelegateMinimizedEvt += winForm_onDelegateMinimizedEvt;
            winForm.onDelegatePosChangedEvt += winForm_onDelegatePosChangedEvt;
            winForm.onDelegateRestoredEvt += winForm_onDelegateRestoredEvt;
            winForm.onDelegateSizeChangedEvt += winForm_onDelegateSizeChangedEvt;
        }

        void winForm_onDelegateSizeChangedEvt(CustomWinForm winForm, Size size)
        {
            if (winForm.ActualSize == mInvalidSize)
            {
                Trace.WriteLine("Invalid size, previous is minimized");
                return;
            }

            if (onDelegateSizeChangedEvt != null)
            {
                Size actualSize = new Size((int)Math.Round((float)size.Width / mScaleX), (int)Math.Round((float)size.Height / mScaleY));
                if (actualSize.Equals(winForm.ActualSize))
                {
                    return;
                }

                Trace.WriteLine(String.Format("size change: {2}, newActualsize:{0}, previousActualSize:{1}", actualSize, winForm.ActualSize, winForm.Id));
                winForm.ActualSize = actualSize;
                onDelegateSizeChangedEvt(winForm.Id, actualSize);
            } 
        }

        void winForm_onDelegateRestoredEvt(CustomWinForm winForm)
        {
            if (onDelegateRestoredEvt != null)
            {
                Trace.WriteLine(String.Format("delegate {0} restore", winForm.Name));
                onDelegateRestoredEvt(winForm.Id);
            }
        }

        void winForm_onDelegatePosChangedEvt(CustomWinForm winForm, int xPos, int yPos)
        {
            if (winForm.ActualPos == mInvalidPoint)
            {
                Trace.WriteLine("Invalid location, previous is minimized");
                return;
            }

            if (onDelegatePosChangedEvt != null)
            {
                
                Point actual = getActualPoint(xPos, yPos);
                if (actual.Equals(winForm.ActualPos))
                {
                    return;
                }
                Trace.WriteLine(String.Format("delegate {0} pos changed: {1},{2}, previous:{3},{4}", winForm.Name, xPos, yPos, winForm.ActualPos.X, winForm.ActualPos.Y));
                winForm.ActualPos = actual;
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
                if (control.ActualSize.Equals(newSize))
                {
                    // callback from server on size changed
                    return;
                }

                Size ratioSize = new Size((int)Math.Round((float)newSize.Width * mScaleX), 
                    (int)Math.Round((float)newSize.Height * mScaleY));

                control.ActualSize = newSize;
                if ((control.Style & Constant.WS_MINIMIZE) != 0)
                {
                    Trace.WriteLine("in minimize state, ignore sizing");
                    return;
                }

                control.SetWindowSize(ratioSize);
            }
        }

        public void ChangeControlPos(int id, Point newPos)
        {
             CustomWinForm control;
             if (mControlsDic.TryGetValue(id, out control))
             {
                 if (control.ActualPos.Equals(newPos))
                 {
                     return;
                 }

                 control.ActualPos = newPos;
                 Point ratioPoint = new Point((int)Math.Round((float)(newPos.X - ReferenceXPos) * mScaleX),
                    (int)Math.Round((float)(newPos.Y - ReferenceYPos) * mScaleY));

                 Trace.WriteLine(String.Format("ChangeControlPos {0} {1}", control.Name, ratioPoint));
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

        public void RefreshLayout()
        {
            HandleSizing();
        }

        /// <summary>
        /// force resize the layout using current setting
        /// location might change as well
        /// </summary>
        public void ForceRefreshLayout()
        {
            foreach (KeyValuePair<int, CustomWinForm> map in mControlsDic)
            {
                // change location
                Point ratioPoint = new Point((int)Math.Round((float)(map.Value.ActualPos.X - ReferenceXPos) * mScaleX),
                   (int)Math.Round((float)(map.Value.ActualPos.Y - ReferenceYPos) * mScaleY));
                map.Value.Location = ratioPoint;

                // change size
                map.Value.SetWindowSize(new Size((int)Math.Round((float)map.Value.ActualSize.Width * mScaleX), (int)Math.Round((float)map.Value.ActualSize.Height * mScaleY)));
            }
        }

        public void SetMaximized()
        {
            // update all controls
            mScaleX = (float)MaximizedSize.Width / (float)VirtualSize.Width;
            mScaleY = (float)MaximizedSize.Height / (float)VirtualSize.Height;
            this.Size = MaximizedSize;

            foreach (KeyValuePair<int, CustomWinForm> map in mControlsDic)
            {
                map.Value.SetWindowSize(new Size((int)Math.Round((float)map.Value.ActualSize.Width * mScaleX), (int)Math.Round((float)map.Value.ActualSize.Height * mScaleY)));
            }

            this.isMaximized = true;
        }

        public void SetRestore()
        {
            if (false == this.isMaximized)
            {
                return;
            }

            this.isMaximized = false;
            this.Size = CurrentSize;
            mScaleX = (float)CurrentSize.Width / (float)VirtualSize.Width;
            mScaleY = (float)CurrentSize.Height / (float)VirtualSize.Height;

            foreach (KeyValuePair<int, CustomWinForm> map in mControlsDic)
            {
                map.Value.SetWindowSize(new Size((int)Math.Round((float)map.Value.ActualSize.Width * mScaleX), (int)Math.Round((float)map.Value.ActualSize.Height * mScaleY)));
            }
        }

        private void HandleSizing()
        {
            float newScaleX = 0;
            if (VirtualSize.Width != 0)
            {
                newScaleX = (float)this.Width / (float)VirtualSize.Width;
            }

            float newScaleY = 0;
            if (VirtualSize.Height != 0)
            {
                newScaleY = (float)this.Height / (float)VirtualSize.Height;
            }

            if(newScaleX == mScaleX &&
                newScaleY == mScaleY)
            {
                return;
            }

            // update all controls
            mScaleX = newScaleX;
            mScaleY = newScaleY;

            foreach (KeyValuePair<int, CustomWinForm> map in mControlsDic)
            {
                map.Value.SetWindowSize(new Size((int)Math.Round((float)map.Value.ActualSize.Width * mScaleX), (int)Math.Round((float)map.Value.ActualSize.Height * mScaleY)));
            }
        }
    }
}
