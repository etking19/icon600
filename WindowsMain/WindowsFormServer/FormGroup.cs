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
    public partial class FormGroup : Form
    {
        private string initialName;

        private string groupName;
        private bool wholeDesktop;
        private bool monitorArea;
        private bool allowMaintenance;
        private int monitorId;

        public string GroupName
        {
            get { return groupName; }
            set { textBoxGroupName.Text = value; }
        }

        public bool AllowMaintenance
        {
            get { return allowMaintenance; }
            set { checkBoxMaintenance.Checked = value; }
        }

        public bool WholeDesktop 
        {
            get { return wholeDesktop; }
            set 
            { 
                radioButtonDesktop.Checked = value;
                radioButtonMonitor.Checked = !value;
            }
        }

        public bool MonitorArea
        {
            get { return monitorArea; }
            set 
            { 
                radioButtonMonitor.Checked = value;
                radioButtonDesktop.Checked = !value;
            }
        }

        public int MonitorId
        {
            get { return monitorId; }
            set { comboBoxMonitors.SelectedValue = value; }
        }

        public string MonitorText
        {
            set { comboBoxMonitors.Text = value; }
        }

        public bool IsDirty { get; set; }

        public FormGroup(string initialName)
        {
            InitializeComponent();
            this.initialName = initialName;

            this.Load += FormGroup_Load;
        }

        void FormGroup_Load(object sender, EventArgs e)
        {
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;

            if (radioButtonDesktop.Checked == false &&
                radioButtonMonitor.Checked == false)
            {
                radioButtonDesktop.Checked = true;
                radioButtonMonitor.Checked = false;
                comboBoxMonitors.Enabled = false;
            }
            

            radioButtonDesktop.CheckedChanged += radioButtonDesktop_CheckedChanged;
            radioButtonMonitor.CheckedChanged += radioButtonMonitor_CheckedChanged;
            comboBoxMonitors.SelectedValueChanged += comboBoxMonitors_SelectedValueChanged;

            checkedListBoxApplications.ItemCheck += checkedListBoxApplications_ItemCheck;
            checkedListBoxApplications.CheckOnClick = true;

            textBoxGroupName.TextChanged += textBoxGroupName_TextChanged;
            checkBoxMaintenance.CheckStateChanged += checkBoxMaintenance_CheckStateChanged;

            this.IsDirty = false;
        }

        void checkedListBoxApplications_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.IsDirty = true;
        }

        void checkBoxMaintenance_CheckStateChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        void textBoxGroupName_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        void radioButtonDesktop_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            wholeDesktop = radioBtn.Checked;

            this.IsDirty = true;
        }

        void radioButtonMonitor_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            monitorArea = radioBtn.Checked;
            comboBoxMonitors.Enabled = radioBtn.Checked;

            this.IsDirty = true;
        }

        void comboBoxMonitors_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            if (cmb.SelectedItem == null)
            {
                return;
            }

            KeyValuePair<int, string> selectedCar = (KeyValuePair<int, string>)cmb.SelectedItem;
            monitorId = selectedCar.Key;

            this.IsDirty = true;
        }

        public void SetMonitorList(Dictionary<int, string> monitorsList)
        {
            comboBoxMonitors.DataSource = new BindingSource(monitorsList.ToList(), null);
            comboBoxMonitors.DisplayMember = "Value";
            comboBoxMonitors.ValueMember = "Key";
        }

        public void SetApplicationList(Dictionary<int, string> applicationsList)
        {
            if (applicationsList.Count > 0)
            {
                ((ListBox)checkedListBoxApplications).DataSource = new BindingSource(applicationsList.ToList(), null);
                ((ListBox)checkedListBoxApplications).DisplayMember = "Value";
                ((ListBox)checkedListBoxApplications).ValueMember = "Key";
            }
        }

        public List<int> GetSelectedApplicationsId()
        {
            List<int> appsId = new List<int>();

            foreach (object itemChecked in checkedListBoxApplications.CheckedItems)
            {
                KeyValuePair<int, string> selectedCar = (KeyValuePair<int, string>)itemChecked;
                appsId.Add(selectedCar.Key);
            }
            return appsId;
        }

        public void SetSelectedApplications(List<int> appsId)
        {
            for (int i = 0; i < checkedListBoxApplications.Items.Count; i++)
            {
                KeyValuePair<int, string> selectedCar = (KeyValuePair<int, string>)checkedListBoxApplications.Items[i];
                if(appsId.Contains(selectedCar.Key))
                {
                    checkedListBoxApplications.SetItemChecked(i, true);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // do nothing
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // validate entry
            bool error = false;
            if (textBoxGroupName.Text == String.Empty)
            {
                textBoxGroupName.BackColor = Color.Red;
                error = true;
            }

            if(radioButtonMonitor.Checked && 
                comboBoxMonitors.SelectedValue == null)
            {
                error = true;
            }

            if (error)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (initialName.CompareTo(textBoxGroupName.Text) != 0)
            {
                // 2. check if database contain same group name
                try
                {
                    // should throw error if no same username found
                    Server.ServerDbHelper.GetInstance().GetAllGroups().First(GroupData => GroupData.name.CompareTo(textBoxGroupName.Text) == 0);

                    // error occurred
                    textBoxGroupName.BackColor = Color.Red;
                    MessageBox.Show("There is a same group name registered in the system.");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
                catch (Exception)
                {
                    // no same username, proceed
                }
            }

            groupName = textBoxGroupName.Text;
            allowMaintenance = checkBoxMaintenance.Checked;
            wholeDesktop = radioButtonDesktop.Checked;
            monitorArea = radioButtonMonitor.Checked;

            if(comboBoxMonitors.SelectedValue == null)
            {
                monitorId = -1;
            }
            else
            {
                monitorId = (int)comboBoxMonitors.SelectedValue;
            }
            
        }
    }
}
