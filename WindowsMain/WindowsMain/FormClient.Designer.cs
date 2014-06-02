namespace WindowsMain
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
            this.SuspendLayout();
            // 
            // hostIp
            // 
            this.hostIp.Location = new System.Drawing.Point(12, 12);
            this.hostIp.Name = "hostIp";
            this.hostIp.Size = new System.Drawing.Size(85, 20);
            this.hostIp.TabIndex = 0;
            this.hostIp.Text = "127.0.0.1";
            // 
            // hostPort
            // 
            this.hostPort.Location = new System.Drawing.Point(104, 12);
            this.hostPort.Name = "hostPort";
            this.hostPort.Size = new System.Drawing.Size(100, 20);
            this.hostPort.TabIndex = 1;
            this.hostPort.Text = "10000";
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(12, 39);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(75, 23);
            this.connect.TabIndex = 2;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // disconnect
            // 
            this.disconnect.Location = new System.Drawing.Point(94, 39);
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
            this.layoutPanel.Location = new System.Drawing.Point(12, 153);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.Size = new System.Drawing.Size(379, 226);
            this.layoutPanel.TabIndex = 4;
            // 
            // minimizedWndComboBox
            // 
            this.minimizedWndComboBox.FormattingEnabled = true;
            this.minimizedWndComboBox.Location = new System.Drawing.Point(230, 12);
            this.minimizedWndComboBox.Name = "minimizedWndComboBox";
            this.minimizedWndComboBox.Size = new System.Drawing.Size(121, 21);
            this.minimizedWndComboBox.TabIndex = 5;
            this.minimizedWndComboBox.SelectedIndexChanged += new System.EventHandler(this.minimizedWndComboBox_SelectedIndexChanged);
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 392);
            this.Controls.Add(this.minimizedWndComboBox);
            this.Controls.Add(this.layoutPanel);
            this.Controls.Add(this.disconnect);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.hostPort);
            this.Controls.Add(this.hostIp);
            this.Name = "FormClient";
            this.Text = "Client";
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
    }
}