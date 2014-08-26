using CustomUI;
using Session.Connection;
using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormClient.Presenter;
using WindowsFormClient.Server;

namespace WindowsFormClient
{
    public partial class FormServer : Form, IServer
    {
        private MainPresenter mainPresenter;
        private UsersPresenter userPresenter;
        private GroupsPresenter groupPresenter;
        private MonitorsPresenter monitorPresenter;
        private ApplicationsPresenter applicationPresenter;
        private ConnectionPresenter connectionPresenter;

        private ConnectionManager connectionMgr;
        private VncMarshall.Client vncClient;

        private int desktopRow = 1;
        private int desktopColumn = 1;

        public FormServer(ConnectionManager connectionMgr)
        {
            InitializeComponent();

            this.connectionMgr = connectionMgr;

            // initialize presenters
            mainPresenter = new MainPresenter();
            userPresenter = new UsersPresenter(connectionMgr);
            groupPresenter = new GroupsPresenter(connectionMgr);
            monitorPresenter = new MonitorsPresenter(connectionMgr);
            applicationPresenter = new ApplicationsPresenter(connectionMgr);
            connectionPresenter = new ConnectionPresenter(this, connectionMgr);

           // tabControl.ImageList = new ImageList();
        }

        private void onFormLoad(object sender, EventArgs e)
        {
            setupDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
            setupDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
            setupDataGrid(dataGridViewMonitors, monitorPresenter.GetMonitorsTable());
            setupDataGrid(dataGridViewApp, applicationPresenter.GetApplicationTable());

            textBoxGeneralMin.Text = mainPresenter.PortMin.ToString();
            textBoxGeneralMax.Text = mainPresenter.PortMax.ToString();
            textBoxGeneralPath.Text = mainPresenter.VncPath;

            comboBoxGeneralRow.SelectedIndex = (mainPresenter.ScreenRow - 1) < 0 ? 0 : mainPresenter.ScreenRow - 1;
            comboBoxGeneralColumn.SelectedIndex = (mainPresenter.ScreenColumn - 1) < 0 ? 0 : mainPresenter.ScreenColumn - 1;

            refreshGeneralPanel();
        }

        #region General
        private void btnGeneralBrowse_Click(object sender, EventArgs e)
        {
            // open file selection dialog
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Programs (*.exe)|*.exe";
            fileDialog.CheckPathExists = true;
            fileDialog.CheckFileExists = true;
            fileDialog.Multiselect = false;
            fileDialog.ShowReadOnly = false;
            fileDialog.ShowHelp = false;
            fileDialog.Title = "Browse TightVNC Client Executable";
            fileDialog.ValidateNames = true;

            if (fileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBoxGeneralPath.Text = fileDialog.FileName;
            }
        }

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

            if (textBoxGeneralPath.Text.Length == 0)
            {
                MessageBox.Show("Invalid data");
                return;
            }

            // initialize vnc client
            vncClient = new VncMarshall.Client(textBoxGeneralPath.Text);

            // save the matrix setting
            desktopRow = comboBoxGeneralRow.SelectedIndex + 1;
            desktopColumn = comboBoxGeneralColumn.SelectedIndex + 1;

            // start the server
            int portOpened = connectionMgr.StartServer(portMin, portMax);
            this.Text = String.Format("Vistrol Server (Listening Port: {0})", portOpened);

            refreshGeneralPanel();

            // save current setting to db
            mainPresenter.PortMin = portMin;
            mainPresenter.PortMax = portMax;
            mainPresenter.ScreenColumn = desktopColumn;
            mainPresenter.ScreenRow = desktopRow;
            mainPresenter.VncPath = textBoxGeneralPath.Text;
        }

        private void btnGeneralStop_Click(object sender, EventArgs e)
        {
            this.Text = String.Format("Vistrol Server - Offline");
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
            // attributes for the datagrid
            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            view.MultiSelect = true;
            view.RowHeadersVisible = false;
            view.AllowUserToDeleteRows = false;
            view.AllowUserToAddRows = false;
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // add checkbox header
            DataGridViewCheckBoxColumn chkbox = new DataGridViewCheckBoxColumn();
            DatagridViewCheckBoxHeaderCell chkHeader = new DatagridViewCheckBoxHeaderCell();
            chkbox.HeaderCell = chkHeader;
            chkHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(chkHeader_OnCheckBoxClicked);
            view.Columns.Add(chkbox);

            // set the data
            view.DataSource = dataSource;
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
            return MessageBox.Show("Are you sure want to delete checked data?");
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

                reloadDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
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

            reloadDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
            reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
        }

        private void btnUsersDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.OK)
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

            reloadDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
            reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
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
                    formGroup.MonitorId,
                    formGroup.GetSelectedApplicationsId());

                reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
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
                    formGroup.SetSelectedApplications(groupPresenter.GetApplicationsId(groupId));
                    formGroup.MonitorId = groupPresenter.GetMonitorId(groupId);

                    if (formGroup.ShowDialog(this) == System.Windows.Forms.DialogResult.OK &&
                        formGroup.IsDirty)
                    {
                        // add to database
                        groupPresenter.EditGroup(
                            groupId,
                            formGroup.GroupName,
                            formGroup.WholeDesktop,
                            formGroup.AllowMaintenance,
                            formGroup.MonitorId,
                            formGroup.GetSelectedApplicationsId());
                    }
                }
            }

            reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
        }

        private void btnGroupsDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.OK)
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

            reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
            reloadDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
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

                reloadDataGrid(dataGridViewApp, applicationPresenter.GetApplicationTable());
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

            reloadDataGrid(dataGridViewApp, applicationPresenter.GetApplicationTable());
        }

        private void btnAppDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.OK)
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

            reloadDataGrid(dataGridViewApp, applicationPresenter.GetApplicationTable());
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

                reloadDataGrid(dataGridViewMonitors, monitorPresenter.GetMonitorsTable());
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

            reloadDataGrid(dataGridViewMonitors, monitorPresenter.GetMonitorsTable());
        }

        private void btnMonitorsDelete_Click(object sender, EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.OK)
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

            reloadDataGrid(dataGridViewMonitors, monitorPresenter.GetMonitorsTable());
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

        public void AddMessageBox(string message, Font font, Color color, int duration, int left, int top, int width, int height)
        {
            throw new NotImplementedException();
        }

        private void onFormClosed(object sender, FormClosedEventArgs e)
        {
            if (vncClient != null)
            {
                vncClient.StopAllClients();
            }
            
            connectionMgr.StopServer();
            connectionPresenter.Dispose();
        }


        public ConnectionManager GetConnectionMgr()
        {
            return this.connectionMgr;
        }
    }
}
