using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Utils.Windows;
using WcfServiceLibrary1;
using WindowsFormClient.Server.Model;

namespace WindowsFormClient.Server
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    class WcfCallbackHandler : IServiceCallback
    {
        private IServer server;

        private ConnectionManager connectionManager;
        private Dictionary<int, Dictionary<int, string>> appUsersList = new Dictionary<int, Dictionary<int, string>>();

        public WcfCallbackHandler(ConnectionManager connectionMgr, IServer server)
        {
            this.connectionManager = connectionMgr;
            this.server = server;
        }

        public void OnUserDBAdded(DBTypeEnum dbType, int dbIndex)
        {
            server.OnGridDataUpdateRequest(ServerCommandType.Added, dbType);

            switch(dbType)
            {
                case DBTypeEnum.RemoteVnc:
                    sendVncUpdateToConnectedClients();
                    break;
                case DBTypeEnum.VisionInput:
                    sendVisionInputUpdate();
                    break;
            }
        }

        public void OnUserDBEditing(DBTypeEnum dbType, int dbIndex)
        {
            switch (dbType)
            {
                case DBTypeEnum.User:
                    removeUserConnection(dbIndex);
                    break;
            }
        }

        public void OnUserDBEdited(DBTypeEnum dbType, int dbIndex)
        {
            server.OnGridDataUpdateRequest(ServerCommandType.Edited, dbType);

            switch (dbType)
            {
                case DBTypeEnum.RemoteVnc:
                    sendVncUpdateToConnectedClients();
                    break;
                case DBTypeEnum.Group:
                    onGroupEdited(dbIndex);
                    break;
                case DBTypeEnum.VisionInput:
                    sendVisionInputUpdate();
                    break;
            }
        }

        public void onUserDBRemoving(DBTypeEnum dbType, int dbIndex)
        {
            switch (dbType)
            {
                case DBTypeEnum.Application:
                    onApplicationRemoving(dbIndex);
                    break;
                case DBTypeEnum.Group:
                    onGroupRemoving(dbIndex);
                    break;
                case DBTypeEnum.User:
                    removeUserConnection(dbIndex);
                    break;
            }
        }

        public void onUserDBRemoved(DBTypeEnum dbType, int dbIndex)
        {
            server.OnGridDataUpdateRequest(ServerCommandType.Removed, dbType);

            switch (dbType)
            {
                case DBTypeEnum.RemoteVnc:
                    sendVncUpdateToConnectedClients();
                    break;
                case DBTypeEnum.Application:
                    onApplicationRemoved(dbIndex);
                    break;
                case DBTypeEnum.Monitor:
                    onMonitorEdited(dbIndex);
                    break;
                case DBTypeEnum.VisionInput:
                    sendVisionInputUpdate();
                    break;
            }
        }

        private void sendVisionInputUpdate()
        {
            Session.Data.ServerInputStatus inputStatus = new Session.Data.ServerInputStatus()
            {
                InputAttributesList = Server.ServerVisionHelper.getInstance().GetAllVisionInputsAttributes(),
            };

            connectionManager.SendData(
                   (int)CommandConst.MainCommandServer.Presents,
                   (int)CommandConst.SubCommandServer.VisionInput,
                   inputStatus,
                   ConnectedClientHelper.GetInstance().GetAllClientsSocketId());
        }

        private List<string> getSocketIdFromUserId(int dbUserId)
        {
            return Server.ConnectedClientHelper.GetInstance().GetAllUsers()
                .Where(clientInfo => clientInfo.DbUserId == dbUserId)
                .Select(t => t.SocketUserId).ToList();
        }

        private void removeUserConnection(int userId)
        {
            List<string> usersList = getSocketIdFromUserId(userId);
            foreach (string socketId in usersList)
            {
                connectionManager.RemoveClient(socketId);
            }
        }


        private List<string> getUsersSocketIdFromMonitorId(int monitorId)
        {
            List<string> usersSocketList = new List<string>();
            foreach (ClientInfoModel clientInfo in Server.ConnectedClientHelper.GetInstance().GetAllUsers())
            {
                if (Server.ServerDbHelper.GetInstance().GetMonitorDataByUserId(clientInfo.DbUserId).MonitorId == monitorId)
                {
                    usersSocketList.Add(clientInfo.SocketUserId);
                }
            }

            return usersSocketList;
        }

        private void onMonitorEdited(int monitorId)
        {
            MonitorData monitorData = ServerDbHelper.GetInstance().GetMonitorsList().First(data => data.MonitorId == monitorId);

            List<string> usersList = getUsersSocketIdFromMonitorId(monitorId);
            ServerViewingAreaStatus viewingAreaCmd = new ServerViewingAreaStatus()
            {
                ViewingArea = new Session.Data.SubData.MonitorInfo()
                {
                    LeftPos = monitorData.Left,
                    TopPos = monitorData.Top,
                    RightPos = monitorData.Right,
                    BottomPos = monitorData.Bottom
                },
            };
            connectionManager.SendData((int)CommandConst.MainCommandServer.UserPriviledge,
            (int)CommandConst.SubCommandServer.ViewingArea,
            viewingAreaCmd,
            usersList);
        }

        private void onMonitorRemoving(int monitorId)
        {
            List<string> usersList = getUsersSocketIdFromMonitorId(monitorId);
            foreach (string userSocketId in usersList)
            {
                connectionManager.RemoveClient(userSocketId);
            }
        }

        private MonitorInfo getDesktopMonitorInfo()
        {
            MonitorInfo monitor = new Session.Data.SubData.MonitorInfo();

            int desktopLeft = 0;
            int desktopTop = 0;
            int desktopRight = 0;
            int desktopBottom = 0;
            foreach (WindowsHelper.MonitorInfo info in Utils.Windows.WindowsHelper.GetMonitorList())
            {
                if (desktopLeft > info.WorkArea.Left)
                {
                    desktopLeft = info.MonitorArea.Left;
                }

                if (desktopTop > info.WorkArea.Top)
                {
                    desktopTop = info.MonitorArea.Top;
                }

                if (desktopRight < info.WorkArea.Right)
                {
                    desktopRight = info.MonitorArea.Right;
                }

                if (desktopBottom < info.WorkArea.Bottom)
                {
                    desktopBottom = info.MonitorArea.Bottom;
                }
            }

            monitor.LeftPos = desktopLeft;
            monitor.TopPos = desktopTop;
            monitor.RightPos = desktopRight;
            monitor.BottomPos = desktopBottom;

            return monitor;
        }

        private MonitorInfo getUserMonitorInfo(int groupId)
        {
            MonitorInfo monitor = new Session.Data.SubData.MonitorInfo();

            MonitorData monitorData = Server.ServerDbHelper.GetInstance().GetMonitorByGroupId(groupId);
            monitor.LeftPos = monitorData.Left;
            monitor.TopPos = monitorData.Top;
            monitor.RightPos = monitorData.Right;
            monitor.BottomPos = monitorData.Bottom;

            return monitor;
        }

        private void onGroupEdited(int groupId)
        {
            GroupData latestData = Server.ServerDbHelper.GetInstance().GetAllGroups().First(groupData => groupData.id == groupId);

            ServerViewingAreaStatus viewingArea = new ServerViewingAreaStatus();
            MonitorInfo monitorInfo = null;
            if (latestData.share_full_desktop)
            {
                monitorInfo = getDesktopMonitorInfo();
            }
            else
            {
                monitorInfo = getUserMonitorInfo(groupId);
            }

            viewingArea.ViewingArea = monitorInfo;


            ServerMaintenanceStatus maintenanceCmd = new ServerMaintenanceStatus();
            maintenanceCmd.AllowMaintenance = latestData.allow_maintenance;


            ServerApplicationStatus appCmd = new ServerApplicationStatus();
            List<ApplicationEntry> applicationList = new List<ApplicationEntry>();
            foreach (ApplicationData appData in Server.ServerDbHelper.GetInstance().GetAppsWithGroupId(groupId))
            {
                applicationList.Add(new ApplicationEntry()
                {
                    Identifier = appData.id,
                    Name = appData.name
                });
            }
            appCmd.UserApplicationList = applicationList;

            // send update to all users
            List<string> usersList = getSocketIdentifierFromGroupId(groupId);
            connectionManager.SendData(
                (int)CommandConst.MainCommandServer.UserPriviledge,
                (int)CommandConst.SubCommandServer.ViewingArea,
                viewingArea,
                usersList);

            connectionManager.SendData(
                (int)CommandConst.MainCommandServer.UserPriviledge,
                (int)CommandConst.SubCommandServer.Maintenance,
                maintenanceCmd,
                usersList);

            connectionManager.SendData(
                (int)CommandConst.MainCommandServer.UserPriviledge,
                (int)CommandConst.SubCommandServer.ApplicationList,
                appCmd,
                usersList);
        }

        private void onGroupRemoving(int groupId)
        {
            List<string> usersList = getSocketIdentifierFromGroupId(groupId);
            foreach (string socketId in usersList)
            {
                connectionManager.RemoveClient(socketId);
            }
        }

        private List<string> getSocketIdentifierFromGroupId(int groupId)
        {
            // get any connected clients with this group id
            List<UserData> userDataList = Server.ServerDbHelper.GetInstance().GetUsersInGroup(groupId);

            List<string> disconnectionUserIdList = new List<string>();
            foreach (UserData dbUserData in userDataList)
            {
                var disconnectList = Server.ConnectedClientHelper.GetInstance().GetAllUsers().Where(clientInfo => clientInfo.DbUserId == dbUserData.id);
                foreach (ClientInfoModel model in disconnectList)
                {
                    disconnectionUserIdList.Add(model.SocketUserId);
                }
            }

            return disconnectionUserIdList;
        }

        private void onApplicationRemoving(int appId)
        {
            Dictionary<int, string> userList = getUsersSocketListByAppId(appId);
            appUsersList.Add(appId, userList);
        }

        private void onApplicationRemoved(int appId)
        {
            Dictionary<int, string> userList;
            appUsersList.TryGetValue(appId, out userList);

            if (userList == null)
            {
                Trace.WriteLine("Application update failed");
                return;
            }
            appUsersList.Remove(appId);

            foreach (KeyValuePair<int, string> dbSocketPair in userList)
            {
                List<ApplicationEntry> appsEntries = new List<ApplicationEntry>();
                List<ApplicationData> appDataList = Server.ServerDbHelper.GetInstance().GetAppsWithUserId(dbSocketPair.Key);
                foreach (ApplicationData data in appDataList)
                {
                    appsEntries.Add(new ApplicationEntry()
                    {
                        Identifier = data.id,
                        Name = data.name
                    });
                }

                // notify new application list
                ServerApplicationStatus appStatus = new ServerApplicationStatus()
                {
                    UserApplicationList = appsEntries,
                };

                connectionManager.SendData(
                    (int)CommandConst.MainCommandServer.UserPriviledge,
                    (int)CommandConst.SubCommandServer.ApplicationList,
                    appStatus,
                    new List<string>() { dbSocketPair.Value });
            }
        }

        private void sendVncUpdateToConnectedClients()
        {
            // notify all connected clients
            ServerVncStatus vncStatus = new ServerVncStatus();
            List<VncEntry> vncEntries = new List<VncEntry>();
            vncStatus.UserVncList = vncEntries;
            foreach (RemoteVncData vncData in Server.ServerDbHelper.GetInstance().GetRemoteVncList())
            {
                vncEntries.Add(new VncEntry()
                {
                    Identifier = vncData.id,
                    DisplayName = vncData.name,
                    IpAddress = vncData.remoteIp,
                    Port = vncData.remotePort,
                });
            }

            connectionManager.SendData(
                (int)CommandConst.MainCommandServer.UserPriviledge,
                (int)CommandConst.SubCommandServer.VncList,
                vncStatus,
                Server.ConnectedClientHelper.GetInstance().GetAllClientsSocketId());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <returns> int: userDBid, string: userSocketId</returns>
        private Dictionary<int, string> getUsersSocketListByAppId(int appId)
        {
            Dictionary<int, string> usersSocketList = new Dictionary<int, string>();
            foreach (ClientInfoModel clientInfo in Server.ConnectedClientHelper.GetInstance().GetAllUsers())
            {
                if (Server.ServerDbHelper.GetInstance().GetAppsWithUserId(clientInfo.DbUserId).Where(t => t.id == appId).Count() > 0)
                {
                    usersSocketList.Add(clientInfo.DbUserId, clientInfo.SocketUserId);
                }
            }

            return usersSocketList;
        }


        
    }
}
