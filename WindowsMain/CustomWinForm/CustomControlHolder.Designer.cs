namespace CustomWinForm
{
    partial class CustomControlHolder
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomControlHolder
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(193)))), ((int)(((byte)(220)))));
            this.BackgroundImage = global::CustomWinForm.Properties.Resources.vistrol_logo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CustomControlHolder";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Size = new System.Drawing.Size(313, 275);
            this.Load += new System.EventHandler(this.CustomControlHolder_Load);
            this.ResumeLayout(false);

        }

        #endregion


    }
}
