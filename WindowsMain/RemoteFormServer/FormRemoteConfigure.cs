using CustomUI;
using System;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Forms;
using WcfServiceLibrary1;
using WindowsFormClient;
using WindowsFormClient.Presenter;
using WindowsFormClient.Server;

namespace RemoteFormServer
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public partial class FormRemoteConfigure : Form, IService1Callback
    {
        IService1 wcfService;

        private UsersPresenter userPresenter;
        private GroupsPresenter groupPresenter;
        private MonitorsPresenter monitorPresenter;
        private ApplicationsPresenter applicationPresenter;
        private RemoteVncPresenter remoteVncPresenter;
        private VisionInputPresenter visionInputPresenter;

        public FormRemoteConfigure()
        {
            InitializeComponent();
            this.Load += FormRemoteConfigure_Load;

            remoteVncPresenter = new RemoteVncPresenter();
            applicationPresenter = new ApplicationsPresenter();
            groupPresenter = new GroupsPresenter();
            userPresenter = new UsersPresenter();
            visionInputPresenter = new VisionInputPresenter();
            monitorPresenter = new MonitorsPresenter();
        }

        void FormRemoteConfigure_Load(object sender, EventArgs e)
        {
            setupDataGrid(dataGridViewRemote, remoteVncPresenter.GetRemoteVncData());
            setupDataGrid(dataGridViewApp, applicationPresenter.GetApplicationTable());
            setupDataGrid(dataGridViewGroup, groupPresenter.GetGroupsTable());
            setupDataGrid(dataGridViewUsers, userPresenter.GetUsersTable());
            setupDataGrid(dataGridVisionInput, visionInputPresenter.GetVisionInputTable());
            setupDataGrid(dataGridViewMonitors, monitorPresenter.GetMonitorsTable());

            // customize for vision control
            dataGridVisionInput.Columns[2].Visible = false;
            dataGridVisionInput.Columns[3].Visible = false;
            dataGridVisionInput.Columns[4].Visible = false;
        }

        public void Initialize(IService1 service)
        {
            this.wcfService = service;

            // initialize db helper
            WindowsFormClient.Server.ServerDbHelper.GetInstance().Initialize(service);
        }

        private void buttonVisionAdd_Click(object sender, System.EventArgs e)
        {
            FormVision visionFrm = new FormVision();
            visionFrm.Text = "Add Capture Card Entry";
            WindowsFormClient.RgbInput.Window window = new WindowsFormClient.RgbInput.Window();
            WindowsFormClient.RgbInput.Input input = new WindowsFormClient.RgbInput.Input();
            WindowsFormClient.RgbInput.OnScreenDisplay osd = new WindowsFormClient.RgbInput.OnScreenDisplay();

            // set the empty object to the form
            visionFrm.WindowObj = window;
            visionFrm.InputObj = input;
            visionFrm.OnScreenDisplayObj = osd;
            visionFrm.NumberOfInputs = ServerDbHelper.GetInstance().GetSystemInputCount();

            if (visionFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // add to db
                visionInputPresenter.AddVisionInput(visionFrm.WindowObj, visionFrm.InputObj, visionFrm.OnScreenDisplayObj);
            }
        }

        private void buttonVisionEdit_Click(object sender, System.EventArgs e)
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

        private void buttonVisionDelete_Click(object sender, System.EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.OK)
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

        System.Windows.Forms.DialogResult showDeleteMessageBox()
        {
            return MessageBox.Show("Are you sure want to delete checked data?");
        }

        System.Windows.Forms.DialogResult showDeleteMessageBoxMonitor()
        {
            return MessageBox.Show("Are you sure want to delete checked data?" + Environment.NewLine + "Groups assosiate with this monitor info will be modify to allow viewing full desktop.");
        }

        System.Windows.Forms.DialogResult showDeleteMessageBoxGroup()
        {
            return MessageBox.Show("Are you sure want to delete checked data?" + Environment.NewLine + "Users assosiate with this group will be deleted.");
        }

        private void buttonRemoteAdd_Click(object sender, System.EventArgs e)
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

        private void buttonRemoteEdit_Click(object sender, System.EventArgs e)
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

        private void buttonRemoteDelete_Click(object sender, System.EventArgs e)
        {
            if (showDeleteMessageBox() != System.Windows.Forms.DialogResult.OK)
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

        private void btnMonitorsAdd_Click(object sender, System.EventArgs e)
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

        private void btnMonitorsEdit_Click(object sender, System.EventArgs e)
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

        private void btnMonitorsDelete_Click(object sender, System.EventArgs e)
        {
            if (showDeleteMessageBoxMonitor() != System.Windows.Forms.DialogResult.OK)
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

        private void btnAppAdd_Click(object sender, System.EventArgs e)
        {
            FormApplication fromApp = new FormApplication(String.Empty);
            fromApp.BrowseButtonEnabled = false;
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

        private void btnAppEdit_Click(object sender, System.EventArgs e)
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
                    formApp.BrowseButtonEnabled = false;

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

        private void btnAppDelete_Click(object sender, System.EventArgs e)
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
        }

        private void btnUsersAdd_Click(object sender, System.EventArgs e)
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

        private void btnUsersEdit_Click(object sender, System.EventArgs e)
        {
            // get current selected item
            foreach (DataGridViewRow row in dataGridViewUsers.Rows)
            {
                if (row.Cells[0].Value != null &&
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

        private void btnUsersDelete_Click(object sender, System.EventArgs e)
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
        }

        private void btnGroupsAdd_Click(object sender, System.EventArgs e)
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

            }
        }

        private void btnGroupsEdit_Click(object sender, System.EventArgs e)
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
                    int currentMonitorId = groupPresenter.GetMonitorId(groupId);
                    if (currentMonitorId != -1)
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
                            formGroup.MonitorId,
                            formGroup.GetSelectedApplicationsId());
                    }
                }
            }
        }

        private void btnGroupsDelete_Click(object sender, System.EventArgs e)
        {
            if (showDeleteMessageBoxGroup() != System.Windows.Forms.DialogResult.OK)
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
            chkHeader.OnCheckBoxClicked += chkHeader_OnCheckBoxClicked;
            view.Columns.Add(chkbox);

            // set the data
            view.DataSource = dataSource;
            view.Columns[1].Visible = false;        // hide the id of the database data
        }

        void chkHeader_OnCheckBoxClicked(DataGridView view, bool state)
        {
            foreach (DataGridViewRow row in view.Rows)
            {
                row.Cells[0].Value = state;
            }
            view.EndEdit();
        }

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

                if (e.RowIndex == (view.RowCount - 1))
                {
                    e.Graphics.DrawLine(p, new Point(e.CellBounds.Left, e.CellBounds.Bottom),
                                       new Point(e.CellBounds.Right, e.CellBounds.Bottom));
                }
            }

            e.PaintContent(e.ClipBounds);
        }

        void reloadDataGrid(DataGridView view, object dataSource)
        {
            view.DataSource = dataSource;
        }


        private delegate void updateDataDelegate(DBType dbType, int index);

        public void OnUserDBAdded(DBType dbType, int dbIndex)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new updateDataDelegate(OnUserDBAdded), dbType, dbIndex);
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

        public void OnUserDBEditing(DBType dbType, int dbIndex)
        {
            // do nothing
        }

        public void onUserDBRemoving(DBType dbType, int dbIndex)
        {
            // do nothing
        }

        public void OnUserDBEdited(DBType dbType, int dbIndex)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new updateDataDelegate(OnUserDBEdited), dbType, dbIndex);
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

        public void onUserDBRemoved(DBType dbType, int dbIndex)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new updateDataDelegate(onUserDBRemoved), dbType, dbIndex);
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
