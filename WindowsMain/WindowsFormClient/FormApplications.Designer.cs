namespace WindowsFormClient
{
    partial class FormApplications
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
            this.listBoxApplications = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxApplications
            // 
            this.listBoxApplications.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(231)))), ((int)(((byte)(236)))));
            this.listBoxApplications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxApplications.FormattingEnabled = true;
            this.listBoxApplications.Location = new System.Drawing.Point(0, 0);
            this.listBoxApplications.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxApplications.Name = "listBoxApplications";
            this.listBoxApplications.Size = new System.Drawing.Size(284, 262);
            this.listBoxApplications.TabIndex = 1;
            // 
            // FormApplications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.listBoxApplications);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormApplications";
            this.Text = "Server Applications";
            this.Load += new System.EventHandler(this.FormApplications_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxApplications;
    }
}