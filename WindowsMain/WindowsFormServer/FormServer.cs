using CustomUI;
using Session.Connection;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WcfServiceLibrary1;
using WindowsFormClient.Presenter;
using WindowsFormClient.Server;

namespace WindowsFormClient
{
    public partial class FormServer : Form, IServer
    {
        private const string USERNAME = "username";
        private const string PASSWORD = "password";

        private MainPresenter mainPresenter;
        private UsersPresenter userPresenter;
        private GroupsPresenter groupPresenter;
        private MonitorsPresenter monitorPresenter;
        private ApplicationsPresenter applicationPresenter;
        private ConnectionPresenter connectionPresenter;
        private RemoteVncPresenter remoteVncPresenter;
        private VisionInputPresenter visionInputPresenter;

        private ConnectionManager connectionMgr;
        private VncMarshall.Client vncClient;

        private string vncServerPath;
        private int desktopRow = 1;
        private int desktopColumn = 1;

        private delegate void updateDataDelegate(DBType dbType);

        public FormServer(ConnectionManager connectionMgr)
        {
            InitializeComponent();

            this.connectionMgr = connectionMgr;

            // initialize presenters
            mainPresenter = new MainPresenter();
            userPresenter = new UsersPresenter();
            groupPresenter = new GroupsPresenter();
            monitorPresenter = new MonitorsPresenter();
            applicationPresenter = new ApplicationsPresenter();
            connectionPresenter = new ConnectionPresenter(this, connectionMgr);
            remoteVncPresenter = new RemoteVncPresenter();
            visionInputPresenter = new VisionInputPresenter();

            // create right click menu
            ContextMenu menuContext = new ContextMenu();
            this.ContextMenu = menuContext;

            MenuItem mnuItemAbout = new MenuItem();
            mnuItemAbout.Text = "About";
            mnuItemAbout.Click += mnuItemAbout_Click;
            menuContext.MenuItems.Add(mnuItemAbout);

           // tabControl.ImageList = new ImageList();
        }

        void mnuItemAbout_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog(this);
        }

        private void onFormLoad(object sender, EventArgs e)
        {
            string vncClientPath = mainPresenter.VncPath;

            // validate if the file existed
            if (vncClientPath == String.Empty ||
                File.Exists(vncClientPath) == false)
            {
                // vncviewer.exe
                foreach (String matchPath in Utils.Files.DirSearch(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "tvnviewer.exe"))
                {
                    vncClientPath = matchPath;
                    break;
                }

                if (vncClientPath == String.Empty)
                {
                    foreach (String matchPath in Utils.Files.DirSearch(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "tvnviewer.exe"))
                    {
                        vncClientPath = matchPath;
                        break;
                    }
                }
            }

            if (vncClientPath == String.Empty)
            {
                MessageBox.Show("VNC executable not found." + Environment.NewLine + "Please install VNC application to use VNC feature.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                vncServerPath = vncClientPath;
            }

            notifyIconServer.Text = "Vistrol server is offline";
            notifyIconServer.BalloonTipTitle = "Vistrol Server";
            notifyIconServer.BalloonTipIcon = ToolTipIcon.Info;
            notifyIconServer.Icon = Properties.Resources.server1;
            notifyIconServer.Visible = true;
            notifyIconServer.DoubleClick += notifyIconServer_DoubleClick;

            this.Resize += FormServer_Resize;

            setupDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
            setupDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
            setupDataGrid(dataGridViewMonitors, monitorPresenter.GetMonitorsTable());
            setupDataGrid(dataGridViewApp, applicationPresenter.GetApplicationTable());
            setupDataGrid(dataGridViewRemote, remoteVncPresenter.GetRemoteVncData());
            setupDataGrid(dataGridVisionInput, visionInputPresenter.GetVisionInputTable());

            // customize for vision control
            dataGridVisionInput.Columns[2].Visible = false;
            dataGridVisionInput.Columns[3].Visible = false;
            dataGridVisionInput.Columns[4].Visible = false;

            textBoxGeneralMin.Text = mainPresenter.PortMin.ToString();
            textBoxGeneralMax.Text = mainPresenter.PortMax.ToString();

            numericUpDownRow.Value = mainPresenter.ScreenRow == 0 ? 1 : mainPresenter.ScreenRow;
            numericUpDownCol.Value = mainPresenter.ScreenColumn == 0 ? 1 : mainPresenter.ScreenColumn;

            refreshGeneralPanel();

            // customization
            if (mainPresenter.PortMin != 0 &&
                mainPresenter.PortMax != 0)
            {
                // start the server automatically
                btnGeneralStart_Click(this, new EventArgs());

                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                FormLogin formLogin = new FormLogin(USERNAME, PASSWORD);
                System.Windows.Forms.DialogResult result = formLogin.ShowDialog(this);
                while (result != System.Windows.Forms.DialogResult.OK)
                {
                    result = formLogin.ShowDialog(this);
                }
            }
        }

        void FormServer_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Visible = false;
            }
        }

        void notifyIconServer_DoubleClick(object sender, EventArgs e)
        {
            if(this.Visible == false)
            {
                FormLogin formLogin = new FormLogin(USERNAME, PASSWORD);
                if(formLogin.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    // show this dialog
                    this.Visible = true;
                    this.WindowState = FormWindowState.Normal;
                }
            }
            
        }

        #region General

        private void btnGeneralStart_Click(object sender, EventArgs e)
        {
            int portMax = 0;
            if(textBoxGeneralMax.Text.Length == 0 ||
                false == int.TryParse(textBoxGeneralMax.Text, out portMax))
            {
                MessageBox.Show("Invalid data");
                return;
            }

            int portMin = 0;
            if (textBoxGeneralMin.Text.Length == 0 ||
                false == int.TryParse(textBoxGeneralMin.Text, out portMin))
            {
                MessageBox.Show("Invalid data");
                return;
            }

            // initialize vnc client
            vncClient = new VncMarshall.Client(vncServerPath);

            // save the matrix setting
            desktopRow = Convert.ToInt32(numericUpDownRow.Value);
            desktopColumn = Convert.ToInt32(numericUpDownCol.Value);

            // start the server
            int portOpened = connectionMgr.StartServer(portMin, portMax);
            if (portOpened == -1)
            {
                // error start server
                MessageBox.Show("Error start server", "Error");
                return;
            }


            this.Text = String.Format("Vistrol Server (Listening Port: {0})", portOpened);
            notifyIconServer.Text = String.Format("Vistrol server is running at port {0}", portOpened);

            refreshGeneralPanel();

            // save current setting to db
            mainPresenter.PortMin = portMin;
            mainPresenter.PortMax = portMax;
            mainPresenter.ScreenColumn = desktopColumn;
            mainPresenter.ScreenRow = desktopRow;
            mainPresenter.VncPath = vncServerPath;
        }

        private void btnGeneralStop_Click(object sender, EventArgs e)
        {
            this.Text = String.Format("Vistrol Server - Offline");
            notifyIconServer.Text = String.Format("Vistrol server is offline");
            connectionMgr.StopServer();

            refreshGeneralPanel();
        }

        private bool isServerRunning()
        {
            return connectionMgr.IsStarted();
        }

        private void refreshGeneralPanel()
        {
            groupBoxGeneral.Enabled = !isServerRunning();
            btnGeneralStart.Enabled = !isServerRunning();
            btnGeneralStop.Enabled = isServerRunning();
        }

        #endregion

        void reloadDataGrid(DataGridView view, object dataSource)
        {
            view.DataSource = dataSource;
        }

        private void setupDataGrid(DataGridView view, object dataSource)
        {
            // change the header color
            view.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(68, 101, 128);
            view.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            view.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            view.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(view.Font, FontStyle.Bold);
            view.EnableHeadersVisualStyles = false;

            // change the selection row color
            view.DefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 169, 236);
            view.DefaultCellStyle.SelectionForeColor = Color.Black;

            // change the grid border style
            view.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            view.CellBorderStyle = DataGridViewCellBorderStyle.None;
            view.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            view.CellPainting += view_CellPainting;

            // attributes for the datagrid
            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            view.MultiSelect = true;
            view.RowHeadersVisible = false;
            view.AllowUserToDeleteRows = false;
            view.AllowUserToAddRows = false;
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // add checkbox header
            DataGridViewCheckBoxColumn chkbox = new DataGridViewCheckBoxColumn();
            chkbox.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DatagridViewCheckBoxHeaderCell chkHeader = new DatagridViewCheckBoxHeaderCell();
            chkbox.HeaderCell = chkHeader;
            chkHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(chkHeader_OnCheckBoxClicked);
            view.Columns.Add(chkbox);

            // set the data
            view.DataSource = dataSource;
            view.Columns[1].Visible = false;        // hide the id of the database data
        }

        /// <summary>
        /// custom draw the column for each datagrid control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void view_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView view = (DataGridView)sender;

            e.Handled = true;

            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                // header color
                using (Brush b = new SolidBrush(view.ColumnHeadersDefaultCellStyle.BackColor))
                {
                    e.Graphics.FillRectangle(b, e.CellBounds);
                }
            }
            else if (e.RowIndex % 2 == 0)
            {
                // custom color
                using (Brush b = new SolidBrush(Color.FromArgb(238, 238, 238)))
                {
                    e.Graphics.FillRectangle(b, e.CellBounds);
                }
            }
            else
            {
                // normal color
                using (Brush b = new SolidBrush(view.DefaultCellStyle.BackColor))
                {
                    e.Graphics.FillRectangle(b, e.CellBounds);
                }
            }

            // draw the vertical line
            using (Pen p = new Pen(Brushes.Black))
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                if (e.ColumnIndex > 0)
                {
                    e.Graphics.DrawLine(p, new Point(e.CellBounds.Left, e.CellBounds.Top),
                                       new Point(e.CellBounds.Left, e.CellBounds.Bottom));
                }

                e.Graphics.DrawLine(p, new Point(e.CellBounds.Right, e.CellBounds.Top),
                                       new Point(e.CellBounds.Right, e.CellBounds.Bottom));

                if (e.RowIndex == (view.RowCount-1))
                {
                    e.Graphics.DrawLine(p, new Point(e.CellBounds.Left, e.CellBounds.Bottom),
                                       new Point(e.CellBounds.Right, e.CellBounds.Bottom));
                }
            }

            e.PaintContent(e.ClipBounds);
        }

        void chkHeader_OnCheckBoxClicked(DataGridView view, bool state)
        {
            foreach (DataGridViewRow row in view.Rows)
            {
                row.Cells[0].Value = state;
            }
            view.EndEdit();
        }

        System.Windows.Forms.DialogResult showDeleteMessageBox()
        {
            return MessageBox.Show(this, "Are you sure want to delete checked data?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        System.Windows.Forms.DialogResult showDeleteMessageBoxMonitor()
        {
            return MessageBox.Show(this, "Are you sure want to delete checked data?" + Environment.NewLine + "Groups assosiate with this monitor info will be modify to allow viewing full desktop.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        System.Windows.Forms.DialogResult showDeleteMessageBoxGroup()
        {
            return MessageBox.Show(this, "Are you sure want to delete checked data?" + Environment.NewLine + "Users assosiate with this group will be deleted.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        #region User
        private void btnUsersAdd_Click(object sender, EventArgs e)
        {
            // show the form user
            FormUser formUser = new FormUser(String.Empty);
            formUser.Text = "Add User";
            formUser.SetGroups(userPresenter.GetGroupsList());
            if (formUser.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // add to database
                userPresenter.AddUser(formUser.DisplayName, formUser.UserName, formUser.Password, formUser.SelectedGroupId);
            }
        }

        private void btnUsersEdit_Click(object sender, EventArgs e)
        {
            // get current selected item
            foreach (DataGridViewRow row in dataGridViewUsers.Rows)
            {
                if(row.Cells[0].Value != null &&
                    (bool)row.Cells[0].Value)
                {
                    int userId = (int)row.Cells[1].Value;
                    string username = (string)row.Cells[3].Value;

                    FormUser formUser = new FormUser(username);
                    formUser.Text = "Edit User";
                    formUser.SetGroups(userPresenter.GetGroupsList());
                    formUser.DisplayName = (string)row.Cells[2].Value;
                    formUser.UserName = username;
                    formUser.Password = (string)row.Cells[4].Value;
                    formUser.SelectedGroupName = (string)row.Cells[5].Value;

                    if (formUser.ShowDialog(this) == System.Windows.Forms.DialogResult.OK &&
                        formUser.IsDirty)
                    {
                        // add to database
                        userPresenter.EditUser(userId, formUser.DisplayName, formUser.UserName, formUser.Password, formUser.SelectedGroupId);
                    }
                }                
            }
        }

        private void btnUsersDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            foreach (DataGridViewRow row in dataGridViewUsers.Rows)
            {
                if (row.Cells[0].Value != null &&
                    (bool)row.Cells[0].Value)
                {
                    int userId = (int)row.Cells[1].Value;
                    userPresenter.RemoveUser(userId);
                }
            }
        }

        #endregion

        #region Group
        private void btnGroupsAdd_Click(object sender, EventArgs e)
        {
            // show the form user
            FormGroup formGroup = new FormGroup(String.Empty);
            formGroup.Text = "Add Group";
            formGroup.SetMonitorList(groupPresenter.GetMonitorsList());
            formGroup.SetApplicationList(groupPresenter.GetApplicationList());
            if (formGroup.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // add to database
                groupPresenter.AddGroup(
                    formGroup.GroupName, 
                    formGroup.WholeDesktop, 
                    formGroup.AllowMaintenance, 
                    formGroup.AllowRemoteControl,
                    formGroup.MonitorId,
                    formGroup.GetSelectedApplicationsId());
            }
        }

        private void btnGroupsEdit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewGroup.Rows)
            {
                if (row.Cells[0].Value != null &&
                        (bool)row.Cells[0].Value)
                {
                    // show the form user
                    int groupId = (int)row.Cells[1].Value;
                    string groupName = (string)row.Cells[2].Value;
                    FormGroup formGroup = new FormGroup(groupName);
                    formGroup.Text = "Edit Group";
                    formGroup.SetMonitorList(groupPresenter.GetMonitorsList());
                    formGroup.SetApplicationList(groupPresenter.GetApplicationList());

                    formGroup.GroupName = groupName;
                    formGroup.WholeDesktop = (bool)row.Cells[3].Value;
                    formGroup.AllowMaintenance = (bool)row.Cells[4].Value;
                    formGroup.AllowRemoteControl = (bool)row.Cells[5].Value;
                    formGroup.SetSelectedApplications(groupPresenter.GetApplicationsId(groupId));
                    int currentMonitorId = groupPresenter.GetMonitorId(groupId);
                    if(currentMonitorId != -1)
                    {
                        formGroup.MonitorId = currentMonitorId;
                    }
                    

                    if (formGroup.ShowDialog(this) == System.Windows.Forms.DialogResult.OK &&
                        formGroup.IsDirty)
                    {
                        // add to database
                        groupPresenter.EditGroup(
                            groupId,
                            formGroup.GroupName,
                            formGroup.WholeDesktop,
                            formGroup.AllowMaintenance,
                            formGroup.AllowRemoteControl,
                            formGroup.MonitorId,
                            formGroup.GetSelectedApplicationsId());
                    }
                }
            }
        }

        private void btnGroupsDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBoxGroup() != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            foreach (DataGridViewRow row in dataGridViewGroup.Rows)
            {
                if (row.Cells[0].Value != null &&
                    (bool)row.Cells[0].Value)
                {
                    int groupId = (int)row.Cells[1].Value;
                    groupPresenter.RemoveGroup(groupId);
                }
            }
        }

        #endregion

        #region Application
        private void btnAppAdd_Click(object sender, EventArgs e)
        {
            FormApplication fromApp = new FormApplication(String.Empty);
            fromApp.Text = "Add Application";

            if (fromApp.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // add to database
                applicationPresenter.AddApplication(
                    fromApp.DisplayName,
                    fromApp.ExecutablePath,
                    fromApp.Arguments,
                    fromApp.PositionLeft,
                    fromApp.PositionTop,
                    fromApp.PositionLeft + fromApp.Width,
                    fromApp.PositionTop + fromApp.Height);
            }
        }

        private void btnAppEdit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewApp.Rows)
            {
                if (row.Cells[0].Value != null &&
                        (bool)row.Cells[0].Value)
                {
                    // show the form user
                    int appId = (int)row.Cells[1].Value;
                    string appName = (string)row.Cells[2].Value;
                    FormApplication formApp = new FormApplication(appName);
                    formApp.Text = "Edit Application";

                    formApp.DisplayName = appName;
                    formApp.ExecutablePath = (string)row.Cells[3].Value;
                    formApp.Arguments = (string)row.Cells[4].Value;

                    string[] area = ((string)row.Cells[5].Value).Split(',');
                    int left = int.Parse(area[0]);
                    int top = int.Parse(area[1]);
                    int right = int.Parse(area[2]);
                    int bottom = int.Parse(area[3]);

                    formApp.PositionLeft = left;
                    formApp.PositionTop = top;
                    formApp.Width = right - left;
                    formApp.Height = bottom - top;

                    if (formApp.ShowDialog(this) == System.Windows.Forms.DialogResult.OK &&
                        formApp.IsDirty)
                    {
                        // add to database
                        applicationPresenter.EditApplication(
                            appId,
                            formApp.DisplayName,
                            formApp.ExecutablePath,
                            formApp.Arguments,
                            formApp.PositionLeft,
                            formApp.PositionTop,
                            formApp.PositionLeft + formApp.Width,
                            formApp.PositionTop + formApp.Height);
                    }
                }
            }
        }

        private void btnAppDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            foreach (DataGridViewRow row in dataGridViewApp.Rows)
            {
                if (row.Cells[0].Value != null &&
                    (bool)row.Cells[0].Value)
                {
                    int appId = (int)row.Cells[1].Value;
                    applicationPresenter.RemoveApplication(appId);
                }
            }
        }
        #endregion

        #region Monitors

        private void btnMonitorsAdd_Click(object sender, EventArgs e)
        {
            FormMonitor formMonitor = new FormMonitor(String.Empty);
            formMonitor.Text = "Add Monitor";
            if (formMonitor.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // add to database
                monitorPresenter.AddMonitor(
                    formMonitor.DisplayName,
                    formMonitor.LocationX,
                    formMonitor.LocationY,
                    formMonitor.LocationX + formMonitor.Width,
                    formMonitor.LocationY + formMonitor.Height);
            }
        }

        private void btnMonitorsEdit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewMonitors.Rows)
            {
                if (row.Cells[0].Value != null &&
                        (bool)row.Cells[0].Value)
                {
                    // show the form user
                    int monitorId = (int)row.Cells[1].Value;
                    string monitorName = (string)row.Cells[2].Value;

                    FormMonitor formMonitor = new FormMonitor(monitorName);
                    formMonitor.Text = "Edit Monitor";

                    formMonitor.DisplayName = monitorName;
                    string[] area = ((string)row.Cells[3].Value).Split(',');
                    int left = int.Parse(area[0]);
                    int top = int.Parse(area[1]);
                    int right = int.Parse(area[2]);
                    int bottom = int.Parse(area[3]);

                    formMonitor.LocationX = left;
                    formMonitor.LocationY = top;
                    formMonitor.Width = right - left;
                    formMonitor.Height = bottom - top;

                    if (formMonitor.ShowDialog(this) == System.Windows.Forms.DialogResult.OK &&
                        formMonitor.IsDirty)
                    {
                        // add to database
                        monitorPresenter.EditMonitor(
                            monitorId,
                            formMonitor.DisplayName,
                            formMonitor.LocationX,
                            formMonitor.LocationY,
                            formMonitor.LocationX + formMonitor.Width,
                            formMonitor.LocationY + formMonitor.Height);
                    }
                }
            }
        }

        private void btnMonitorsDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBoxMonitor() != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            foreach (DataGridViewRow row in dataGridViewMonitors.Rows)
            {
                if (row.Cells[0].Value != null &&
                    (bool)row.Cells[0].Value)
                {
                    int monitorId = (int)row.Cells[1].Value;
                    monitorPresenter.RemoveMonitor(monitorId);
                }
            }

        }
        #endregion

        #region Connection Methods
        public void ClientLogin(Server.Model.ClientInfoModel model)
        {
            connectionPresenter.ClientCredentialReceived(model, desktopRow, desktopColumn);
        }

        public VncMarshall.Client GetVncClient()
        {
            return vncClient;
        }

        public Server.Model.ClientInfoModel GetClientInfo(string userId)
        {
            return ConnectedClientHelper.GetInstance().GetClientInfo(userId);
        }
        #endregion

        public void AddMessageBox(string message, Font font, Color color, Color bgndColor, int duration, int left, int top, int width, int height, bool animation)
        {
            /*
            if(this.InvokeRequired)
            {
                this.Invoke(new DelegateAddMessageBox(AddMessageBox), message, font, color, bgndColor, duration, left, top, width, height);
                return;
            }

            FormMessage formMessage = new FormMessage();
            formMessage.Message = message;
            formMessage.MessageFont = font;
            formMessage.MessageColor = color;
            formMessage.MessageDuration = duration;
            formMessage.Show(null);
            formMessage.Location = new Point(left, top);
            formMessage.BackColor = bgndColor;
             */

            string arg = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" {4} {5} {6} {7} {8} {9}", 
                message,
                new Session.Common.SerializableFont(font).SerializeFontAttribute,
                System.Drawing.ColorTranslator.ToHtml(color),
                System.Drawing.ColorTranslator.ToHtml(bgndColor),
                duration,
                left,
                top,
                width,
                height,
                animation);
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = "CustomMessageBox.exe",
                Arguments = arg
            };

            try
            {
                Process.Start(info);
            }
            catch (Exception)
            {

            }
            
        }

        private void onFormClosed(object sender, FormClosedEventArgs e)
        {
            if (vncClient != null)
            {
                vncClient.StopAllClients();
            }
            
            connectionMgr.StopServer();
            connectionPresenter.Dispose();

            Server.ServerDbHelper.GetInstance().Shutdown();
        }


        public ConnectionManager GetConnectionMgr()
        {
            return this.connectionMgr;
        }

        private void buttonRemoteAdd_Click(object sender, EventArgs e)
        {
            FormRemoteVnc formVnc = new FormRemoteVnc();
            formVnc.Text = "Add Remote Data";
            if (formVnc.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // add to database
                remoteVncPresenter.AddVnc(
                    formVnc.DisplayName,
                    formVnc.RemoteIp,
                    formVnc.RemotePort);
            }
        }

        private void buttonRemoteEdit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewRemote.Rows)
            {
                if (row.Cells[0].Value != null &&
                        (bool)row.Cells[0].Value)
                {
                    // show the form user
                    int vncDataId = (int)row.Cells[1].Value;
                    string vncLabel = (string)row.Cells[2].Value;
                    string remoteIp = (string)row.Cells[3].Value;
                    int remotePort = (int)row.Cells[4].Value;

                    FormRemoteVnc formVnc = new FormRemoteVnc();
                    formVnc.Text = "Edit Remote Data";
                    formVnc.DisplayName = vncLabel;
                    formVnc.RemoteIp = remoteIp;
                    formVnc.RemotePort = remotePort;

                    if (formVnc.ShowDialog(this) == System.Windows.Forms.DialogResult.OK &&
                        formVnc.IsDirty)
                    {
                        // add to database
                        remoteVncPresenter.EditVnc(
                            vncDataId,
                            formVnc.DisplayName,
                            formVnc.RemoteIp,
                            formVnc.RemotePort);
                    }
                }
            }
        }

        private void buttonRemoteDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            foreach (DataGridViewRow row in dataGridViewRemote.Rows)
            {
                if (row.Cells[0].Value != null &&
                    (bool)row.Cells[0].Value)
                {
                    int vncDataId = (int)row.Cells[1].Value;
                    remoteVncPresenter.RemoveVnc(vncDataId);
                }
            }
        }

        private void buttonVisionAdd_Click(object sender, EventArgs e)
        {
            FormVision visionFrm = new FormVision();
            visionFrm.Text = "Add Capture Card Entry";
            WindowsFormClient.RgbInput.Window window = new RgbInput.Window();
            WindowsFormClient.RgbInput.Input input = new RgbInput.Input();
            WindowsFormClient.RgbInput.OnScreenDisplay osd = new RgbInput.OnScreenDisplay();

            // set the empty object to the form
            visionFrm.WindowObj = window;
            visionFrm.InputObj = input;
            visionFrm.OnScreenDisplayObj = osd;
            visionFrm.NumberOfInputs = ServerDbHelper.GetInstance().GetSystemInputCount();

            if( visionFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // add to db
                visionInputPresenter.AddVisionInput(visionFrm.WindowObj, visionFrm.InputObj, visionFrm.OnScreenDisplayObj);
            }
        }

        private void buttonVisionEdit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridVisionInput.Rows)
            {
                if (row.Cells[0].Value != null &&
                        (bool)row.Cells[0].Value)
                {
                    // show the form user
                    uint visionDataId = (uint)row.Cells[1].Value;
                    string visionWnd = (string)row.Cells[2].Value;
                    string visionInput = (string)row.Cells[3].Value;
                    string visionOSD = (string)row.Cells[4].Value;

                    FormVision formVision = new FormVision();
                    formVision.WindowObj = visionInputPresenter.GetWindowFromString(visionWnd);
                    formVision.InputObj = visionInputPresenter.GetInputFromString(visionInput);
                    formVision.OnScreenDisplayObj = visionInputPresenter.GetOSDFromString(visionOSD);
                    formVision.NumberOfInputs = ServerDbHelper.GetInstance().GetSystemInputCount();

                    if (formVision.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        visionInputPresenter.EditVisionInput(
                            visionDataId,
                            formVision.WindowObj,
                            formVision.InputObj,
                            formVision.OnScreenDisplayObj);
                    }
                }
            }
        }

        private void buttonVisionDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            foreach (DataGridViewRow row in dataGridVisionInput.Rows)
            {
                if (row.Cells[0].Value != null &&
                    (bool)row.Cells[0].Value)
                {
                    uint visionDataId = (uint)row.Cells[1].Value;
                    visionInputPresenter.RemoveVisionInput(visionDataId);
                }
            }
        }


        public void OnGridDataUpdateRequest(WindowsFormClient.ServerCommandType command, DBType dbType)
        {
            switch (command)
            {
                case ServerCommandType.Added:
                    OnUserDBAdded(dbType);
                    break;
                case ServerCommandType.Edited:
                    OnUserDBEdited(dbType);
                    break;
                case ServerCommandType.Removed:
                    onUserDBRemoved(dbType);
                    break;
            }
        }

        private void OnUserDBAdded(DBType dbType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new updateDataDelegate(OnUserDBAdded), dbType);
                return;
            }

            UpdateUIView(dbType);

            switch (dbType)
            {
                case DBType.User:
                    reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
                    break;
            }
        }

        private void OnUserDBEdited(DBType dbType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new updateDataDelegate(OnUserDBEdited), dbType);
                return;
            }

            UpdateUIView(dbType);

            switch (dbType)
            {
                case DBType.User:
                    reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
                    break;
            }
        }

        private void onUserDBRemoved(DBType dbType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new updateDataDelegate(onUserDBRemoved), dbType);
                return;
            }

            UpdateUIView(dbType);

            switch (dbType)
            {
                case DBType.User:
                    reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
                    break;
                case DBType.Group:
                    reloadDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
                    break;
                case DBType.Monitor:
                    reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
                    break;
            }
        }

        private void UpdateUIView(DBType dbType)
        {
            switch (dbType)
            {
                case DBType.RemoteVnc:
                    reloadDataGrid(dataGridViewRemote, remoteVncPresenter.GetRemoteVncData());
                    break;
                case DBType.Application:
                    reloadDataGrid(dataGridViewApp, applicationPresenter.GetApplicationTable());
                    break;
                case DBType.Group:
                    reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
                    break;
                case DBType.User:
                    reloadDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
                    break;
                case DBType.VisionInput:
                    reloadDataGrid(dataGridVisionInput, visionInputPresenter.GetVisionInputTable());
                    break;
                case DBType.Monitor:
                    reloadDataGrid(dataGridViewMonitors, monitorPresenter.GetMonitorsTable());
                    break;
            }
        }
    }
}
