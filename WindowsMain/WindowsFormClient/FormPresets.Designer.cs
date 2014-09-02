namespace WindowsFormClient
{
    partial class FormPresets
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
            this.listBoxPreset = new System.Windows.Forms.ListBox();
            this.contextMenuStripControl = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxPreset
            // 
            this.listBoxPreset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(231)))), ((int)(((byte)(236)))));
            this.listBoxPreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPreset.FormattingEnabled = true;
            this.listBoxPreset.ItemHeight = 18;
            this.listBoxPreset.Location = new System.Drawing.Point(0, 0);
            this.listBoxPreset.Name = "listBoxPreset";
            this.listBoxPreset.Size = new System.Drawing.Size(284, 262);
            this.listBoxPreset.TabIndex = 3;
            // 
            // contextMenuStripControl
            // 
            this.contextMenuStripControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.contextMenuStripControl.Name = "contextMenuStripControl";
            this.contextMenuStripControl.Size = new System.Drawing.Size(153, 70);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Image = global::WindowsFormClient.Properties.Resources.add;
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = global::WindowsFormClient.Properties.Resources.remove;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // FormPresets
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(217)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.listBoxPreset);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.HideOnClose = true;
            this.Name = "FormPresets";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide;
            this.TabPageContextMenuStrip = this.contextMenuStripControl;
            this.TabText = "Presets";
            this.Text = "Presets";
            this.Load += new System.EventHandler(this.FormPresets_Load);
            this.contextMenuStripControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxPreset;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripControl;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}
