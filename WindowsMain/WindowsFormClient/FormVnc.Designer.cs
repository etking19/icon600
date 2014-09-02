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
            this.listBoxVnc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(231)))), ((int)(((byte)(236)))));
            this.listBoxVnc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxVnc.FormattingEnabled = true;
            this.listBoxVnc.ItemHeight = 18;
            this.listBoxVnc.Location = new System.Drawing.Point(0, 0);
            this.listBoxVnc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxVnc.Name = "listBoxVnc";
            this.listBoxVnc.Size = new System.Drawing.Size(426, 363);
            this.listBoxVnc.TabIndex = 0;
            // 
            // FormVnc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(217)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(426, 363);
            this.Controls.Add(this.listBoxVnc);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormVnc";
            this.Text = "VNC Servers";
            this.Load += new System.EventHandler(this.FormVnc_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxVnc;

    }
}