﻿using System.Collections.Generic;
using System.Data;
using WcfServiceLibrary1;

namespace WindowsFormClient.Presenter
{
    public class UsersPresenter
    {
        public UsersPresenter()
        {
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
            

            foreach(UserData data in Server.ServerDbHelper.GetInstance().GetAllUsers())
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

        public void RemoveUser(int userId)
        {
            Server.ServerDbHelper.GetInstance().RemoveUser(userId);
        }

        public void EditUser(int userId, string displayName, string userName, string password, int groupId)
        {
            Server.ServerDbHelper.GetInstance().EditUser(userId, displayName, userName, password, groupId);
        }

        public Dictionary<int, string> GetGroupsList()
        {
            Dictionary<int, string> dicGroup = new Dictionary<int, string>();
            foreach (GroupData data in Server.ServerDbHelper.GetInstance().GetAllGroups())
            {
                dicGroup.Add(data.id, data.name);
            }

            return dicGroup;
        }
    }
}
