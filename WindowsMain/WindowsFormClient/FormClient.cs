using CustomWinForm;
using Session.Connection;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utils.Hooks;
using Utils.Windows;
using WeifenLuo.WinFormsUI.Docking;
using WindowsFormClient.Client.Model;
using WindowsFormClient.Comparer;

namespace WindowsFormClient
{
    public partial class FormClient : Form, IClient
    {
        public delegate void DelegateServerReply(FormClient sender);
        public event DelegateServerReply EvtServerReply;

        private const string CONFIG_FILE_NAME = "DockPanel.config";

        private ConnectionManager connectionMgr;
        private Presenter.ClientPresenter clientPresenter;

        /// <summary>
        /// helper class
        /// </summary>
        private MouseHook mouseHook;
        private KeyboardHook keyboardHook;

        /// <summary>
        ///  UI class
        /// </summary>
        private FormPresets formPreset;
        private FormRunningApps formRunningApps;
        private FormVnc formVnc;
        private FormMimic formMimic;
        private FormApplications formApps;
        private FormInput formInput;
        private CustomControlHolder holder;

        /// <summary>
        /// dock UI related
        /// </summary>
        private DeserializeDockContent deserializeDockContent;

        /// <summary>
        /// storage
        /// </summary>
        /// <param name="mgr"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private List<Client.Model.WindowsModel> applicationList = new List<Client.Model.WindowsModel>();

        private delegate void DelegateUI();
        private delegate void DelegateWndChange(IList<Client.Model.WindowsModel> wndsList);
        private delegate void DelegateVncList(IList<Client.Model.VncModel> vncList);
        private delegate void DelegatePresetList(IList<Client.Model.PresetModel> presetList);
        private delegate void DelegateMaintenanceStatus(Client.Model.UserPriviledgeModel priviledge);
        private delegate void DelegateRefreshLayout(Client.Model.UserInfoModel user, Client.Model.ServerLayoutModel layout, WindowsModel viewingArea);
        private delegate void DelegateRefreshViewArea(WindowsModel viewingArea);
        private delegate void DelegateRefreshAppList(IList<Client.Model.ApplicationModel> appsList);
        private delegate void DelegateRefreshVisionInputList(List<InputAttributes> attributeList);

        public FormClient(ConnectionManager mgr, string username, string password)
        {
            InitializeComponent();
            this.connectionMgr = mgr;

            // initialize helper classes
            clientPresenter = new Presenter.ClientPresenter(this, mgr, username, password);
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            notifyIconClient.BalloonTipTitle = "Vistrol Client";
            notifyIconClient.BalloonTipIcon = ToolTipIcon.Info;
            notifyIconClient.Icon = Properties.Resources.client;
            notifyIconClient.Visible = true;
            notifyIconClient.DoubleClick += notifyIconClient_DoubleClick;

            this.IsMdiContainer = false;
            dockPanel.DocumentStyle = DocumentStyle.DockingSdi;

            // create the dock controls
            createControls();
            deserializeDockContent = new DeserializeDockContent(getContentFromPersistString);

            string configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), CONFIG_FILE_NAME);
            if (File.Exists(configFile))
            {
                dockPanel.LoadFromXml(configFile, deserializeDockContent);
            }
            else
            {
                loadNewLayout();
            }

            // add the custom mimic windows holder
            holder = new CustomControlHolder(new Size(0,0), 0, 0);
            formMimic.AddMimicHolder(holder);

            // register events from the holder
            holder.onDelegateClosedEvt += holder_onDelegateClosedEvt;
            holder.onDelegateMaximizedEvt += holder_onDelegateMaximizedEvt;
            holder.onDelegateMinimizedEvt += holder_onDelegateMinimizedEvt;
            holder.onDelegatePosChangedEvt += holder_onDelegatePosChangedEvt;
            holder.onDelegateRestoredEvt += holder_onDelegateRestoredEvt;
            holder.onDelegateSizeChangedEvt += holder_onDelegateSizeChangedEvt;
            holderPointer = holder.Handle;

            // initialize helper classes
            mouseHook = new MouseHook();
            mouseHook.HookInvoked += mouseHook_HookInvoked;
            keyboardHook = new KeyboardHook();
            keyboardHook.HookInvoked += keyboardHook_HookInvoked;
        }

        void notifyIconClient_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        /*
        /// <summary>
        /// get the maximized size of the control by not actually maxmized the parent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormClient_SizeChanged(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
            {
                holder.MaximizedSize = getMaximizedClientSize();
                holder.SetMaximized();
            }
            else if(this.WindowState == FormWindowState.Normal)
            {
                holder.SetRestore();
            }
        }*/

        /*
        private Size getMaximizedClientSize()
        {
            var original = this.WindowState;
            try
            {
                NativeMethods.SendMessage(this.Handle, (int)Constant.WM_SETREDRAW, new IntPtr(0), IntPtr.Zero);

                this.WindowState = FormWindowState.Maximized;
                return holder.ClientSize;

            }
            finally
            {
                this.WindowState = original;
                NativeMethods.SendMessage(this.Handle, (int)Constant.WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            }
        }
        */

        /*
        void formMimic_SizeChanged(object sender, EventArgs e)
        {
            if (holder != null)
            {
                holder.RefreshLayout();
            }
        }*/

        void holder_onDelegateSizeChangedEvt(int id, Size newSize)
        {
            clientPresenter.SetApplicationSize(id, newSize);
        }

        void holder_onDelegateRestoredEvt(int id)
        {
            clientPresenter.SetApplicationRestore(id);
        }

        void holder_onDelegatePosChangedEvt(int id, int xPos, int yPos)
        {
            clientPresenter.SetApplicationPos(id, xPos, yPos);
        }

        void holder_onDelegateMinimizedEvt(int id)
        {
            clientPresenter.SetApplicationMinimize(id);
        }

        void holder_onDelegateMaximizedEvt(int id)
        {
            clientPresenter.SetApplicationMaximize(id);
        }

        void holder_onDelegateClosedEvt(int id)
        {
            clientPresenter.SetApplicationClose(id);
        }

        private void createControls()
        {
            formPreset = new FormPresets();
            formPreset.CloseButtonVisible = false;
            formPreset.DockAreas = DockAreas.DockLeft | DockAreas.DockTop | DockAreas.DockRight | DockAreas.DockBottom | DockAreas.Float;
            formPreset.EvtPresetAdded += formPreset_EvtPresetAdded;
            formPreset.EvtPresetRemoved += formPreset_EvtPresetRemoved;

            formVnc = new FormVnc();
            formVnc.CloseButtonVisible = false;
            formVnc.DockAreas = DockAreas.DockLeft | DockAreas.DockTop | DockAreas.DockRight | DockAreas.DockBottom | DockAreas.Float;

            formRunningApps = new FormRunningApps();
            formRunningApps.CloseButtonVisible = false;
            formRunningApps.DockAreas = DockAreas.DockLeft | DockAreas.DockTop | DockAreas.DockRight | DockAreas.DockBottom | DockAreas.Float;

            formApps = new FormApplications();
            formApps.CloseButtonVisible = false;
            formApps.DockAreas = DockAreas.DockLeft | DockAreas.DockTop | DockAreas.DockRight | DockAreas.DockBottom | DockAreas.Float;

            formInput = new FormInput();
            formInput.CloseButtonVisible = false;
            formInput.DockAreas = DockAreas.DockLeft | DockAreas.DockTop | DockAreas.DockRight | DockAreas.DockBottom | DockAreas.Float;

            formMimic = new FormMimic();
            formMimic.CloseButtonVisible = false;
            formMimic.DockAreas = DockAreas.Document;
            formMimic.AllowDrop = true;
            formMimic.DragEnter += formMimic_DragEnter;
            formMimic.DragDrop += formMimic_DragDrop;
        }

        void formPreset_EvtPresetRemoved(FormPresets form, Client.Model.PresetModel item)
        {
            // remove a preset
            clientPresenter.RemovePreset(item);
        }

        void formPreset_EvtPresetAdded(FormPresets form)
        {
            // add a preset
            FormAddPreset addPreset = new FormAddPreset();
            addPreset.SetAppList(Settings.ApplicationSettings.GetInstance().ApplicationList);
            if (addPreset.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clientPresenter.AddPreset(addPreset.PresetName, addPreset.GetSelectedAppList());
            }
        }

        void formMimic_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        void formMimic_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Client.Model.PresetModel)))
            {
                Client.Model.PresetModel presetData = null;
                if ((presetData = (Client.Model.PresetModel)e.Data.GetData(typeof(Client.Model.PresetModel))) != null)
                {
                    clientPresenter.TriggerPreset(presetData);
                }
            }
            else if(e.Data.GetDataPresent(typeof(Client.Model.VncModel)))
            {
                Client.Model.VncModel vncData = null;
                if ((vncData = (Client.Model.VncModel)e.Data.GetData(typeof(Client.Model.VncModel))) != null)
                {
                    clientPresenter.TriggerVnc(vncData);
                }
            }
            else if (e.Data.GetDataPresent(typeof(Client.Model.WindowsModel)))
            {
                Client.Model.WindowsModel appData = null;
                if ((appData = (Client.Model.WindowsModel)e.Data.GetData(typeof(Client.Model.WindowsModel))) != null)
                {
                    if ((appData.Style & Constant.WS_MINIMIZE) != 0)
                    {
                        // restore the window first
                        clientPresenter.SetApplicationRestore(appData.WindowsId);
                    }
                    else
                    {
                        clientPresenter.SetApplicationForeground(appData.WindowsId);
                    }
                }
            }
            else if (e.Data.GetDataPresent(typeof(Client.Model.ApplicationModel)))
            {
                Client.Model.ApplicationModel applicationData = null;
                if ((applicationData = (Client.Model.ApplicationModel)e.Data.GetData(typeof(Client.Model.ApplicationModel))) != null)
                {
                    clientPresenter.TriggerApplication(applicationData);
                }
            }
            else if (e.Data.GetDataPresent(typeof(InputAttributes)))
            {
                InputAttributes attributeData = null;
                if ((attributeData = (InputAttributes)e.Data.GetData(typeof(InputAttributes))) != null)
                {
                    clientPresenter.TriggerVisionInput(attributeData);
                }
            }
        }

        private IDockContent getContentFromPersistString(string persistString)
        {
            if (persistString == typeof(FormPresets).ToString())
                return formPreset;
            else if (persistString == typeof(FormVnc).ToString())
                return formVnc;
            else if (persistString == typeof(FormRunningApps).ToString())
                return formRunningApps;
            else if (persistString == typeof(FormApplications).ToString())
                return formApps;
            else if (persistString == typeof(FormInput).ToString())
                return formInput;
            else
                return formMimic;
        }


        private void loadNewLayout()
        {
            dockPanel.SuspendLayout(true);

            // load the dock form
            formPreset.Show(dockPanel, DockState.DockLeft);
            formVnc.Show(formPreset.Pane, DockAlignment.Bottom, 0.7);
            formInput.Show(formVnc.Pane, formVnc);

            formApps.Show(formVnc.Pane, DockAlignment.Bottom, 0.5);
            formRunningApps.Show(formApps.Pane, formApps);

            formMimic.Show(dockPanel, DockState.Document);

            dockPanel.ResumeLayout(true, true);
        }

        void keyboardHook_HookInvoked(object sender, KeyboardHook.KeyboardHookEventArgs data)
        {
            if (data.wParam == Constant.WM_KEYDOWN)
            {
                // only handle keydown and keyup events, reference: http://msdn.microsoft.com/en-us/library/windows/desktop/ms646271(v=vs.85).aspx
                data.lParam.flags = 0;
            }
            else
            {
                data.lParam.flags = 0x0002;
            }

            Trace.WriteLine("keyboard");

            clientPresenter.ControlServerKeyboard(
                (UInt16)data.lParam.vkCode,
                (UInt16)data.lParam.scanCode, 
                data.lParam.time, 
                data.lParam.flags);
        }

        private IntPtr holderPointer;
        void mouseHook_HookInvoked(object sender, MouseHook.MouseHookEventArgs arg)
        {
            if (arg.lParam.flags == 1)
            {
                // do not handle injected mouse event
                return;
            }

            int relativeX = arg.lParam.pt.x - this.Location.X - formMimic.Location.X - 10;      // 10 is offset for the control size
            int relativeY = arg.lParam.pt.y - this.Location.Y - formMimic.Location.Y - 30;      // offset for title bar and dock panel header

            NativeMethods.Point relativePt = new NativeMethods.Point() {x = arg.lParam.pt.x, y = arg.lParam.pt.y };

            if (Utils.Windows.NativeMethods.ScreenToClient(holderPointer, ref relativePt))
            {
                if (relativePt.x < -2 ||
                    relativePt.y < -2 ||
                    relativePt.x > (holder.Bounds.Width + 2) ||
                    relativePt.y > (holder.Bounds.Height + 2))
                {
                    Trace.WriteLine("not in client");
                    return;
                }
            }
            if (relativeX < -2 ||
                relativeY < -2 ||
                relativeX > (holder.Bounds.Width + 2) ||
                relativeY > (holder.Bounds.Height + 2))         // 2 pixel offset to allow hidden control visible in server when mouse move
            {
                // not in client bound
                return;
            }

            float relativePosX = (float)relativeX * 65535.0f / (float)holder.Width;
            float relativePosY = (float)relativeY * 65535.0f / (float)holder.Height;

            UInt32 actualFlags = InputConstants.MOUSEEVENTF_MOVE | InputConstants.MOUSEEVENTF_ABSOLUTE | InputConstants.MOUSEEVENTF_VIRTUALDESK;
            switch(arg.wParam.ToInt32())
            {
                case (Int32)InputConstants.WM_LBUTTONDOWN:
                    actualFlags = InputConstants.MOUSEEVENTF_LEFTDOWN;
                    break;
                case (Int32)InputConstants.WM_LBUTTONUP:
                    actualFlags = InputConstants.MOUSEEVENTF_LEFTUP;
                    break;
                case (Int32)InputConstants.WM_MOUSEWHEEL:
                case (Int32)InputConstants.WM_MOUSEHWHEEL:
                    actualFlags = InputConstants.MOUSEEVENTF_WHEEL;
                    break;
                case (Int32)InputConstants.WM_RBUTTONDOWN:
                    actualFlags = InputConstants.MOUSEEVENTF_RIGHTDOWN;
                    break;
                case (Int32)InputConstants.WM_RBUTTONUP:
                    actualFlags = InputConstants.MOUSEEVENTF_RIGHTUP;
                    break;
                default:
                    break;
            }

            clientPresenter.ControlServerMouse((int)relativePosX, (int)relativePosY, arg.lParam.mouseData, actualFlags);
        }


        private void buttonMessage_Click(object sender, EventArgs e)
        {
            FormMessageBox messageBox = new FormMessageBox();
            if (messageBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // sent to server for display
                clientPresenter.ShowMessage(messageBox.Message,
                    messageBox.SelectedFont,
                    messageBox.SelectedColor,
                    messageBox.BackgroundColor,
                    messageBox.Duration,
                    messageBox.LocationX,
                    messageBox.LocationY,
                    messageBox.Width,
                    messageBox.Height);
            }
        }

        private void buttonMaintenance_Click(object sender, EventArgs e)
        {
            FormServerMaintenance formServerMaintenance = new FormServerMaintenance();
            DialogResult result = formServerMaintenance.ShowDialog();
            Presenter.ClientPresenter.ServerMaintenanceMode mode = Presenter.ClientPresenter.ServerMaintenanceMode.Standby;
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    // restart
                    mode = Presenter.ClientPresenter.ServerMaintenanceMode.Restart;
                    break;
                case System.Windows.Forms.DialogResult.Retry:
                    // shutdown
                    mode = Presenter.ClientPresenter.ServerMaintenanceMode.Shutdown;
                    break;
                case System.Windows.Forms.DialogResult.Yes:
                    // standby
                    mode = Presenter.ClientPresenter.ServerMaintenanceMode.Standby;
                    break;
                default:
                    // close without action
                    return;
            }

            clientPresenter.ServerMaintenance(mode);
        }

        public void RefreshLayout(Client.Model.UserInfoModel user, Client.Model.ServerLayoutModel layout, WindowsModel viewingArea)
        {
            if (EvtServerReply != null)
            {
                EvtServerReply(this);
            }

            if(this.InvokeRequired)
            {
                this.Invoke(new DelegateRefreshLayout(RefreshLayout), user, layout, viewingArea);
                return;
            }

            if (holder != null)
            {
                holder.ReferenceXPos = viewingArea.PosLeft;
                holder.ReferenceYPos = viewingArea.PosTop;
                holder.VirtualSize = new Size(viewingArea.Width, viewingArea.Height);
            }
            
            formMimic.Row = layout.LayoutRow;
            formMimic.Column = layout.LayoutColumn;
            formMimic.FullSize = new Size(layout.DesktopLayout.Width, layout.DesktopLayout.Height);
            formMimic.VisibleSize = new Size(viewingArea.Width, viewingArea.Height);
            formMimic.ReferenceLeft = viewingArea.PosLeft;
            formMimic.ReferenceTop = viewingArea.PosTop;

           // holder.RefreshLayout();

            formMimic.Text = user.DisplayName;
            formMimic.RefreshMatrixLayout();
        }

        public void RefreshViewingArea(WindowsModel viewingArea)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateRefreshViewArea(RefreshViewingArea), viewingArea);
                return;
            }

            holder.ReferenceXPos = viewingArea.PosLeft;
            holder.ReferenceYPos = viewingArea.PosTop;
            holder.VirtualSize = new Size(viewingArea.Width, viewingArea.Height);

            formMimic.VisibleSize = new Size(viewingArea.Width, viewingArea.Height);
            formMimic.ReferenceLeft = viewingArea.PosLeft;
            formMimic.ReferenceTop = viewingArea.PosTop;

            // need to use force as changing the virtual member will auto change the scale as well
            // no invalidation happen when the old scale same as new scale when calling normal refresh method
            holder.RefreshLayout();
            formMimic.RefreshMatrixLayout();
        }

        public void RefreshAppList(IList<Client.Model.ApplicationModel> appList)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateRefreshAppList(RefreshAppList), appList);
                return;
            }

            formApps.SetApplicationsListData(appList);
        }

        public void RefreshWndList(IList<Client.Model.WindowsModel> wndsList)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWndChange(RefreshWndList), wndsList);
                return;
            }

            // find difference with current list and updated list

            List<Client.Model.WindowsModel> tempList = new List<Client.Model.WindowsModel>(wndsList);

            // 1. find the windows that newly added
            List<Client.Model.WindowsModel> addedQuery = tempList.Except(applicationList, new WndObjComparer()).ToList();

            // 2. find the windows that removed
            List<Client.Model.WindowsModel> removedQuery = applicationList.Except(tempList, new WndObjComparer()).ToList();

            // 3. find the windows with attributes changed
            foreach (Client.Model.WindowsModel windows in addedQuery)
            {
                // remove the additional objects in list
                wndsList.Remove(windows);
            }

            foreach (Client.Model.WindowsModel windows in removedQuery)
            {
                // remove the additional objects in list
                wndsList.Remove(windows);
            }

            // compare the remaining list, these lists should contain the same object's id and count
            List<Client.Model.WindowsModel> modifiedNameQuery = wndsList.Except(applicationList, new WndNameComparer()).ToList();
            List<Client.Model.WindowsModel> modifiedPosQuery = wndsList.Except(applicationList, new WndPosComparer()).ToList();
            List<Client.Model.WindowsModel> modifiedSizeQuery = wndsList.Except(applicationList, new WndSizeComparer()).ToList();
            List<Client.Model.WindowsModel> modifiedStyleQuery = wndsList.Except(applicationList, new WndStyleComparer()).ToList();

            // save the updated list
            applicationList.Clear();
            applicationList.AddRange(tempList);

            // Trace.WriteLine(String.Format("add:{0}, remove:{1}, modified:{2}, totalNow:{3}", addedQuery.Count.ToString(), removedQuery.Count.ToString(), modifiedQuery.Count.ToString(), applicationList.Count.ToString()));

            bool refreshAppList = false;
            foreach (Client.Model.WindowsModel windows in addedQuery)
            {
                refreshAppList |= true;
                AddWindow(windows);
            }

            foreach (Client.Model.WindowsModel windows in removedQuery)
            {
                refreshAppList |= true;
                RemoveWindow(windows);
            }

            foreach (Client.Model.WindowsModel windows in modifiedNameQuery)
            {
                ChangeWindowName(windows);
            }

            foreach (Client.Model.WindowsModel windows in modifiedPosQuery)
            {
                ChangeWindowPos(windows);
            }

            foreach (Client.Model.WindowsModel windows in modifiedStyleQuery)
            {
                refreshAppList |= true;
                ChangeWindowStyle(windows);
            }

            foreach (Client.Model.WindowsModel windows in modifiedSizeQuery)
            {
                ChangeWindowSize(windows);
            }

            // update entire list z-order
            foreach (Client.Model.WindowsModel windows in applicationList)
            {
                ChangeWindowZOrder(windows);
            }

            if (refreshAppList)
            {
                refreshAppListing();
            }
        }

        private void refreshAppListing()
        {
            formRunningApps.SetApplicationListData(applicationList);
        }

        private void AddWindow(Client.Model.WindowsModel wndPos)
        {
            holder.AddControl(new CustomWinForm.CustomControlHolder.ControlAttributes
            {
                Id = wndPos.WindowsId,
                WindowName = wndPos.DisplayName,
                Xpos = wndPos.PosLeft,
                Ypos = wndPos.PosTop,
                Width = wndPos.Width,
                Height = wndPos.Height,
                Style = wndPos.Style,
                ZOrder = wndPos.ZOrder
            });
        }

        private void RemoveWindow(Client.Model.WindowsModel wndPos)
        {
            holder.RemoveControl(wndPos.WindowsId);
        }

        private void ChangeWindowName(Client.Model.WindowsModel wndPos)
        {
            holder.ChangeControlName(wndPos.WindowsId, wndPos.DisplayName);
        }

        private void ChangeWindowPos(Client.Model.WindowsModel wndPos)
        {
            holder.ChangeControlPos(wndPos.WindowsId, new Point(wndPos.PosLeft, wndPos.PosTop));
        }

        private void ChangeWindowSize(Client.Model.WindowsModel wndPos)
        {
            holder.ChangeControlSize(wndPos.WindowsId, new Size(wndPos.Width, wndPos.Height));
        }

        private void ChangeWindowStyle(Client.Model.WindowsModel wndPos)
        {
            holder.ChangeControlStyle(wndPos.WindowsId, wndPos.Style);
        }

        private void ChangeWindowZOrder(Client.Model.WindowsModel wndPos)
        {
            holder.ChangeControlZOrder(wndPos.WindowsId, wndPos.ZOrder);
        }

        public void RefreshVncList(IList<Client.Model.VncModel> vncList)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateVncList(RefreshVncList), vncList);
                return;
            }

            formVnc.SetVNCList(vncList);
        }

        public void RefreshPresetList(IList<Client.Model.PresetModel> presetList)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegatePresetList(RefreshPresetList), presetList);
                return;
            }

            formPreset.SetPresetList(presetList);
        }

        public void RefreshMaintenanceStatus(Client.Model.UserPriviledgeModel privilegde)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateMaintenanceStatus(RefreshMaintenanceStatus), privilegde);
                return;
            }

            buttonMaintenance.Enabled = privilegde.AllowMaintenance;
        }

        private void FormClient_Closing(object sender, FormClosingEventArgs e)
        {
            // shutdown
            mouseHook.StopHook();
            keyboardHook.StopHook();

            // save current UI state
            string configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), CONFIG_FILE_NAME);
            dockPanel.SaveAsXml(configFile);

            // clean up connection
            holder.RemoveAllControls();
        }


        public void CloseApplication()
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateUI(CloseApplication));
                return;
            }

            try
            {
                this.Close();
            }
            catch (Exception)
            {
            }
        }

        private void FormClient_Closed(object sender, FormClosedEventArgs e)
        {
            connectionMgr.StopClient();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        protected override void WndProc(ref Message m)
        {
            if ((UInt32)m.Msg == Constant.WM_SYSCOMMAND)
            {
                switch ((UInt32)m.WParam)
                {
                    case Constant.SC_CLOSE:
                        {
                            mouseHook.StopHook();
                            keyboardHook.StopHook();
                            break;
                        }
                    case Constant.SC_MINIMIZE:
                        {
                            this.Visible = false;
                            return;
                        }
                    default:
                        break;
                }
            }

            base.WndProc(ref m);
        }

        private void checkBoxMouse_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox button = sender as CheckBox;
            if (button.Checked)
            {
                button.BackColor = Color.FromArgb(20, 116, 186);
                mouseHook.StartHook(0);
            }
            else
            {
                button.BackColor = Color.FromArgb(79, 169, 236);
                mouseHook.StopHook();
            }
        }

        private void checkBoxKeyboard_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox button = sender as CheckBox;
            if (button.Checked)
            {
                button.BackColor = Color.FromArgb(20, 116, 186);
                keyboardHook.StartHook(0);
            }
            else
            {
                button.BackColor = Color.FromArgb(79, 169, 236);
                keyboardHook.StopHook();
            }
        }


        public void RefreshVisionInputStatus(List<Session.Data.SubData.InputAttributes> inputAttrList)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateRefreshVisionInputList(RefreshVisionInputStatus), inputAttrList);
                return;
            }

            formInput.SetVisionInputList(inputAttrList);
        }
    }
}
