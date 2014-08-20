namespace WindowsFormClient
{
    partial class FormApplication
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDisplayName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxWindows = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.radioButtonAuto = new System.Windows.Forms.RadioButton();
            this.radioButtonManual = new System.Windows.Forms.RadioButton();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxArguments = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Display Name:";
            // 
            // textBoxDisplayName
            // 
            this.textBoxDisplayName.Location = new System.Drawing.Point(93, 12);
            this.textBoxDisplayName.Name = "textBoxDisplayName";
            this.textBoxDisplayName.Size = new System.Drawing.Size(279, 20);
            this.textBoxDisplayName.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxWindows);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.radioButtonAuto);
            this.groupBox1.Controls.Add(this.radioButtonManual);
            this.groupBox1.Location = new System.Drawing.Point(12, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 200);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Windows Placement and Positioning";
            // 
            // comboBoxWindows
            // 
            this.comboBoxWindows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWindows.FormattingEnabled = true;
            this.comboBoxWindows.Location = new System.Drawing.Point(41, 161);
            this.comboBoxWindows.Name = "comboBoxWindows";
            this.comboBoxWindows.Size = new System.Drawing.Size(287, 21);
            this.comboBoxWindows.TabIndex = 3;
            this.comboBoxWindows.SelectedIndexChanged += new System.EventHandler(this.comboBoxWindows_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxHeight);
            this.groupBox2.Controls.Add(this.textBoxX);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxWidth);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxY);
            this.groupBox2.Location = new System.Drawing.Point(28, 43);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(323, 77);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(240, 45);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(60, 20);
            this.textBoxHeight.TabIndex = 26;
            this.textBoxHeight.Text = "0";
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(77, 19);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(60, 20);
            this.textBoxX.TabIndex = 20;
            this.textBoxX.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(173, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Height:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Location X:";
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(77, 47);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(60, 20);
            this.textBoxWidth.TabIndex = 24;
            this.textBoxWidth.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(173, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Location Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Width:";
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(240, 19);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(60, 20);
            this.textBoxY.TabIndex = 22;
            this.textBoxY.Text = "0";
            // 
            // radioButtonAuto
            // 
            this.radioButtonAuto.AutoSize = true;
            this.radioButtonAuto.Location = new System.Drawing.Point(6, 137);
            this.radioButtonAuto.Name = "radioButtonAuto";
            this.radioButtonAuto.Size = new System.Drawing.Size(159, 17);
            this.radioButtonAuto.TabIndex = 1;
            this.radioButtonAuto.TabStop = true;
            this.radioButtonAuto.Text = "Detect Using Window\'s Title";
            this.radioButtonAuto.UseVisualStyleBackColor = true;
            this.radioButtonAuto.CheckedChanged += new System.EventHandler(this.radioButtonAuto_CheckedChanged);
            // 
            // radioButtonManual
            // 
            this.radioButtonManual.AutoSize = true;
            this.radioButtonManual.Location = new System.Drawing.Point(7, 20);
            this.radioButtonManual.Name = "radioButtonManual";
            this.radioButtonManual.Size = new System.Drawing.Size(60, 17);
            this.radioButtonManual.TabIndex = 0;
            this.radioButtonManual.TabStop = true;
            this.radioButtonManual.Text = "Manual";
            this.radioButtonManual.UseVisualStyleBackColor = true;
            this.radioButtonManual.CheckedChanged += new System.EventHandler(this.radioButtonManual_CheckedChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(297, 331);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(216, 331);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 12;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Path:";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(93, 38);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.ReadOnly = true;
            this.textBoxPath.Size = new System.Drawing.Size(198, 20);
            this.textBoxPath.TabIndex = 6;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(297, 36);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 14;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Arguments";
            // 
            // textBoxArguments
            // 
            this.textBoxArguments.Location = new System.Drawing.Point(93, 64);
            this.textBoxArguments.Name = "textBoxArguments";
            this.textBoxArguments.Size = new System.Drawing.Size(279, 20);
            this.textBoxArguments.TabIndex = 15;
            // 
            // FormApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 366);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxArguments);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDisplayName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormApplication";
            this.Text = "FormApplication";
            this.Load += new System.EventHandler(this.FormApplication_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDisplayName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.RadioButton radioButtonManual;
        private System.Windows.Forms.RadioButton radioButtonAuto;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.ComboBox comboBoxWindows;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxArguments;
    }
}