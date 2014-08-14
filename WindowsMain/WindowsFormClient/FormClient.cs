using CustomWinForm;
using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils.Hooks;
using Utils.Windows;
using WindowsFormClient.Client;
using WindowsFormClient.Comparer;

namespace WindowsFormClient
{
    public partial class FormClient : Form, IClient
    {
        private ConnectionManager connectionMgr = new ConnectionManager();

        private List<Client.Model.WindowsModel> applicationList = new List<Client.Model.WindowsModel>();
        private CustomControlHolder mHolder = new CustomControlHolder(new Size(0, 0), Int32.MinValue, Int32.MinValue);

        private Dictionary<int, int> mWindowsDic = new Dictionary<int, int>();

        private delegate void DelegateWindow(Client.Model.WindowsModel wndPos);
        private delegate void DelegateEvt();

        MouseHook mouseHook = new MouseHook();
        KeyboardHook keyboardHook = new KeyboardHook();

        private VncMarshall.Server vncServer = new VncMarshall.Server(@"C:\Program Files\TightVNC\tvnserver.exe");
        private Client.ClientCmdMgr clienCmdMgr;

        private int UserId = -1;

        public FormClient()
        {
            InitializeComponent();

            layoutPanel.Controls.Add(mHolder);
            mHolder.Location = new Point(0, 0);
            mHolder.Size = layoutPanel.Size;
            mHolder.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            connectionMgr.EvtConnected += connectionMgr_EvtConnected;
            connectionMgr.EvtDisconnected += connectionMgr_EvtDisconnected;
            connectionMgr.EvtServerDataReceived += connectionMgr_EvtServerDataReceived;

            mHolder.onDelegateClosedEvt += mHolder_onDelegateClosedEvt;
            mHolder.onDelegateMinimizedEvt += mHolder_onDelegateMinimizedEvt;
            mHolder.onDelegateMaximizedEvt += mHolder_onDelegateMaximizedEvt;
            mHolder.onDelegatePosChangedEvt += mHolder_onDelegatePosChangedEvt;
            mHolder.onDelegateRestoredEvt += mHolder_onDelegateRestoredEvt;
            mHolder.onDelegateSizeChangedEvt += mHolder_onDelegateSizeChangedEvt;

            mouseHook.HookInvoked += mouseHook_HookInvoked;
            keyboardHook.HookInvoked += keyboardHook_HookInvoked;

            applicationsListbox.DataSource = applicationList;
            applicationsListbox.DisplayMember = "DisplayName";      // map to WindowsModel.DisplayName
            applicationsListbox.ValueMember = "WindowsId";        // map to WindowsModel.WindowsId
            applicationsListbox.ClearSelected();
            applicationsListbox.SelectedIndexChanged += applicationsListbox_SelectedIndexChanged;

            // initialize command manager
            clienCmdMgr = new ClientCmdMgr(this);

            // TODO: start vnc client, store monitor list and set the corresponding listening port to registry
            vncServer.StartVncServer();
        }

        void applicationsListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            WndPos item = (WndPos)applicationsListbox.SelectedItem;
            if (item != null)
            {
                int wndId = item.id;
                applicationsListbox.ClearSelected();

                // send to server for setting it to foreground
                ClientWndCmd wndCommand = new ClientWndCmd();
                wndCommand.CommandType = ClientWndCmd.CommandId.ESetForeground;
                wndCommand.Id = wndId;

                connectionMgr.BroadcastMessage(
                    (int)CommandConst.MainCommandClient.ControlInfo,
                    (int)CommandConst.SubCommandClient.WindowsAttributes,
                    wndCommand);                
            }
        }

        void mHolder_onDelegateMinimizedEvt(int id)
        {
            if(mWindowsDic.ContainsValue(id))
            {
                var wndId = mWindowsDic.FirstOrDefault(x => x.Value == id).Key;
                ClientWndCmd wndCommand = new ClientWndCmd();
                wndCommand.CommandType = ClientWndCmd.CommandId.EMinimize;
                wndCommand.Id = wndId;

                connectionMgr.BroadcastMessage(
                    (int)CommandConst.MainCommandClient.ControlInfo,
                    (int)CommandConst.SubCommandClient.WindowsAttributes,
                    wndCommand);
            }
            
        }

        void mHolder_onDelegateSizeChangedEvt(int id, Size newSize)
        {
            if (mWindowsDic.ContainsValue(id))
            {
                var wndId = mWindowsDic.FirstOrDefault(x => x.Value == id).Key;
                ClientWndCmd wndCommand = new ClientWndCmd();
                wndCommand.CommandType = ClientWndCmd.CommandId.EResize;
                wndCommand.Id = wndId;
                wndCommand.Width = newSize.Width;
                wndCommand.Height = newSize.Height;

                connectionMgr.BroadcastMessage(
                    (int)CommandConst.MainCommandClient.ControlInfo,
                    (int)CommandConst.SubCommandClient.WindowsAttributes,
                    wndCommand);
            }
        }

        void mHolder_onDelegateRestoredEvt(int id)
        {
            if (mWindowsDic.ContainsValue(id))
            {
                var wndId = mWindowsDic.FirstOrDefault(x => x.Value == id).Key;
                ClientWndCmd wndCommand = new ClientWndCmd();
                wndCommand.CommandType = ClientWndCmd.CommandId.ERestore;
                wndCommand.Id = wndId;

                connectionMgr.BroadcastMessage(
                    (int)CommandConst.MainCommandClient.ControlInfo,
                    (int)CommandConst.SubCommandClient.WindowsAttributes,
                    wndCommand);
            }
        }

        void mHolder_onDelegatePosChangedEvt(int id, int xPos, int yPos)
        {
            if (mWindowsDic.ContainsValue(id))
            {
                var wndId = mWindowsDic.FirstOrDefault(x => x.Value == id).Key;
                ClientWndCmd wndCommand = new ClientWndCmd();
                wndCommand.CommandType = ClientWndCmd.CommandId.ERelocation;
                wndCommand.Id = wndId;
                wndCommand.PositionX = xPos;
                wndCommand.PositionY = yPos;

                connectionMgr.BroadcastMessage(
                    (int)CommandConst.MainCommandClient.ControlInfo,
                    (int)CommandConst.SubCommandClient.WindowsAttributes,
                    wndCommand);
            }
        }

        void mHolder_onDelegateMaximizedEvt(int id)
        {
            if (mWindowsDic.ContainsValue(id))
            {
                var wndId = mWindowsDic.FirstOrDefault(x => x.Value == id).Key;
                ClientWndCmd wndCommand = new ClientWndCmd();
                wndCommand.CommandType = ClientWndCmd.CommandId.EMaximize;
                wndCommand.Id = wndId;

                connectionMgr.BroadcastMessage(
                    (int)CommandConst.MainCommandClient.ControlInfo,
                    (int)CommandConst.SubCommandClient.WindowsAttributes,
                    wndCommand);
            }
        }

        void mHolder_onDelegateClosedEvt(int id)
        {
            if (mWindowsDic.ContainsValue(id))
            {
                var wndId = mWindowsDic.FirstOrDefault(x => x.Value == id).Key;
                ClientWndCmd wndCommand = new ClientWndCmd();
                wndCommand.CommandType = ClientWndCmd.CommandId.EClose;
                wndCommand.Id = wndId;

                connectionMgr.BroadcastMessage(
                    (int)CommandConst.MainCommandClient.ControlInfo,
                    (int)CommandConst.SubCommandClient.WindowsAttributes,
                    wndCommand);
            }
        }

        void connectionMgr_EvtServerDataReceived(int mainId, int subId, string commandData)
        {
            // server user id undefined = "0"
            clienCmdMgr.ExeCommand("0", mainId, subId, commandData);
        }

        void connectionMgr_EvtDisconnected()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateEvt(connectionMgr_EvtDisconnected));
                return;
            }

            mHolder.RemoveAllControls();
            applicationList.Clear();
            refreshApplicationList();
            mWindowsDic.Clear();
            minimizedWndComboBox.Items.Clear();

            vncServer.StopVncServer();
        }

        void connectionMgr_EvtConnected()
        {
            ClientLoginCmd loginCmd = new ClientLoginCmd();
            loginCmd.Username = username.Text;
            loginCmd.Password = password.Text;

            // get the VNC status, if there is VNC server installed, get the ip address and port shared
            int port;
            if((port = VncMarshall.VncRegistryHelper.GetListeningPort()) != -1)
            {
                loginCmd.VncServerPort = port;
                loginCmd.VncServerIp = Utils.Socket.LocalIPAddress();
            }

            // get how many monitors attached to this PC
            List<MonitorInfo> monitorList = new List<MonitorInfo>();
            foreach(WindowsHelper.MonitorInfo info in Utils.Windows.WindowsHelper.GetMonitorList())
            {
                monitorList.Add(new MonitorInfo { 
                    LeftPos = info.WorkArea.Left, 
                    TopPos = info.WorkArea.Top, 
                    RightPos = info.WorkArea.Right, 
                    BottomPos = info.WorkArea.Bottom });
            }
            loginCmd.MonitorsInfo = monitorList;

            connectionMgr.BroadcastMessage((int)CommandConst.MainCommandClient.LoginInfo, (int)CommandConst.SubCommandClient.Credential, loginCmd);
        }

        private void connect_Click(object sender, EventArgs e)
        {
            connectionMgr.StartClient(hostIp.Text, Convert.ToInt32(hostPort.Text));
        }

        void keyboardHook_HookInvoked(object sender, KeyboardHook.KeyboardHookEventArgs data)
        {
            if (captureKeyboard.Checked == false)
            {
                return;
            }

            Trace.WriteLine(String.Format("keyboard code:{2}, vk:{0}, scan:{1}, wParam:{3}, flag:{4}", data.lParam.vkCode, data.lParam.scanCode, data.code, data.wParam, data.lParam.flags));
            if (data.wParam == Constant.WM_KEYDOWN)
            {
                // only handle keydown and keyup events, reference: http://msdn.microsoft.com/en-us/library/windows/desktop/ms646271(v=vs.85).aspx
                data.lParam.flags = 0;
            }
            else
            {
                data.lParam.flags = 0x0002;
            }

            ClientKeyboardCmd keyboardCmd = new ClientKeyboardCmd();
            keyboardCmd.data = new ClientKeyboardCmd.KeyboardData
            {
                wVk = (UInt16)data.lParam.vkCode,
                wScan = (UInt16)data.lParam.scanCode,
                time = data.lParam.time,
                dwFlags = (UInt32)data.lParam.flags,
                dwExtraInfo = 0
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.Keyboard,
                keyboardCmd);
        }

        void mouseHook_HookInvoked(object sender, MouseHook.MouseHookEventArgs arg)
        {
            if (captureMouse.Checked == false)
            {
                return;
            }

            if (arg.lParam.flags == 1)
            {
                // do not handle injected mouse event
                return;
            }

            int offsetTop = (this.Size.Height - this.ClientSize.Height);
            int offsetWidth = (this.Size.Width - this.ClientSize.Width) / 2;

            int relativeX = arg.lParam.pt.x - this.Location.X - offsetTop - layoutPanel.Bounds.X - mHolder.Bounds.X;
            int relativeY = arg.lParam.pt.y - this.Location.Y - offsetWidth - layoutPanel.Bounds.Y - mHolder.Bounds.Y;

            if (relativeX < 0 ||
                relativeY < 0 ||
                relativeX > mHolder.Bounds.Width ||
                relativeY > mHolder.Bounds.Height)
            {
                // not in client bound
                Trace.WriteLine("mouse not in client area");
                return;
            }

            float relativePosX = (float)relativeX / (float)mHolder.Width * 65535.0f;
            float relativePosY = (float)relativeY / (float)mHolder.Height * 65535.0f;

            Trace.WriteLine(String.Format("mouse {0},{1}, flags:{2}, mouseData:{3}", relativePosX, relativePosY, arg.lParam.flags, arg.lParam.mouseData));
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

            ClientMouseCmd mouseCmd = new ClientMouseCmd();
            mouseCmd.data = new ClientMouseCmd.MouseData 
            {
                dx = (int)relativePosX,
                dy = (int)relativePosY,
                mouseData = arg.lParam.mouseData,
                dwFlags = actualFlags,
                time = 0,
                dwExtraInfo = 0
            };

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.Mouse,
                mouseCmd);
        }


        private void disconnect_Click(object sender, EventArgs e)
        {
            connectionMgr.StopClient();
            connectionMgr_EvtDisconnected();

            vncServer.StopVncServer();
        }

        private void AddWindow(Client.Model.WindowsModel wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(AddWindow), wndPos);
                return;
            }


            int wndId = mHolder.AddControl(new CustomWinForm.CustomControlHolder.ControlAttributes {
                WindowName = wndPos.DisplayName, 
                Xpos = wndPos.PosLeft, 
                Ypos = wndPos.PosTop, 
                Width = wndPos.Width, 
                Height = wndPos.Height,
                Style = wndPos.Style,
                ZOrder = wndPos.ZOrder
            });

            mWindowsDic.Add(wndPos.WindowsId, wndId);

            // check if the control was minimized
            if ((wndPos.Style & Constant.WS_MINIMIZE) != 0)
            {
                AddMinimizedWindow(wndPos.WindowsId, wndPos);
            }
        }

        private void RemoveWindow(Client.Model.WindowsModel wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(RemoveWindow), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.WindowsId, out wndId))
            {
                mHolder.RemoveControl(wndId);
                mWindowsDic.Remove(wndPos.WindowsId);

                RemoveMinimizedWindow(wndPos.WindowsId);
            }
        }

        private void ChangeWindowName(Client.Model.WindowsModel wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowName), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.WindowsId, out wndId))
            {
                mHolder.ChangeControlName(wndId, wndPos.DisplayName);
            }

        }

        private void ChangeWindowPos(Client.Model.WindowsModel wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowPos), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.WindowsId, out wndId))
            {
                mHolder.ChangeControlPos(wndId, new Point(wndPos.PosLeft, wndPos.PosTop));
            }
        }

        private void ChangeWindowSize(Client.Model.WindowsModel wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowSize), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.WindowsId, out wndId))
            {
                mHolder.ChangeControlSize(wndId, new Size(wndPos.Width, wndPos.Height));
            }
        }

        private void ChangeWindowStyle(Client.Model.WindowsModel wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowStyle), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.WindowsId, out wndId))
            {
                mHolder.ChangeControlStyle(wndId, wndPos.Style);

                // check if the control was minimized
                if ((wndPos.Style & Constant.WS_MINIMIZE) != 0)
                {
                    AddMinimizedWindow(wndPos.WindowsId, wndPos);
                }
                else
                {
                    RemoveMinimizedWindow(wndPos.WindowsId);
                }

            }
        }

        private void ChangeWindowZOrder(Client.Model.WindowsModel wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowZOrder), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.WindowsId, out wndId))
            {
                mHolder.ChangeControlZOrder(wndId, wndPos.ZOrder);
            }
        }

        private void AddMinimizedWindow(int id, Client.Model.WindowsModel data)
        {
            MinimizeComboBox comboBox = new MinimizeComboBox();
            comboBox.Id = id;
            comboBox.Text = data.DisplayName;
            comboBox.Data = data;

            minimizedWndComboBox.Items.Add(comboBox);
        }

        private void RemoveMinimizedWindow(int id)
        {
            foreach (Object obj in minimizedWndComboBox.Items)
            {
                MinimizeComboBox comboBox = (MinimizeComboBox)obj;
                if (comboBox.Id == id)
                {
                    minimizedWndComboBox.Items.Remove(obj);
                    return;
                }
            }
        }

        private void minimizedWndComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MinimizeComboBox comboBox = (MinimizeComboBox)minimizedWndComboBox.SelectedItem;
            RemoveMinimizedWindow(comboBox.Id);

            // send command to server for notification
            ClientWndCmd wndCommand = new ClientWndCmd();
            wndCommand.CommandType = ClientWndCmd.CommandId.ERestore;
            wndCommand.Id = comboBox.Id;

            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.WindowsAttributes,
                wndCommand);
        }

        private void captureKeyboard_CheckedChanged(object sender, EventArgs e)
        {
            if(captureKeyboard.Checked)
            {
                keyboardHook.StartHook(0);
            }
            else
            {
                keyboardHook.StopHook();
            }
        }

        private void captureMouse_CheckedChanged(object sender, EventArgs e)
        {
            if (captureMouse.Checked)
            {
                mouseHook.StartHook(0);
            }
            else
            {
                mouseHook.StopHook();
            }
        }

        private void onFormClosing(object sender, FormClosingEventArgs e)
        {
            keyboardHook.StopHook();
            mouseHook.StopHook();

            connectionMgr.StopClient();

            vncServer.StopVncServer();
        }

        private void refreshApplicationList()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateEvt(refreshApplicationList));
                return;
            }

            if (applicationsListbox.SelectedIndex != -1)
            {
                return;
            }
            applicationsListbox.DataSource = null;
            applicationsListbox.DataSource = applicationList;
            applicationsListbox.DisplayMember = "DisplayName";
            applicationsListbox.ValueMember = "WindowsId";
        }

        private void shutdownBtn_Click(object sender, EventArgs e)
        {
            sendMaintenanceCmd(ClientMaintenanceCmd.CommandId.EShutdown);
        }

        private void rebootBtn_Click(object sender, EventArgs e)
        {
            sendMaintenanceCmd(ClientMaintenanceCmd.CommandId.EReboot);
        }

        private void logOffBtn_Click(object sender, EventArgs e)
        {
            sendMaintenanceCmd(ClientMaintenanceCmd.CommandId.ELogOff);
        }

        void sendMaintenanceCmd(ClientMaintenanceCmd.CommandId id)
        {
            ClientMaintenanceCmd command = new ClientMaintenanceCmd { CommandType = id };
            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ControlInfo,
                (int)CommandConst.SubCommandClient.Maintenance,
                command);
        }

        private void startVNC_Click(object sender, EventArgs e)
        {
            //int portNumber = Utils.Socket.getUnusedPort(VNC_PORTSTART, VNC_PORTSTOP);

            //// start vnc server
            //vncServer.StartServer(portNumber, new VncMarshall.Server.SharingAttributes { ShareMode=VncMarshall.Server.SharingMode.ShareRect, PosLeft=0, PosTop=0, PosRight=100, PosBottom=100});

            //// send signal to server to indicate vnc server info
            //ClientVncCmd command = new ClientVncCmd { PortNumber = portNumber, IpAddress=Utils.Socket.LocalIPAddress() };
            //connectionMgr.BroadcastMessage(
            //    (int)CommandConst.MainCommandClient.ClientVncInfo,
            //    (int)CommandConst.SubCmdVncInfo.Start,
            //    command);
        }

        private void stopVNC_Click(object sender, EventArgs e)
        {
            //vncServer.StopServer();

            //// send signal to server to indicate vnc server info
            //ClientVncCmd command = new ClientVncCmd();
            //connectionMgr.BroadcastMessage(
            //    (int)CommandConst.MainCommandClient.ClientVncInfo,
            //    (int)CommandConst.SubCmdVncInfo.Stop,
            //    command);
        }

        public void RefreshLayout(Client.Model.UserInfoModel user, Client.Model.ServerLayoutModel layout)
        {
            mHolder.MaxSize = new Size(layout.DesktopLayout.Width, layout.DesktopLayout.Height);
            mHolder.ReferenceXPos = layout.DesktopLayout.PosLeft;
            mHolder.ReferenceYPos = layout.DesktopLayout.PosTop;

            // TODO: 
            // 1. keep the user info
            UserId = user.UserId;
            // 2. drawn the matrix layout
        }

        public void RefreshAppList(IList<Client.Model.ApplicationModel> appList)
        {
            throw new NotImplementedException();
        }

        public void RefreshWndList(IList<Client.Model.WindowsModel> wndsList)
        {
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
                //Trace.WriteLine(String.Format("Added - name:{5} id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id, windows.name));
                refreshAppList |= true;
                AddWindow(windows);
            }

            foreach (Client.Model.WindowsModel windows in removedQuery)
            {
                //Trace.WriteLine(String.Format("Removed - name:{5}  id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id, windows.name));
                refreshAppList |= true;
                RemoveWindow(windows);
            }

            foreach (Client.Model.WindowsModel windows in modifiedNameQuery)
            {
                //Trace.WriteLine(String.Format("Modified - id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id));
                ChangeWindowName(windows);
            }

            foreach (Client.Model.WindowsModel windows in modifiedPosQuery)
            {
                //Trace.WriteLine(String.Format("Modified - id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id));
                ChangeWindowPos(windows);
            }

            foreach (Client.Model.WindowsModel windows in modifiedStyleQuery)
            {
                // NOTE: must set style then only set size as maximize state will not shown in the entire screen
                ChangeWindowStyle(windows);
            }

            foreach (Client.Model.WindowsModel windows in modifiedSizeQuery)
            {
                //Trace.WriteLine(String.Format("Modified - id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id));
                ChangeWindowSize(windows);
            }

            // update entire list z-order
            foreach (Client.Model.WindowsModel windows in applicationList)
            {
                //Trace.WriteLine(String.Format("Modified - id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id));
                ChangeWindowZOrder(windows);
            }

            if (refreshAppList)
            {
                refreshApplicationList();
            }
        }

        public void RefreshVncList(IList<Client.Model.VncModel> vncList)
        {
            throw new NotImplementedException();
        }

        public void RefreshPresetList(IList<Client.Model.PresetModel> presetList)
        {
            throw new NotImplementedException();
        }

        public void RefreshMaintenanceStatus(Client.Model.UserPriviledgeModel privilegde)
        {
            throw new NotImplementedException();
        }
    }
}
