namespace WindowsFormClient
{
    partial class FormServerMaintenance
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
            this.buttonShutdown = new System.Windows.Forms.Button();
            this.buttonRestart = new System.Windows.Forms.Button();
            this.buttonStandby = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonShutdown
            // 
            this.buttonShutdown.Location = new System.Drawing.Point(12, 34);
            this.buttonShutdown.Name = "buttonShutdown";
            this.buttonShutdown.Size = new System.Drawing.Size(260, 62);
            this.buttonShutdown.TabIndex = 0;
            this.buttonShutdown.Text = "Shutdown Server";
            this.buttonShutdown.UseVisualStyleBackColor = true;
            this.buttonShutdown.Click += new System.EventHandler(this.buttonShutdown_Click);
            // 
            // buttonRestart
            // 
            this.buttonRestart.Location = new System.Drawing.Point(12, 100);
            this.buttonRestart.Name = "buttonRestart";
            this.buttonRestart.Size = new System.Drawing.Size(260, 62);
            this.buttonRestart.TabIndex = 1;
            this.buttonRestart.Text = "Restart Server";
            this.buttonRestart.UseVisualStyleBackColor = true;
            this.buttonRestart.Click += new System.EventHandler(this.buttonRestart_Click);
            // 
            // buttonStandby
            // 
            this.buttonStandby.Location = new System.Drawing.Point(12, 168);
            this.buttonStandby.Name = "buttonStandby";
            this.buttonStandby.Size = new System.Drawing.Size(260, 62);
            this.buttonStandby.TabIndex = 2;
            this.buttonStandby.Text = "Put Server on Standby";
            this.buttonStandby.UseVisualStyleBackColor = true;
            this.buttonStandby.Click += new System.EventHandler(this.buttonStandby_Click);
            // 
            // FormServerMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonStandby);
            this.Controls.Add(this.buttonRestart);
            this.Controls.Add(this.buttonShutdown);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormServerMaintenance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Maintenance";
            this.Load += new System.EventHandler(this.FormServerMaintenance_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonShutdown;
        private System.Windows.Forms.Button buttonRestart;
        private System.Windows.Forms.Button buttonStandby;
    }
}