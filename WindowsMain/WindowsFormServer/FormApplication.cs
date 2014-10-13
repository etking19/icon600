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
    public partial class FormApplication : Form
    {
        private string initialName;

        private string displayName;
        private string exePath;
        private string arguments;
        private int left;
        private int top;
        private int width;
        private int height;

        public string DisplayName
        {
            get { return displayName; }
            set { textBoxDisplayName.Text = value; }
        }

        public string ExecutablePath
        {
            get { return exePath; }
            set { textBoxPath.Text = value; }
        }

        public string Arguments
        {
            get { return arguments; }
            set { textBoxArguments.Text = value; }
        }

        public int PositionLeft
        {
            get { return left; }
            set { textBoxX.Text = value.ToString(); }
        }

        public int PositionTop
        {
            get { return top; }
            set { textBoxY.Text = value.ToString(); }
        }

        public int Width
        {
            get { return width; }
            set { textBoxWidth.Text = value.ToString(); }
        }

        public int Height
        {
            get { return height; }
            set { textBoxHeight.Text = value.ToString(); }
        }

        public bool IsDirty { get; set; }

        public FormApplication(string initialName)
        {
            InitializeComponent();

            this.initialName = initialName;
        }

        private void FormApplication_Load(object sender, EventArgs e)
        {
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;

            radioButtonManual.Checked = true;

            textBoxDisplayName.TextChanged += textBoxDisplayName_TextChanged;
            textBoxPath.TextChanged += textBoxPath_TextChanged;
            textBoxArguments.TextChanged += textBoxArguments_TextChanged;
            textBoxX.TextChanged += textBoxX_TextChanged;
            textBoxY.TextChanged += textBoxY_TextChanged;
            textBoxWidth.TextChanged += textBoxWidth_TextChanged;
            textBoxHeight.TextChanged += textBoxHeight_TextChanged;

            this.IsDirty = false;
        }

        void textBoxHeight_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        void textBoxWidth_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        void textBoxY_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        void textBoxX_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        void textBoxArguments_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        void textBoxDisplayName_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        private void radioButtonManual_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;

            textBoxX.Enabled = radioBtn.Checked;
            textBoxY.Enabled = radioBtn.Checked;
            textBoxWidth.Enabled = radioBtn.Checked;
            textBoxHeight.Enabled = radioBtn.Checked;

            comboBoxWindows.Enabled = !radioBtn.Checked;

            this.IsDirty = true;
        }

        private void radioButtonAuto_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;

            textBoxX.Enabled = !radioBtn.Checked;
            textBoxY.Enabled = !radioBtn.Checked;
            textBoxWidth.Enabled = !radioBtn.Checked;
            textBoxHeight.Enabled = !radioBtn.Checked;

            comboBoxWindows.Enabled = radioBtn.Checked;

            if (radioBtn.Checked)
            {
                comboBoxWindows.DataSource = new BindingSource(Utils.Windows.WindowsHelper.GetRunningApplicationInfo(), null);
                comboBoxWindows.DisplayMember = "name";
                comboBoxWindows.ValueMember = "id";
            }

            this.IsDirty = true;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            // launch the file search dialog
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Programs (*.exe, *.bat)|*.exe; *.bat|All Files (*.*)|*.*";
            fileDialog.CheckPathExists = true;
            fileDialog.CheckFileExists = true;
            fileDialog.Multiselect = false;
            fileDialog.ShowReadOnly = false;
            fileDialog.ShowHelp = false;
            fileDialog.Title = "Browse Executable Path";
            fileDialog.ValidateNames = true;

            if (fileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBoxPath.Text = fileDialog.FileName;
            }
        }

        private void comboBoxWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            // load the current window's title positioning to textbox
            ComboBox cmb = (ComboBox)sender;
             Utils.Windows.WindowsHelper.ApplicationInfo selectedWnd = (Utils.Windows.WindowsHelper.ApplicationInfo)cmb.SelectedItem;
            foreach(Utils.Windows.WindowsHelper.ApplicationInfo appInfo in Utils.Windows.WindowsHelper.GetRunningApplicationInfo())
            {
                if (selectedWnd.id == appInfo.id)
                {
                    textBoxX.Text = appInfo.posX.ToString();
                    textBoxY.Text = appInfo.posY.ToString();
                    textBoxWidth.Text = appInfo.width.ToString();
                    textBoxHeight.Text = appInfo.height.ToString();

                    break;
                }
            }

            this.IsDirty = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // validate
            bool error = false;
            displayName = textBoxDisplayName.Text;
            exePath = textBoxPath.Text;
            arguments = textBoxArguments.Text;

            if (displayName == String.Empty)
            {
                textBoxDisplayName.BackColor = Color.Red;
                error = true;
            }
            
            if (exePath == String.Empty)
            {
                textBoxPath.BackColor = Color.Red;
                error = true;
            }

            if (textBoxX.Text == String.Empty ||
                int.TryParse(textBoxX.Text, out left) == false)
            {
                textBoxX.BackColor = Color.Red;
                error = true;
            }

            if (textBoxY.Text == String.Empty ||
                int.TryParse(textBoxY.Text, out top) == false)
            {
                textBoxY.BackColor = Color.Red;
                error = true;
            }

            if (textBoxWidth.Text == String.Empty ||
                int.TryParse(textBoxWidth.Text, out width) == false)
            {
                textBoxWidth.BackColor = Color.Red;
                error = true;
            }

            if (textBoxHeight.Text == String.Empty ||
                int.TryParse(textBoxHeight.Text, out height) == false)
            {
                textBoxHeight.BackColor = Color.Red;
                error = true;
            }

            if (error)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (initialName.CompareTo(displayName) != 0)
            {
                // 2. check if database contain same username
                try
                {
                    // should throw error if no same username found
                    Server.ServerDbHelper.GetInstance().GetAllApplications().First(ApplicationData => ApplicationData.name.CompareTo(displayName) == 0);

                    // error occurred
                    textBoxDisplayName.BackColor = Color.Red;
                    MessageBox.Show("There is a same monitor name registered in the system.");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
                catch (Exception)
                {
                    // no same username, proceed
                }
            }
        }
    }
}
