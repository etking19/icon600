namespace WindowsFormClient
{
    partial class FormGroup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxGroupName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxMonitors = new System.Windows.Forms.ComboBox();
            this.radioButtonMonitor = new System.Windows.Forms.RadioButton();
            this.radioButtonDesktop = new System.Windows.Forms.RadioButton();
            this.checkedListBoxApplications = new System.Windows.Forms.CheckedListBox();
            this.checkBoxMaintenance = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(346, 407);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(87, 28);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonOK.ForeColor = System.Drawing.Color.White;
            this.buttonOK.Location = new System.Drawing.Point(252, 407);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(87, 28);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "Group Name:";
            // 
            // textBoxGroupName
            // 
            this.textBoxGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGroupName.Location = new System.Drawing.Point(111, 15);
            this.textBoxGroupName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxGroupName.Name = "textBoxGroupName";
            this.textBoxGroupName.Size = new System.Drawing.Size(260, 22);
            this.textBoxGroupName.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBoxMonitors);
            this.groupBox1.Controls.Add(this.radioButtonMonitor);
            this.groupBox1.Controls.Add(this.radioButtonDesktop);
            this.groupBox1.Location = new System.Drawing.Point(20, 81);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(351, 124);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Share Area";
            // 
            // comboBoxMonitors
            // 
            this.comboBoxMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonitors.FormattingEnabled = true;
            this.comboBoxMonitors.Location = new System.Drawing.Point(91, 82);
            this.comboBoxMonitors.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxMonitors.Name = "comboBoxMonitors";
            this.comboBoxMonitors.Size = new System.Drawing.Size(176, 24);
            this.comboBoxMonitors.TabIndex = 4;
            // 
            // radioButtonMonitor
            // 
            this.radioButtonMonitor.AutoSize = true;
            this.radioButtonMonitor.Location = new System.Drawing.Point(8, 54);
            this.radioButtonMonitor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonMonitor.Name = "radioButtonMonitor";
            this.radioButtonMonitor.Size = new System.Drawing.Size(154, 20);
            this.radioButtonMonitor.TabIndex = 3;
            this.radioButtonMonitor.TabStop = true;
            this.radioButtonMonitor.Text = "Selected Monitor Area";
            this.radioButtonMonitor.UseVisualStyleBackColor = true;
            // 
            // radioButtonDesktop
            // 
            this.radioButtonDesktop.AutoSize = true;
            this.radioButtonDesktop.Location = new System.Drawing.Point(8, 25);
            this.radioButtonDesktop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonDesktop.Name = "radioButtonDesktop";
            this.radioButtonDesktop.Size = new System.Drawing.Size(115, 20);
            this.radioButtonDesktop.TabIndex = 3;
            this.radioButtonDesktop.TabStop = true;
            this.radioButtonDesktop.Text = "Whole Desktop";
            this.radioButtonDesktop.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxApplications
            // 
            this.checkedListBoxApplications.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxApplications.FormattingEnabled = true;
            this.checkedListBoxApplications.Location = new System.Drawing.Point(20, 210);
            this.checkedListBoxApplications.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkedListBoxApplications.Name = "checkedListBoxApplications";
            this.checkedListBoxApplications.Size = new System.Drawing.Size(413, 174);
            this.checkedListBoxApplications.TabIndex = 5;
            // 
            // checkBoxMaintenance
            // 
            this.checkBoxMaintenance.AutoSize = true;
            this.checkBoxMaintenance.Location = new System.Drawing.Point(111, 53);
            this.checkBoxMaintenance.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxMaintenance.Name = "checkBoxMaintenance";
            this.checkBoxMaintenance.Size = new System.Drawing.Size(176, 20);
            this.checkBoxMaintenance.TabIndex = 2;
            this.checkBoxMaintenance.Text = "Allow Server Maintenance";
            this.checkBoxMaintenance.UseVisualStyleBackColor = true;
            // 
            // FormGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(217)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(448, 450);
            this.Controls.Add(this.checkBoxMaintenance);
            this.Controls.Add(this.checkedListBoxApplications);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxGroupName);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormGroup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGroupName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonMonitor;
        private System.Windows.Forms.RadioButton radioButtonDesktop;
        private System.Windows.Forms.CheckedListBox checkedListBoxApplications;
        private System.Windows.Forms.ComboBox comboBoxMonitors;
        private System.Windows.Forms.CheckBox checkBoxMaintenance;
    }
}