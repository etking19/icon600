using Session;
using Session.Connection;
using Session.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;
using WindowsFormClient.Server.Model;

namespace WindowsFormClient.Presenter
{
    public class MonitorsPresenter
    {
        private ConnectionManager connectionMgr;

        public MonitorsPresenter(ConnectionManager connectionMgr)
        {
            this.connectionMgr = connectionMgr;
        }

        public DataTable GetMonitorsTable()
        {
            // get user's info (display name, username, group name)
            DataTable table = new DataTable();
            table.Columns.Add("Monitor Id", typeof(int)).ReadOnly = true;
            table.Columns.Add("Name", typeof(string)).ReadOnly = true;
            table.Columns.Add("Area (left,top,right,bottom)", typeof(string)).ReadOnly = true;

            foreach (MonitorData data in Server.ServerDbHelper.GetInstance().GetMonitorsList())
            {
                table.Rows.Add(data.MonitorId, data.Name, String.Format("{0}, {1}, {2}, {3}", data.Left, data.Top, data.Right, data.Bottom));
            }

            return table;
        }

        public void AddMonitor(string monitorName, int left, int top, int right, int bottom)
        {
            Server.ServerDbHelper.GetInstance().AddMonitor(monitorName, left, top, right, bottom);
        }

        private List<string> getUsersSocketIdFromMonitorId(int monitorId)
        {
            List<string> usersSocketList = new List<string>();
            foreach(ClientInfoModel clientInfo in Server.ConnectedClientHelper.GetInstance().GetAllUsers())
            {
                if (Server.ServerDbHelper.GetInstance().GetMonitorDataByUserId(clientInfo.DbUserId).MonitorId == monitorId)
                {
                    usersSocketList.Add(clientInfo.SocketUserId);
                }
            }

            return usersSocketList;
        }

        private List<GroupData> getGroupsFromMonitorId(int monitorId)
        {
            List<GroupData> groupsId = new List<GroupData>();
            foreach(GroupData groupData in Server.ServerDbHelper.GetInstance().GetAllGroups())
            {
                if(Server.ServerDbHelper.GetInstance().GetMonitorByGroupId(groupData.id).MonitorId == monitorId)
                {
                    groupsId.Add(groupData);
                }
            }

            return groupsId;
        }

        public void RemoveMonitor(int monitorId)
        {
            List<string> usersList = getUsersSocketIdFromMonitorId(monitorId);
            List<GroupData> groupDataList = getGroupsFromMonitorId(monitorId);

            if(Server.ServerDbHelper.GetInstance().RemoveMonitor(monitorId))
            {
                foreach(string userSocketId in usersList)
                {
                    connectionMgr.RemoveClient(userSocketId);
                }

                foreach (GroupData groupData in groupDataList)
                {
                    Server.ServerDbHelper.GetInstance().EditGroup(groupData.id, groupData.name, true, groupData.allow_maintenance, -1, getApplicationsId(groupData.id));
                }
            }
        }

        private List<int> getApplicationsId(int groupId)
        {
            List<int> appList = new List<int>();
            foreach (ApplicationData data in Server.ServerDbHelper.GetInstance().GetAppsWithGroupId(groupId))
            {
                appList.Add(data.id);
            }

            return appList;
        }

        public void EditMonitor(int monitorId, string monitorName, int left, int top, int right, int bottom)
        {
            List<string> usersList = getUsersSocketIdFromMonitorId(monitorId);

            if (Server.ServerDbHelper.GetInstance().EditMonitor(monitorId, monitorName, left, top, right, bottom))
            {
                foreach (string userSocketId in usersList)
                {
                    //connectionMgr.RemoveClient(userSocketId);

                    ServerViewingAreaStatus viewingAreaCmd = new ServerViewingAreaStatus()
                    {
                        ViewingArea = new Session.Data.SubData.MonitorInfo()
                        {
                            LeftPos = left,
                            TopPos = top,
                            RightPos = right,
                            BottomPos = bottom
                        },
                    };
                    connectionMgr.SendData((int)CommandConst.MainCommandServer.UserPriviledge,
                    (int)CommandConst.SubCommandServer.ViewingArea,
                    viewingAreaCmd,
                    new List<string> { userSocketId }); 
                }
            }
        }
    }
}
