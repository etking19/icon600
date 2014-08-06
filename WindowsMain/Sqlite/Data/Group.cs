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

        public const string SHARE_LEFT = "monitor_left";
        public const string SHARE_TOP = "monitor_top";
        public const string SHARE_RIGHT = "monitor_right";
        public const string SHARE_BOTTOM = "monitor_bottom";

        public const string MAINTENANCE = "server_maintenance";

        public int id { get; set; }
        public string label { get; set; }
        public bool share_full_desktop { get; set; }
        public int monitor_left { get; set; }
        public int monitor_top { get; set; }
        public int monitor_right { get; set; }
        public int monitor_bottom { get; set; }
        public bool allow_maintenance { get; set; }


        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY AUTOINCREMENT, {2} VARCHAR(100) UNIQUE NOT NULL, {3} INTEGER NOT NULL DEFAULT 0, {4} INTEGER NOT NULL DEFAULT 0, {5} INTEGER NOT NULL DEFAULT 0, {6} INTEGER NOT NULL DEFAULT 0, {7} INTEGER NOT NULL DEFAULT 0, {8} INTEGER NOT NULL DEFAULT 0)";
            return String.Format(query, TABLE_NAME, GROUP_ID, NAME, SHARE_FULL, SHARE_LEFT, SHARE_TOP, SHARE_RIGHT, SHARE_BOTTOM, MAINTENANCE);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}) VALUES ('{8}', {9}, {10}, {11}, {12}, {13}, {14})";
            return String.Format(query, TABLE_NAME,
                NAME, SHARE_FULL, SHARE_LEFT, SHARE_TOP, SHARE_RIGHT, SHARE_BOTTOM, MAINTENANCE,
                label, Convert.ToInt32(share_full_desktop), monitor_left, monitor_top, monitor_right, monitor_bottom, Convert.ToInt32(allow_maintenance));
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
            string query = "UPDATE {0} SET {1}='{2}', {3}={4}, {5}={6}, {7}={8}, {9}={10}, {11}={12}, {13}={14} WHERE {15}={16};";
            return String.Format(query, TABLE_NAME, 
                NAME, label,
                SHARE_FULL, Convert.ToInt32(share_full_desktop),
                SHARE_LEFT, monitor_left,
                SHARE_TOP, monitor_top,
                SHARE_RIGHT, monitor_right,
                SHARE_BOTTOM, monitor_bottom,
                MAINTENANCE, Convert.ToInt32(allow_maintenance),
                GROUP_ID, id);
        }
    }
}
