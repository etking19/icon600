﻿using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Utils.Windows;
using WcfServiceLibrary1;
using Windows;
using WindowsFormClient.Server;
using WindowsFormClient.Server.Model;

namespace WindowsFormClient.Presenter
{
    class ConnectionPresenter : IDisposable
    {
        private IServer server;
        private WindowsAppMgr runningWndsMgr;
        private ConnectionManager connectionMgr;
        private ServerCmdMgr commandManager;

        /// <summary>
        /// string = client's user id
        /// manual reset event used to keep track the connection socket and login status
        /// </summary>
        private Dictionary<object, ManualResetEvent> connectionEvtDictionary = new Dictionary<object, ManualResetEvent>();

        public ConnectionPresenter(IServer server, ConnectionManager mgr)
        {
            this.server = server;

            runningWndsMgr = new WindowsAppMgr();
            runningWndsMgr.EvtApplicationWndChanged += runningWndsMgr_EvtApplicationWndChanged;
            runningWndsMgr.StartMonitor();

            commandManager = new ServerCmdMgr(server);

            this.connectionMgr = mgr;
            connectionMgr.EvtClientConnected += connectionMgr_EvtClientConnected;
            connectionMgr.EvtClientDisconnected += connectionMgr_EvtClientDisconnected;
            connectionMgr.EvtClientDataReceived += connectionMgr_EvtClientDataReceived;
        }

        void connectionMgr_EvtClientDataReceived(string userId, int mainId, int subId, string commandData)
        {
            commandManager.ExeCommand(userId, mainId, subId, commandData);
        }

        void connectionMgr_EvtClientDisconnected(string userId)
        {
            Server.ConnectedClientHelper.GetInstance().RemoveClient(userId);
        }

        void connectionMgr_EvtClientConnected(string userId)
        {
            // start a thread to monitor the username & password sending
            WindowsFormClient.Server.ThreadClientLogin clientLogin = new Server.ThreadClientLogin { Id = userId, ResetEvt = new ManualResetEvent(false) };
            connectionEvtDictionary.Add(userId, clientLogin.ResetEvt);

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
            catch (Exception)
            {
            }

            connectionMgr.RemoveClient(clientLogin.Id);
            connectionEvtDictionary.Remove(clientLogin.Id);
        }

        void runningWndsMgr_EvtApplicationWndChanged(List<WindowsAppMgr.WndAttributes> wndAttributes)
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
                WndPos wndPos = new WndPos
                {
                    id = attribute.id,
                    name = attribute.name,
                    posX = attribute.posX,
                    posY = attribute.posY,
                    width = attribute.width,
                    height = attribute.height,
                    style = attribute.style,
                    ZOrder = zOrderCounter
                };

                windowList.Add(wndPos);

                zOrderCounter++;
            }
            windowsPos.WindowsAttributes = windowList;

            try
            {
                connectionMgr.SendData((int)CommandConst.MainCommandServer.WindowsInfo,
                    (int)CommandConst.SubCommandServer.WindowsList, 
                    windowsPos, 
                    Server.ConnectedClientHelper.GetInstance().GetAllClientsSocketId());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void ClientCredentialReceived(Server.Model.ClientInfoModel model, int desktopRow, int desktopCol)
        {
            ManualResetEvent resetEvt;
            if (connectionEvtDictionary.TryGetValue(model.SocketUserId, out resetEvt))
            {
                resetEvt.Set();
                connectionEvtDictionary.Remove(model.SocketUserId);

                // Reply to user
                sendLoginReply(model, desktopRow, desktopCol);

                // add the user to list
                ConnectedClientHelper.GetInstance().AddClient(model.SocketUserId, model);
            }  
        }

        private void sendLoginReply(Server.Model.ClientInfoModel model, int dekstopRow, int desktopCol)
        {
            // get server's monitor info
            List<Session.Data.SubData.MonitorInfo> monitorList = new List<MonitorInfo>();
            int desktopLeft = 0;
            int desktopTop = 0;
            int desktopRight = 0;
            int desktopBottom = 0;
            foreach (WindowsHelper.MonitorInfo monitor in Utils.Windows.WindowsHelper.GetMonitorList())
            {
                if(desktopLeft > monitor.WorkArea.Left)
                {
                    desktopLeft = monitor.MonitorArea.Left;
                }

                if(desktopTop > monitor.WorkArea.Top)
                {
                    desktopTop = monitor.MonitorArea.Top;
                }

                if(desktopRight < monitor.WorkArea.Right)
                {
                    desktopRight = monitor.MonitorArea.Right;
                }

                if(desktopBottom < monitor.WorkArea.Bottom)
                {
                    desktopBottom = monitor.MonitorArea.Bottom;
                }

                monitorList.Add(new Session.Data.SubData.MonitorInfo()
                {
                    LeftPos = monitor.MonitorArea.Left,
                    TopPos = monitor.MonitorArea.Top,
                    RightPos = monitor.MonitorArea.Right,
                    BottomPos = monitor.MonitorArea.Bottom
                });
            }


            // get user's application list
            ServerApplicationStatus serverAppStatus = new ServerApplicationStatus();
            serverAppStatus.UserApplicationList = new List<ApplicationEntry>();
            foreach (ApplicationData appData in Server.ServerDbHelper.GetInstance().GetAppsWithUserId(model.DbUserId))
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
            foreach (PresetData presetData in Server.ServerDbHelper.GetInstance().GetPresetByUserId(model.DbUserId))
            {
                List<ApplicationEntry> presetAppEntries = new List<ApplicationEntry>();
                foreach (ApplicationData appData in presetData.AppDataList)
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
            GroupData groupData = Server.ServerDbHelper.GetInstance().GetGroupByUserId(model.DbUserId);
            maintenanceStatus.AllowMaintenance = groupData.allow_maintenance;

            MonitorInfo allowViewingArea = new MonitorInfo();
            if (groupData.share_full_desktop)
            {
                // same as full desktop
                allowViewingArea.LeftPos = desktopLeft;
                allowViewingArea.TopPos = desktopTop;
                allowViewingArea.RightPos = desktopRight;
                allowViewingArea.BottomPos = desktopBottom;
            }
            else
            {
                // get monitor info
                MonitorData monitorData = Server.ServerDbHelper.GetInstance().GetMonitorByGroupId(groupData.id);

                allowViewingArea.LeftPos = monitorData.Left;
                allowViewingArea.TopPos = monitorData.Top;
                allowViewingArea.RightPos = monitorData.Right;
                allowViewingArea.BottomPos = monitorData.Bottom;
            }

            // prepare the VNC list
            ServerVncStatus vncStatus = new ServerVncStatus();
            List<VncEntry> vncEntries = new List<VncEntry>();
            vncStatus.UserVncList = vncEntries;
            foreach (RemoteVncData vncData in ServerDbHelper.GetInstance().GetRemoteVncList())
            {
                vncEntries.Add(new VncEntry()
                    {
                        DisplayName = vncData.name,
                        IpAddress = vncData.remoteIp,
                        Port = vncData.remotePort,
                    });
            }

            ServerLoginReply reply = new ServerLoginReply()
            {
                LoginName = model.Name,
                UserId = model.DbUserId,
                ServerLayout = new ServerScreenInfo()
                {
                    MatrixCol = desktopCol,
                    MatrixRow = dekstopRow,
                    ServerMonitorsList = monitorList
                },
                // UserApplications
                UserApplications = serverAppStatus,

                // UserPresets
                UserPresets = serverPresetStatus,

                // UserMaintenance
                UserMaintenance = maintenanceStatus,

                // allowed viewing area
                ViewingArea = allowViewingArea,

                // Current vnc list
                VncStatus = vncStatus,
            };

            connectionMgr.SendData(
                (int)CommandConst.MainCommandServer.UserPriviledge,
                (int)CommandConst.SubCommandServer.DisplayInfo,
                reply,
                new List<string>() { model.SocketUserId });


            // send Input info to client
            Session.Data.ServerInputStatus inputStatus = new Session.Data.ServerInputStatus()
            {
                InputAttributesList = Server.ServerVisionHelper.getInstance().GetAllVisionInputsAttributes(),
            };

            connectionMgr.SendData(
                   (int)CommandConst.MainCommandServer.Presents,
                   (int)CommandConst.SubCommandServer.VisionInput,
                   inputStatus,
                   new List<string>() { model.SocketUserId });
        }

        public void Dispose()
        {
            runningWndsMgr.StopMonitor();
            Server.ConnectedClientHelper.GetInstance().RemoveAllClients();
        }
    }
}
