using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WindowsFormClient.Server.Model;

namespace WindowsFormClient.Presenter
{
    class ApplicationsPresenter
    {
        private ConnectionManager connectionMgr;

        public ApplicationsPresenter(ConnectionManager connectionMgr)
        {
            this.connectionMgr = connectionMgr;
        }

        public DataTable GetApplicationTable()
        {
            // get user's info (display name, username, group name)
            DataTable table = new DataTable();
            table.Columns.Add("Application Id", typeof(int)).ReadOnly = true;
            table.Columns.Add("Name", typeof(string)).ReadOnly = true;
            table.Columns.Add("Executable Path", typeof(string)).ReadOnly = true;
            table.Columns.Add("Arguments", typeof(string)).ReadOnly = true;
            table.Columns.Add("Display Area", typeof(string)).ReadOnly = true;

            foreach (WindowsFormClient.Server.ServerDbHelper.ApplicationData data in Server.ServerDbHelper.GetInstance().GetAllApplications())
            {
                table.Rows.Add(data.id, data.name, data.applicationPath, data.arguments, String.Format("{0}, {1}, {2}, {3}", data.rect.Left, data.rect.Top, data.rect.Right, data.rect.Bottom));
            }

            return table;
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

        public void AddApplication(string appName, string exePath, string arguments, int left, int top, int right, int bottom)
        {
            Server.ServerDbHelper.GetInstance().AddApplication(appName, arguments, exePath, left, top, right, bottom);
        }

        public void RemoveApplication(int appId)
        {
            Dictionary<int, string> userList = getUsersSocketListByAppId(appId);
            if(Server.ServerDbHelper.GetInstance().RemoveApplication(appId))
            {
                foreach (KeyValuePair<int, string> dbSocketPair in userList)
                {
                    List<ApplicationEntry> appsEntries = new List<ApplicationEntry>();
                    List<WindowsFormClient.Server.ServerDbHelper.ApplicationData> appDataList = Server.ServerDbHelper.GetInstance().GetAppsWithUserId(dbSocketPair.Key);
                    foreach (WindowsFormClient.Server.ServerDbHelper.ApplicationData data in appDataList)
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

                    connectionMgr.SendData(
                    (int)CommandConst.MainCommandServer.UserPriviledge,
                    (int)CommandConst.SubCommandServer.ApplicationList,
                    appStatus,
                    new List<string>() { dbSocketPair.Value });
                }
            }
        }

        public void EditApplication(int appId, string appName, string exePath, string arguments, int left, int top, int right, int bottom)
        {
            Server.ServerDbHelper.GetInstance().EditApplication(appId, appName, arguments, exePath, left, top, right, bottom);
        }
    }
}
