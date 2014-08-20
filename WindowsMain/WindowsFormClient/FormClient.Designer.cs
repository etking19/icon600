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
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.groupBoxControls = new System.Windows.Forms.GroupBox();
            this.buttonKeyboard = new System.Windows.Forms.Button();
            this.buttonMouse = new System.Windows.Forms.Button();
            this.buttonMaintenance = new System.Windows.Forms.Button();
            this.buttonMessage = new System.Windows.Forms.Button();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.groupBoxControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxControls
            // 
            this.groupBoxControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxControls.Controls.Add(this.buttonKeyboard);
            this.groupBoxControls.Controls.Add(this.buttonMouse);
            this.groupBoxControls.Controls.Add(this.buttonMaintenance);
            this.groupBoxControls.Controls.Add(this.buttonMessage);
            this.groupBoxControls.Location = new System.Drawing.Point(12, 12);
            this.groupBoxControls.Name = "groupBoxControls";
            this.groupBoxControls.Size = new System.Drawing.Size(760, 69);
            this.groupBoxControls.TabIndex = 0;
            this.groupBoxControls.TabStop = false;
            // 
            // buttonKeyboard
            // 
            this.buttonKeyboard.Location = new System.Drawing.Point(475, 19);
            this.buttonKeyboard.Name = "buttonKeyboard";
            this.buttonKeyboard.Size = new System.Drawing.Size(150, 40);
            this.buttonKeyboard.TabIndex = 3;
            this.buttonKeyboard.Text = "Keyboard";
            this.buttonKeyboard.UseVisualStyleBackColor = true;
            this.buttonKeyboard.Click += new System.EventHandler(this.buttonKeyboard_Click);
            // 
            // buttonMouse
            // 
            this.buttonMouse.Location = new System.Drawing.Point(319, 19);
            this.buttonMouse.Name = "buttonMouse";
            this.buttonMouse.Size = new System.Drawing.Size(150, 40);
            this.buttonMouse.TabIndex = 2;
            this.buttonMouse.Text = "Mouse";
            this.buttonMouse.UseVisualStyleBackColor = true;
            this.buttonMouse.Click += new System.EventHandler(this.buttonMouse_Click);
            // 
            // buttonMaintenance
            // 
            this.buttonMaintenance.Location = new System.Drawing.Point(163, 19);
            this.buttonMaintenance.Name = "buttonMaintenance";
            this.buttonMaintenance.Size = new System.Drawing.Size(150, 40);
            this.buttonMaintenance.TabIndex = 1;
            this.buttonMaintenance.Text = "Server Maintenance";
            this.buttonMaintenance.UseVisualStyleBackColor = true;
            this.buttonMaintenance.Click += new System.EventHandler(this.buttonMaintenance_Click);
            // 
            // buttonMessage
            // 
            this.buttonMessage.Location = new System.Drawing.Point(7, 20);
            this.buttonMessage.Name = "buttonMessage";
            this.buttonMessage.Size = new System.Drawing.Size(150, 40);
            this.buttonMessage.TabIndex = 0;
            this.buttonMessage.Text = "Message Box";
            this.buttonMessage.UseVisualStyleBackColor = true;
            this.buttonMessage.Click += new System.EventHandler(this.buttonMessage_Click);
            // 
            // dockPanel
            // 
            this.dockPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dockPanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.dockPanel.Location = new System.Drawing.Point(12, 87);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(760, 463);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            autoHideStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            dockPaneStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel.Skin = dockPanelSkin1;
            this.dockPanel.TabIndex = 1;
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.groupBoxControls);
            this.IsMdiContainer = true;
            this.Name = "FormClient";
            this.Text = "VISTROL Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClient_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormClient_Closed);
            this.Load += new System.EventHandler(this.FormClient_Load);
            this.groupBoxControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxControls;
        private System.Windows.Forms.Button buttonKeyboard;
        private System.Windows.Forms.Button buttonMouse;
        private System.Windows.Forms.Button buttonMaintenance;
        private System.Windows.Forms.Button buttonMessage;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
    }
}