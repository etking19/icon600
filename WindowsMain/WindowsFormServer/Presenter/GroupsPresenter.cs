using System.Collections.Generic;
using System.Data;
using System.Linq;
using WcfServiceLibrary1;

namespace WindowsFormClient.Presenter
{
    public class GroupsPresenter
    {
        public GroupsPresenter()
        {
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
            Server.ServerDbHelper.GetInstance().RemoveGroup(groupId);
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
            if (shareDesktop)
            {
                monitorId = -1;
            }

            Server.ServerDbHelper.GetInstance().EditGroup(groupId, groupName, shareDesktop, allowMaintenance, monitorId, appIds);
        }   
    }
}
