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
    public partial class FormMonitor : Form
    {
        private string initialDisplayName;

        private string displayName;
        private int locationX;
        private int locationY;
        private int width;
        private int height;

        public string DisplayName 
        {
            get { return displayName; }
            set { textBoxDisplayName.Text = value; }
        }

        public int LocationX
        {
            get { return locationX; }
            set { textBoxX.Text = value.ToString(); }
        }

        public int LocationY
        {
            get { return locationY; }
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

        public FormMonitor(string initialName)
        {
            InitializeComponent();

            this.initialDisplayName = initialName;
        }

        private void FormMonitor_Load(object sender, EventArgs e)
        {
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;

            textBoxDisplayName.TextChanged += textBoxDisplayName_TextChanged;
            textBoxX.TextChanged += textBoxX_TextChanged;
            textBoxY.TextChanged += textBoxY_TextChanged;
            textBoxWidth.TextChanged += textBoxWidth_TextChanged;
            textBoxHeight.TextChanged += textBoxHeight_TextChanged;

            IsDirty = false;
        }

        void textBoxHeight_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        void textBoxWidth_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        void textBoxY_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        void textBoxX_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        void textBoxDisplayName_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // validation
            // 1. check all fields entered
            bool error = false;
            displayName = textBoxDisplayName.Text;
            if (displayName == String.Empty)
            {
                textBoxDisplayName.BackColor = Color.Red;
                error = true;
            }

            if (textBoxX.Text == String.Empty ||
                int.TryParse(textBoxX.Text, out locationX) == false)
            {
                textBoxX.BackColor = Color.Red;
                error = true;
            }

            if (textBoxY.Text == String.Empty ||
                int.TryParse(textBoxY.Text, out locationY) == false)
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

            if (initialDisplayName.CompareTo(displayName) != 0)
            {
                // 2. check if database contain same username
                try
                {
                    // should throw error if no same username found
                    Server.ServerDbHelper.GetInstance().GetMonitorsList().First(MonitorData => MonitorData.Name.CompareTo(displayName) == 0);

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

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }


    }
}
