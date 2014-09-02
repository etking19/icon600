using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class GroupMonitor : ISqlData
    {
        public const string TABLE_NAME = "group_monitor";

        public const string GROUP_MONITOR_ID = "group_monitor_id";
        public const string GROUP_ID = "group_id";
        public const string MONITOR_ID = "monitor_id";

        public int group_monitor_id { get; set; }
        public int monitor_id { get; set; }
        public int group_id { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} REFERENCES {2}({3}) ON DELETE CASCADE, {4} REFERENCES {5}({6}) ON DELETE CASCADE, {7} INTEGER PRIMARY KEY AUTOINCREMENT)";
            return String.Format(query, TABLE_NAME,
                GROUP_ID, Group.TABLE_NAME, Group.GROUP_ID,
                MONITOR_ID, Monitor.TABLE_NAME, Monitor.MONITOR_ID,
                GROUP_MONITOR_ID);    
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}) VALUES ({3}, {4})";
            return String.Format(query,
                TABLE_NAME,
                GROUP_ID,
                MONITOR_ID,
                group_id,
                monitor_id);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1}={2}";
            return String.Format(query, TABLE_NAME, GROUP_ID, group_id);
        }

        public string GetTableName()
        {
            return TABLE_NAME;
        }

        public string GetQueryCommand()
        {
            string query = "SELECT * FROM {0} WHERE {1}={2}";
            return String.Format(query, TABLE_NAME, GROUP_ID, group_id);
        }

        public string GetUpdateDataCommand()
        {
            throw new NotImplementedException();
        }
    }
}
