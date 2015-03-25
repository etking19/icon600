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

    public partial class CustomControlHolder : UserControl, ICustomHolder
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

        public Size FullSize
        {
            private get
            {
                return mFullSize;
            }
            set
            {
                mFullSize = value;
            }
        }

        /// <summary>
        /// the reference point of the origin of the control
        /// </summary>
        public int ReferenceXPos { get; set; }
        public int ReferenceYPos { get; set; }

        private Size mVirtualSize;
        private Size mFullSize;

        private float mScaleX = 1.0f;
        private float mScaleY = 1.0f;

        /// <summary>
        /// store the child controls
        /// int: window's unique id from server
        /// CustomWinForm: mimic window created
        /// </summary>
        private Dictionary<int, CustomWinForm> mControlsDic;

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
            int actualX = (int)Math.Floor((float)x / mScaleX) + ReferenceXPos;
            int actualY = (int)Math.Floor((float)y / mScaleY) + ReferenceYPos;

            return new Point(actualX, actualY);
        }

        Point getRelativePoint(int x, int y)
        {
            double referenceX = x - ReferenceXPos;
            double referenceY = y - ReferenceYPos;
            int actualX = (int)Math.Ceiling(referenceX * mScaleX);
            int actualY = (int)Math.Ceiling(referenceY * mScaleY);

            return new Point(actualX, actualY);
        }

        Size getActualSize(int width, int height)
        {
            double actualWidth = Math.Floor((float)width / mScaleX);
            double actualHeight = Math.Floor((float)height / mScaleY);

            return new Size((int)actualWidth, (int)actualHeight);
        }

        Size getRelativeSize(int width, int height)
        {
            double relativeWidth = Math.Ceiling((float)width * mScaleX);
            double relativeHeight = Math.Ceiling((float)height * mScaleY);

            return new Size((int)relativeWidth, (int)relativeHeight);
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

                    Size actualSize = getActualSize(winForm.Size.Width, winForm.Size.Height); 
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

                        Size actualSize = getActualSize(winForm.Size.Width, winForm.Size.Height);
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
                CustomWinForm winForm = new CustomWinForm(controlAttr.Id, controlAttr.Style, this);
                mControlsDic.Add(controlAttr.Id, winForm);

                this.Controls.Add(winForm);
                this.Controls.SetChildIndex(winForm, controlAttr.ZOrder);

                // set the win size
                winForm.LatestSize = new Size(controlAttr.Width, controlAttr.Height);
                Size relativeSize = getRelativeSize(controlAttr.Width, controlAttr.Height);
                winForm.LatestRelativeSize = relativeSize;
                winForm.SetWindowSize(relativeSize);

                // set the win pos
                winForm.LatestPos = new Point(controlAttr.Xpos, controlAttr.Ypos);
                Point relativePoint = getRelativePoint(controlAttr.Xpos, controlAttr.Ypos);
                winForm.LatestRelativePos = relativePoint;
                winForm.SetWindowLocation(relativePoint.X, relativePoint.Y);

                // add the tooltip control
                windowToolTip.SetToolTip(winForm, controlAttr.WindowName);

                winForm.SetWindowName(controlAttr.WindowName);
                winForm.Style = (int)(controlAttr.Style);

                // register the event callback
                winForm.onDelegateClosedEvt += winForm_onDelegateClosedEvt;
                winForm.onDelegateMaximizedEvt += winForm_onDelegateMaximizedEvt;
                winForm.onDelegateMinimizedEvt += winForm_onDelegateMinimizedEvt;
                winForm.onDelegatePosChangedEvt += winForm_onDelegatePosChangedEvt;
                winForm.onDelegateRestoredEvt += winForm_onDelegateRestoredEvt;
                winForm.onDelegateSizeChangedEvt += winForm_onDelegateSizeChangedEvt;

                Trace.WriteLine(String.Format("Add control - pos:{0},{1} size:{2},{3} : {4},{5}",
                    relativePoint.X, relativePoint.Y, relativeSize.Width, relativeSize.Height, controlAttr.Width, controlAttr.Height));
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        void winForm_onDelegateSizeChangedEvt(CustomWinForm winForm, Size size)
        {
            if (winForm.LatestRelativeSize == size)
            {
                return;
            }

            if (onDelegateSizeChangedEvt != null)
            {
                // update the latest actual size passed to server, assuming the passing MUST succeed
                Point actualPos = getActualPoint(winForm.Location.X, winForm.Location.Y);
                Size actualSize = getActualSize(size.Width, size.Height);


                // TODO: check the edges which might snap to boundary
                int actualX, actualY;
                if (checkActualEdges(actualPos.X + actualSize.Width, actualPos.Y + actualSize.Height, out actualX, out actualY))
                {
                    int modifiedWidth = actualX - actualPos.X;
                    int modifiedHeight = actualY - actualPos.Y;

                    // modified the relative sizing
                    Size adjustedSize = getRelativeSize(modifiedWidth, modifiedHeight);
                    
                    //winForm.LatestRelativeSize = adjustedSize;
                    //winForm.SetWindowSize(adjustedSize);

                    Trace.WriteLine(String.Format("delegateSizeChanged - {0},{1} : {2},{3} modified: {4},{5}, previousRelativeSize: {6}, {7}, latestSize: {8},{9}",
                        size.Width, size.Height, actualSize.Width, actualSize.Height, modifiedWidth, modifiedHeight, winForm.LatestRelativeSize.Width, winForm.LatestRelativeSize.Height, size.Width, size.Height));

                    size = adjustedSize;

                    actualSize.Width = modifiedWidth;
                    actualSize.Height = modifiedHeight;
                }

                winForm.LatestRelativeSize = size;
                winForm.LatestSize = actualSize;
                onDelegateSizeChangedEvt(winForm.Id, new Size(actualSize.Width, actualSize.Height));
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
            if (winForm.LatestRelativePos.X == xPos &&
                winForm.LatestRelativePos.Y == yPos)
            {
                return;
            }

            if (onDelegatePosChangedEvt != null)
            {
                Point actual = getActualPoint(xPos, yPos);
                Size actualSize = getActualSize(winForm.Width, winForm.Height);

                // TODO: save the size as well?
                int adjustedX, adjustedY;
                if (checkActualEdges(actual.X, actual.Y, out adjustedX, out adjustedY))
                {
                    Trace.WriteLine(String.Format("delegatePosChange - {0},{1} : {2},{3}, adjusted {4},{5}",
                    xPos, yPos, actual.X, actual.Y, adjustedX, adjustedY));
                }

                winForm.LatestPos = new Point(adjustedX, adjustedY);
                winForm.LatestRelativePos = new Point(xPos, yPos);

                onDelegatePosChangedEvt(winForm.Id, adjustedX, adjustedY, actualSize.Width, actualSize.Height);
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

        private delegate void DelegateRemoveControl(int id);
        public void RemoveControl(int id)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateRemoveControl(RemoveControl), id);
                return;
            }

            try
            {
                CustomWinForm control;
                if (mControlsDic.TryGetValue(id, out control))
                {
                    this.Controls.Remove(control);
                    mControlsDic.Remove(id);
                }
            }
            catch (Exception)
            {
            }
            
        }

        public void RemoveAllControls()
        {
            try
            {
                foreach (KeyValuePair<int, CustomWinForm> keyValuePair in mControlsDic)
                {
                    this.Controls.Remove(keyValuePair.Value);
                }

                mControlsDic.Clear();
            }
            catch (Exception)
            {
            }
            
        }

        public void ChangeControlSize(int id, Size newSize)
        {
            try
            {
                CustomWinForm control;
                if (mControlsDic.TryGetValue(id, out control))
                {
                    if (control.LatestSize == newSize)
                    {
                        return;
                    }

                    Size ratioSize = getRelativeSize(newSize.Width, newSize.Height);

                    control.LatestSize = newSize;
                    control.LatestRelativeSize = ratioSize;
                    control.SetWindowSize(ratioSize);
                }
            }
            catch (Exception)
            {
            }
            
        }

        public void ChangeControlPos(int id, Point newPos)
        {
            try
            {
                CustomWinForm control;

                if (mControlsDic.TryGetValue(id, out control))
                {
                    Point ratioPoint = getRelativePoint(newPos.X, newPos.Y);

                    control.LatestPos = newPos;
                    control.LatestRelativePos = ratioPoint;
                    control.SetWindowLocation(ratioPoint.X, ratioPoint.Y);
                }
            }
            catch (Exception)
            {
            }
             
        }

        public void ChangeControlName(int id, String name)
        {
            try
            {
                CustomWinForm control;
                if (mControlsDic.TryGetValue(id, out control))
                {
                    windowToolTip.SetToolTip(control, name);
                    control.SetWindowName(name);
                }
            }
            catch (Exception)
            {

            }
            
        }

        public void ChangeControlStyle(int id, int Style)
        {
            try
            {
                CustomWinForm control;
                if (mControlsDic.TryGetValue(id, out control))
                {
                    control.Style = Style;
                }
            }
            catch (Exception)
            {
            }
            
        }

        public void ChangeControlZOrder(int id, int newZOrder)
        {
            try
            {
                CustomWinForm control;
                if (mControlsDic.TryGetValue(id, out control))
                {
                    this.Controls.SetChildIndex(control, newZOrder);
                }
            }
            catch (Exception)
            {
            }
            
        }

        
        public void RefreshLayout()
        {
            foreach (KeyValuePair<int, CustomWinForm> map in mControlsDic)
            {
                // change location
                Point ratioPoint = getRelativePoint(map.Value.LatestPos.X, map.Value.LatestPos.Y);
                map.Value.SetWindowLocation(ratioPoint.X, ratioPoint.Y);

                Size ratioSize = getRelativeSize(map.Value.LatestSize.Width, map.Value.LatestSize.Height);
                map.Value.SetWindowSize(ratioSize);
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
                Point ratioPoint = getRelativePoint(map.Value.LatestPos.X, map.Value.LatestPos.Y);
                map.Value.LatestRelativePos = ratioPoint;
                map.Value.SetWindowLocation(ratioPoint.X, ratioPoint.Y);

                Size ratioSize = getRelativeSize(map.Value.LatestSize.Width, map.Value.LatestSize.Height);
                map.Value.LatestRelativeSize = ratioSize;
                map.Value.SetWindowSize(ratioSize);
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

        private int _userSnapX = 0;
        private int _userSnapY = 0;

        private int _systemSnapX = 0;
        private int _systemSnapY = 0;

        public void SetUserSnap(int column, int row)
        {
            _userSnapX = column;
            _userSnapY = row;
        }

        public void SetSystemSnap(int column, int row)
        {
            _systemSnapX = column;
            _systemSnapY = row;
        }

        public bool performSizeSnapCheck(int xRelativePos, int yRelativePos, int xRelativeWidth, int yRelativeHeight, 
            out int xRelativeSnapPos, out int yRelativeSnapPos, out int relativeSnapWidth, out int relativeSnapHeight)
        {
            xRelativeSnapPos = xRelativePos;
            yRelativeSnapPos = yRelativePos;
            relativeSnapWidth = xRelativeWidth;
            relativeSnapHeight = yRelativeHeight;

            int adjustedWidth, adjustedHeight;
            if (checkRelativeEdges(xRelativePos + xRelativeWidth, yRelativePos + yRelativeHeight,
                out adjustedWidth, out adjustedHeight))
            {
                relativeSnapWidth = adjustedWidth - xRelativePos;
                relativeSnapHeight = adjustedHeight - yRelativePos;

                Trace.WriteLine(String.Format("performSizeSnapCheck initial: {0},{1}, modified:{2},{3} ",
                    xRelativeWidth, yRelativeHeight, relativeSnapWidth, relativeSnapHeight));
                return true;
            }

            return false;
        }

        public bool performLocationSnapCheck(int xRelativePos, int yRelativePos, out int snapX, out int snapY)
        {
            snapX = xRelativePos;
            snapY = yRelativePos;

            int adjustedX, adjustedY;
            if (checkRelativeEdges(xRelativePos, yRelativePos, out adjustedX, out adjustedY))
            {
                snapX = adjustedX;
                snapY = adjustedY;

                Trace.WriteLine(String.Format("performLocationSnapCheck initial: {0},{1}, modified:{2},{3} ",
                    xRelativePos, yRelativePos, adjustedX, adjustedY));

                return true;
            }

            return false;
        }

        private bool doSnapActual(int pos, int edge)
        {
            int delta = pos - edge;
            return (delta >= -20) && (delta <= 20) && (delta != 0);
        }

        private bool doSnapRelative(int pos, int edge)
        {
            int delta = pos - edge;
            return (delta >= -10) && (delta <= 10);
        }

        private bool checkRelativeEdges(int xPos, int yPos, out int xAdjustedPos, out int yAdjustedPos)
        {
            xAdjustedPos = xPos;
            yAdjustedPos = yPos;

            bool isSnapX = false;
            bool isSnapY = false;

            // check for user's setting first
            if (_userSnapX > 0)
            {
                for (int col = 0; col <= _userSnapX; col++)
                {
                    int snapXPos = this.Size.Width * col / _userSnapX;
                    if (doSnapRelative(xPos, snapXPos))
                    {
                        isSnapX = true;
                        xAdjustedPos = snapXPos;
                        break;
                    }
                }
            }

            if (_userSnapY > 0)
            {
                for (int row = 0; row <= _userSnapY; row++)
                {
                    int snapYPos = this.Size.Height * row / _userSnapY;
                    if (doSnapRelative(yPos, snapYPos))
                    {
                        isSnapY = true;
                        Trace.WriteLine(String.Format("check _userSnapY: {0} : {1}", yPos, snapYPos));
                        yAdjustedPos = snapYPos;
                        break;
                    }
                }
            }

            // check for system snap
            if (false == isSnapX)
            {
                for (int col = 0; col <= _systemSnapX; col++)
                {
                    int snapXPos = mFullSize.Width * col / _systemSnapX;
                    Point relativePt = getRelativePoint(snapXPos, 0);

                    if (doSnapRelative(xPos, relativePt.X))
                    {
                        Trace.WriteLine(String.Format("check snapX: {0}, {1}", relativePt.X, relativePt.Y));
                        isSnapX = true;
                        xAdjustedPos = relativePt.X;
                        break;
                    }
                }
            }

            if (false == isSnapY)
            {
                for (int row = 0; row <= _systemSnapY; row++)
                {
                    int snapYPos = mFullSize.Height * row / _systemSnapY;
                    Point relativePt = getRelativePoint(0, snapYPos);

                    if (doSnapRelative(yPos, relativePt.Y))
                    {
                        Trace.WriteLine(String.Format("check snapY: {0}, {1}", relativePt.X, relativePt.Y));
                        isSnapY = true;
                        yAdjustedPos = relativePt.Y;
                        break;
                    }
                }
            }

            return isSnapX | isSnapY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xPos">actual screen pos on server</param>
        /// <param name="yPos">actual screen pos on server</param>
        /// <param name="xAdjustedPos"></param>
        /// <param name="yAdjustedPos"></param>
        private bool checkActualEdges(int xPos, int yPos, out int xAdjustedPos, out int yAdjustedPos)
        {
            xAdjustedPos = xPos;
            yAdjustedPos = yPos;

            bool isSnapX = false;
            bool isSnapY = false;

            // check for user's setting first
            if (_userSnapX > 0)
            {
                for (int col = 0; col <= _userSnapX; col++)
                {
                    int snapXPos = mVirtualSize.Width * col / _userSnapX + ReferenceXPos;
                    if (doSnapActual(xPos, snapXPos))
                    {
                        isSnapX = true;
                        xAdjustedPos = snapXPos;
                        break;
                    }
                }
            }

            if (_userSnapY > 0)
            {
                for (int row = 0; row <= _userSnapY; row++)
                {
                    int snapYPos = mVirtualSize.Height * row / _userSnapY + ReferenceYPos;
                    if (doSnapActual(yPos, snapYPos))
                    {
                        isSnapY = true;
                        yAdjustedPos = snapYPos;
                        break;
                    }
                }
            }

            // check for system snap
            if (false == isSnapX)
            {
                for (int col = 0; col <= _systemSnapX; col++)
                {
                    int snapXPos = mFullSize.Width * col / _systemSnapX;
                    if (doSnapActual(xPos, snapXPos))
                    {
                        isSnapX = true;
                        xAdjustedPos = snapXPos;
                        break;
                    }
                }
            }

            if (false == isSnapY)
            {
                for (int row = 0; row <= _systemSnapY; row++)
                {
                    int snapYPos = mFullSize.Height * row / _systemSnapY;
                    if (doSnapActual(yPos, snapYPos))
                    {
                        isSnapY = true;
                        yAdjustedPos = snapYPos;
                        break;
                    }
                }
            }

            return isSnapX | isSnapY;
        }
    }
}
