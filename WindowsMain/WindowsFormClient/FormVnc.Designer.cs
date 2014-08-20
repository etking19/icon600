namespace WindowsFormClient
{
    partial class FormVnc
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
            this.listBoxVnc = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxVnc
            // 
            this.listBoxVnc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxVnc.FormattingEnabled = true;
            this.listBoxVnc.Location = new System.Drawing.Point(0, 0);
            this.listBoxVnc.Name = "listBoxVnc";
            this.listBoxVnc.Size = new System.Drawing.Size(284, 262);
            this.listBoxVnc.TabIndex = 0;
            // 
            // FormVnc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.listBoxVnc);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormVnc";
            this.Text = "FormVnc";
            this.Load += new System.EventHandler(this.FormVnc_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxVnc;

    }
}