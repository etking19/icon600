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
using WindowsMain.Client;
using WindowsMain.Comparer;

namespace WindowsMain
{
    public partial class FormClient : Form
    {
        private ConnectionManager connectionMgr = new ConnectionManager();
        private System.Web.Script.Serialization.JavaScriptSerializer deserialize = new System.Web.Script.Serialization.JavaScriptSerializer();

        private List<WndPos> applicationList = new List<WndPos>();
        private CustomControlHolder mHolder = new CustomControlHolder(new Size(0, 0), Int32.MinValue, Int32.MinValue);

        private Dictionary<int, int> mWindowsDic = new Dictionary<int, int>();

        private delegate void DelegateWindow(WndPos wndPos);
        private delegate void DelegateEvt();

        MouseHook mouseHook = new MouseHook();
        KeyboardHook keyboardHook = new KeyboardHook();

        private const int VNC_PORTSTART = 5800;
        private const int VNC_PORTSTOP = 5810;

        private VncMarshall.Server vncServer = new VncMarshall.Server();

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
            applicationsListbox.DisplayMember = "name";
            applicationsListbox.ValueMember = "id";
            applicationsListbox.ClearSelected();
            applicationsListbox.SelectedIndexChanged += applicationsListbox_SelectedIndexChanged;
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
                    (int)CommandConst.MainCommandClient.ClientControlInfo,
                    (int)CommandConst.SubCmdClientControlInfo.WindowsAttributes,
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
                    (int)CommandConst.MainCommandClient.ClientControlInfo,
                    (int)CommandConst.SubCmdClientControlInfo.WindowsAttributes,
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
                    (int)CommandConst.MainCommandClient.ClientControlInfo,
                    (int)CommandConst.SubCmdClientControlInfo.WindowsAttributes,
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
                    (int)CommandConst.MainCommandClient.ClientControlInfo,
                    (int)CommandConst.SubCmdClientControlInfo.WindowsAttributes,
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
                    (int)CommandConst.MainCommandClient.ClientControlInfo,
                    (int)CommandConst.SubCmdClientControlInfo.WindowsAttributes,
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
                    (int)CommandConst.MainCommandClient.ClientControlInfo,
                    (int)CommandConst.SubCmdClientControlInfo.WindowsAttributes,
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
                    (int)CommandConst.MainCommandClient.ClientControlInfo,
                    (int)CommandConst.SubCmdClientControlInfo.WindowsAttributes,
                    wndCommand);
            }
        }

        void connectionMgr_EvtServerDataReceived(int mainId, int subId, string commandData)
        {
            switch ((CommandConst.MainCommandServer)mainId)
            {
                case CommandConst.MainCommandServer.ServerWindowsInfo:
                    // windows data
                    handleWindowsData(subId, commandData);
                    break;
                case CommandConst.MainCommandServer.ServerMonitorsInfo:
                    handleMonitorsData(subId, commandData);
                    break;
                default:
                    break;
            }
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

            vncServer.StopServer();
        }

        void connectionMgr_EvtConnected()
        {
            ClientLoginCmd loginCmd = new ClientLoginCmd();
            loginCmd.username = username.Text;
            loginCmd.password = password.Text;

            connectionMgr.BroadcastMessage((int)CommandConst.MainCommandClient.ClientLoginInfo, 0, loginCmd);
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
                (int)CommandConst.MainCommandClient.ClientControlInfo,
                (int)CommandConst.SubCmdClientControlInfo.Keyboard,
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
                (int)CommandConst.MainCommandClient.ClientControlInfo,
                (int)CommandConst.SubCmdClientControlInfo.Mouse,
                mouseCmd);
        }


        private void disconnect_Click(object sender, EventArgs e)
        {
            connectionMgr.StopClient();
            connectionMgr_EvtDisconnected();

            vncServer.StopServer();
        }

        void handleMonitorsData(int subId, string commandData)
        {
            switch ((CommandConst.SubCmdServerMonitorsInfo)subId)
            {
                case CommandConst.SubCmdServerMonitorsInfo.MonitorList:
                    ServerMonitorsInfo data = deserialize.Deserialize<ServerMonitorsInfo>(commandData);
                    if (data == null)
                    {
                        return;
                    }

                    int minPosX = 0;
                    int minPosY = 0;
                    int maxPosX = 0;
                    int maxPosY = 0;
                    foreach(MonitorInfo monitor in data.monitorAttributes)
                    {
                        minPosX = Math.Min(monitor.leftPos, minPosX);
                        minPosY = Math.Min(monitor.topPos, minPosY);

                        maxPosX = Math.Max(monitor.rightPos, maxPosX);
                        maxPosY = Math.Max(monitor.bottomPos, maxPosY);
                    }

                    mHolder.MaxSize = new Size(maxPosX - minPosX, maxPosY - minPosY);
                    mHolder.ReferenceXPos = minPosX;
                    mHolder.ReferenceYPos = minPosY;

                    break;
            }
        }


        void handleWindowsData(int subId, string commandData)
        {
            switch(subId)
            {
                case (int)CommandConst.SubCmdServerWindowsInfo.WindowsList:
                    // windows application's positions
                    handleWindowsAppPos(commandData);
                    break;
            }
        }

        void handleWindowsAppPos(string commandData)
        {
            ServerWindowsPos data = deserialize.Deserialize<ServerWindowsPos>(commandData);
            if (data == null)
            {
                return;
            }

            // find difference with current list and updated list

             List<WndPos> tempList = new List<WndPos>(data.windowsAttributes);

            // 1. find the windows that newly added
            List<WndPos> addedQuery = data.windowsAttributes.Except(applicationList, new WndObjComparer()).ToList();

            // 2. find the windows that removed
            List<WndPos> removedQuery = applicationList.Except(data.windowsAttributes, new WndObjComparer()).ToList();

            // 3. find the windows with attributes changed
            foreach (WndPos windows in addedQuery)
            {
                // remove the additional objects in list
                data.windowsAttributes.Remove(windows);
            }

            foreach (WndPos windows in removedQuery)
            {
                // remove the additional objects in list
                applicationList.Remove(windows);
            }

            // compare the remaining list, these lists should contain the same object's id and count
            List<WndPos> modifiedNameQuery = data.windowsAttributes.Except(applicationList, new WndNameComparer()).ToList();
            List<WndPos> modifiedPosQuery = data.windowsAttributes.Except(applicationList, new WndPosComparer()).ToList();
            List<WndPos> modifiedSizeQuery = data.windowsAttributes.Except(applicationList, new WndSizeComparer()).ToList();
            List<WndPos> modifiedStyleQuery = data.windowsAttributes.Except(applicationList, new WndStyleComparer()).ToList();

            // save the updated list
            applicationList.Clear();
            applicationList.AddRange(tempList);

           // Trace.WriteLine(String.Format("add:{0}, remove:{1}, modified:{2}, totalNow:{3}", addedQuery.Count.ToString(), removedQuery.Count.ToString(), modifiedQuery.Count.ToString(), applicationList.Count.ToString()));

            bool refreshAppList = false;
            foreach (WndPos windows in addedQuery)
            {
                Trace.WriteLine(String.Format("Added - name:{5} id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id, windows.name));
                refreshAppList |= true;
                AddWindow(windows);
            }

            foreach (WndPos windows in removedQuery)
            {
                Trace.WriteLine(String.Format("Removed - name:{5}  id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id, windows.name));
                refreshAppList |= true;
                RemoveWindow(windows);
            }

            foreach (WndPos windows in modifiedNameQuery)
            {
                //Trace.WriteLine(String.Format("Modified - id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id));
                ChangeWindowName(windows);
            }

            foreach (WndPos windows in modifiedPosQuery)
            {
                //Trace.WriteLine(String.Format("Modified - id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id));
                ChangeWindowPos(windows);
            }

            foreach (WndPos windows in modifiedStyleQuery)
            {
                // NOTE: must set style then only set size as maximize state will not shown in the entire screen
                ChangeWindowStyle(windows);
            }

            foreach (WndPos windows in modifiedSizeQuery)
            {
                //Trace.WriteLine(String.Format("Modified - id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id));
                ChangeWindowSize(windows);
            }

            // update entire list z-order
            foreach (WndPos windows in applicationList)
            {
                //Trace.WriteLine(String.Format("Modified - id:{4} X:{0} Y:{1} Width:{2} Height:{3}", windows.posX, windows.posY, windows.width, windows.height, windows.id));
                ChangeWindowZOrder(windows);
            }

            if (refreshAppList)
            {
                refreshApplicationList();
            }
        }

        private void AddWindow(WndPos wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(AddWindow), wndPos);
                return;
            }


            int wndId = mHolder.AddControl(new CustomWinForm.CustomControlHolder.ControlAttributes {
                WindowName = wndPos.name, 
                Xpos = wndPos.posX, 
                Ypos = wndPos.posY, 
                Width = wndPos.width, 
                Height = wndPos.height,
                Style = wndPos.style,
                ZOrder = wndPos.ZOrder
            });

            mWindowsDic.Add(wndPos.id, wndId);

            // check if the control was minimized
            if ((wndPos.style & Constant.WS_MINIMIZE) != 0)
            {
                AddMinimizedWindow(wndPos.id, wndPos);
            }
        }

        private void RemoveWindow(WndPos wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(RemoveWindow), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.id, out wndId))
            {
                mHolder.RemoveControl(wndId);
                mWindowsDic.Remove(wndPos.id);

                RemoveMinimizedWindow(wndPos.id);
            }
        }

        private void ChangeWindowName(WndPos wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowName), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.id, out wndId))
            {
                mHolder.ChangeControlName(wndId, wndPos.name);
            }

        }

        private void ChangeWindowPos(WndPos wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowPos), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.id, out wndId))
            {
                mHolder.ChangeControlPos(wndId, new Point(wndPos.posX, wndPos.posY));
            }
        }

        private void ChangeWindowSize(WndPos wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowSize), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.id, out wndId))
            {
                mHolder.ChangeControlSize(wndId, new Size(wndPos.width, wndPos.height));
            }
        }

        private void ChangeWindowStyle(WndPos wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowStyle), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.id, out wndId))
            {
                mHolder.ChangeControlStyle(wndId, wndPos.style);

                // check if the control was minimized
                if ((wndPos.style & Constant.WS_MINIMIZE) != 0)
                {
                    AddMinimizedWindow(wndPos.id, wndPos);
                }
                else
                {
                    RemoveMinimizedWindow(wndPos.id);
                }

            }
        }

        private void ChangeWindowZOrder(WndPos wndPos)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateWindow(ChangeWindowZOrder), wndPos);
                return;
            }

            int wndId;
            if (mWindowsDic.TryGetValue(wndPos.id, out wndId))
            {
                mHolder.ChangeControlZOrder(wndId, wndPos.ZOrder);
            }
        }

        private void AddMinimizedWindow(int id, WndPos data)
        {
            MinimizeComboBox comboBox = new MinimizeComboBox();
            comboBox.Id = id;
            comboBox.Text = data.name;
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
                (int)CommandConst.MainCommandClient.ClientControlInfo,
                (int)CommandConst.SubCmdClientControlInfo.WindowsAttributes,
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

            vncServer.StopServer();
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
            applicationsListbox.DisplayMember = "name";
            applicationsListbox.ValueMember = "id";
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
                (int)CommandConst.MainCommandClient.ClientControlInfo,
                (int)CommandConst.SubCmdClientControlInfo.Maintenance,
                command);
        }

        private void startVNC_Click(object sender, EventArgs e)
        {
            int portNumber = Utils.Socket.getUnusedPort(VNC_PORTSTART, VNC_PORTSTOP);

            // start vnc server
            vncServer.StartServer(portNumber);

            // send signal to server to indicate vnc server info
            ClientVncCmd command = new ClientVncCmd { PortNumber = portNumber, IpAddress=Utils.Socket.LocalIPAddress() };
            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ClientVncInfo,
                (int)CommandConst.SubCmdVncInfo.Start,
                command);
        }

        private void stopVNC_Click(object sender, EventArgs e)
        {
            vncServer.StopServer();

            // send signal to server to indicate vnc server info
            ClientVncCmd command = new ClientVncCmd();
            connectionMgr.BroadcastMessage(
                (int)CommandConst.MainCommandClient.ClientVncInfo,
                (int)CommandConst.SubCmdVncInfo.Stop,
                command);
        }
    }
}
