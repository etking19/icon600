namespace WindowsFormClient
{
    partial class FormRunningApps
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
            this.listBoxApps = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxApps
            // 
            this.listBoxApps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(231)))), ((int)(((byte)(236)))));
            this.listBoxApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxApps.FormattingEnabled = true;
            this.listBoxApps.ItemHeight = 18;
            this.listBoxApps.Location = new System.Drawing.Point(0, 0);
            this.listBoxApps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxApps.Name = "listBoxApps";
            this.listBoxApps.Size = new System.Drawing.Size(426, 363);
            this.listBoxApps.TabIndex = 0;
            // 
            // FormRunningApps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(217)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(426, 363);
            this.Controls.Add(this.listBoxApps);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormRunningApps";
            this.Text = "Running Applications";
            this.Load += new System.EventHandler(this.FormRunningApps_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxApps;

    }
}