﻿namespace WindowsFormClient
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
            this.components = new System.ComponentModel.Container();
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin15 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin15 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient43 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient99 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin15 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient15 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient100 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient44 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient101 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient15 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient102 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient103 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient45 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient104 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient105 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.groupBoxControls = new System.Windows.Forms.GroupBox();
            this.buttonLogout = new System.Windows.Forms.Button();
            this.buttonMaintenance = new System.Windows.Forms.Button();
            this.buttonMessage = new System.Windows.Forms.Button();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.notifyIconClient = new System.Windows.Forms.NotifyIcon(this.components);
            this.checkBoxMouse = new System.Windows.Forms.CheckBox();
            this.checkBoxKeyboard = new System.Windows.Forms.CheckBox();
            this.groupBoxControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxControls
            // 
            this.groupBoxControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(214)))), ((int)(((byte)(233)))));
            this.groupBoxControls.Controls.Add(this.checkBoxKeyboard);
            this.groupBoxControls.Controls.Add(this.checkBoxMouse);
            this.groupBoxControls.Controls.Add(this.buttonLogout);
            this.groupBoxControls.Controls.Add(this.buttonMaintenance);
            this.groupBoxControls.Controls.Add(this.buttonMessage);
            this.groupBoxControls.ForeColor = System.Drawing.Color.White;
            this.groupBoxControls.Location = new System.Drawing.Point(0, 0);
            this.groupBoxControls.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxControls.Name = "groupBoxControls";
            this.groupBoxControls.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxControls.Size = new System.Drawing.Size(854, 65);
            this.groupBoxControls.TabIndex = 0;
            this.groupBoxControls.TabStop = false;
            // 
            // buttonLogout
            // 
            this.buttonLogout.BackColor = System.Drawing.Color.Red;
            this.buttonLogout.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogout.Location = new System.Drawing.Point(740, 16);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(94, 40);
            this.buttonLogout.TabIndex = 4;
            this.buttonLogout.Text = "Logout";
            this.buttonLogout.UseVisualStyleBackColor = false;
            this.buttonLogout.Click += new System.EventHandler(this.buttonLogout_Click);
            // 
            // buttonMaintenance
            // 
            this.buttonMaintenance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonMaintenance.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMaintenance.Image = global::WindowsFormClient.Properties.Resources.server_maintenance;
            this.buttonMaintenance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonMaintenance.Location = new System.Drawing.Point(188, 16);
            this.buttonMaintenance.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMaintenance.Name = "buttonMaintenance";
            this.buttonMaintenance.Size = new System.Drawing.Size(179, 40);
            this.buttonMaintenance.TabIndex = 1;
            this.buttonMaintenance.Text = "Server Maintenance";
            this.buttonMaintenance.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMaintenance.UseVisualStyleBackColor = false;
            this.buttonMaintenance.Click += new System.EventHandler(this.buttonMaintenance_Click);
            // 
            // buttonMessage
            // 
            this.buttonMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMessage.Image = global::WindowsFormClient.Properties.Resources.message_box;
            this.buttonMessage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonMessage.Location = new System.Drawing.Point(9, 16);
            this.buttonMessage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMessage.Name = "buttonMessage";
            this.buttonMessage.Size = new System.Drawing.Size(179, 40);
            this.buttonMessage.TabIndex = 0;
            this.buttonMessage.Text = "Message Box";
            this.buttonMessage.UseVisualStyleBackColor = false;
            this.buttonMessage.Click += new System.EventHandler(this.buttonMessage_Click);
            // 
            // dockPanel
            // 
            this.dockPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dockPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(214)))), ((int)(((byte)(233)))));
            this.dockPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.dockPanel.Location = new System.Drawing.Point(0, 68);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(854, 494);
            dockPanelGradient43.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient43.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin15.DockStripGradient = dockPanelGradient43;
            tabGradient99.EndColor = System.Drawing.SystemColors.Control;
            tabGradient99.StartColor = System.Drawing.SystemColors.Control;
            tabGradient99.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin15.TabGradient = tabGradient99;
            autoHideStripSkin15.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            dockPanelSkin15.AutoHideStripSkin = autoHideStripSkin15;
            tabGradient100.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient100.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient100.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient15.ActiveTabGradient = tabGradient100;
            dockPanelGradient44.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient44.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient15.DockStripGradient = dockPanelGradient44;
            tabGradient101.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient101.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient101.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient15.InactiveTabGradient = tabGradient101;
            dockPaneStripSkin15.DocumentGradient = dockPaneStripGradient15;
            dockPaneStripSkin15.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            tabGradient102.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient102.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient102.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient102.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient15.ActiveCaptionGradient = tabGradient102;
            tabGradient103.EndColor = System.Drawing.SystemColors.Control;
            tabGradient103.StartColor = System.Drawing.SystemColors.Control;
            tabGradient103.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient15.ActiveTabGradient = tabGradient103;
            dockPanelGradient45.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient45.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient15.DockStripGradient = dockPanelGradient45;
            tabGradient104.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient104.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient104.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient104.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient15.InactiveCaptionGradient = tabGradient104;
            tabGradient105.EndColor = System.Drawing.Color.Transparent;
            tabGradient105.StartColor = System.Drawing.Color.Transparent;
            tabGradient105.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient15.InactiveTabGradient = tabGradient105;
            dockPaneStripSkin15.ToolWindowGradient = dockPaneStripToolWindowGradient15;
            dockPanelSkin15.DockPaneStripSkin = dockPaneStripSkin15;
            this.dockPanel.Skin = dockPanelSkin15;
            this.dockPanel.TabIndex = 1;
            // 
            // notifyIconClient
            // 
            this.notifyIconClient.Text = "Vistrol Client";
            this.notifyIconClient.Visible = true;
            // 
            // checkBoxMouse
            // 
            this.checkBoxMouse.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.checkBoxMouse.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMouse.ForeColor = System.Drawing.Color.White;
            this.checkBoxMouse.Image = global::WindowsFormClient.Properties.Resources.mouse;
            this.checkBoxMouse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxMouse.Location = new System.Drawing.Point(370, 16);
            this.checkBoxMouse.Name = "checkBoxMouse";
            this.checkBoxMouse.Size = new System.Drawing.Size(179, 40);
            this.checkBoxMouse.TabIndex = 4;
            this.checkBoxMouse.Text = "Control Mouse";
            this.checkBoxMouse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxMouse.UseVisualStyleBackColor = false;
            this.checkBoxMouse.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxKeyboard
            // 
            this.checkBoxKeyboard.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxKeyboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.checkBoxKeyboard.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxKeyboard.ForeColor = System.Drawing.Color.White;
            this.checkBoxKeyboard.Image = global::WindowsFormClient.Properties.Resources.keyboard;
            this.checkBoxKeyboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxKeyboard.Location = new System.Drawing.Point(555, 16);
            this.checkBoxKeyboard.Name = "checkBoxKeyboard";
            this.checkBoxKeyboard.Size = new System.Drawing.Size(179, 40);
            this.checkBoxKeyboard.TabIndex = 5;
            this.checkBoxKeyboard.Text = "Control Keyboard";
            this.checkBoxKeyboard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxKeyboard.UseVisualStyleBackColor = false;
            this.checkBoxKeyboard.CheckedChanged += new System.EventHandler(this.checkBoxKeyboard_CheckedChanged);
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(214)))), ((int)(((byte)(233)))));
            this.ClientSize = new System.Drawing.Size(854, 562);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.groupBoxControls);
            this.IsMdiContainer = true;
            this.Name = "FormClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VISTROL Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClient_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormClient_Closed);
            this.Load += new System.EventHandler(this.FormClient_Load);
            this.groupBoxControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxControls;
        private System.Windows.Forms.Button buttonMaintenance;
        private System.Windows.Forms.Button buttonMessage;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.Button buttonLogout;
        private System.Windows.Forms.NotifyIcon notifyIconClient;
        private System.Windows.Forms.CheckBox checkBoxMouse;
        private System.Windows.Forms.CheckBox checkBoxKeyboard;
    }
}