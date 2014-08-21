using Session.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Presenter
{
    class UsersPresenter
    {
        private ConnectionManager connectionMgr;

        public UsersPresenter(ConnectionManager connectionMgr)
        {
            this.connectionMgr = connectionMgr;
        }

        public DataTable GetUsersTable()
        {
            // get user's info (display name, username, group name)
            DataTable table = new DataTable();
            table.Columns.Add("User Id", typeof(int)).ReadOnly = true;
            table.Columns.Add("Display Name", typeof(string)).ReadOnly = true;
            table.Columns.Add("Username", typeof(string)).ReadOnly = true;
            table.Columns.Add("Password", typeof(string)).ReadOnly = true;
            table.Columns.Add("Group Name", typeof(string)).ReadOnly = true;
            

            foreach(WindowsFormClient.Server.ServerDbHelper.UserData data in Server.ServerDbHelper.GetInstance().GetAllUsers())
            {
                string groupName = Server.ServerDbHelper.GetInstance().GetGroupByUserId(data.id).name;
                table.Rows.Add(data.id, data.name, data.username, data.password, groupName);
            }

            return table;
        }

        public void AddUser(string displayName, string userName, string password, int groupId)
        {
            Server.ServerDbHelper.GetInstance().AddUser(displayName, userName, password, groupId);
        }

        private List<string> getSocketIdFromUserId(int dbUserId)
        {
            return Server.ConnectedClientHelper.GetInstance().GetAllUsers()
                .Where(clientInfo => clientInfo.DbUserId == dbUserId)
                .Select(t => t.SocketUserId).ToList();
        }

        public void RemoveUser(int userId)
        {
            List<string> usersList = getSocketIdFromUserId(userId);
            if(Server.ServerDbHelper.GetInstance().RemoveUser(userId))
            {
                foreach (string socketId in usersList)
                {
                    connectionMgr.RemoveClient(socketId);
                }
            }
        }

        public void EditUser(int userId, string displayName, string userName, string password, int groupId)
        {
            List<string> usersList = getSocketIdFromUserId(userId);
            if (Server.ServerDbHelper.GetInstance().EditUser(userId, displayName, userName, password, groupId))
            {
                foreach (string socketId in usersList)
                {
                    connectionMgr.RemoveClient(socketId);
                }
            }
        }

        public Dictionary<int, string> GetGroupsList()
        {
            Dictionary<int, string> dicGroup = new Dictionary<int, string>();
            foreach (WindowsFormClient.Server.ServerDbHelper.GroupData data in Server.ServerDbHelper.GetInstance().GetAllGroups())
            {
                dicGroup.Add(data.id, data.name);
            }

            return dicGroup;
        }
    }
}
