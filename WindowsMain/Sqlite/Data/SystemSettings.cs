using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class SystemSettings : ISqlData
    {
        public const string TABLE_NAME = "system_settings";

        public const string ID = "system_settings_id";
        public const string INPUT = "input_count";

        public int InputCount { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY, {2} INTEGER NOT NULL DEFAULT 0)";
            return String.Format(query, TABLE_NAME, ID, INPUT);
        }

        public string GetAddCommand()
        {
            string query = "INSERT OR REPLACE INTO {0} ({1}, {2}) VALUES ({3}, {4})";
            return String.Format(query, TABLE_NAME, ID, INPUT, 0, InputCount);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0}";
            return String.Format(query, TABLE_NAME);
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
            return GetAddCommand();
        }
    }
}
