using Session.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

            foreach (WindowsFormClient.Server.ServerDbHelper.GroupData data in Server.ServerDbHelper.GetInstance().GetAllGroups())
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
            List<WindowsFormClient.Server.ServerDbHelper.UserData> userDataList = Server.ServerDbHelper.GetInstance().GetUsersInGroup(groupId);

            List<string> disconnectionUserIdList = new List<string>();
            foreach (WindowsFormClient.Server.ServerDbHelper.UserData dbUserData in userDataList)
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

        public List<int> GetApplicationsId(int groupId)
        {
            List<int> appList = new List<int>();
            foreach(WindowsFormClient.Server.ServerDbHelper.ApplicationData data in Server.ServerDbHelper.GetInstance().GetAppsWithGroupId(groupId))
            {
                appList.Add(data.id);
            }

            return appList;
        }

        public Dictionary<int, string> GetApplicationList()
        {
            Dictionary<int, string> dicApp = new Dictionary<int, string>();

            foreach(WindowsFormClient.Server.ServerDbHelper.ApplicationData data in Server.ServerDbHelper.GetInstance().GetAllApplications())
            {
                dicApp.Add(data.id, data.name);
            }

            return dicApp;
        }

        public Dictionary<int, string> GetMonitorsList()
        {
            Dictionary<int, string> dicMonitors = new Dictionary<int, string>();
            foreach (WindowsFormClient.Server.ServerDbHelper.MonitorData data in Server.ServerDbHelper.GetInstance().GetMonitorsList())
            {
                dicMonitors.Add(data.MonitorId, data.Name);
            }

            return dicMonitors;
        }

        public void EditGroup(int groupId, string groupName, bool shareDesktop, bool allowMaintenance, int monitorId, List<int> appIds)
        {
            List<string> usersList = getSocketIdentifierFromGroupId(groupId);
            if (Server.ServerDbHelper.GetInstance().EditGroup(groupId, groupName, shareDesktop, allowMaintenance, monitorId, appIds))
            {
                // disconnect the affect user's that connected to server
                foreach (string socketId in usersList)
                {
                    connectionMgr.RemoveClient(socketId);
                }
            }
        }
    }
}
