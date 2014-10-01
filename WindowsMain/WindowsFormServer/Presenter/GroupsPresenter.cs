using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Utils.Windows;
using WcfServiceLibrary1;
using WindowsFormClient.Server.Model;

namespace WindowsFormClient.Presenter
{
    class GroupsPresenter
    {
        private ConnectionManager connectionMgr;

        public GroupsPresenter(ConnectionManager connectionMgr)
        {
            this.connectionMgr = connectionMgr;
        }
        
        public DataTable GetGroupsTable()
        {
            // get user's info (display name, username, group name)
            DataTable table = new DataTable();
            table.Columns.Add("Group Id", typeof(int)).ReadOnly = true;
            table.Columns.Add("Group Name", typeof(string)).ReadOnly = true;
            table.Columns.Add("View Full Desktop", typeof(bool)).ReadOnly = true;
            table.Columns.Add("Allow Maintenance", typeof(bool)).ReadOnly = true;
            table.Columns.Add("Users Count", typeof(int)).ReadOnly = true;

            foreach (GroupData data in Server.ServerDbHelper.GetInstance().GetAllGroups())
            {
                int numberOfUsers = Server.ServerDbHelper.GetInstance().GetUsersInGroup(data.id).Count();
                table.Rows.Add(data.id, data.name, data.share_full_desktop, data.allow_maintenance, numberOfUsers);
            }

            return table;
        }

        public void AddGroup(string groupName, bool shareDesktop, bool allowMaintenance, int monitorId, List<int> appIds)
        {
            Server.ServerDbHelper.GetInstance().AddGroup(groupName, shareDesktop, allowMaintenance, monitorId, appIds);
        }

        public void RemoveGroup(int groupId)
        {
            // modify database
            List<string> usersList = getSocketIdentifierFromGroupId(groupId);
            if(Server.ServerDbHelper.GetInstance().RemoveGroup(groupId))
            {
                // disconnect the affect user's that connected to server
                foreach (string socketId in usersList)
                {
                    connectionMgr.RemoveClient(socketId);
                }
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

        public int GetMonitorId(int groupId)
        {
            return Server.ServerDbHelper.GetInstance().GetMonitorByGroupId(groupId).MonitorId;
        }

        /// <summary>
        /// get applications id allowed by giving group id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<int> GetApplicationsId(int groupId)
        {
            List<int> appList = new List<int>();
            foreach(ApplicationData data in Server.ServerDbHelper.GetInstance().GetAppsWithGroupId(groupId))
            {
                appList.Add(data.id);
            }

            return appList;
        }

        public Dictionary<int, string> GetApplicationList()
        {
            Dictionary<int, string> dicApp = new Dictionary<int, string>();

            foreach(ApplicationData data in Server.ServerDbHelper.GetInstance().GetAllApplications())
            {
                dicApp.Add(data.id, data.name);
            }

            return dicApp;
        }

        public Dictionary<int, string> GetMonitorsList()
        {
            Dictionary<int, string> dicMonitors = new Dictionary<int, string>();
            foreach (MonitorData data in Server.ServerDbHelper.GetInstance().GetMonitorsList())
            {
                dicMonitors.Add(data.MonitorId, data.Name);
            }

            return dicMonitors;
        }

        public void EditGroup(int groupId, string groupName, bool shareDesktop, bool allowMaintenance, int monitorId, List<int> appIds)
        {
            // share desktop will ignore monitor id
            if(shareDesktop)
            {
                monitorId = -1;
            }

            // get the current setting of the group
            GroupData oldData = Server.ServerDbHelper.GetInstance().GetAllGroups().First(groupData => groupData.id == groupId);
            int oldMonitorId = GetMonitorId(groupId);
            List<int> oldAppIdList = GetApplicationsId(groupId);

            // Three possible changes here:
            // 1. viewing area changed
            // 2. maintenance mode changed
            // 3. user application list changed
            bool viewAreaChanged = false;
            bool maintenanceModeChanged = false;
            bool applicationListChanged = false;

            if(oldData.share_full_desktop != shareDesktop ||
                oldMonitorId != monitorId)
            {
                viewAreaChanged = true;
            }

            if(oldData.allow_maintenance != allowMaintenance)
            {
                maintenanceModeChanged = true;
            }

            if(appIds.Except(oldAppIdList).Count() != 0 ||
                oldAppIdList.Except(appIds).Count() != 0)
            {
                applicationListChanged = true;
            }

            List<string> usersList = getSocketIdentifierFromGroupId(groupId);
            if (Server.ServerDbHelper.GetInstance().EditGroup(groupId, groupName, shareDesktop, allowMaintenance, monitorId, appIds))
            {
                ServerViewingAreaStatus viewingArea = new ServerViewingAreaStatus();
                if(viewAreaChanged)
                {
                    // get current monitor area
                    MonitorInfo monitorInfo = null;
                    if (shareDesktop)
                    {
                        monitorInfo = getDesktopMonitorInfo();
                    }
                    else
                    {
                        monitorInfo = getUserMonitorInfo(groupId);
                    }

                    viewingArea.ViewingArea = monitorInfo;
                }

                ServerMaintenanceStatus maintenanceCmd = new ServerMaintenanceStatus();
                if(maintenanceModeChanged)
                {
                    maintenanceCmd.AllowMaintenance = allowMaintenance;
                }

                ServerApplicationStatus appCmd = new ServerApplicationStatus();
                if(applicationListChanged)
                {
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
                }

                foreach (string socketId in usersList)
                {
                   // connectionMgr.RemoveClient(socketId);

                    if (viewAreaChanged)
                    {
                        connectionMgr.SendData(
                            (int)CommandConst.MainCommandServer.UserPriviledge,
                            (int)CommandConst.SubCommandServer.ViewingArea,
                            viewingArea,
                            new List<string>() { socketId});
                    }

                    if (maintenanceModeChanged)
                    {
                        connectionMgr.SendData(
                            (int)CommandConst.MainCommandServer.UserPriviledge,
                            (int)CommandConst.SubCommandServer.Maintenance,
                            maintenanceCmd,
                            new List<string>() { socketId });
                    }

                    if (applicationListChanged)
                    {
                        connectionMgr.SendData(
                            (int)CommandConst.MainCommandServer.UserPriviledge,
                            (int)CommandConst.SubCommandServer.ApplicationList,
                            appCmd,
                            new List<string>() { socketId });
                    }

                }
            }
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
    }
}
