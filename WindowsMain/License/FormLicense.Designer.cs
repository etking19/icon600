namespace License
{
    partial class FormLicense
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
            this.btnGenerateLicence = new System.Windows.Forms.Button();
            this.btnGetIndetifier = new System.Windows.Forms.Button();
            this.textBoxIdentifier = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnGenerateLicence
            // 
            this.btnGenerateLicence.Location = new System.Drawing.Point(56, 174);
            this.btnGenerateLicence.Name = "btnGenerateLicence";
            this.btnGenerateLicence.Size = new System.Drawing.Size(160, 23);
            this.btnGenerateLicence.TabIndex = 0;
            this.btnGenerateLicence.Text = "Generate License";
            this.btnGenerateLicence.UseVisualStyleBackColor = true;
            this.btnGenerateLicence.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGetIndetifier
            // 
            this.btnGetIndetifier.Location = new System.Drawing.Point(56, 26);
            this.btnGetIndetifier.Name = "btnGetIndetifier";
            this.btnGetIndetifier.Size = new System.Drawing.Size(160, 23);
            this.btnGetIndetifier.TabIndex = 1;
            this.btnGetIndetifier.Text = "Get PC Identifier";
            this.btnGetIndetifier.UseVisualStyleBackColor = true;
            this.btnGetIndetifier.Click += new System.EventHandler(this.btnGetIndetifier_Click);
            // 
            // textBoxIdentifier
            // 
            this.textBoxIdentifier.Location = new System.Drawing.Point(56, 55);
            this.textBoxIdentifier.Name = "textBoxIdentifier";
            this.textBoxIdentifier.Size = new System.Drawing.Size(160, 20);
            this.textBoxIdentifier.TabIndex = 2;
            // 
            // FormLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 209);
            this.Controls.Add(this.textBoxIdentifier);
            this.Controls.Add(this.btnGetIndetifier);
            this.Controls.Add(this.btnGenerateLicence);
            this.Name = "FormLicense";
            this.Text = "FormLicense";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerateLicence;
        private System.Windows.Forms.Button btnGetIndetifier;
        private System.Windows.Forms.TextBox textBoxIdentifier;
    }
}