using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// string = client's user id
        /// manual reset event used to keep track the connection socket and login status
        /// </summary>
        private Dictionary<string, ManualResetEvent> connectionDic = new Dictionary<string, ManualResetEvent>();

        private System.Web.Script.Serialization.JavaScriptSerializer deserialize = new System.Web.Script.Serialization.JavaScriptSerializer();
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
            // Sqlite.Helper.GetInstance().Initialize("iCon600DB.sqlite");

           // User user = new User();
           // //Sqlite.Helper.getInstance().createTable(user);

           // User newUser = new User("test", "12534");
           //// Sqlite.Helper.getInstance().updateData(newUser);
           // try
           // {
           //     Sqlite.Helper.GetInstance().AddData(newUser);
           // }
           // catch (Exception)
           // {

           //     Trace.WriteLine("error");
           // }
           // Sqlite.Helper.GetInstance().UpdateData(newUser);
           // //Sqlite.Helper.getInstance().removeData(newUser);
        }

        protected override void OnClosed(EventArgs e)
        {
            _WndsMgr.StopMonitor();
            connectionMgr.StopServer();
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
                default:
                    SetReceivedText(Environment.NewLine + String.Format("Data received with userId: {0}, message: {1}", userId, cmdData));
                    break;
            }
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
            ManualResetEvent resetEvt;
            if (connectionDic.TryGetValue(userId, out resetEvt))
            {
                resetEvt.Set();
                connectionDic.Remove(userId);
            }


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
            connectionMgr.SendData((int)CommandConst.MainCommandServer.ServerMonitorsInfo,
                (int)CommandConst.SubCmdServerMonitorsInfo.MonitorList, monitorCmd);
        }

        void handleClientControl(int subId, string cmdData)
        {
            ClientWndCmd data = deserialize.Deserialize<ClientWndCmd>(cmdData);
            if(data == null)
            {
                return;
            }

            switch(data.CommandType)
            {
                case ClientWndCmd.CommandId.EMinimize:
                    User32.ShowWindow(new IntPtr(data.Id), Constant.SW_SHOWMINIMIZED);
                    break;
                case ClientWndCmd.CommandId.EMaximize:
                    User32.ShowWindow(new IntPtr(data.Id), Constant.SW_SHOWMAXIMIZED);
                    User32.SetWindowPos(new IntPtr(data.Id), Constant.HWND_TOP, 0, 0, 0, 0, (Int32)(Constant.SWP_NOMOVE | Constant.SWP_NOSIZE));
                    break;
                case ClientWndCmd.CommandId.ERelocation:
                    User32.SetWindowPos(new IntPtr(data.Id), Constant.HWND_TOP, data.PositionX, data.PositionY, 0, 0, (Int32)(Constant.SWP_NOSIZE));
                    break;
                case ClientWndCmd.CommandId.EResize:
                    User32.SetWindowPos(new IntPtr(data.Id), Constant.HWND_TOP, 0, 0, data.Width, data.Height, (Int32)Constant.SWP_NOMOVE);
                    break;
                case ClientWndCmd.CommandId.ERestore:
                    User32.ShowWindow(new IntPtr(data.Id), Constant.SW_SHOWNORMAL);
                    User32.SetWindowPos(new IntPtr(data.Id), Constant.HWND_TOP, 0, 0, 0, 0, (Int32)(Constant.SWP_NOMOVE | Constant.SWP_NOSIZE));
                    break;
                case ClientWndCmd.CommandId.EClose:
                    User32.SendMessage(new IntPtr(data.Id), Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    break;
                default:
                    break;
            }
        }

        void GetInstance_EvtClientDisconnected(string userId)
        {
            SetReceivedText(Environment.NewLine + String.Format("Client disconnected with userId: {0}", userId));
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

            if(clientLogin.ResetEvt.WaitOne(1000))
            {
                return;
            }
            connectionMgr.RemoveClient(clientLogin.Id);
            connectionDic.Remove(clientLogin.Id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int portOpened = connectionMgr.StartServer(12000, 12050);

            output.AppendText(Environment.NewLine + "Server port opened: " + portOpened.ToString());

            _WndsMgr.EvtApplicationWndChanged += new Windows.WindowsMgr.OnApplicationWndChanged(_WndsMgr_EvtApplicationChanged);
            _WndsMgr.StartMonitor();
            
        }
        void _WndsMgr_EvtApplicationChanged(List<Windows.WindowsMgr.WndAttributes> wndAttributes)
        {
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
                    (int)CommandConst.SubCmdServerWindowsInfo.WindowsList, windowsPos);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            
        }

        private void stopServer_Click(object sender, EventArgs e)
        {
            connectionMgr.StopServer();
            _WndsMgr.StopMonitor();
        }


        private void sendMsg_Click(object sender, EventArgs e)
        {
        }
    }
}
