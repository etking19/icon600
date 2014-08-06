using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class PresetName : ISqlData
    {
        public const string TABLE_NAME = "presets_name";

        public const string PRESET_ID = "preset_name_id";
        public const string PRESET_NAME = "preset_name";
        public const string USER_ID = "user_id";

        public int preset_id { get; set; }
        public string preset_name { get; set; }
        public int user_id { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY AUTOINCREMENT, {2} VARCHAR(100) NOT NULL, {3} REFERENCES {4}({5}) ON DELETE CASCADE)";
            return String.Format(query, TABLE_NAME,
                PRESET_ID,
                PRESET_NAME,
                USER_ID, User.TABLE_NAME, User.USER_ID);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}) VALUES ('{3}', {4})";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME, USER_ID,
                preset_name, user_id);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1}={2}";
            return String.Format(query, TABLE_NAME, PRESET_ID, preset_id);
        }

        public string GetTableName()
        {
            return TABLE_NAME;
        }

        public string GetQueryCommand()
        {
            string query = "SELECT * FROM {0} where {1}={2}";
            return String.Format(query, TABLE_NAME, USER_ID, user_id);
        }

        /// <summary>
        /// update name or owner of the preset
        /// </summary>
        /// <returns></returns>
        public string GetUpdateDataCommand()
        {
            string query = "UPDATE {0} SET {1}='{2}', {3}={4} WHERE {5}={6};";
            return String.Format(query, TABLE_NAME,
               PRESET_NAME, preset_name,
               USER_ID, user_id,
               PRESET_ID, preset_id);
        }
    }
}
