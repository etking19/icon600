using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CustomUI
{
    public delegate void CheckBoxClickedHandler(DataGridView view, bool state);

    public class DataGridViewCheckBoxHeaderCellEventArgs : EventArgs
    {
        DataGridView _view;
        bool _bChecked;
        public DataGridViewCheckBoxHeaderCellEventArgs(DataGridView view, bool bChecked)
        {
            _view = view;
            _bChecked = bChecked;
        }
        public bool Checked
        {
            get { return _bChecked; }
        }

        public DataGridView DataGridView
        {
            get { return _view; }
        }
    }
    public class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        Point checkBoxLocation;
        Size checkBoxSize;
        bool _checked = false;
        Point _cellLocation = new Point();
        System.Windows.Forms.VisualStyles.CheckBoxState _cbState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
        public event CheckBoxClickedHandler OnCheckBoxClicked;

        public DatagridViewCheckBoxHeaderCell()
        {
        }

        protected override void Paint(System.Drawing.Graphics graphics, 
            System.Drawing.Rectangle clipBounds, 
            System.Drawing.Rectangle cellBounds, 
            int rowIndex, 
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle, 
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
            dataGridViewElementState, value,
            formattedValue, errorText, cellStyle,
            advancedBorderStyle, paintParts);

            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(graphics, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            p.X = cellBounds.Location.X + (cellBounds.Width / 2) - (s.Width / 2);
            p.Y = cellBounds.Location.Y + (cellBounds.Height / 2) - (s.Height / 2);
            _cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;

            if (_checked)
                _cbState = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;
            else
                _cbState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;

            CheckBoxRenderer.DrawCheckBox(graphics, checkBoxLocation, _cbState);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
            if (p.X >= checkBoxLocation.X && 
                p.X <= checkBoxLocation.X + checkBoxSize.Width && 
                p.Y >= checkBoxLocation.Y && p.Y <= checkBoxLocation.Y + checkBoxSize.Height)
            {
                _checked = !_checked;
                if (OnCheckBoxClicked != null)
                {
                    OnCheckBoxClicked(this.DataGridView, _checked);
                    this.DataGridView.InvalidateCell(this);
                }
            }
            base.OnMouseClick(e);
        }


        /// <summary>
        /// To avoid event not fire when setting data property name
        /// Reference: http://techisolutions.blogspot.com/2008/02/datagridview-checkbox-select-all.html
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DatagridViewCheckBoxHeaderCell obj = (DatagridViewCheckBoxHeaderCell)base.Clone();
            obj.checkBoxLocation = checkBoxLocation;
            obj.checkBoxSize = checkBoxSize;
            obj._checked = _checked;
            obj._cellLocation = _cellLocation;
            obj._cbState = _cbState;
            obj.OnCheckBoxClicked = OnCheckBoxClicked;
            return obj;
        }
    }

}
