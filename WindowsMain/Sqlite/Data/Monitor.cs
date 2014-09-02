using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class Monitor : ISqlData
    {
        public const string TABLE_NAME = "monitor_table";

        public const string MONITOR_ID = "monitor_id";
        public const string NAME = "label";

        public const string SHARE_LEFT = "monitor_left";
        public const string SHARE_TOP = "monitor_top";
        public const string SHARE_RIGHT = "monitor_right";
        public const string SHARE_BOTTOM = "monitor_bottom";

        public int id { get; set; }
        public string label { get; set; }
        public int monitor_left { get; set; }
        public int monitor_top { get; set; }
        public int monitor_right { get; set; }
        public int monitor_bottom { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY AUTOINCREMENT, {2} VARCHAR(100) UNIQUE NOT NULL, {3} INTEGER NOT NULL DEFAULT 0, {4} INTEGER NOT NULL DEFAULT 0, {5} INTEGER NOT NULL DEFAULT 0, {6} INTEGER NOT NULL DEFAULT 0)";
            return String.Format(query, 
                TABLE_NAME,
                MONITOR_ID, NAME, SHARE_LEFT, SHARE_TOP, SHARE_RIGHT, SHARE_BOTTOM);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}) VALUES ('{6}', {7}, {8}, {9}, {10})";
            return String.Format(query, TABLE_NAME,
                NAME, SHARE_LEFT, SHARE_TOP, SHARE_RIGHT, SHARE_BOTTOM,
                label, monitor_left, monitor_top, monitor_right, monitor_bottom);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1} = {2}";
            return String.Format(query, TABLE_NAME, MONITOR_ID, id);
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
            string query = "UPDATE {0} SET {1}='{2}', {3}={4}, {5}={6}, {7}={8}, {9}={10} WHERE {11}={12}";
            return String.Format(query, TABLE_NAME,
                NAME, label,
                SHARE_LEFT, monitor_left,
                SHARE_TOP, monitor_top,
                SHARE_RIGHT, monitor_right,
                SHARE_BOTTOM, monitor_bottom,
                MONITOR_ID, id);
        }
    }
}
