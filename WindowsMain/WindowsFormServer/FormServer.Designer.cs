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
            this.tabControl = new WindowsFormClient.CustomTabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupBoxGeneral = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxGeneralPath = new System.Windows.Forms.TextBox();
            this.btnGeneralBrowse = new System.Windows.Forms.Button();
            this.textBoxGeneralMax = new System.Windows.Forms.TextBox();
            this.textBoxGeneralMin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxGeneralColumn = new System.Windows.Forms.ComboBox();
            this.labelMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.comboBoxGeneralRow = new System.Windows.Forms.ComboBox();
            this.btnGeneralStop = new System.Windows.Forms.Button();
            this.btnGeneralStart = new System.Windows.Forms.Button();
            this.labelGeneral = new System.Windows.Forms.Label();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.tabUsersUser = new System.Windows.Forms.TabControl();
            this.tabPageUser = new System.Windows.Forms.TabPage();
            this.btnUsersDelete = new System.Windows.Forms.Button();
            this.btnUsersEdit = new System.Windows.Forms.Button();
            this.btnUsersAdd = new System.Windows.Forms.Button();
            this.dataGridViewUsers = new System.Windows.Forms.DataGridView();
            this.dataGridUsers = new System.Windows.Forms.DataGridView();
            this.tabPageGroup = new System.Windows.Forms.TabPage();
            this.btnGroupsDelete = new System.Windows.Forms.Button();
            this.btnGroupsEdit = new System.Windows.Forms.Button();
            this.btnGroupsAdd = new System.Windows.Forms.Button();
            this.dataGridViewGroup = new System.Windows.Forms.DataGridView();
            this.tabApplications = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAppDelete = new System.Windows.Forms.Button();
            this.btnAppEdit = new System.Windows.Forms.Button();
            this.btnAppAdd = new System.Windows.Forms.Button();
            this.dataGridViewApp = new System.Windows.Forms.DataGridView();
            this.tabMonitors = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.btnMonitorsDelete = new System.Windows.Forms.Button();
            this.btnMonitorsEdit = new System.Windows.Forms.Button();
            this.btnMonitorsAdd = new System.Windows.Forms.Button();
            this.dataGridViewMonitors = new System.Windows.Forms.DataGridView();
            this.tabDrivers = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBoxGeneral.SuspendLayout();
            this.tabUsers.SuspendLayout();
            this.tabUsersUser.SuspendLayout();
            this.tabPageUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsers)).BeginInit();
            this.tabPageGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroup)).BeginInit();
            this.tabApplications.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewApp)).BeginInit();
            this.tabMonitors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonitors)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "general.ico");
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabUsers);
            this.tabControl.Controls.Add(this.tabApplications);
            this.tabControl.Controls.Add(this.tabMonitors);
            this.tabControl.Controls.Add(this.tabDrivers);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageList1;
            this.tabControl.ItemSize = new System.Drawing.Size(100, 100);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(784, 562);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.groupBoxGeneral);
            this.tabGeneral.Controls.Add(this.btnGeneralStop);
            this.tabGeneral.Controls.Add(this.btnGeneralStart);
            this.tabGeneral.Controls.Add(this.labelGeneral);
            this.tabGeneral.ImageIndex = 0;
            this.tabGeneral.Location = new System.Drawing.Point(104, 4);
            this.tabGeneral.Margin = new System.Windows.Forms.Padding(0);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(676, 554);
            this.tabGeneral.TabIndex = 5;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBoxGeneral
            // 
            this.groupBoxGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxGeneral.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxGeneral.Controls.Add(this.label4);
            this.groupBoxGeneral.Controls.Add(this.textBoxGeneralPath);
            this.groupBoxGeneral.Controls.Add(this.btnGeneralBrowse);
            this.groupBoxGeneral.Controls.Add(this.textBoxGeneralMax);
            this.groupBoxGeneral.Controls.Add(this.textBoxGeneralMin);
            this.groupBoxGeneral.Controls.Add(this.label3);
            this.groupBoxGeneral.Controls.Add(this.label1);
            this.groupBoxGeneral.Controls.Add(this.comboBoxGeneralColumn);
            this.groupBoxGeneral.Controls.Add(this.labelMin);
            this.groupBoxGeneral.Controls.Add(this.label2);
            this.groupBoxGeneral.Controls.Add(this.labelServerPort);
            this.groupBoxGeneral.Controls.Add(this.comboBoxGeneralRow);
            this.groupBoxGeneral.Location = new System.Drawing.Point(30, 44);
            this.groupBoxGeneral.Name = "groupBoxGeneral";
            this.groupBoxGeneral.Size = new System.Drawing.Size(575, 422);
            this.groupBoxGeneral.TabIndex = 16;
            this.groupBoxGeneral.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(243, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "TightVNC Client Installation Path:";
            // 
            // textBoxGeneralPath
            // 
            this.textBoxGeneralPath.Location = new System.Drawing.Point(28, 249);
            this.textBoxGeneralPath.Name = "textBoxGeneralPath";
            this.textBoxGeneralPath.ReadOnly = true;
            this.textBoxGeneralPath.Size = new System.Drawing.Size(453, 20);
            this.textBoxGeneralPath.TabIndex = 10;
            this.textBoxGeneralPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnGeneralBrowse
            // 
            this.btnGeneralBrowse.Location = new System.Drawing.Point(487, 249);
            this.btnGeneralBrowse.Name = "btnGeneralBrowse";
            this.btnGeneralBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnGeneralBrowse.TabIndex = 12;
            this.btnGeneralBrowse.Text = "Browse";
            this.btnGeneralBrowse.UseVisualStyleBackColor = true;
            this.btnGeneralBrowse.Click += new System.EventHandler(this.btnGeneralBrowse_Click);
            // 
            // textBoxGeneralMax
            // 
            this.textBoxGeneralMax.Location = new System.Drawing.Point(68, 77);
            this.textBoxGeneralMax.Name = "textBoxGeneralMax";
            this.textBoxGeneralMax.Size = new System.Drawing.Size(100, 20);
            this.textBoxGeneralMax.TabIndex = 5;
            this.textBoxGeneralMax.Text = "8010";
            this.textBoxGeneralMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxGeneralMin
            // 
            this.textBoxGeneralMin.Location = new System.Drawing.Point(68, 48);
            this.textBoxGeneralMin.Name = "textBoxGeneralMin";
            this.textBoxGeneralMin.Size = new System.Drawing.Size(100, 20);
            this.textBoxGeneralMin.TabIndex = 4;
            this.textBoxGeneralMin.Text = "8000";
            this.textBoxGeneralMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(97, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "x";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(32, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Max";
            // 
            // comboBoxGeneralColumn
            // 
            this.comboBoxGeneralColumn.FormattingEnabled = true;
            this.comboBoxGeneralColumn.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.comboBoxGeneralColumn.Location = new System.Drawing.Point(119, 165);
            this.comboBoxGeneralColumn.Name = "comboBoxGeneralColumn";
            this.comboBoxGeneralColumn.Size = new System.Drawing.Size(49, 21);
            this.comboBoxGeneralColumn.TabIndex = 8;
            this.comboBoxGeneralColumn.Text = "1";
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Location = new System.Drawing.Point(32, 51);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(24, 13);
            this.labelMin.TabIndex = 2;
            this.labelMin.Text = "Min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Server Screen Matrix:";
            // 
            // labelServerPort
            // 
            this.labelServerPort.AutoSize = true;
            this.labelServerPort.BackColor = System.Drawing.Color.Transparent;
            this.labelServerPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServerPort.Location = new System.Drawing.Point(28, 16);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(144, 20);
            this.labelServerPort.TabIndex = 1;
            this.labelServerPort.Text = "Server Port Range:";
            // 
            // comboBoxGeneralRow
            // 
            this.comboBoxGeneralRow.FormattingEnabled = true;
            this.comboBoxGeneralRow.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.comboBoxGeneralRow.Location = new System.Drawing.Point(35, 165);
            this.comboBoxGeneralRow.Name = "comboBoxGeneralRow";
            this.comboBoxGeneralRow.Size = new System.Drawing.Size(54, 21);
            this.comboBoxGeneralRow.TabIndex = 7;
            this.comboBoxGeneralRow.Text = "1";
            // 
            // btnGeneralStop
            // 
            this.btnGeneralStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGeneralStop.Location = new System.Drawing.Point(557, 494);
            this.btnGeneralStop.Name = "btnGeneralStop";
            this.btnGeneralStop.Size = new System.Drawing.Size(75, 23);
            this.btnGeneralStop.TabIndex = 15;
            this.btnGeneralStop.Text = "Stop Server";
            this.btnGeneralStop.UseVisualStyleBackColor = true;
            this.btnGeneralStop.Click += new System.EventHandler(this.btnGeneralStop_Click);
            // 
            // btnGeneralStart
            // 
            this.btnGeneralStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGeneralStart.Location = new System.Drawing.Point(476, 494);
            this.btnGeneralStart.Name = "btnGeneralStart";
            this.btnGeneralStart.Size = new System.Drawing.Size(75, 23);
            this.btnGeneralStart.TabIndex = 14;
            this.btnGeneralStart.Text = "Start Server";
            this.btnGeneralStart.UseVisualStyleBackColor = true;
            this.btnGeneralStart.Click += new System.EventHandler(this.btnGeneralStart_Click);
            // 
            // labelGeneral
            // 
            this.labelGeneral.AutoSize = true;
            this.labelGeneral.BackColor = System.Drawing.Color.Transparent;
            this.labelGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGeneral.Location = new System.Drawing.Point(4, 4);
            this.labelGeneral.Name = "labelGeneral";
            this.labelGeneral.Size = new System.Drawing.Size(130, 37);
            this.labelGeneral.TabIndex = 0;
            this.labelGeneral.Text = "General";
            // 
            // tabUsers
            // 
            this.tabUsers.Controls.Add(this.tabUsersUser);
            this.tabUsers.Location = new System.Drawing.Point(104, 4);
            this.tabUsers.Margin = new System.Windows.Forms.Padding(0);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Size = new System.Drawing.Size(676, 554);
            this.tabUsers.TabIndex = 6;
            this.tabUsers.Text = "Users";
            this.tabUsers.UseVisualStyleBackColor = true;
            // 
            // tabUsersUser
            // 
            this.tabUsersUser.Controls.Add(this.tabPageUser);
            this.tabUsersUser.Controls.Add(this.tabPageGroup);
            this.tabUsersUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabUsersUser.Location = new System.Drawing.Point(0, 0);
            this.tabUsersUser.Name = "tabUsersUser";
            this.tabUsersUser.SelectedIndex = 0;
            this.tabUsersUser.Size = new System.Drawing.Size(676, 554);
            this.tabUsersUser.TabIndex = 0;
            // 
            // tabPageUser
            // 
            this.tabPageUser.Controls.Add(this.btnUsersDelete);
            this.tabPageUser.Controls.Add(this.btnUsersEdit);
            this.tabPageUser.Controls.Add(this.btnUsersAdd);
            this.tabPageUser.Controls.Add(this.dataGridViewUsers);
            this.tabPageUser.Controls.Add(this.dataGridUsers);
            this.tabPageUser.Location = new System.Drawing.Point(4, 22);
            this.tabPageUser.Name = "tabPageUser";
            this.tabPageUser.Size = new System.Drawing.Size(668, 528);
            this.tabPageUser.TabIndex = 0;
            this.tabPageUser.Text = "User Management";
            this.tabPageUser.UseVisualStyleBackColor = true;
            // 
            // btnUsersDelete
            // 
            this.btnUsersDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsersDelete.Location = new System.Drawing.Point(589, 3);
            this.btnUsersDelete.Name = "btnUsersDelete";
            this.btnUsersDelete.Size = new System.Drawing.Size(75, 23);
            this.btnUsersDelete.TabIndex = 4;
            this.btnUsersDelete.Text = "Delete";
            this.btnUsersDelete.UseVisualStyleBackColor = true;
            this.btnUsersDelete.Click += new System.EventHandler(this.btnUsersDelete_Click);
            // 
            // btnUsersEdit
            // 
            this.btnUsersEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsersEdit.Location = new System.Drawing.Point(507, 3);
            this.btnUsersEdit.Name = "btnUsersEdit";
            this.btnUsersEdit.Size = new System.Drawing.Size(75, 23);
            this.btnUsersEdit.TabIndex = 3;
            this.btnUsersEdit.Text = "Edit";
            this.btnUsersEdit.UseVisualStyleBackColor = true;
            this.btnUsersEdit.Click += new System.EventHandler(this.btnUsersEdit_Click);
            // 
            // btnUsersAdd
            // 
            this.btnUsersAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsersAdd.Location = new System.Drawing.Point(426, 3);
            this.btnUsersAdd.Name = "btnUsersAdd";
            this.btnUsersAdd.Size = new System.Drawing.Size(75, 23);
            this.btnUsersAdd.TabIndex = 2;
            this.btnUsersAdd.Text = "Add";
            this.btnUsersAdd.UseVisualStyleBackColor = true;
            this.btnUsersAdd.Click += new System.EventHandler(this.btnUsersAdd_Click);
            // 
            // dataGridViewUsers
            // 
            this.dataGridViewUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewUsers.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridViewUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsers.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGridViewUsers.Location = new System.Drawing.Point(0, 32);
            this.dataGridViewUsers.Name = "dataGridViewUsers";
            this.dataGridViewUsers.Size = new System.Drawing.Size(668, 496);
            this.dataGridViewUsers.TabIndex = 1;
            // 
            // dataGridUsers
            // 
            this.dataGridUsers.AllowUserToOrderColumns = true;
            this.dataGridUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridUsers.Location = new System.Drawing.Point(0, 0);
            this.dataGridUsers.Name = "dataGridUsers";
            this.dataGridUsers.Size = new System.Drawing.Size(668, 528);
            this.dataGridUsers.TabIndex = 0;
            // 
            // tabPageGroup
            // 
            this.tabPageGroup.Controls.Add(this.btnGroupsDelete);
            this.tabPageGroup.Controls.Add(this.btnGroupsEdit);
            this.tabPageGroup.Controls.Add(this.btnGroupsAdd);
            this.tabPageGroup.Controls.Add(this.dataGridViewGroup);
            this.tabPageGroup.Location = new System.Drawing.Point(4, 22);
            this.tabPageGroup.Name = "tabPageGroup";
            this.tabPageGroup.Size = new System.Drawing.Size(668, 528);
            this.tabPageGroup.TabIndex = 1;
            this.tabPageGroup.Text = "Group Management";
            this.tabPageGroup.UseVisualStyleBackColor = true;
            // 
            // btnGroupsDelete
            // 
            this.btnGroupsDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupsDelete.Location = new System.Drawing.Point(589, 3);
            this.btnGroupsDelete.Name = "btnGroupsDelete";
            this.btnGroupsDelete.Size = new System.Drawing.Size(75, 23);
            this.btnGroupsDelete.TabIndex = 7;
            this.btnGroupsDelete.Text = "Delete";
            this.btnGroupsDelete.UseVisualStyleBackColor = true;
            this.btnGroupsDelete.Click += new System.EventHandler(this.btnGroupsDelete_Click);
            // 
            // btnGroupsEdit
            // 
            this.btnGroupsEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupsEdit.Location = new System.Drawing.Point(507, 3);
            this.btnGroupsEdit.Name = "btnGroupsEdit";
            this.btnGroupsEdit.Size = new System.Drawing.Size(75, 23);
            this.btnGroupsEdit.TabIndex = 6;
            this.btnGroupsEdit.Text = "Edit";
            this.btnGroupsEdit.UseVisualStyleBackColor = true;
            this.btnGroupsEdit.Click += new System.EventHandler(this.btnGroupsEdit_Click);
            // 
            // btnGroupsAdd
            // 
            this.btnGroupsAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupsAdd.Location = new System.Drawing.Point(426, 3);
            this.btnGroupsAdd.Name = "btnGroupsAdd";
            this.btnGroupsAdd.Size = new System.Drawing.Size(75, 23);
            this.btnGroupsAdd.TabIndex = 5;
            this.btnGroupsAdd.Text = "Add";
            this.btnGroupsAdd.UseVisualStyleBackColor = true;
            this.btnGroupsAdd.Click += new System.EventHandler(this.btnGroupsAdd_Click);
            // 
            // dataGridViewGroup
            // 
            this.dataGridViewGroup.AllowUserToOrderColumns = true;
            this.dataGridViewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGroup.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.dataGridViewGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGroup.Location = new System.Drawing.Point(0, 32);
            this.dataGridViewGroup.Name = "dataGridViewGroup";
            this.dataGridViewGroup.Size = new System.Drawing.Size(668, 496);
            this.dataGridViewGroup.TabIndex = 0;
            // 
            // tabApplications
            // 
            this.tabApplications.Controls.Add(this.label5);
            this.tabApplications.Controls.Add(this.btnAppDelete);
            this.tabApplications.Controls.Add(this.btnAppEdit);
            this.tabApplications.Controls.Add(this.btnAppAdd);
            this.tabApplications.Controls.Add(this.dataGridViewApp);
            this.tabApplications.Location = new System.Drawing.Point(104, 4);
            this.tabApplications.Margin = new System.Windows.Forms.Padding(0);
            this.tabApplications.Name = "tabApplications";
            this.tabApplications.Size = new System.Drawing.Size(676, 554);
            this.tabApplications.TabIndex = 2;
            this.tabApplications.Text = "Applications";
            this.tabApplications.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(3, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Application Management";
            // 
            // btnAppDelete
            // 
            this.btnAppDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppDelete.Location = new System.Drawing.Point(593, 8);
            this.btnAppDelete.Name = "btnAppDelete";
            this.btnAppDelete.Size = new System.Drawing.Size(75, 23);
            this.btnAppDelete.TabIndex = 11;
            this.btnAppDelete.Text = "Delete";
            this.btnAppDelete.UseVisualStyleBackColor = true;
            this.btnAppDelete.Click += new System.EventHandler(this.btnAppDelete_Click);
            // 
            // btnAppEdit
            // 
            this.btnAppEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppEdit.Location = new System.Drawing.Point(511, 8);
            this.btnAppEdit.Name = "btnAppEdit";
            this.btnAppEdit.Size = new System.Drawing.Size(75, 23);
            this.btnAppEdit.TabIndex = 10;
            this.btnAppEdit.Text = "Edit";
            this.btnAppEdit.UseVisualStyleBackColor = true;
            this.btnAppEdit.Click += new System.EventHandler(this.btnAppEdit_Click);
            // 
            // btnAppAdd
            // 
            this.btnAppAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppAdd.Location = new System.Drawing.Point(430, 8);
            this.btnAppAdd.Name = "btnAppAdd";
            this.btnAppAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAppAdd.TabIndex = 9;
            this.btnAppAdd.Text = "Add";
            this.btnAppAdd.UseVisualStyleBackColor = true;
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
            this.dataGridViewApp.Location = new System.Drawing.Point(3, 37);
            this.dataGridViewApp.Name = "dataGridViewApp";
            this.dataGridViewApp.Size = new System.Drawing.Size(670, 514);
            this.dataGridViewApp.TabIndex = 8;
            // 
            // tabMonitors
            // 
            this.tabMonitors.Controls.Add(this.label6);
            this.tabMonitors.Controls.Add(this.btnMonitorsDelete);
            this.tabMonitors.Controls.Add(this.btnMonitorsEdit);
            this.tabMonitors.Controls.Add(this.btnMonitorsAdd);
            this.tabMonitors.Controls.Add(this.dataGridViewMonitors);
            this.tabMonitors.Location = new System.Drawing.Point(104, 4);
            this.tabMonitors.Margin = new System.Windows.Forms.Padding(0);
            this.tabMonitors.Name = "tabMonitors";
            this.tabMonitors.Size = new System.Drawing.Size(676, 554);
            this.tabMonitors.TabIndex = 3;
            this.tabMonitors.Text = "Monitors";
            this.tabMonitors.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(3, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Monitor Management";
            // 
            // btnMonitorsDelete
            // 
            this.btnMonitorsDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMonitorsDelete.Location = new System.Drawing.Point(593, 8);
            this.btnMonitorsDelete.Name = "btnMonitorsDelete";
            this.btnMonitorsDelete.Size = new System.Drawing.Size(75, 23);
            this.btnMonitorsDelete.TabIndex = 15;
            this.btnMonitorsDelete.Text = "Delete";
            this.btnMonitorsDelete.UseVisualStyleBackColor = true;
            this.btnMonitorsDelete.Click += new System.EventHandler(this.btnMonitorsDelete_Click);
            // 
            // btnMonitorsEdit
            // 
            this.btnMonitorsEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMonitorsEdit.Location = new System.Drawing.Point(511, 8);
            this.btnMonitorsEdit.Name = "btnMonitorsEdit";
            this.btnMonitorsEdit.Size = new System.Drawing.Size(75, 23);
            this.btnMonitorsEdit.TabIndex = 14;
            this.btnMonitorsEdit.Text = "Edit";
            this.btnMonitorsEdit.UseVisualStyleBackColor = true;
            this.btnMonitorsEdit.Click += new System.EventHandler(this.btnMonitorsEdit_Click);
            // 
            // btnMonitorsAdd
            // 
            this.btnMonitorsAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMonitorsAdd.Location = new System.Drawing.Point(430, 8);
            this.btnMonitorsAdd.Name = "btnMonitorsAdd";
            this.btnMonitorsAdd.Size = new System.Drawing.Size(75, 23);
            this.btnMonitorsAdd.TabIndex = 13;
            this.btnMonitorsAdd.Text = "Add";
            this.btnMonitorsAdd.UseVisualStyleBackColor = true;
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
            this.dataGridViewMonitors.Location = new System.Drawing.Point(3, 37);
            this.dataGridViewMonitors.Name = "dataGridViewMonitors";
            this.dataGridViewMonitors.Size = new System.Drawing.Size(670, 514);
            this.dataGridViewMonitors.TabIndex = 12;
            // 
            // tabDrivers
            // 
            this.tabDrivers.Location = new System.Drawing.Point(104, 4);
            this.tabDrivers.Margin = new System.Windows.Forms.Padding(0);
            this.tabDrivers.Name = "tabDrivers";
            this.tabDrivers.Size = new System.Drawing.Size(676, 554);
            this.tabDrivers.TabIndex = 4;
            this.tabDrivers.Text = "Drivers";
            this.tabDrivers.UseVisualStyleBackColor = true;
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tabControl);
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
            this.tabPageUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsers)).EndInit();
            this.tabPageGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroup)).EndInit();
            this.tabApplications.ResumeLayout(false);
            this.tabApplications.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewApp)).EndInit();
            this.tabMonitors.ResumeLayout(false);
            this.tabMonitors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonitors)).EndInit();
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
        private System.Windows.Forms.ComboBox comboBoxGeneralColumn;
        private System.Windows.Forms.ComboBox comboBoxGeneralRow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxGeneralPath;
        private System.Windows.Forms.Button btnGeneralBrowse;
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnMonitorsDelete;
        private System.Windows.Forms.Button btnMonitorsEdit;
        private System.Windows.Forms.Button btnMonitorsAdd;
        private System.Windows.Forms.DataGridView dataGridViewMonitors;
        private System.Windows.Forms.Button btnGeneralStart;
        private System.Windows.Forms.Button btnGeneralStop;
        private System.Windows.Forms.GroupBox groupBoxGeneral;

    }
}