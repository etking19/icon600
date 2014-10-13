using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormClient
{
    public partial class FormAddPreset : Form
    {
        private string presetName;

        public string PresetName
        {
            get
            {
                return presetName;
            }
        }

        public FormAddPreset()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            presetName = textBoxName.Text;
            if (presetName.Length == 0)
            {
                textBoxName.BackColor = Color.Red;
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        public void SetAppList(List<ApplicationEntry> appList)
        {
            ((ListBox)checkedListBoxApp).DataSource = new BindingSource(appList, null);
            ((ListBox)checkedListBoxApp).DisplayMember = "Name";
            ((ListBox)checkedListBoxApp).ValueMember = "Identifier";
        }

        public List<ApplicationEntry> GetSelectedAppList()
        {
            List<ApplicationEntry> appList = new List<ApplicationEntry>();

            foreach (object itemChecked in checkedListBoxApp.CheckedItems)
            {
                ApplicationEntry entry = itemChecked as ApplicationEntry;
                appList.Add(entry);
            }

            return appList;
        }

        public void SetVncList(List<VncEntry> vncList)
        {
            ((ListBox)checkedListBoxVnc).DataSource = new BindingSource(vncList, null);
            ((ListBox)checkedListBoxVnc).DisplayMember = "DisplayName";
            ((ListBox)checkedListBoxVnc).ValueMember = "Identifier";
        }

        public List<VncEntry> GetSelectedVncList()
        {
            List<VncEntry> vncList = new List<VncEntry>();

            foreach (object itemChecked in checkedListBoxVnc.CheckedItems)
            {
                VncEntry entry = itemChecked as VncEntry;
                vncList.Add(entry);
            }

            return vncList;
        }

        public void SetInputList(List<InputAttributes> inputList)
        {
            ((ListBox)checkedListBoxInput).DataSource = new BindingSource(inputList, null);
            ((ListBox)checkedListBoxInput).DisplayMember = "DisplayName";
            ((ListBox)checkedListBoxInput).ValueMember = "InputId";
        }

        public List<InputAttributes> GetSelectedInputList()
        {
            List<InputAttributes> inputList = new List<InputAttributes>();

            foreach (object itemChecked in checkedListBoxInput.CheckedItems)
            {
                InputAttributes entry = itemChecked as InputAttributes;
                inputList.Add(entry);
            }

            return inputList;
        }

        private void FormAddPreset_Load(object sender, EventArgs e)
        {
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
        }
    }
}
