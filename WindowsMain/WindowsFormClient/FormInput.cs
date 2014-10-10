using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WindowsFormClient
{
    public partial class FormInput : DockContent
    {
        public FormInput()
        {
            InitializeComponent();
            this.Load += FormInput_Load;
        }

        void FormInput_Load(object sender, EventArgs e)
        {
            listBoxInput.MouseDown += listBoxInput_MouseDown;
        }

        void listBoxInput_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBoxInput.Items.Count == 0)
                return;

            int index = listBoxInput.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                DragDropEffects dde1 = DoDragDrop(listBoxInput.Items[index], DragDropEffects.All);
            }
        }

        public void SetVisionInputList(List<InputAttributes> visionList)
        {
            listBoxInput.DataSource = new BindingSource(visionList, null);
            listBoxInput.DisplayMember = "DisplayName";
            listBoxInput.ValueMember = "InputId";
        }
    }
}
