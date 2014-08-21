﻿using Database.Data;
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

namespace WindowsFormClient 
{
    delegate void SetTextCallback(string text);

    public partial class FormMain : Form, IServer
    {
        private Windows.WindowsAppMgr _WndsMgr = new Windows.WindowsAppMgr();
        private ConnectionManager connectionMgr = new ConnectionManager();

        private License.LicenseChecker licenseChecker = null;

        /// <summary>
        /// string = client's user id
        /// manual reset event used to keep track the connection socket and login status
        /// </summary>
        private Dictionary<object, ManualResetEvent> connectionDic = new Dictionary<object, ManualResetEvent>();

        // TODO:
        private VncMarshall.Client vncClient = new VncMarshall.Client(@"C:\Program Files\TightVNC\tvnviewer.exe");

        private Server.ServerCmdMgr serverCmdMgr = null;

        public FormMain()
        {
            InitializeComponent();

            System.Windows.Forms.Application.ApplicationExit += Application_ApplicationExit;
            CustomWinForm.AutoCloseMessageBox.msgBoxResultEvent += AutoCloseMessageBox_msgBoxResultEvent;

            serverCmdMgr = new Server.ServerCmdMgr(this);            
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (licenseChecker != null)
            {
                licenseChecker.StopCheck();
                licenseChecker = null;
            }

            Database.DbHelper.GetInstance().Shutdown();
            _WndsMgr.StopMonitor();
            connectionMgr.StopServer();
            vncClient.StopAllClients();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            connectionMgr.EvtClientConnected += new ConnectionManager.OnClientConnectedEvt(GetInstance_EvtClientConnected);
            connectionMgr.EvtClientDisconnected += new ConnectionManager.OnClientConnectedEvt(GetInstance_EvtClientDisconnected);
            connectionMgr.EvtClientDataReceived += new ConnectionManager.OnClientDataReceived(GetInstance_EvtClientDataReceived);

            Server.ServerDbHelper.GetInstance().Initialize();

            if (Properties.Settings.Default.CheckLicense)
            {
                // check for license
                System.IO.DriveInfo[] driveInfoList = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo drive in driveInfoList)
                {
                    // check for removable drive
                    if (drive.DriveType == System.IO.DriveType.Removable)
                    {
                        if (Utils.Files.IsFileExists(drive.RootDirectory + "VostrolLicense.dat"))
                        {
                            // start the license checking
                            licenseChecker = new License.LicenseChecker(drive.RootDirectory + "VostrolLicense.dat");
                            licenseChecker.EvtLicenseCheckStatus += licenseChecker_EvtLicenseCheckStatus;
                            licenseChecker.StartCheck();
                            break;
                        }
                    }
                }

                if (licenseChecker == null)
                {
                    System.Windows.Forms.Application.Exit();
                    return;
                }
            }
        }

        public delegate void InvokeDelegate();
        void CloseApplication()
        {
            // display an message box and wait for 1 minute
            /* DialogResult result = CustomWinForm.AutoCloseMessageBox.Show(
                                 "Application will be close in 60 seconds." + Environment.NewLine + "Please plug in the license dodger.",
                                 "License not found",
                                 10000,
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error); */


            CustomWinForm.AutoCloseMessageBox.Show(new CustomWinForm.MsgBoxExtOptions(
                "Application will be close in 60 seconds." + Environment.NewLine + "Please plug in the license dodger.",
                "License not found",
                CustomWinForm.MsgBoxResultReferences.EMPTY,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                60000,
                true
            ));
        }

        void AutoCloseMessageBox_msgBoxResultEvent(object sender, CustomWinForm.MsgBoxResultEventArgs e)
        {
            Trace.WriteLine(String.Format("result: {0}, reference: {1}", e.resultButton, e.resultReference));
            if (licenseChecker != null)
            {
                licenseChecker.StopCheck();
                licenseChecker = null;
            }
            System.Windows.Forms.Application.Exit();
        }

        void licenseChecker_EvtLicenseCheckStatus(License.LicenseChecker checker, bool isValid)
        {
            if(isValid == false)
            {
                this.BeginInvoke(new InvokeDelegate(CloseApplication));
            }
            else
            {
                // dispose the created message box
                CustomWinForm.AutoCloseMessageBox.closeMessageBox("License not found");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (licenseChecker != null)
            {
                licenseChecker.StopCheck();
                licenseChecker = null;
            }

            Database.DbHelper.GetInstance().Shutdown();
            _WndsMgr.StopMonitor();
            connectionMgr.StopServer();
            vncClient.StopAllClients();

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
            serverCmdMgr.ExeCommand(userId, mainId, subId, cmdData);
        }

        void GetInstance_EvtClientDisconnected(string userId)
        {
            SetReceivedText(Environment.NewLine + String.Format("Client disconnected with userId: {0}", userId));
            Server.ConnectedClientHelper.GetInstance().RemoveClient(userId);
        }

        void GetInstance_EvtClientConnected(string userId)
        {
            SetReceivedText(Environment.NewLine + String.Format("Client connects with userId: {0}", userId));

            // start a thread to monitor the username & password sending
            WindowsFormClient.Server.ThreadClientLogin clientLogin = new Server.ThreadClientLogin { Id = userId, ResetEvt = new ManualResetEvent(false) };
            connectionDic.Add(userId, clientLogin.ResetEvt);

            WaitCallback callback = new WaitCallback(clientDisconnectionThread);
            ThreadPool.QueueUserWorkItem(callback, clientLogin);
        }

        private void clientDisconnectionThread(Object stateInfo)
        {
            WindowsFormClient.Server.ThreadClientLogin clientLogin = stateInfo as WindowsFormClient.Server.ThreadClientLogin;

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
                _WndsMgr.EvtApplicationWndChanged += new Windows.WindowsAppMgr.OnApplicationWndChanged(_WndsMgr_EvtApplicationChanged);
                _WndsMgr.StartMonitor();
            }
        }

        void _WndsMgr_EvtApplicationChanged(List<Windows.WindowsAppMgr.WndAttributes> wndAttributes)
        {
            if (Server.ConnectedClientHelper.GetInstance().GetClientsCount() == 0)
            {
                return;
            }

            ServerWindowsPos windowsPos = new ServerWindowsPos();

            List<WndPos> windowList = new List<WndPos>();
            int zOrderCounter = 0;
            foreach (Windows.WindowsAppMgr.WndAttributes attribute in wndAttributes)
            {
                WndPos wndPos = new WndPos { id=attribute.id, name = attribute.name, posX = attribute.posX, posY = attribute.posY, 
                    width = attribute.width, height=attribute.height, style=attribute.style, ZOrder=zOrderCounter };

                windowList.Add(wndPos);

                zOrderCounter++;
            }
            windowsPos.WindowsAttributes = windowList;
            //Trace.WriteLine(String.Format("windows list size: {0}", windowList.Count.ToString()));

            try
            {
                connectionMgr.SendData((int)CommandConst.MainCommandServer.WindowsInfo,
                    (int)CommandConst.SubCommandServer.WindowsList, windowsPos, Server.ConnectedClientHelper.GetInstance().GetAllClientsSocketId());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            
        }

        private void stopServer_Click(object sender, EventArgs e)
        {
            Server.ConnectedClientHelper.GetInstance().RemoveAllClients();
            connectionMgr.StopServer();
            _WndsMgr.StopMonitor();
        }

        private void addDB_Click(object sender, EventArgs e)
        {
            Group group = new Group() { label="group1", share_full_desktop=true, allow_maintenance=true};
            Database.DbHelper.GetInstance().AddData(group);

            User user = new User { username=username.Text, password=password.Text, label="display name", group=3 };
            Database.DbHelper.GetInstance().AddData(user);
        }

        private void removeDB_Click(object sender, EventArgs e)
        {
            User user = new User { id=0 };
            Database.DbHelper.GetInstance().RemoveData(user);
        }

        private void updateDB_Click(object sender, EventArgs e)
        {
            User user = new User { id = 0, label = "asd", username = username.Text, password = password.Text };
            Database.DbHelper.GetInstance().UpdateData(user);
        }

        private void startVncClient_Click(object sender, EventArgs e)
        {
            //vncClient.StartClient();
        }

        private void stopVncClient_Click(object sender, EventArgs e)
        {
            //vncClient.StopClient();
        }

        public void ClientLogin(Server.Model.ClientInfoModel model)
        {
            ManualResetEvent resetEvt;
            if (connectionDic.TryGetValue(model.SocketUserId, out resetEvt))
            {
                SetReceivedText(Environment.NewLine + String.Format("reset event with user id: {0}", model.SocketUserId));
                resetEvt.Set();
                if (connectionDic.Remove(model.SocketUserId) == false)
                {
                    SetReceivedText(Environment.NewLine + String.Format("failed to remove userId from connectionDic: {0}", model.SocketUserId));
                }

                // add the user to list
                Server.ConnectedClientHelper.GetInstance().AddClient(model.SocketUserId, model);

                // Reply to user
                SendLoginReply(model);


                // TODO: broadcast vnc to all

                
            }  
        }

        public VncMarshall.Client GetVncClient()
        {
            return this.vncClient;
        }


        public Server.Model.ClientInfoModel GetClientInfo(string socketId)
        {
            return Server.ConnectedClientHelper.GetInstance().GetClientInfo(socketId);
        }

        private void SendLoginReply(Server.Model.ClientInfoModel model)
        {
            // get server's monitor info
            List<Session.Data.SubData.MonitorInfo> monitorList = new List<MonitorInfo>();
            foreach(WindowsHelper.MonitorInfo monitor in Utils.Windows.WindowsHelper.GetMonitorList())
            {
                monitorList.Add(new Session.Data.SubData.MonitorInfo() 
                { 
                    LeftPos=monitor.WorkArea.Left,
                    TopPos = monitor.WorkArea.Top,
                    RightPos = monitor.WorkArea.Right,
                    BottomPos = monitor.WorkArea.Bottom
                });
            }


            // get user's application list
            ServerApplicationStatus serverAppStatus = new ServerApplicationStatus();
            serverAppStatus.UserApplicationList = new List<ApplicationEntry>();
            foreach(Server.ServerDbHelper.ApplicationData appData in Server.ServerDbHelper.GetInstance().GetAppsWithUserId(model.DbUserId))
            {
                serverAppStatus.UserApplicationList.Add(new ApplicationEntry() 
                { 
                    Identifier = appData.id, 
                    Name = appData.name 
                });
            }

            // get user's preset list
            ServerPresetsStatus serverPresetStatus = new ServerPresetsStatus();
            serverPresetStatus.UserPresetList = new List<PresetsEntry>();
            foreach(Server.ServerDbHelper.PresetData presetData in Server.ServerDbHelper.GetInstance().GetPresetByUserId(model.DbUserId))
            {
                List<ApplicationEntry> presetAppEntries = new List<ApplicationEntry>();
                foreach (Server.ServerDbHelper.ApplicationData appData in presetData.AppDataList)
                {
                    presetAppEntries.Add(new ApplicationEntry()
                    {
                        Identifier = appData.id,
                        Name = appData.name
                    });
                }
                serverPresetStatus.UserPresetList.Add(new PresetsEntry() 
                {
                    Identifier = presetData.Id,
                    Name = presetData.Name,
                    ApplicationList = presetAppEntries
                });
            }

            // get user's priviledge
            ServerMaintenanceStatus maintenanceStatus = new ServerMaintenanceStatus();
            maintenanceStatus.AllowMaintenance = Server.ServerDbHelper.GetInstance().GetGroupByUserId(model.DbUserId).allow_maintenance;
             
            ServerLoginReply reply = new ServerLoginReply()
            {
                LoginName = model.Name,
                UserId = model.DbUserId,
                ServerLayout = new ServerScreenInfo()
                {
                    MatrixCol = 2,
                    MatrixRow = 2,
                    ServerMonitorsList = monitorList
                },
                // UserApplications
                UserApplications = serverAppStatus,

                // UserPresets
                UserPresets = serverPresetStatus,

                // UserMaintenance
                UserMaintenance = maintenanceStatus,
                
            };

            connectionMgr.SendData(
                (int)CommandConst.MainCommandServer.UserPriviledge,
                (int)CommandConst.SubCommandServer.DisplayInfo,
                reply, 
                new List<string>() { model.SocketUserId });

        }


        public void AddMessageBox(string message, System.Drawing.Font font, System.Drawing.Color color, int duration, int left, int top, int width, int height)
        {
            throw new NotImplementedException();
        }


        public ConnectionManager GetConnectionMgr()
        {
            return connectionMgr;
        }
    }
}