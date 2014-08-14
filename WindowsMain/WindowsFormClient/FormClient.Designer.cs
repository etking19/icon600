namespace WindowsFormClient
{
    partial class FormClient
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
            this.hostIp = new System.Windows.Forms.TextBox();
            this.hostPort = new System.Windows.Forms.TextBox();
            this.connect = new System.Windows.Forms.Button();
            this.disconnect = new System.Windows.Forms.Button();
            this.layoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.minimizedWndComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.captureKeyboard = new System.Windows.Forms.CheckBox();
            this.captureMouse = new System.Windows.Forms.CheckBox();
            this.applicationsListbox = new System.Windows.Forms.ListBox();
            this.shutdownBtn = new System.Windows.Forms.Button();
            this.rebootBtn = new System.Windows.Forms.Button();
            this.logOffBtn = new System.Windows.Forms.Button();
            this.startVNC = new System.Windows.Forms.Button();
            this.stopVNC = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hostIp
            // 
            this.hostIp.Location = new System.Drawing.Point(8, 19);
            this.hostIp.Name = "hostIp";
            this.hostIp.Size = new System.Drawing.Size(85, 20);
            this.hostIp.TabIndex = 0;
            this.hostIp.Text = "127.0.0.1";
            // 
            // hostPort
            // 
            this.hostPort.Location = new System.Drawing.Point(100, 19);
            this.hostPort.Name = "hostPort";
            this.hostPort.Size = new System.Drawing.Size(100, 20);
            this.hostPort.TabIndex = 1;
            this.hostPort.Text = "10000";
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(8, 105);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(75, 23);
            this.connect.TabIndex = 2;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // disconnect
            // 
            this.disconnect.Location = new System.Drawing.Point(89, 105);
            this.disconnect.Name = "disconnect";
            this.disconnect.Size = new System.Drawing.Size(75, 23);
            this.disconnect.TabIndex = 3;
            this.disconnect.Text = "Disconnect";
            this.disconnect.UseVisualStyleBackColor = true;
            this.disconnect.Click += new System.EventHandler(this.disconnect_Click);
            // 
            // layoutPanel
            // 
            this.layoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutPanel.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.layoutPanel.Location = new System.Drawing.Point(12, 153);
            this.layoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.Size = new System.Drawing.Size(505, 371);
            this.layoutPanel.TabIndex = 4;
            // 
            // minimizedWndComboBox
            // 
            this.minimizedWndComboBox.FormattingEnabled = true;
            this.minimizedWndComboBox.Location = new System.Drawing.Point(520, 153);
            this.minimizedWndComboBox.Name = "minimizedWndComboBox";
            this.minimizedWndComboBox.Size = new System.Drawing.Size(294, 21);
            this.minimizedWndComboBox.TabIndex = 5;
            this.minimizedWndComboBox.SelectedIndexChanged += new System.EventHandler(this.minimizedWndComboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.password);
            this.groupBox1.Controls.Add(this.username);
            this.groupBox1.Controls.Add(this.hostPort);
            this.groupBox1.Controls.Add(this.hostIp);
            this.groupBox1.Controls.Add(this.disconnect);
            this.groupBox1.Controls.Add(this.connect);
            this.groupBox1.Location = new System.Drawing.Point(19, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 134);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(115, 46);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 20);
            this.password.TabIndex = 5;
            this.password.Text = "password";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(8, 46);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(100, 20);
            this.username.TabIndex = 4;
            this.username.Text = "username";
            // 
            // captureKeyboard
            // 
            this.captureKeyboard.AutoSize = true;
            this.captureKeyboard.Location = new System.Drawing.Point(279, 32);
            this.captureKeyboard.Name = "captureKeyboard";
            this.captureKeyboard.Size = new System.Drawing.Size(111, 17);
            this.captureKeyboard.TabIndex = 7;
            this.captureKeyboard.Text = "Capture Keyboard";
            this.captureKeyboard.UseVisualStyleBackColor = true;
            this.captureKeyboard.CheckedChanged += new System.EventHandler(this.captureKeyboard_CheckedChanged);
            // 
            // captureMouse
            // 
            this.captureMouse.AutoSize = true;
            this.captureMouse.Location = new System.Drawing.Point(279, 56);
            this.captureMouse.Name = "captureMouse";
            this.captureMouse.Size = new System.Drawing.Size(98, 17);
            this.captureMouse.TabIndex = 8;
            this.captureMouse.Text = "Capture Mouse";
            this.captureMouse.UseVisualStyleBackColor = true;
            this.captureMouse.CheckedChanged += new System.EventHandler(this.captureMouse_CheckedChanged);
            // 
            // applicationsListbox
            // 
            this.applicationsListbox.FormattingEnabled = true;
            this.applicationsListbox.Location = new System.Drawing.Point(520, 179);
            this.applicationsListbox.Name = "applicationsListbox";
            this.applicationsListbox.Size = new System.Drawing.Size(294, 342);
            this.applicationsListbox.TabIndex = 9;
            // 
            // shutdownBtn
            // 
            this.shutdownBtn.Location = new System.Drawing.Point(279, 91);
            this.shutdownBtn.Name = "shutdownBtn";
            this.shutdownBtn.Size = new System.Drawing.Size(75, 23);
            this.shutdownBtn.TabIndex = 10;
            this.shutdownBtn.Text = "shutdown";
            this.shutdownBtn.UseVisualStyleBackColor = true;
            this.shutdownBtn.Click += new System.EventHandler(this.shutdownBtn_Click);
            // 
            // rebootBtn
            // 
            this.rebootBtn.Location = new System.Drawing.Point(360, 91);
            this.rebootBtn.Name = "rebootBtn";
            this.rebootBtn.Size = new System.Drawing.Size(75, 23);
            this.rebootBtn.TabIndex = 11;
            this.rebootBtn.Text = "reboot";
            this.rebootBtn.UseVisualStyleBackColor = true;
            this.rebootBtn.Click += new System.EventHandler(this.rebootBtn_Click);
            // 
            // logOffBtn
            // 
            this.logOffBtn.Location = new System.Drawing.Point(442, 91);
            this.logOffBtn.Name = "logOffBtn";
            this.logOffBtn.Size = new System.Drawing.Size(75, 23);
            this.logOffBtn.TabIndex = 12;
            this.logOffBtn.Text = "log off";
            this.logOffBtn.UseVisualStyleBackColor = true;
            this.logOffBtn.Click += new System.EventHandler(this.logOffBtn_Click);
            // 
            // startVNC
            // 
            this.startVNC.Location = new System.Drawing.Point(441, 32);
            this.startVNC.Name = "startVNC";
            this.startVNC.Size = new System.Drawing.Size(75, 23);
            this.startVNC.TabIndex = 13;
            this.startVNC.Text = "Start VNC";
            this.startVNC.UseVisualStyleBackColor = true;
            this.startVNC.Click += new System.EventHandler(this.startVNC_Click);
            // 
            // stopVNC
            // 
            this.stopVNC.Location = new System.Drawing.Point(523, 32);
            this.stopVNC.Name = "stopVNC";
            this.stopVNC.Size = new System.Drawing.Size(75, 23);
            this.stopVNC.TabIndex = 14;
            this.stopVNC.Text = "Stop VNC";
            this.stopVNC.UseVisualStyleBackColor = true;
            this.stopVNC.Click += new System.EventHandler(this.stopVNC_Click);
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 537);
            this.Controls.Add(this.stopVNC);
            this.Controls.Add(this.startVNC);
            this.Controls.Add(this.logOffBtn);
            this.Controls.Add(this.rebootBtn);
            this.Controls.Add(this.shutdownBtn);
            this.Controls.Add(this.applicationsListbox);
            this.Controls.Add(this.captureMouse);
            this.Controls.Add(this.captureKeyboard);
            this.Controls.Add(this.minimizedWndComboBox);
            this.Controls.Add(this.layoutPanel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormClient";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onFormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox hostIp;
        private System.Windows.Forms.TextBox hostPort;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Button disconnect;
        private System.Windows.Forms.FlowLayoutPanel layoutPanel;
        private System.Windows.Forms.ComboBox minimizedWndComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.CheckBox captureKeyboard;
        private System.Windows.Forms.CheckBox captureMouse;
        private System.Windows.Forms.ListBox applicationsListbox;
        private System.Windows.Forms.Button shutdownBtn;
        private System.Windows.Forms.Button rebootBtn;
        private System.Windows.Forms.Button logOffBtn;
        private System.Windows.Forms.Button startVNC;
        private System.Windows.Forms.Button stopVNC;
    }
}