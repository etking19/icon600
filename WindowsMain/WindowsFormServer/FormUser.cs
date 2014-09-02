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
    public partial class FormUser : Form
    {
        private string initialName;

        private string displayName;
        private string username;
        private string password;
        private int groupId;

        public string DisplayName 
        { 
            get
            {
                return displayName;
            }
            set
            {
                textBoxDisplayName.Text = value;
            }
        }

        public string UserName 
        { 
            get
            {
                return username;
            }
            set
            {
                textBoxUsername.Text = value;
            }
        }

        public string Password 
        { 
            get
            {
                return password;
            }
            set
            {
                textBoxPassword.Text = value;
            }
        }

        public int SelectedGroupId 
        {
            get
            {
                return groupId;
            }
            set
            {
                comboBoxGroup.SelectedValue = value;
            }
        }

        public string SelectedGroupName
        {
            set
            {
                comboBoxGroup.Text = value;
            }
        }

        public bool IsDirty { get; set; }

        public FormUser(string initialName)
        {
            InitializeComponent();
            this.initialName = initialName;

            this.Load += FormUser_Load;
        }

        void FormUser_Load(object sender, EventArgs e)
        {
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;

            comboBoxGroup.SelectedIndexChanged += comboBoxGroup_SelectedIndexChanged;
            textBoxDisplayName.TextChanged += textBoxDisplayName_TextChanged;
            textBoxUsername.TextChanged += textBoxUsername_TextChanged;
            textBoxPassword.TextChanged += textBoxPassword_TextChanged;

            this.IsDirty = false;
        }

        void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        void textBoxUsername_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        void textBoxDisplayName_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        void comboBoxGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            KeyValuePair<int, string> selectedCar = (KeyValuePair<int, string>)cmb.SelectedItem;
            groupId = selectedCar.Key;

            IsDirty = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // validate the data

            // 1. check all fields entered
            bool error = false;
            if (textBoxDisplayName.Text == String.Empty)
            {
                textBoxDisplayName.BackColor = Color.Red;
                error = true;
            }

            if(textBoxPassword.Text == String.Empty)
            {
                textBoxPassword.BackColor = Color.Red;
                error = true;
            }

            if(textBoxUsername.Text == String.Empty)
            {
                textBoxUsername.BackColor = Color.Red;
                error = true;
            }

            KeyValuePair<int, string> selectedCar = (KeyValuePair<int, string>)comboBoxGroup.SelectedItem;
            groupId = selectedCar.Key;

            if(error)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (initialName.CompareTo(textBoxUsername.Text) != 0)
            {
                // 2. check if database contain same username
                try
                {
                    // should throw error if no same username found
                    Server.ServerDbHelper.GetInstance().GetAllUsers().First(UserData => UserData.username.CompareTo(textBoxUsername.Text) == 0);

                    // error occurred
                    textBoxUsername.BackColor = Color.Red;
                    MessageBox.Show("There is a same username registered in the system.");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
                catch (Exception)
                {
                    // no same username, proceed
                }
            }

            // assign member to user's input
            displayName = textBoxDisplayName.Text;
            username = textBoxUsername.Text;
            password = textBoxPassword.Text;
        }

        public void SetGroups(Dictionary<int, string> group)
        {
            comboBoxGroup.DataSource = new BindingSource(group.ToList(), null);
            comboBoxGroup.ValueMember = "Key";
            comboBoxGroup.DisplayMember = "Value";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // do nothing
        }
    }
}
