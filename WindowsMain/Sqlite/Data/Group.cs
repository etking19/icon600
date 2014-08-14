using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class Group : ISqlData
    {
        public const string TABLE_NAME = "groups";

        public const string GROUP_ID = "group_id";
        public const string NAME = "label";
        public const string SHARE_FULL = "share_full";

        public const string MAINTENANCE = "server_maintenance";

        public int id { get; set; }
        public string label { get; set; }
        public bool share_full_desktop { get; set; }
        public bool allow_maintenance { get; set; }


        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY AUTOINCREMENT, {2} VARCHAR(100) UNIQUE NOT NULL, {3} INTEGER NOT NULL DEFAULT 0, {4} INTEGER NOT NULL DEFAULT 0)";
            return String.Format(query, TABLE_NAME, GROUP_ID, NAME, SHARE_FULL, MAINTENANCE);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {3}) VALUES ('{4}', {5}, {6})";
            return String.Format(query, TABLE_NAME,
                NAME, SHARE_FULL, MAINTENANCE,
                label, Convert.ToInt32(share_full_desktop), Convert.ToInt32(allow_maintenance));
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1} = {2}";
            return String.Format(query, TABLE_NAME, GROUP_ID, id);
        }

        public string GetTableName()
        {
            return TABLE_NAME;
        }

        public string GetQueryCommand()
        {
            string query = "SELECT * FROM {0}";
            return String.Format(query, TABLE_NAME);
        }

        public string GetUpdateDataCommand()
        {
            string query = "UPDATE {0} SET {1}='{2}', {3}={4}, {5}={6} WHERE {7}={8};";
            return String.Format(query, TABLE_NAME, 
                NAME, label,
                SHARE_FULL, Convert.ToInt32(share_full_desktop),
                MAINTENANCE, Convert.ToInt32(allow_maintenance),
                GROUP_ID, id);
        }
    }
}
