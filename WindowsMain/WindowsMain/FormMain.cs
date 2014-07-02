using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using Sqlite.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Utils.Windows;

namespace WindowsMain
{
    delegate void SetTextCallback(string text);

    public partial class FormMain : Form
    {
        private Windows.WindowsMgr _WndsMgr = new Windows.WindowsMgr();
        private ConnectionManager connectionMgr = new ConnectionManager();
        private List<string> connectedClients = new List<string>();

        /// <summary>
        /// string = client's user id
        /// manual reset event used to keep track the connection socket and login status
        /// </summary>
        private Dictionary<string, ManualResetEvent> connectionDic = new Dictionary<string, ManualResetEvent>();

        private System.Web.Script.Serialization.JavaScriptSerializer deserialize = new System.Web.Script.Serialization.JavaScriptSerializer();

        private VncMarshall.Client vncClient = new VncMarshall.Client();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            connectionMgr.EvtClientConnected += new ConnectionManager.OnClientConnectedEvt(GetInstance_EvtClientConnected);
            connectionMgr.EvtClientDisconnected += new ConnectionManager.OnClientConnectedEvt(GetInstance_EvtClientDisconnected);
            connectionMgr.EvtClientDataReceived += new ConnectionManager.OnClientDataReceived(GetInstance_EvtClientDataReceived);

            foreach (Windows.WindowsMgr.DisplayInfo info in _WndsMgr.GetScreens())
            {
                Trace.WriteLine(string.Format("left {0}, top {1}, width {2} height {3}", info.WorkArea.Left, info.MonitorArea.Top, info.ScreenWidth, info.ScreenHeight));
            }
            
            // TODO: get the path to store the database 
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            //System.IO.DirectoryInfo directoryInfo = System.IO.Directory.CreateDirectory(appDataPath + "\\icon6000");

            Sqlite.Helper.GetInstance().Initialize(appDataPath + "\\icon6000\\iCon600DB.sqlite");
            Sqlite.Helper.GetInstance().CreateTable(new User());

            User newUser = new User { mUsername = "username", mPassword = "password"};
            // Sqlite.Helper.getInstance().updateData(newUser);
            try
            {
                Sqlite.Helper.GetInstance().AddData(newUser);
            }
            catch (Exception)
            {

                Trace.WriteLine("error");
            }
            Sqlite.Helper.GetInstance().UpdateData(newUser);
            //Sqlite.Helper.getInstance().removeData(newUser);

            // check for license
            System.IO.DriveInfo[] driveInfoList = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in driveInfoList)
            {
                Trace.WriteLine(String.Format("drive name: {0}", drive.RootDirectory));
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Sqlite.Helper.GetInstance().Shutdown();
            _WndsMgr.StopMonitor();
            connectionMgr.StopServer();
            vncClient.StopClient();

            base.OnClosed(e);
        }

        private void SetReceivedText(string text)
        {
            if (output.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetReceivedText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                output.AppendText(text);
            }
        }

        void SetTextCallback(String text)
        {
            output.AppendText(text);
        }

        void GetInstance_EvtClientDataReceived(string userId, int mainId, int subId, string cmdData)
        {
            switch(mainId)
            {
                case (int)CommandConst.MainCommandClient.ClientLoginInfo:
                    handleClientLogin(userId, subId, cmdData);
                    break;
                case (int)CommandConst.MainCommandClient.ClientControlInfo:
                    handleClientControl(subId, cmdData);
                    break;
                case (int)CommandConst.MainCommandClient.ClientVncInfo:
                    handleClientVnc(userId, subId, cmdData);
                    break;
                default:
                    SetReceivedText(Environment.NewLine + String.Format("Data received with userId: {0}, message: {1}", userId, cmdData));
                    break;
            }
        }

        void handleClientVnc(string userId, int subId, string cmdData)
        {
            switch(subId)
            {
                case (int)CommandConst.SubCmdVncInfo.Start:
                    startVncClientCmd(cmdData);
                    break;
                case (int)CommandConst.SubCmdVncInfo.Stop:
                    stopVncClientCmd(cmdData);
                    break;
                default:
                    break;
            }

            
        }

        void startVncClientCmd(string cmdData)
        {
            ClientVncCmd data = deserialize.Deserialize<ClientVncCmd>(cmdData);
            if (data == null)
            {
                return;
            }

            SetReceivedText(Environment.NewLine + String.Format("client vnc starts: {0}:{1}", data.IpAddress, data.PortNumber));

            // start the vnc client to connect
            vncClient.StartClient(data.IpAddress, data.PortNumber);
        }

        void stopVncClientCmd(string cmdData)
        {
            SetReceivedText(Environment.NewLine + String.Format("client vnc stopped"));

            vncClient.StopClient();
        }

        void handleClientLogin(string userId, int subId, string cmdData)
        {
            ClientLoginCmd data = deserialize.Deserialize<ClientLoginCmd>(cmdData);
            if(data == null)
            {
                return;
            }

            SetReceivedText(Environment.NewLine + String.Format("client login with username: {0}, password: {1}", data.username, data.password));

            // get the login status matches with database
            bool matchedUserData = false;
            DataTable dataTable = Sqlite.Helper.GetInstance().ReadData(new User());
            foreach (DataRow dataRow in dataTable.Rows)
            {
                string username = dataRow["username"].ToString();
                string password = dataRow["password"].ToString();

                SetReceivedText(Environment.NewLine + String.Format("database username: {0}, password: {1}", username, password));

                if (username.CompareTo(data.username) == 0 &&
                    password.CompareTo(data.password) == 0)
                {
                    // found matched username and password
                    matchedUserData = true;
                    break;
                }
            }

            if (matchedUserData == false)
            {
                // let the disconnection event timeout
                return;
            }

            connectedClients.Add(userId);

            ManualResetEvent resetEvt;
            foreach(KeyValuePair<string, ManualResetEvent> pair in connectionDic)
            {
                SetReceivedText(Environment.NewLine + String.Format("data in connectionDic, userId: {0}", pair.Key));
            }

            if (connectionDic.TryGetValue(userId, out resetEvt))
            {
                SetReceivedText(Environment.NewLine + String.Format("reset event with user id: {0}", userId));
                resetEvt.Set();
                if(connectionDic.Remove(userId) == false)
                {
                    SetReceivedText(Environment.NewLine + String.Format("failed to remove userId from connectionDic: {0}", userId));
                }
            }

            SetReceivedText(Environment.NewLine + String.Format("send monitor info with userId: {0}", userId));

            List<MonitorInfo> monitors = new List<MonitorInfo>();
            foreach (Windows.WindowsMgr.DisplayInfo info in _WndsMgr.GetScreens())
            {
                MonitorInfo monitorInfo = new MonitorInfo();
                monitorInfo.leftPos = info.WorkArea.Left;
                monitorInfo.topPos = info.WorkArea.Top;
                monitorInfo.rightPos = info.WorkArea.Right;
                monitorInfo.bottomPos = info.WorkArea.Bottom;

                monitors.Add(monitorInfo);
            }

            ServerMonitorsInfo monitorCmd = new ServerMonitorsInfo { monitorAttributes = monitors };
            connectionMgr.BroadcastMessage((int)CommandConst.MainCommandServer.ServerMonitorsInfo,
                (int)CommandConst.SubCmdServerMonitorsInfo.MonitorList, monitorCmd);
        }

        void handleClientControl(int subId, string cmdData)
        {
            switch(subId)
            {
                case (int)CommandConst.SubCmdClientControlInfo.WindowsAttributes:
                    handleWndCommand(cmdData);
                    break;
                case (int)CommandConst.SubCmdClientControlInfo.Mouse:
                    handleMouseCommand(cmdData);
                    break;
                case (int)CommandConst.SubCmdClientControlInfo.Keyboard:
                    handleKeyboardCommand(cmdData);
                    break;
                case (int)CommandConst.SubCmdClientControlInfo.Maintenance:
                    handleMaintenanceCommand(cmdData);
                    break;
            }

        }

        void handleMaintenanceCommand(string cmdData)
        {
            ClientMaintenanceCmd maintenaceCmd = deserialize.Deserialize<ClientMaintenanceCmd>(cmdData);
            if (maintenaceCmd == null)
            {
                return;
            }

            switch (maintenaceCmd.CommandType)
            {
                case ClientMaintenanceCmd.CommandId.EShutdown:
                    DoExitWindow(Constant.EWX_SHUTDOWN);
                    break;
                case ClientMaintenanceCmd.CommandId.EReboot:
                    DoExitWindow(Constant.EWX_REBOOT);
                    break;
                case ClientMaintenanceCmd.CommandId.ELogOff:
                    DoExitWindow(Constant.EWX_LOGOFF);
                    break;
                default:
                    break;
            }
        }

        bool DoExitWindow(int exitCode)
        {
            bool result;
            Utils.Windows.NativeMethods.TokPriv1Luid tp;
            IntPtr hproc = Utils.Windows.NativeMethods.GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            result = Utils.Windows.NativeMethods.OpenProcessToken(hproc, Constant.TOKEN_ADJUST_PRIVILEGES | Constant.TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = Constant.SE_PRIVILEGE_ENABLED;
            result &= Utils.Windows.NativeMethods.LookupPrivilegeValue(null, Constant.SE_SHUTDOWN_NAME, ref tp.Luid);
            result &= Utils.Windows.NativeMethods.AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            result &= Utils.Windows.NativeMethods.ExitWindowsEx(exitCode, 0);

            return result;
        }
        void handleMouseCommand(string cmdData)
        {
            ClientMouseCmd mouseData = deserialize.Deserialize<ClientMouseCmd>(cmdData);
            if (mouseData == null)
            {
                return;
            }

            InputConstants.MOUSEINPUT mouseInput = new InputConstants.MOUSEINPUT();
            mouseInput.dx = mouseData.data.dx;
            mouseInput.dy = mouseData.data.dy;
            mouseInput.mouseData = mouseData.data.mouseData;
            mouseInput.dwFlags = mouseData.data.dwFlags;
            mouseInput.time = mouseData.data.time;
            mouseInput.dwExtraInfo = UIntPtr.Zero;


            // create input object
            InputConstants.INPUT input = new InputConstants.INPUT();
            input.type = InputConstants.MOUSE;
            input.mi = mouseInput;

            // send input to Windows
            
            InputConstants.INPUT[] inputArray = new InputConstants.INPUT[] { input };
            uint result = NativeMethods.SendInput(1, inputArray, System.Runtime.InteropServices.Marshal.SizeOf(input));
            Trace.WriteLine(String.Format("Send input result: {1},{2}, flags:{3}, send:{0}", result, mouseInput.dx, mouseInput.dy, mouseInput.dwFlags));
        }

        void handleKeyboardCommand(string cmdData)
        {
            ClientKeyboardCmd keyboardData = deserialize.Deserialize<ClientKeyboardCmd>(cmdData);
            if (keyboardData == null)
            {
                return;
            }

            InputConstants.KEYBOARDINPUT keyboardInput = new InputConstants.KEYBOARDINPUT();
            keyboardInput.wScan = keyboardData.data.wScan;
            keyboardInput.wVk = keyboardData.data.wVk;
            keyboardInput.dwFlags = keyboardData.data.dwFlags;
            keyboardInput.time = keyboardData.data.time;
            keyboardInput.dwExtraInfo = IntPtr.Zero;

            // create input object
            InputConstants.INPUT input = new InputConstants.INPUT();
            input.type = InputConstants.KEYBOARD;
            input.ki = keyboardInput;

            // send input to Windows
            InputConstants.INPUT[] inputArray = new InputConstants.INPUT[] { input };
            uint result = NativeMethods.SendInput(1, inputArray, System.Runtime.InteropServices.Marshal.SizeOf(input));
        }

        void handleWndCommand(string cmdData)
        {
            ClientWndCmd data = deserialize.Deserialize<ClientWndCmd>(cmdData);
            if (data == null)
            {
                return;
            }

            switch (data.CommandType)
            {
                case ClientWndCmd.CommandId.EMinimize:
                    NativeMethods.ShowWindow(new IntPtr(data.Id), Constant.SW_SHOWMINIMIZED);
                    break;
                case ClientWndCmd.CommandId.EMaximize:
                    NativeMethods.ShowWindow(new IntPtr(data.Id), Constant.SW_SHOWMAXIMIZED);
                    NativeMethods.SetForegroundWindow(new IntPtr(data.Id));
                    break;
                case ClientWndCmd.CommandId.ERelocation:
                    NativeMethods.SetWindowPos(new IntPtr(data.Id), Constant.HWND_TOP, data.PositionX, data.PositionY, 0, 0, (Int32)(Constant.SWP_NOSIZE));
                    NativeMethods.SetForegroundWindow(new IntPtr(data.Id));
                    break;
                case ClientWndCmd.CommandId.EResize:
                    NativeMethods.SetWindowPos(new IntPtr(data.Id), Constant.HWND_TOP, 0, 0, data.Width, data.Height, (Int32)Constant.SWP_NOMOVE);
                    NativeMethods.SetForegroundWindow(new IntPtr(data.Id));
                    break;
                case ClientWndCmd.CommandId.ERestore:
                    NativeMethods.ShowWindow(new IntPtr(data.Id), Constant.SW_SHOWNORMAL);
                    NativeMethods.SetForegroundWindow(new IntPtr(data.Id));
                    break;
                case ClientWndCmd.CommandId.EClose:
                    NativeMethods.SendMessage(new IntPtr(data.Id), Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    break;
                case ClientWndCmd.CommandId.ESetForeground:
                    NativeMethods.SetForegroundWindow(new IntPtr(data.Id));
                    break;
                default:
                    break;
            }
        }

        void GetInstance_EvtClientDisconnected(string userId)
        {
            SetReceivedText(Environment.NewLine + String.Format("Client disconnected with userId: {0}", userId));
            connectedClients.Remove(userId);
        }

        void GetInstance_EvtClientConnected(string userId)
        {
            SetReceivedText(Environment.NewLine + String.Format("Client connects with userId: {0}", userId));

            // start a thread to monitor the username & password sending
            WindowsMain.Server.ThreadClientLogin clientLogin = new Server.ThreadClientLogin { Id = userId, ResetEvt = new ManualResetEvent(false) };
            connectionDic.Add(userId, clientLogin.ResetEvt);

            WaitCallback callback = new WaitCallback(clientDisconnectionThread);
            ThreadPool.QueueUserWorkItem(callback, clientLogin);
        }

        private void clientDisconnectionThread(Object stateInfo)
        {
            WindowsMain.Server.ThreadClientLogin clientLogin = stateInfo as WindowsMain.Server.ThreadClientLogin;

            try
            {
                if (clientLogin.ResetEvt.WaitOne(5000))
                {
                    return;
                }
            }
            catch (Exception e)
            {
                SetReceivedText(Environment.NewLine + String.Format("exception in disconnection thread: {0}", e));
            }

            SetReceivedText(Environment.NewLine + String.Format("disconnection thread exit, userId: {0}", clientLogin.Id));
            connectionMgr.RemoveClient(clientLogin.Id);
            connectionDic.Remove(clientLogin.Id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int portOpened = connectionMgr.StartServer(Int32.Parse(startPort.Text), Int32.Parse(endPort.Text));

            output.AppendText(Environment.NewLine + "Server port opened: " + portOpened.ToString());
            if (portOpened != -1)
            {
                _WndsMgr.EvtApplicationWndChanged += new Windows.WindowsMgr.OnApplicationWndChanged(_WndsMgr_EvtApplicationChanged);
                _WndsMgr.StartMonitor();
            }
        }

        void _WndsMgr_EvtApplicationChanged(List<Windows.WindowsMgr.WndAttributes> wndAttributes)
        {
            if (connectedClients.Count == 0)
            {
                return;
            }

            ServerWindowsPos windowsPos = new ServerWindowsPos();

            List<WndPos> windowList = new List<WndPos>();
            int zOrderCounter = 0;
            foreach (Windows.WindowsMgr.WndAttributes attribute in wndAttributes)
            {
                WndPos wndPos = new WndPos { id=attribute.id, name = attribute.name, posX = attribute.posX, posY = attribute.posY, 
                    width = attribute.width, height=attribute.height, style=attribute.style, ZOrder=zOrderCounter };

                windowList.Add(wndPos);

                zOrderCounter++;
            }
            windowsPos.windowsAttributes = windowList;
            //Trace.WriteLine(String.Format("windows list size: {0}", windowList.Count.ToString()));

            try
            {
                connectionMgr.SendData((int)CommandConst.MainCommandServer.ServerWindowsInfo,
                    (int)CommandConst.SubCmdServerWindowsInfo.WindowsList, windowsPos, new List<string>(connectedClients));
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            
        }

        private void stopServer_Click(object sender, EventArgs e)
        {
            connectedClients.Clear();
            connectionMgr.StopServer();
            _WndsMgr.StopMonitor();
        }

        private void addDB_Click(object sender, EventArgs e)
        {
            User user = new User { mUsername = username.Text, mPassword=password.Text};
            Sqlite.Helper.GetInstance().AddData(user);
        }

        private void removeDB_Click(object sender, EventArgs e)
        {
            User user = new User { mUsername = username.Text };
            Sqlite.Helper.GetInstance().RemoveData(user);
        }

        private void updateDB_Click(object sender, EventArgs e)
        {
            User user = new User { mUsername = username.Text, mPassword = password.Text };
            Sqlite.Helper.GetInstance().UpdateData(user);
        }

        private void startVncClient_Click(object sender, EventArgs e)
        {
            //vncClient.StartClient();
        }

        private void stopVncClient_Click(object sender, EventArgs e)
        {
            //vncClient.StopClient();
        }
    }
}
