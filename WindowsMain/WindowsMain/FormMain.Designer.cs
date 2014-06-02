namespace WindowsMain
{
    partial class FormMain
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
            this.startServer = new System.Windows.Forms.Button();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stopServer = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.updateDB = new System.Windows.Forms.Button();
            this.removeDB = new System.Windows.Forms.Button();
            this.addDB = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.TextBox();
            this.msgBox = new System.Windows.Forms.TextBox();
            this.sendMsg = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // startServer
            // 
            this.startServer.Location = new System.Drawing.Point(12, 25);
            this.startServer.Name = "startServer";
            this.startServer.Size = new System.Drawing.Size(75, 23);
            this.startServer.TabIndex = 0;
            this.startServer.Text = "Start";
            this.startServer.UseVisualStyleBackColor = true;
            this.startServer.Click += new System.EventHandler(this.button1_Click);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(12, 18);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(100, 20);
            this.username.TabIndex = 3;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(129, 18);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 20);
            this.password.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stopServer);
            this.panel1.Controls.Add(this.startServer);
            this.panel1.Location = new System.Drawing.Point(28, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 77);
            this.panel1.TabIndex = 5;
            // 
            // stopServer
            // 
            this.stopServer.Location = new System.Drawing.Point(93, 25);
            this.stopServer.Name = "stopServer";
            this.stopServer.Size = new System.Drawing.Size(75, 23);
            this.stopServer.TabIndex = 6;
            this.stopServer.Text = "Stop";
            this.stopServer.UseVisualStyleBackColor = true;
            this.stopServer.Click += new System.EventHandler(this.stopServer_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.password);
            this.panel2.Controls.Add(this.updateDB);
            this.panel2.Controls.Add(this.removeDB);
            this.panel2.Controls.Add(this.addDB);
            this.panel2.Controls.Add(this.username);
            this.panel2.Location = new System.Drawing.Point(25, 196);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(253, 100);
            this.panel2.TabIndex = 6;
            // 
            // updateDB
            // 
            this.updateDB.Location = new System.Drawing.Point(174, 64);
            this.updateDB.Name = "updateDB";
            this.updateDB.Size = new System.Drawing.Size(75, 23);
            this.updateDB.TabIndex = 6;
            this.updateDB.Text = "Update";
            this.updateDB.UseVisualStyleBackColor = true;
            // 
            // removeDB
            // 
            this.removeDB.Location = new System.Drawing.Point(93, 64);
            this.removeDB.Name = "removeDB";
            this.removeDB.Size = new System.Drawing.Size(75, 23);
            this.removeDB.TabIndex = 5;
            this.removeDB.Text = "Remove";
            this.removeDB.UseVisualStyleBackColor = true;
            // 
            // addDB
            // 
            this.addDB.Location = new System.Drawing.Point(12, 64);
            this.addDB.Name = "addDB";
            this.addDB.Size = new System.Drawing.Size(75, 23);
            this.addDB.TabIndex = 4;
            this.addDB.Text = "Add";
            this.addDB.UseVisualStyleBackColor = true;
            // 
            // output
            // 
            this.output.Location = new System.Drawing.Point(288, 12);
            this.output.Multiline = true;
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.Size = new System.Drawing.Size(314, 513);
            this.output.TabIndex = 7;
            // 
            // msgBox
            // 
            this.msgBox.Location = new System.Drawing.Point(25, 316);
            this.msgBox.Multiline = true;
            this.msgBox.Name = "msgBox";
            this.msgBox.Size = new System.Drawing.Size(249, 89);
            this.msgBox.TabIndex = 8;
            // 
            // sendMsg
            // 
            this.sendMsg.Location = new System.Drawing.Point(28, 412);
            this.sendMsg.Name = "sendMsg";
            this.sendMsg.Size = new System.Drawing.Size(75, 23);
            this.sendMsg.TabIndex = 9;
            this.sendMsg.Text = "send";
            this.sendMsg.UseVisualStyleBackColor = true;
            this.sendMsg.Click += new System.EventHandler(this.sendMsg_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 537);
            this.Controls.Add(this.sendMsg);
            this.Controls.Add(this.msgBox);
            this.Controls.Add(this.output);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startServer;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button updateDB;
        private System.Windows.Forms.Button removeDB;
        private System.Windows.Forms.Button addDB;
        private System.Windows.Forms.Button stopServer;
        private System.Windows.Forms.TextBox output;
        private System.Windows.Forms.TextBox msgBox;
        private System.Windows.Forms.Button sendMsg;
    }
}

