using CustomUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormClient.Presenter;

namespace WindowsFormClient
{
    public partial class FormServer : Form
    {
        private MainPresenter mainPresenter;
        private UsersPresenter userPresenter;
        private GroupsPresenter groupPresenter;
        private MonitorsPresenter monitorPresenter;
        private ApplicationsPresenter applicationPresenter;

        public FormServer()
        {
            InitializeComponent();

            // initialize database
            Server.ServerDbHelper.GetInstance().Initialize();

            // initialize presenters
            mainPresenter = new MainPresenter();
            userPresenter = new UsersPresenter();
            groupPresenter = new GroupsPresenter();
            monitorPresenter = new MonitorsPresenter();
            applicationPresenter = new ApplicationsPresenter();

           // tabControl.ImageList = new ImageList();
        }

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

            if(fileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBoxGeneralPath.Text = fileDialog.FileName;
            }
        }

        private void onFormLoad(object sender, EventArgs e)
        {
            setupDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
            setupDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
            setupDataGrid(dataGridViewMonitors, monitorPresenter.GetMonitorsTable());
            setupDataGrid(dataGridViewApp, applicationPresenter.GetApplicationTable());
        }

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
            if(formUser.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // add to database
                Server.ServerDbHelper.GetInstance().AddUser(formUser.DisplayName, formUser.UserName, formUser.Password, formUser.SelectedGroupId);

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
                    string username = (string)row.Cells[2].Value;

                    FormUser formUser = new FormUser(username);
                    formUser.Text = "Edit User";
                    formUser.SetGroups(userPresenter.GetGroupsList());
                    formUser.DisplayName = username;
                    formUser.UserName = (string)row.Cells[3].Value;
                    formUser.Password = (string)row.Cells[4].Value;
                    formUser.SelectedGroupName = (string)row.Cells[5].Value;

                    if (formUser.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        // add to database
                        Server.ServerDbHelper.GetInstance().EditUser(userId, formUser.DisplayName, formUser.UserName, formUser.Password, formUser.SelectedGroupId);
                    }
                }                
            }

            reloadDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
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
                    Server.ServerDbHelper.GetInstance().RemoveUser(userId);
                }
            }

            reloadDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
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
                Server.ServerDbHelper.GetInstance().AddGroup(
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

                    if (formGroup.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        // add to database
                        Server.ServerDbHelper.GetInstance().EditGroup(
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
                    Server.ServerDbHelper.GetInstance().RemoveGroup(groupId);
                }
            }

            reloadDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
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
                Server.ServerDbHelper.GetInstance().AddApplication(
                    fromApp.DisplayName,
                    fromApp.Arguments,
                    fromApp.ExecutablePath,
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

                    if (formApp.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        // add to database
                        Server.ServerDbHelper.GetInstance().EditApplication(
                            appId,
                            formApp.DisplayName,
                            formApp.Arguments,
                            formApp.ExecutablePath,
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
                    Server.ServerDbHelper.GetInstance().RemoveApplication(appId);
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
                Server.ServerDbHelper.GetInstance().AddMonitor(
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

                    if (formMonitor.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        // add to database
                        Server.ServerDbHelper.GetInstance().EditMonitor(
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
                    Server.ServerDbHelper.GetInstance().RemoveMonitor(monitorId);
                }
            }

            reloadDataGrid(dataGridViewMonitors, monitorPresenter.GetMonitorsTable());
        }
        #endregion
    }
}
