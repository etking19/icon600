namespace WindowsFormClient
{
    partial class FormServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServer));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.notifyIconServer = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl = new WindowsFormClient.CustomTabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupBoxGeneral = new System.Windows.Forms.GroupBox();
            this.textBoxGeneralMax = new System.Windows.Forms.TextBox();
            this.textBoxGeneralMin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.btnGeneralStop = new System.Windows.Forms.Button();
            this.btnGeneralStart = new System.Windows.Forms.Button();
            this.labelGeneral = new System.Windows.Forms.Label();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.tabUsersUser = new System.Windows.Forms.TabControl();
            this.tabPageGroup = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.btnGroupsDelete = new System.Windows.Forms.Button();
            this.btnGroupsEdit = new System.Windows.Forms.Button();
            this.btnGroupsAdd = new System.Windows.Forms.Button();
            this.dataGridViewGroup = new System.Windows.Forms.DataGridView();
            this.tabPageUser = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.btnUsersDelete = new System.Windows.Forms.Button();
            this.btnUsersEdit = new System.Windows.Forms.Button();
            this.btnUsersAdd = new System.Windows.Forms.Button();
            this.dataGridViewUsers = new System.Windows.Forms.DataGridView();
            this.dataGridUsers = new System.Windows.Forms.DataGridView();
            this.tabApplications = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAppDelete = new System.Windows.Forms.Button();
            this.btnAppEdit = new System.Windows.Forms.Button();
            this.btnAppAdd = new System.Windows.Forms.Button();
            this.dataGridViewApp = new System.Windows.Forms.DataGridView();
            this.tabMonitors = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.btnMonitorsDelete = new System.Windows.Forms.Button();
            this.btnMonitorsEdit = new System.Windows.Forms.Button();
            this.btnMonitorsAdd = new System.Windows.Forms.Button();
            this.dataGridViewMonitors = new System.Windows.Forms.DataGridView();
            this.tabVnc = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonRemoteDelete = new System.Windows.Forms.Button();
            this.buttonRemoteEdit = new System.Windows.Forms.Button();
            this.buttonRemoteAdd = new System.Windows.Forms.Button();
            this.dataGridViewRemote = new System.Windows.Forms.DataGridView();
            this.tabDrivers = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonVisionDelete = new System.Windows.Forms.Button();
            this.buttonVisionEdit = new System.Windows.Forms.Button();
            this.buttonVisionAdd = new System.Windows.Forms.Button();
            this.dataGridVisionInput = new System.Windows.Forms.DataGridView();
            this.numericUpDownRow = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCol = new System.Windows.Forms.NumericUpDown();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBoxGeneral.SuspendLayout();
            this.tabUsers.SuspendLayout();
            this.tabUsersUser.SuspendLayout();
            this.tabPageGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroup)).BeginInit();
            this.tabPageUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsers)).BeginInit();
            this.tabApplications.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewApp)).BeginInit();
            this.tabMonitors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonitors)).BeginInit();
            this.tabVnc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRemote)).BeginInit();
            this.tabDrivers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVisionInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.Tag = "";
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "main.png");
            this.imageList1.Images.SetKeyName(1, "users.png");
            this.imageList1.Images.SetKeyName(2, "app_management.png");
            this.imageList1.Images.SetKeyName(3, "monitor.png");
            this.imageList1.Images.SetKeyName(4, "driver.png");
            // 
            // notifyIconServer
            // 
            this.notifyIconServer.Text = "notifyIcon";
            this.notifyIconServer.Visible = true;
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(160)))), ((int)(((byte)(189)))));
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabUsers);
            this.tabControl.Controls.Add(this.tabApplications);
            this.tabControl.Controls.Add(this.tabMonitors);
            this.tabControl.Controls.Add(this.tabVnc);
            this.tabControl.Controls.Add(this.tabDrivers);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageList1;
            this.tabControl.ItemSize = new System.Drawing.Size(110, 100);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(834, 672);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.Color.White;
            this.tabGeneral.Controls.Add(this.groupBoxGeneral);
            this.tabGeneral.Controls.Add(this.btnGeneralStop);
            this.tabGeneral.Controls.Add(this.btnGeneralStart);
            this.tabGeneral.Controls.Add(this.labelGeneral);
            this.tabGeneral.ImageIndex = 0;
            this.tabGeneral.Location = new System.Drawing.Point(104, 4);
            this.tabGeneral.Margin = new System.Windows.Forms.Padding(0);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(726, 664);
            this.tabGeneral.TabIndex = 5;
            this.tabGeneral.Text = "Main";
            // 
            // groupBoxGeneral
            // 
            this.groupBoxGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxGeneral.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxGeneral.Controls.Add(this.numericUpDownCol);
            this.groupBoxGeneral.Controls.Add(this.numericUpDownRow);
            this.groupBoxGeneral.Controls.Add(this.textBoxGeneralMax);
            this.groupBoxGeneral.Controls.Add(this.textBoxGeneralMin);
            this.groupBoxGeneral.Controls.Add(this.label3);
            this.groupBoxGeneral.Controls.Add(this.label1);
            this.groupBoxGeneral.Controls.Add(this.labelMin);
            this.groupBoxGeneral.Controls.Add(this.label2);
            this.groupBoxGeneral.Controls.Add(this.labelServerPort);
            this.groupBoxGeneral.Location = new System.Drawing.Point(10, 29);
            this.groupBoxGeneral.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxGeneral.Name = "groupBoxGeneral";
            this.groupBoxGeneral.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxGeneral.Size = new System.Drawing.Size(709, 407);
            this.groupBoxGeneral.TabIndex = 16;
            this.groupBoxGeneral.TabStop = false;
            // 
            // textBoxGeneralMax
            // 
            this.textBoxGeneralMax.Location = new System.Drawing.Point(101, 107);
            this.textBoxGeneralMax.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxGeneralMax.Name = "textBoxGeneralMax";
            this.textBoxGeneralMax.Size = new System.Drawing.Size(148, 23);
            this.textBoxGeneralMax.TabIndex = 5;
            this.textBoxGeneralMax.Text = "8010";
            this.textBoxGeneralMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxGeneralMin
            // 
            this.textBoxGeneralMin.Location = new System.Drawing.Point(101, 67);
            this.textBoxGeneralMin.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxGeneralMin.Name = "textBoxGeneralMin";
            this.textBoxGeneralMin.Size = new System.Drawing.Size(148, 23);
            this.textBoxGeneralMin.TabIndex = 4;
            this.textBoxGeneralMin.Text = "8000";
            this.textBoxGeneralMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(138, 206);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "x";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(48, 111);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Max";
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Location = new System.Drawing.Point(48, 71);
            this.labelMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(30, 16);
            this.labelMin.TabIndex = 2;
            this.labelMin.Text = "Min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 163);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "Server Screen Matrix:";
            // 
            // labelServerPort
            // 
            this.labelServerPort.AutoSize = true;
            this.labelServerPort.BackColor = System.Drawing.Color.Transparent;
            this.labelServerPort.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServerPort.Location = new System.Drawing.Point(18, 23);
            this.labelServerPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(155, 19);
            this.labelServerPort.TabIndex = 1;
            this.labelServerPort.Text = "Server Port Range:";
            // 
            // btnGeneralStop
            // 
            this.btnGeneralStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGeneralStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnGeneralStop.ForeColor = System.Drawing.Color.White;
            this.btnGeneralStop.Image = global::WindowsFormClient.Properties.Resources.stop_serve;
            this.btnGeneralStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGeneralStop.Location = new System.Drawing.Point(564, 623);
            this.btnGeneralStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnGeneralStop.Name = "btnGeneralStop";
            this.btnGeneralStop.Size = new System.Drawing.Size(154, 31);
            this.btnGeneralStop.TabIndex = 15;
            this.btnGeneralStop.Text = "Stop Server";
            this.btnGeneralStop.UseVisualStyleBackColor = false;
            this.btnGeneralStop.Click += new System.EventHandler(this.btnGeneralStop_Click);
            // 
            // btnGeneralStart
            // 
            this.btnGeneralStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGeneralStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnGeneralStart.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneralStart.ForeColor = System.Drawing.Color.White;
            this.btnGeneralStart.Image = global::WindowsFormClient.Properties.Resources.start_server;
            this.btnGeneralStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGeneralStart.Location = new System.Drawing.Point(402, 623);
            this.btnGeneralStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnGeneralStart.Name = "btnGeneralStart";
            this.btnGeneralStart.Size = new System.Drawing.Size(154, 31);
            this.btnGeneralStart.TabIndex = 14;
            this.btnGeneralStart.Text = "Start Server";
            this.btnGeneralStart.UseVisualStyleBackColor = false;
            this.btnGeneralStart.Click += new System.EventHandler(this.btnGeneralStart_Click);
            // 
            // labelGeneral
            // 
            this.labelGeneral.AutoSize = true;
            this.labelGeneral.BackColor = System.Drawing.Color.Transparent;
            this.labelGeneral.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGeneral.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.labelGeneral.Location = new System.Drawing.Point(6, 6);
            this.labelGeneral.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGeneral.Name = "labelGeneral";
            this.labelGeneral.Size = new System.Drawing.Size(45, 19);
            this.labelGeneral.TabIndex = 0;
            this.labelGeneral.Text = "Main";
            // 
            // tabUsers
            // 
            this.tabUsers.Controls.Add(this.tabUsersUser);
            this.tabUsers.ImageIndex = 1;
            this.tabUsers.Location = new System.Drawing.Point(104, 4);
            this.tabUsers.Margin = new System.Windows.Forms.Padding(0);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Size = new System.Drawing.Size(726, 664);
            this.tabUsers.TabIndex = 6;
            this.tabUsers.Text = "Users";
            this.tabUsers.UseVisualStyleBackColor = true;
            // 
            // tabUsersUser
            // 
            this.tabUsersUser.Controls.Add(this.tabPageGroup);
            this.tabUsersUser.Controls.Add(this.tabPageUser);
            this.tabUsersUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabUsersUser.Location = new System.Drawing.Point(0, 0);
            this.tabUsersUser.Margin = new System.Windows.Forms.Padding(4);
            this.tabUsersUser.Name = "tabUsersUser";
            this.tabUsersUser.SelectedIndex = 0;
            this.tabUsersUser.Size = new System.Drawing.Size(726, 664);
            this.tabUsersUser.TabIndex = 0;
            // 
            // tabPageGroup
            // 
            this.tabPageGroup.BackColor = System.Drawing.Color.White;
            this.tabPageGroup.Controls.Add(this.label7);
            this.tabPageGroup.Controls.Add(this.btnGroupsDelete);
            this.tabPageGroup.Controls.Add(this.btnGroupsEdit);
            this.tabPageGroup.Controls.Add(this.btnGroupsAdd);
            this.tabPageGroup.Controls.Add(this.dataGridViewGroup);
            this.tabPageGroup.Location = new System.Drawing.Point(4, 25);
            this.tabPageGroup.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageGroup.Name = "tabPageGroup";
            this.tabPageGroup.Size = new System.Drawing.Size(718, 635);
            this.tabPageGroup.TabIndex = 1;
            this.tabPageGroup.Text = "Group Management";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.label7.Location = new System.Drawing.Point(4, 11);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(159, 19);
            this.label7.TabIndex = 8;
            this.label7.Text = "Group Management";
            // 
            // btnGroupsDelete
            // 
            this.btnGroupsDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupsDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnGroupsDelete.ForeColor = System.Drawing.Color.White;
            this.btnGroupsDelete.Image = global::WindowsFormClient.Properties.Resources.delete;
            this.btnGroupsDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGroupsDelete.Location = new System.Drawing.Point(602, 4);
            this.btnGroupsDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnGroupsDelete.Name = "btnGroupsDelete";
            this.btnGroupsDelete.Size = new System.Drawing.Size(112, 31);
            this.btnGroupsDelete.TabIndex = 7;
            this.btnGroupsDelete.Text = "Delete";
            this.btnGroupsDelete.UseVisualStyleBackColor = false;
            this.btnGroupsDelete.Click += new System.EventHandler(this.btnGroupsDelete_Click);
            // 
            // btnGroupsEdit
            // 
            this.btnGroupsEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupsEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnGroupsEdit.ForeColor = System.Drawing.Color.White;
            this.btnGroupsEdit.Image = global::WindowsFormClient.Properties.Resources.edit;
            this.btnGroupsEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGroupsEdit.Location = new System.Drawing.Point(482, 4);
            this.btnGroupsEdit.Margin = new System.Windows.Forms.Padding(4);
            this.btnGroupsEdit.Name = "btnGroupsEdit";
            this.btnGroupsEdit.Size = new System.Drawing.Size(112, 31);
            this.btnGroupsEdit.TabIndex = 6;
            this.btnGroupsEdit.Text = "Edit";
            this.btnGroupsEdit.UseVisualStyleBackColor = false;
            this.btnGroupsEdit.Click += new System.EventHandler(this.btnGroupsEdit_Click);
            // 
            // btnGroupsAdd
            // 
            this.btnGroupsAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupsAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnGroupsAdd.ForeColor = System.Drawing.Color.White;
            this.btnGroupsAdd.Image = global::WindowsFormClient.Properties.Resources.Add;
            this.btnGroupsAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGroupsAdd.Location = new System.Drawing.Point(361, 4);
            this.btnGroupsAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnGroupsAdd.Name = "btnGroupsAdd";
            this.btnGroupsAdd.Size = new System.Drawing.Size(112, 31);
            this.btnGroupsAdd.TabIndex = 5;
            this.btnGroupsAdd.Text = "Add";
            this.btnGroupsAdd.UseVisualStyleBackColor = false;
            this.btnGroupsAdd.Click += new System.EventHandler(this.btnGroupsAdd_Click);
            // 
            // dataGridViewGroup
            // 
            this.dataGridViewGroup.AllowUserToOrderColumns = true;
            this.dataGridViewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGroup.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.dataGridViewGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGroup.Location = new System.Drawing.Point(4, 44);
            this.dataGridViewGroup.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewGroup.Name = "dataGridViewGroup";
            this.dataGridViewGroup.Size = new System.Drawing.Size(714, 587);
            this.dataGridViewGroup.TabIndex = 0;
            // 
            // tabPageUser
            // 
            this.tabPageUser.Controls.Add(this.label4);
            this.tabPageUser.Controls.Add(this.btnUsersDelete);
            this.tabPageUser.Controls.Add(this.btnUsersEdit);
            this.tabPageUser.Controls.Add(this.btnUsersAdd);
            this.tabPageUser.Controls.Add(this.dataGridViewUsers);
            this.tabPageUser.Controls.Add(this.dataGridUsers);
            this.tabPageUser.Location = new System.Drawing.Point(4, 22);
            this.tabPageUser.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageUser.Name = "tabPageUser";
            this.tabPageUser.Size = new System.Drawing.Size(718, 638);
            this.tabPageUser.TabIndex = 0;
            this.tabPageUser.Text = "User Management";
            this.tabPageUser.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.label4.Location = new System.Drawing.Point(4, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 19);
            this.label4.TabIndex = 5;
            this.label4.Text = "User Management";
            // 
            // btnUsersDelete
            // 
            this.btnUsersDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsersDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnUsersDelete.ForeColor = System.Drawing.Color.White;
            this.btnUsersDelete.Image = global::WindowsFormClient.Properties.Resources.delete;
            this.btnUsersDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsersDelete.Location = new System.Drawing.Point(602, 4);
            this.btnUsersDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnUsersDelete.Name = "btnUsersDelete";
            this.btnUsersDelete.Size = new System.Drawing.Size(112, 31);
            this.btnUsersDelete.TabIndex = 4;
            this.btnUsersDelete.Text = "Delete";
            this.btnUsersDelete.UseVisualStyleBackColor = false;
            this.btnUsersDelete.Click += new System.EventHandler(this.btnUsersDelete_Click);
            // 
            // btnUsersEdit
            // 
            this.btnUsersEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsersEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnUsersEdit.ForeColor = System.Drawing.Color.White;
            this.btnUsersEdit.Image = global::WindowsFormClient.Properties.Resources.edit;
            this.btnUsersEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsersEdit.Location = new System.Drawing.Point(482, 4);
            this.btnUsersEdit.Margin = new System.Windows.Forms.Padding(4);
            this.btnUsersEdit.Name = "btnUsersEdit";
            this.btnUsersEdit.Size = new System.Drawing.Size(112, 31);
            this.btnUsersEdit.TabIndex = 3;
            this.btnUsersEdit.Text = "Edit";
            this.btnUsersEdit.UseVisualStyleBackColor = false;
            this.btnUsersEdit.Click += new System.EventHandler(this.btnUsersEdit_Click);
            // 
            // btnUsersAdd
            // 
            this.btnUsersAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsersAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnUsersAdd.ForeColor = System.Drawing.Color.White;
            this.btnUsersAdd.Image = global::WindowsFormClient.Properties.Resources.Add;
            this.btnUsersAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsersAdd.Location = new System.Drawing.Point(361, 4);
            this.btnUsersAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnUsersAdd.Name = "btnUsersAdd";
            this.btnUsersAdd.Size = new System.Drawing.Size(112, 31);
            this.btnUsersAdd.TabIndex = 2;
            this.btnUsersAdd.Text = "Add";
            this.btnUsersAdd.UseVisualStyleBackColor = false;
            this.btnUsersAdd.Click += new System.EventHandler(this.btnUsersAdd_Click);
            // 
            // dataGridViewUsers
            // 
            this.dataGridViewUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewUsers.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.dataGridViewUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsers.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGridViewUsers.Location = new System.Drawing.Point(4, 44);
            this.dataGridViewUsers.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewUsers.Name = "dataGridViewUsers";
            this.dataGridViewUsers.Size = new System.Drawing.Size(710, 590);
            this.dataGridViewUsers.TabIndex = 1;
            // 
            // dataGridUsers
            // 
            this.dataGridUsers.AllowUserToOrderColumns = true;
            this.dataGridUsers.BackgroundColor = System.Drawing.Color.White;
            this.dataGridUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridUsers.Location = new System.Drawing.Point(0, 0);
            this.dataGridUsers.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridUsers.Name = "dataGridUsers";
            this.dataGridUsers.Size = new System.Drawing.Size(718, 638);
            this.dataGridUsers.TabIndex = 0;
            // 
            // tabApplications
            // 
            this.tabApplications.Controls.Add(this.label8);
            this.tabApplications.Controls.Add(this.btnAppDelete);
            this.tabApplications.Controls.Add(this.btnAppEdit);
            this.tabApplications.Controls.Add(this.btnAppAdd);
            this.tabApplications.Controls.Add(this.dataGridViewApp);
            this.tabApplications.ImageIndex = 2;
            this.tabApplications.Location = new System.Drawing.Point(104, 4);
            this.tabApplications.Margin = new System.Windows.Forms.Padding(0);
            this.tabApplications.Name = "tabApplications";
            this.tabApplications.Size = new System.Drawing.Size(726, 664);
            this.tabApplications.TabIndex = 2;
            this.tabApplications.Text = "Applications";
            this.tabApplications.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.label8.Location = new System.Drawing.Point(4, 18);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(197, 19);
            this.label8.TabIndex = 13;
            this.label8.Text = "Application Management";
            // 
            // btnAppDelete
            // 
            this.btnAppDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnAppDelete.ForeColor = System.Drawing.Color.White;
            this.btnAppDelete.Image = global::WindowsFormClient.Properties.Resources.delete;
            this.btnAppDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppDelete.Location = new System.Drawing.Point(611, 11);
            this.btnAppDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnAppDelete.Name = "btnAppDelete";
            this.btnAppDelete.Size = new System.Drawing.Size(112, 31);
            this.btnAppDelete.TabIndex = 11;
            this.btnAppDelete.Text = "Delete";
            this.btnAppDelete.UseVisualStyleBackColor = false;
            this.btnAppDelete.Click += new System.EventHandler(this.btnAppDelete_Click);
            // 
            // btnAppEdit
            // 
            this.btnAppEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnAppEdit.ForeColor = System.Drawing.Color.White;
            this.btnAppEdit.Image = global::WindowsFormClient.Properties.Resources.edit;
            this.btnAppEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppEdit.Location = new System.Drawing.Point(491, 11);
            this.btnAppEdit.Margin = new System.Windows.Forms.Padding(4);
            this.btnAppEdit.Name = "btnAppEdit";
            this.btnAppEdit.Size = new System.Drawing.Size(112, 31);
            this.btnAppEdit.TabIndex = 10;
            this.btnAppEdit.Text = "Edit";
            this.btnAppEdit.UseVisualStyleBackColor = false;
            this.btnAppEdit.Click += new System.EventHandler(this.btnAppEdit_Click);
            // 
            // btnAppAdd
            // 
            this.btnAppAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnAppAdd.ForeColor = System.Drawing.Color.White;
            this.btnAppAdd.Image = global::WindowsFormClient.Properties.Resources.Add;
            this.btnAppAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppAdd.Location = new System.Drawing.Point(371, 11);
            this.btnAppAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAppAdd.Name = "btnAppAdd";
            this.btnAppAdd.Size = new System.Drawing.Size(112, 31);
            this.btnAppAdd.TabIndex = 9;
            this.btnAppAdd.Text = "Add";
            this.btnAppAdd.UseVisualStyleBackColor = false;
            this.btnAppAdd.Click += new System.EventHandler(this.btnAppAdd_Click);
            // 
            // dataGridViewApp
            // 
            this.dataGridViewApp.AllowUserToOrderColumns = true;
            this.dataGridViewApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewApp.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.dataGridViewApp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewApp.Location = new System.Drawing.Point(4, 51);
            this.dataGridViewApp.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewApp.Name = "dataGridViewApp";
            this.dataGridViewApp.Size = new System.Drawing.Size(720, 604);
            this.dataGridViewApp.TabIndex = 8;
            // 
            // tabMonitors
            // 
            this.tabMonitors.Controls.Add(this.label5);
            this.tabMonitors.Controls.Add(this.btnMonitorsDelete);
            this.tabMonitors.Controls.Add(this.btnMonitorsEdit);
            this.tabMonitors.Controls.Add(this.btnMonitorsAdd);
            this.tabMonitors.Controls.Add(this.dataGridViewMonitors);
            this.tabMonitors.ImageIndex = 3;
            this.tabMonitors.Location = new System.Drawing.Point(104, 4);
            this.tabMonitors.Margin = new System.Windows.Forms.Padding(0);
            this.tabMonitors.Name = "tabMonitors";
            this.tabMonitors.Size = new System.Drawing.Size(726, 664);
            this.tabMonitors.TabIndex = 3;
            this.tabMonitors.Text = "Monitors";
            this.tabMonitors.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.label5.Location = new System.Drawing.Point(4, 18);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(169, 19);
            this.label5.TabIndex = 17;
            this.label5.Text = "Monitor Management";
            // 
            // btnMonitorsDelete
            // 
            this.btnMonitorsDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMonitorsDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnMonitorsDelete.ForeColor = System.Drawing.Color.White;
            this.btnMonitorsDelete.Image = global::WindowsFormClient.Properties.Resources.delete;
            this.btnMonitorsDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMonitorsDelete.Location = new System.Drawing.Point(611, 11);
            this.btnMonitorsDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnMonitorsDelete.Name = "btnMonitorsDelete";
            this.btnMonitorsDelete.Size = new System.Drawing.Size(112, 31);
            this.btnMonitorsDelete.TabIndex = 15;
            this.btnMonitorsDelete.Text = "Delete";
            this.btnMonitorsDelete.UseVisualStyleBackColor = false;
            this.btnMonitorsDelete.Click += new System.EventHandler(this.btnMonitorsDelete_Click);
            // 
            // btnMonitorsEdit
            // 
            this.btnMonitorsEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMonitorsEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnMonitorsEdit.ForeColor = System.Drawing.Color.White;
            this.btnMonitorsEdit.Image = global::WindowsFormClient.Properties.Resources.edit;
            this.btnMonitorsEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMonitorsEdit.Location = new System.Drawing.Point(491, 11);
            this.btnMonitorsEdit.Margin = new System.Windows.Forms.Padding(4);
            this.btnMonitorsEdit.Name = "btnMonitorsEdit";
            this.btnMonitorsEdit.Size = new System.Drawing.Size(112, 31);
            this.btnMonitorsEdit.TabIndex = 14;
            this.btnMonitorsEdit.Text = "Edit";
            this.btnMonitorsEdit.UseVisualStyleBackColor = false;
            this.btnMonitorsEdit.Click += new System.EventHandler(this.btnMonitorsEdit_Click);
            // 
            // btnMonitorsAdd
            // 
            this.btnMonitorsAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMonitorsAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.btnMonitorsAdd.ForeColor = System.Drawing.Color.White;
            this.btnMonitorsAdd.Image = global::WindowsFormClient.Properties.Resources.Add;
            this.btnMonitorsAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMonitorsAdd.Location = new System.Drawing.Point(371, 11);
            this.btnMonitorsAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnMonitorsAdd.Name = "btnMonitorsAdd";
            this.btnMonitorsAdd.Size = new System.Drawing.Size(112, 31);
            this.btnMonitorsAdd.TabIndex = 13;
            this.btnMonitorsAdd.Text = "Add";
            this.btnMonitorsAdd.UseVisualStyleBackColor = false;
            this.btnMonitorsAdd.Click += new System.EventHandler(this.btnMonitorsAdd_Click);
            // 
            // dataGridViewMonitors
            // 
            this.dataGridViewMonitors.AllowUserToOrderColumns = true;
            this.dataGridViewMonitors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMonitors.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.dataGridViewMonitors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMonitors.Location = new System.Drawing.Point(4, 51);
            this.dataGridViewMonitors.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewMonitors.Name = "dataGridViewMonitors";
            this.dataGridViewMonitors.Size = new System.Drawing.Size(720, 604);
            this.dataGridViewMonitors.TabIndex = 12;
            // 
            // tabVnc
            // 
            this.tabVnc.Controls.Add(this.label6);
            this.tabVnc.Controls.Add(this.buttonRemoteDelete);
            this.tabVnc.Controls.Add(this.buttonRemoteEdit);
            this.tabVnc.Controls.Add(this.buttonRemoteAdd);
            this.tabVnc.Controls.Add(this.dataGridViewRemote);
            this.tabVnc.ImageIndex = 4;
            this.tabVnc.Location = new System.Drawing.Point(104, 4);
            this.tabVnc.Name = "tabVnc";
            this.tabVnc.Size = new System.Drawing.Size(726, 664);
            this.tabVnc.TabIndex = 7;
            this.tabVnc.Text = "Remote";
            this.tabVnc.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.label6.Location = new System.Drawing.Point(3, 17);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 19);
            this.label6.TabIndex = 22;
            this.label6.Text = "Remote Management";
            // 
            // buttonRemoteDelete
            // 
            this.buttonRemoteDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoteDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonRemoteDelete.ForeColor = System.Drawing.Color.White;
            this.buttonRemoteDelete.Image = global::WindowsFormClient.Properties.Resources.delete;
            this.buttonRemoteDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRemoteDelete.Location = new System.Drawing.Point(610, 10);
            this.buttonRemoteDelete.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRemoteDelete.Name = "buttonRemoteDelete";
            this.buttonRemoteDelete.Size = new System.Drawing.Size(112, 31);
            this.buttonRemoteDelete.TabIndex = 21;
            this.buttonRemoteDelete.Text = "Delete";
            this.buttonRemoteDelete.UseVisualStyleBackColor = false;
            this.buttonRemoteDelete.Click += new System.EventHandler(this.buttonRemoteDelete_Click);
            // 
            // buttonRemoteEdit
            // 
            this.buttonRemoteEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoteEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonRemoteEdit.ForeColor = System.Drawing.Color.White;
            this.buttonRemoteEdit.Image = global::WindowsFormClient.Properties.Resources.edit;
            this.buttonRemoteEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRemoteEdit.Location = new System.Drawing.Point(490, 10);
            this.buttonRemoteEdit.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRemoteEdit.Name = "buttonRemoteEdit";
            this.buttonRemoteEdit.Size = new System.Drawing.Size(112, 31);
            this.buttonRemoteEdit.TabIndex = 20;
            this.buttonRemoteEdit.Text = "Edit";
            this.buttonRemoteEdit.UseVisualStyleBackColor = false;
            this.buttonRemoteEdit.Click += new System.EventHandler(this.buttonRemoteEdit_Click);
            // 
            // buttonRemoteAdd
            // 
            this.buttonRemoteAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoteAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonRemoteAdd.ForeColor = System.Drawing.Color.White;
            this.buttonRemoteAdd.Image = global::WindowsFormClient.Properties.Resources.Add;
            this.buttonRemoteAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRemoteAdd.Location = new System.Drawing.Point(370, 10);
            this.buttonRemoteAdd.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRemoteAdd.Name = "buttonRemoteAdd";
            this.buttonRemoteAdd.Size = new System.Drawing.Size(112, 31);
            this.buttonRemoteAdd.TabIndex = 19;
            this.buttonRemoteAdd.Text = "Add";
            this.buttonRemoteAdd.UseVisualStyleBackColor = false;
            this.buttonRemoteAdd.Click += new System.EventHandler(this.buttonRemoteAdd_Click);
            // 
            // dataGridViewRemote
            // 
            this.dataGridViewRemote.AllowUserToOrderColumns = true;
            this.dataGridViewRemote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewRemote.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.dataGridViewRemote.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRemote.Location = new System.Drawing.Point(4, 51);
            this.dataGridViewRemote.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewRemote.Name = "dataGridViewRemote";
            this.dataGridViewRemote.Size = new System.Drawing.Size(720, 604);
            this.dataGridViewRemote.TabIndex = 18;
            // 
            // tabDrivers
            // 
            this.tabDrivers.Controls.Add(this.label9);
            this.tabDrivers.Controls.Add(this.buttonVisionDelete);
            this.tabDrivers.Controls.Add(this.buttonVisionEdit);
            this.tabDrivers.Controls.Add(this.buttonVisionAdd);
            this.tabDrivers.Controls.Add(this.dataGridVisionInput);
            this.tabDrivers.ImageIndex = 4;
            this.tabDrivers.Location = new System.Drawing.Point(104, 4);
            this.tabDrivers.Margin = new System.Windows.Forms.Padding(0);
            this.tabDrivers.Name = "tabDrivers";
            this.tabDrivers.Size = new System.Drawing.Size(726, 664);
            this.tabDrivers.TabIndex = 4;
            this.tabDrivers.Text = "Drivers";
            this.tabDrivers.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.label9.Location = new System.Drawing.Point(3, 17);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(213, 19);
            this.label9.TabIndex = 27;
            this.label9.Text = "Capture Card Management";
            // 
            // buttonVisionDelete
            // 
            this.buttonVisionDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonVisionDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonVisionDelete.ForeColor = System.Drawing.Color.White;
            this.buttonVisionDelete.Image = global::WindowsFormClient.Properties.Resources.delete;
            this.buttonVisionDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonVisionDelete.Location = new System.Drawing.Point(610, 10);
            this.buttonVisionDelete.Margin = new System.Windows.Forms.Padding(4);
            this.buttonVisionDelete.Name = "buttonVisionDelete";
            this.buttonVisionDelete.Size = new System.Drawing.Size(112, 31);
            this.buttonVisionDelete.TabIndex = 26;
            this.buttonVisionDelete.Text = "Delete";
            this.buttonVisionDelete.UseVisualStyleBackColor = false;
            this.buttonVisionDelete.Click += new System.EventHandler(this.buttonVisionDelete_Click);
            // 
            // buttonVisionEdit
            // 
            this.buttonVisionEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonVisionEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonVisionEdit.ForeColor = System.Drawing.Color.White;
            this.buttonVisionEdit.Image = global::WindowsFormClient.Properties.Resources.edit;
            this.buttonVisionEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonVisionEdit.Location = new System.Drawing.Point(490, 10);
            this.buttonVisionEdit.Margin = new System.Windows.Forms.Padding(4);
            this.buttonVisionEdit.Name = "buttonVisionEdit";
            this.buttonVisionEdit.Size = new System.Drawing.Size(112, 31);
            this.buttonVisionEdit.TabIndex = 25;
            this.buttonVisionEdit.Text = "Edit";
            this.buttonVisionEdit.UseVisualStyleBackColor = false;
            this.buttonVisionEdit.Click += new System.EventHandler(this.buttonVisionEdit_Click);
            // 
            // buttonVisionAdd
            // 
            this.buttonVisionAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonVisionAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(169)))), ((int)(((byte)(236)))));
            this.buttonVisionAdd.ForeColor = System.Drawing.Color.White;
            this.buttonVisionAdd.Image = global::WindowsFormClient.Properties.Resources.Add;
            this.buttonVisionAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonVisionAdd.Location = new System.Drawing.Point(370, 10);
            this.buttonVisionAdd.Margin = new System.Windows.Forms.Padding(4);
            this.buttonVisionAdd.Name = "buttonVisionAdd";
            this.buttonVisionAdd.Size = new System.Drawing.Size(112, 31);
            this.buttonVisionAdd.TabIndex = 24;
            this.buttonVisionAdd.Text = "Add";
            this.buttonVisionAdd.UseVisualStyleBackColor = false;
            this.buttonVisionAdd.Click += new System.EventHandler(this.buttonVisionAdd_Click);
            // 
            // dataGridVisionInput
            // 
            this.dataGridVisionInput.AllowUserToOrderColumns = true;
            this.dataGridVisionInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridVisionInput.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.dataGridVisionInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridVisionInput.Location = new System.Drawing.Point(4, 51);
            this.dataGridVisionInput.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridVisionInput.Name = "dataGridVisionInput";
            this.dataGridVisionInput.Size = new System.Drawing.Size(720, 604);
            this.dataGridVisionInput.TabIndex = 23;
            // 
            // numericUpDownRow
            // 
            this.numericUpDownRow.Location = new System.Drawing.Point(51, 207);
            this.numericUpDownRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRow.Name = "numericUpDownRow";
            this.numericUpDownRow.Size = new System.Drawing.Size(80, 23);
            this.numericUpDownRow.TabIndex = 10;
            this.numericUpDownRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownCol
            // 
            this.numericUpDownCol.Location = new System.Drawing.Point(161, 207);
            this.numericUpDownCol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCol.Name = "numericUpDownCol";
            this.numericUpDownCol.Size = new System.Drawing.Size(80, 23);
            this.numericUpDownCol.TabIndex = 11;
            this.numericUpDownCol.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FormServer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(834, 672);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(300, 710);
            this.Name = "FormServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vistrol Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.onFormClosed);
            this.Load += new System.EventHandler(this.onFormLoad);
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.groupBoxGeneral.ResumeLayout(false);
            this.groupBoxGeneral.PerformLayout();
            this.tabUsers.ResumeLayout(false);
            this.tabUsersUser.ResumeLayout(false);
            this.tabPageGroup.ResumeLayout(false);
            this.tabPageGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroup)).EndInit();
            this.tabPageUser.ResumeLayout(false);
            this.tabPageUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsers)).EndInit();
            this.tabApplications.ResumeLayout(false);
            this.tabApplications.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewApp)).EndInit();
            this.tabMonitors.ResumeLayout(false);
            this.tabMonitors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonitors)).EndInit();
            this.tabVnc.ResumeLayout(false);
            this.tabVnc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRemote)).EndInit();
            this.tabDrivers.ResumeLayout(false);
            this.tabDrivers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVisionInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomTabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabApplications;
        private System.Windows.Forms.TabPage tabMonitors;
        private System.Windows.Forms.TabPage tabDrivers;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label labelGeneral;
        private System.Windows.Forms.Label labelServerPort;
        private System.Windows.Forms.TextBox textBoxGeneralMax;
        private System.Windows.Forms.TextBox textBoxGeneralMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabUsersUser;
        private System.Windows.Forms.TabPage tabPageUser;
        private System.Windows.Forms.TabPage tabPageGroup;
        private System.Windows.Forms.DataGridView dataGridUsers;
        private System.Windows.Forms.DataGridView dataGridViewGroup;
        private System.Windows.Forms.DataGridView dataGridViewUsers;
        private System.Windows.Forms.Button btnUsersDelete;
        private System.Windows.Forms.Button btnUsersEdit;
        private System.Windows.Forms.Button btnUsersAdd;
        private System.Windows.Forms.Button btnGroupsDelete;
        private System.Windows.Forms.Button btnGroupsEdit;
        private System.Windows.Forms.Button btnGroupsAdd;
        private System.Windows.Forms.Button btnAppDelete;
        private System.Windows.Forms.Button btnAppEdit;
        private System.Windows.Forms.Button btnAppAdd;
        private System.Windows.Forms.DataGridView dataGridViewApp;
        private System.Windows.Forms.Button btnMonitorsDelete;
        private System.Windows.Forms.Button btnMonitorsEdit;
        private System.Windows.Forms.Button btnMonitorsAdd;
        private System.Windows.Forms.DataGridView dataGridViewMonitors;
        private System.Windows.Forms.Button btnGeneralStart;
        private System.Windows.Forms.Button btnGeneralStop;
        private System.Windows.Forms.GroupBox groupBoxGeneral;
        private System.Windows.Forms.NotifyIcon notifyIconServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabVnc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonRemoteDelete;
        private System.Windows.Forms.Button buttonRemoteEdit;
        private System.Windows.Forms.Button buttonRemoteAdd;
        private System.Windows.Forms.DataGridView dataGridViewRemote;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonVisionDelete;
        private System.Windows.Forms.Button buttonVisionEdit;
        private System.Windows.Forms.Button buttonVisionAdd;
        private System.Windows.Forms.DataGridView dataGridVisionInput;
        private System.Windows.Forms.NumericUpDown numericUpDownCol;
        private System.Windows.Forms.NumericUpDown numericUpDownRow;

    }
}