namespace WindowsMain
{
    partial class FormServer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServer));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl = new WindowsMain.CustomTabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.textBoxPortMax = new System.Windows.Forms.TextBox();
            this.textBoxPortMin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelMin = new System.Windows.Forms.Label();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.labelGeneral = new System.Windows.Forms.Label();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.tabApplications = new System.Windows.Forms.TabPage();
            this.tabMonitors = new System.Windows.Forms.TabPage();
            this.tabDrivers = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "general.ico");
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabUsers);
            this.tabControl.Controls.Add(this.tabApplications);
            this.tabControl.Controls.Add(this.tabMonitors);
            this.tabControl.Controls.Add(this.tabDrivers);
            this.tabControl.ImageList = this.imageList1;
            this.tabControl.ItemSize = new System.Drawing.Size(100, 100);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(760, 538);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.textBoxPortMax);
            this.tabGeneral.Controls.Add(this.textBoxPortMin);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Controls.Add(this.labelMin);
            this.tabGeneral.Controls.Add(this.labelServerPort);
            this.tabGeneral.Controls.Add(this.labelGeneral);
            this.tabGeneral.ImageIndex = 0;
            this.tabGeneral.Location = new System.Drawing.Point(104, 4);
            this.tabGeneral.Margin = new System.Windows.Forms.Padding(0);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(652, 530);
            this.tabGeneral.TabIndex = 5;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // textBoxPortMax
            // 
            this.textBoxPortMax.Location = new System.Drawing.Point(91, 131);
            this.textBoxPortMax.Name = "textBoxPortMax";
            this.textBoxPortMax.Size = new System.Drawing.Size(100, 20);
            this.textBoxPortMax.TabIndex = 5;
            this.textBoxPortMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxPortMin
            // 
            this.textBoxPortMin.Location = new System.Drawing.Point(91, 102);
            this.textBoxPortMin.Name = "textBoxPortMin";
            this.textBoxPortMin.Size = new System.Drawing.Size(100, 20);
            this.textBoxPortMin.TabIndex = 4;
            this.textBoxPortMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(58, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Max";
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Location = new System.Drawing.Point(55, 105);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(24, 13);
            this.labelMin.TabIndex = 2;
            this.labelMin.Text = "Min";
            // 
            // labelServerPort
            // 
            this.labelServerPort.AutoSize = true;
            this.labelServerPort.BackColor = System.Drawing.Color.Transparent;
            this.labelServerPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServerPort.Location = new System.Drawing.Point(51, 70);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(140, 20);
            this.labelServerPort.TabIndex = 1;
            this.labelServerPort.Text = "Server Port Range";
            // 
            // labelGeneral
            // 
            this.labelGeneral.AutoSize = true;
            this.labelGeneral.BackColor = System.Drawing.Color.Transparent;
            this.labelGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGeneral.Location = new System.Drawing.Point(4, 4);
            this.labelGeneral.Name = "labelGeneral";
            this.labelGeneral.Size = new System.Drawing.Size(130, 37);
            this.labelGeneral.TabIndex = 0;
            this.labelGeneral.Text = "General";
            // 
            // tabUsers
            // 
            this.tabUsers.Location = new System.Drawing.Point(104, 4);
            this.tabUsers.Margin = new System.Windows.Forms.Padding(0);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Size = new System.Drawing.Size(652, 530);
            this.tabUsers.TabIndex = 6;
            this.tabUsers.Text = "Users";
            this.tabUsers.UseVisualStyleBackColor = true;
            // 
            // tabApplications
            // 
            this.tabApplications.Location = new System.Drawing.Point(104, 4);
            this.tabApplications.Margin = new System.Windows.Forms.Padding(0);
            this.tabApplications.Name = "tabApplications";
            this.tabApplications.Size = new System.Drawing.Size(652, 530);
            this.tabApplications.TabIndex = 2;
            this.tabApplications.Text = "Applications";
            this.tabApplications.UseVisualStyleBackColor = true;
            // 
            // tabMonitors
            // 
            this.tabMonitors.Location = new System.Drawing.Point(104, 4);
            this.tabMonitors.Margin = new System.Windows.Forms.Padding(0);
            this.tabMonitors.Name = "tabMonitors";
            this.tabMonitors.Size = new System.Drawing.Size(652, 530);
            this.tabMonitors.TabIndex = 3;
            this.tabMonitors.Text = "Monitors";
            this.tabMonitors.UseVisualStyleBackColor = true;
            // 
            // tabDrivers
            // 
            this.tabDrivers.Location = new System.Drawing.Point(104, 4);
            this.tabDrivers.Margin = new System.Windows.Forms.Padding(0);
            this.tabDrivers.Name = "tabDrivers";
            this.tabDrivers.Size = new System.Drawing.Size(652, 530);
            this.tabDrivers.TabIndex = 4;
            this.tabDrivers.Text = "Drivers";
            this.tabDrivers.UseVisualStyleBackColor = true;
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tabControl);
            this.Name = "FormServer";
            this.Text = "FormServer";
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomTabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabApplications;
        private System.Windows.Forms.TabPage tabMonitors;
        private System.Windows.Forms.TabPage tabDrivers;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label labelGeneral;
        private System.Windows.Forms.Label labelServerPort;
        private System.Windows.Forms.TextBox textBoxPortMax;
        private System.Windows.Forms.TextBox textBoxPortMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelMin;

    }
}