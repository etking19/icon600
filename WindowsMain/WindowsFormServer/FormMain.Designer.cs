namespace WindowsFormClient
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
            this.endPort = new System.Windows.Forms.TextBox();
            this.startPort = new System.Windows.Forms.TextBox();
            this.stopServer = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.updateDB = new System.Windows.Forms.Button();
            this.removeDB = new System.Windows.Forms.Button();
            this.addDB = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.TextBox();
            this.startVncClient = new System.Windows.Forms.Button();
            this.stopVncClient = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // startServer
            // 
            this.startServer.Location = new System.Drawing.Point(12, 51);
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
            this.username.Text = "username";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(129, 18);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 20);
            this.password.TabIndex = 4;
            this.password.Text = "password";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.endPort);
            this.panel1.Controls.Add(this.startPort);
            this.panel1.Controls.Add(this.stopServer);
            this.panel1.Controls.Add(this.startServer);
            this.panel1.Location = new System.Drawing.Point(28, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 77);
            this.panel1.TabIndex = 5;
            // 
            // endPort
            // 
            this.endPort.Location = new System.Drawing.Point(129, 14);
            this.endPort.Name = "endPort";
            this.endPort.Size = new System.Drawing.Size(100, 20);
            this.endPort.TabIndex = 8;
            this.endPort.Text = "10010";
            // 
            // startPort
            // 
            this.startPort.Location = new System.Drawing.Point(12, 14);
            this.startPort.Name = "startPort";
            this.startPort.Size = new System.Drawing.Size(100, 20);
            this.startPort.TabIndex = 7;
            this.startPort.Text = "10000";
            // 
            // stopServer
            // 
            this.stopServer.Location = new System.Drawing.Point(93, 51);
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
            this.panel2.Location = new System.Drawing.Point(28, 107);
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
            this.updateDB.Click += new System.EventHandler(this.updateDB_Click);
            // 
            // removeDB
            // 
            this.removeDB.Location = new System.Drawing.Point(93, 64);
            this.removeDB.Name = "removeDB";
            this.removeDB.Size = new System.Drawing.Size(75, 23);
            this.removeDB.TabIndex = 5;
            this.removeDB.Text = "Remove";
            this.removeDB.UseVisualStyleBackColor = true;
            this.removeDB.Click += new System.EventHandler(this.removeDB_Click);
            // 
            // addDB
            // 
            this.addDB.Location = new System.Drawing.Point(12, 64);
            this.addDB.Name = "addDB";
            this.addDB.Size = new System.Drawing.Size(75, 23);
            this.addDB.TabIndex = 4;
            this.addDB.Text = "Add";
            this.addDB.UseVisualStyleBackColor = true;
            this.addDB.Click += new System.EventHandler(this.addDB_Click);
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
            // startVncClient
            // 
            this.startVncClient.Location = new System.Drawing.Point(40, 238);
            this.startVncClient.Name = "startVncClient";
            this.startVncClient.Size = new System.Drawing.Size(75, 23);
            this.startVncClient.TabIndex = 8;
            this.startVncClient.Text = "Start VNC";
            this.startVncClient.UseVisualStyleBackColor = true;
            this.startVncClient.Click += new System.EventHandler(this.startVncClient_Click);
            // 
            // stopVncClient
            // 
            this.stopVncClient.Location = new System.Drawing.Point(121, 238);
            this.stopVncClient.Name = "stopVncClient";
            this.stopVncClient.Size = new System.Drawing.Size(75, 23);
            this.stopVncClient.TabIndex = 9;
            this.stopVncClient.Text = "Stop VNC";
            this.stopVncClient.UseVisualStyleBackColor = true;
            this.stopVncClient.Click += new System.EventHandler(this.stopVncClient_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 537);
            this.Controls.Add(this.stopVncClient);
            this.Controls.Add(this.startVncClient);
            this.Controls.Add(this.output);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.TextBox endPort;
        private System.Windows.Forms.TextBox startPort;
        private System.Windows.Forms.Button startVncClient;
        private System.Windows.Forms.Button stopVncClient;
    }
}

