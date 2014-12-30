using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Utils.Windows;

namespace CustomWinForm
{
    public class ControlAttributes
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

    public partial class CustomControlHolder: UserControl
    {

        public delegate void OnControlSizeChanged(int id, Size newSize);
        public delegate void OnControlPosChanged(int id, int xPos, int yPos, int width, int height);
        public delegate void OnControlMinimize(int id);
        public delegate void OnControlMaximize(int id);
        public delegate void OnControlRestore(int id);
        public delegate void OnControlClose(int id);

        public event OnControlSizeChanged onDelegateSizeChangedEvt;
        public event OnControlPosChanged onDelegatePosChangedEvt;
        public event OnControlMaximize onDelegateMaximizedEvt;
        public event OnControlMinimize onDelegateMinimizedEvt;
        public event OnControlRestore onDelegateRestoredEvt;
        public event OnControlClose onDelegateClosedEvt;

        private ToolTip windowToolTip;

        /// <summary>
        /// keep the row and column grid view (reference size) so can perform snap to border feature
        /// </summary>
        private IList<int> columnGrid = null;
        private IList<int> rowGrid = null;

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

        private Size mVirtualSize;

        private float mScaleX = 1.0f;
        private float mScaleY = 1.0f;

        /// <summary>
        /// store the child controls
        /// int: window's unique id from server
        /// CustomWinForm: mimic window created
        /// </summary>
        private Dictionary<int, CustomWinForm> mControlsDic;

        //private Size mInvalidSize = new Size(-int.MaxValue, -int.MaxValue);
        //private Point mInvalidPoint = new Point(-int.MaxValue, -int.MaxValue);

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

        public ControlAttributes GetControl(int controlId)
        {
            ControlAttributes attr = new ControlAttributes();

            foreach (Control control in this.Controls)
            {
                CustomWinForm winForm = control as CustomWinForm;
                if (winForm != null &&
                    winForm.Id == controlId)
                {
                    attr.Id = winForm.Id;

                    Point actualPt = getActualPoint(winForm.Location.X, winForm.Location.Y);
                    attr.Xpos = actualPt.X;
                    attr.Ypos = actualPt.Y;

                    Size actualSize = new Size((int)Math.Round((float)winForm.Size.Width / mScaleX), (int)Math.Round((float)winForm.Size.Height / mScaleY));
                    attr.Width = actualSize.Width;
                    attr.Height = actualSize.Height;

                    attr.Style = winForm.Style;

                    break;
                }
            }

            return attr;
        }

        public ControlAttributes GetTopMostControl()
        {
            ControlAttributes attr = new ControlAttributes();

            int largestIndex = 0;
            foreach (Control control in this.Controls)
            {
                CustomWinForm winForm = control as CustomWinForm;
                if (winForm != null)
                {
                    int index = this.Controls.IndexOf(control);
                    if (index >= largestIndex)
                    {
                        largestIndex = index;

                        // update the attr
                        attr.Id = winForm.Id;

                        Point actualPt = getActualPoint(winForm.Location.X, winForm.Location.Y);
                        attr.Xpos = actualPt.X;
                        attr.Ypos = actualPt.Y;

                        Size actualSize = new Size((int)Math.Round((float)winForm.Size.Width / mScaleX), (int)Math.Round((float)winForm.Size.Height / mScaleY));
                        attr.Width = actualSize.Width;
                        attr.Height = actualSize.Height;

                        attr.Style = winForm.Style;
                    }
                }
            }

            return attr;
        }

        public void AddControl(ControlAttributes controlAttr)
        {
            try
            {
                CustomWinForm winForm = new CustomWinForm(controlAttr.Id, controlAttr.Style);

                this.Controls.Add(winForm);
                this.Controls.SetChildIndex(winForm, controlAttr.ZOrder);

                // add the tooltip control
                windowToolTip.SetToolTip(winForm, controlAttr.WindowName);

                mControlsDic.Add(controlAttr.Id, winForm);
                winForm.SetColumnSnapGrid(this.columnGrid);
                winForm.SetRowSnapGrid(this.rowGrid);

                winForm.SetWindowName(controlAttr.WindowName);
                winForm.Style = (int)(controlAttr.Style);

                // set the win size
                winForm.LatestSize = new Size(controlAttr.Width, controlAttr.Height);
                winForm.SetWindowSize(new Size((int)Math.Round((float)controlAttr.Width * mScaleX), (int)Math.Round((float)controlAttr.Height * mScaleY)));

                // set the win pos
                winForm.LatestPos = new Point(controlAttr.Xpos, controlAttr.Ypos);
                Point relativePoint = getRelativePoint(controlAttr.Xpos, controlAttr.Ypos);
                winForm.SetWindowLocation(relativePoint.X, relativePoint.Y);

                // register the event callback
                winForm.onDelegateClosedEvt += winForm_onDelegateClosedEvt;
                winForm.onDelegateMaximizedEvt += winForm_onDelegateMaximizedEvt;
                winForm.onDelegateMinimizedEvt += winForm_onDelegateMinimizedEvt;
                winForm.onDelegatePosChangedEvt += winForm_onDelegatePosChangedEvt;
                winForm.onDelegateRestoredEvt += winForm_onDelegateRestoredEvt;
                winForm.onDelegateSizeChangedEvt += winForm_onDelegateSizeChangedEvt;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
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

                // forward the size as well for boundary checking
                Size actualSize = new Size((int)Math.Round((float)winForm.Size.Width / mScaleX), 
                    (int)Math.Round((float)winForm.Size.Height / mScaleY));

                onDelegatePosChangedEvt(winForm.Id, actual.X, actual.Y, actualSize.Width, actualSize.Height);
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
                mControlsDic.Remove(id);
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

                control.LatestSize = newSize;
                control.SetWindowSize(ratioSize);
            }
        }

        public void ChangeControlPos(int id, Point newPos)
        {
             CustomWinForm control;

             if (mControlsDic.TryGetValue(id, out control))
             {
                 Point ratioPoint = new Point((int)Math.Round((float)(newPos.X - ReferenceXPos) * mScaleX),
                    (int)Math.Round((float)(newPos.Y - ReferenceYPos) * mScaleY));

                 control.LatestPos = newPos;
                 control.Location = ratioPoint;
             }
        }

        public void ChangeControlName(int id, String name)
        {
            CustomWinForm control;
            if (mControlsDic.TryGetValue(id, out control))
            {
                windowToolTip.SetToolTip(control, name);
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
            foreach (KeyValuePair<int, CustomWinForm> map in mControlsDic)
            {
                // change location
                Point ratioPoint = new Point((int)Math.Round((float)(map.Value.LatestPos.X - ReferenceXPos) * mScaleX),
                   (int)Math.Round((float)(map.Value.LatestPos.Y - ReferenceYPos) * mScaleY));
                map.Value.Location = ratioPoint;

                map.Value.SetWindowSize(new Size((int)Math.Round((float)map.Value.LatestSize.Width * mScaleX), (int)Math.Round((float)map.Value.LatestSize.Height * mScaleY)));
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
                // change location
                Point ratioPoint = new Point((int)Math.Round((float)(map.Value.LatestPos.X - ReferenceXPos) * mScaleX),
                   (int)Math.Round((float)(map.Value.LatestPos.Y - ReferenceYPos) * mScaleY));
                map.Value.Location = ratioPoint;

                map.Value.SetWindowSize(new Size((int)Math.Round((float)map.Value.LatestSize.Width * mScaleX), (int)Math.Round((float)map.Value.LatestSize.Height * mScaleY)));
            }
        }

        private void CustomControlHolder_Load(object sender, EventArgs e)
        {
            this.SizeChanged += CustomControlHolder_SizeChanged;

            // Create the ToolTip and associate with the Form container.
            windowToolTip = new ToolTip();

            // Set up the delays for the ToolTip.
            windowToolTip.AutoPopDelay = 2000;
            windowToolTip.InitialDelay = 500;
            windowToolTip.ReshowDelay = 500;
            windowToolTip.ShowAlways = true;
        }

        void CustomControlHolder_SizeChanged(object sender, EventArgs e)
        {
            HandleSizing();
        }

        public void SetSnapGrid(IList<int> columnGrid, IList<int> rowGrid)
        {
            this.columnGrid = columnGrid;
            this.rowGrid = rowGrid;

            foreach (KeyValuePair<int, CustomWinForm> map in mControlsDic)
            {
                map.Value.SetColumnSnapGrid(columnGrid);
                map.Value.SetRowSnapGrid(rowGrid);
            }
        }
    }
}
