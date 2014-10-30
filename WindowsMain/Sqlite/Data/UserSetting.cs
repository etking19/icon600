using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class UserSetting : ISqlData
    {
        public const string TABLE_NAME = "user_settings";

        public const string USER_SETTING_ID = "user_settings_id";
        public const string USER_ID = "user_table_id";
        public const string GRID_X = "grid_x";
        public const string GRID_Y = "grid_y";
        public const string APPLY_SNAP = "is_snap";

        public int id { get; set; }
        public int userId { get; set; }
        public int gridX { get; set; }
        public int gridY { get; set; }
        public bool isSnap { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY AUTOINCREMENT, {5} INTEGER NOT NULL DEFAULT 0, {6} INTEGER NOT NULL DEFAULT 0, {7} INTEGER NOT NULL DEFAULT 0, {2} INTEGER, FOREIGN KEY({2}) REFERENCES {3}({4}) ON DELETE CASCADE)";
            return String.Format(query, TABLE_NAME, USER_SETTING_ID, USER_ID, User.TABLE_NAME, User.USER_ID, GRID_X, GRID_Y, APPLY_SNAP);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {3}, {4}) VALUES ({5}, {6}, {7}, {8})";
            return String.Format(query, TABLE_NAME,
                USER_ID, GRID_X, GRID_Y, APPLY_SNAP,
                userId, gridX, gridY, isSnap? 1:0);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1}={2}";
            return String.Format(query, TABLE_NAME, USER_SETTING_ID, id);
        }

        public string GetTableName()
        {
            return TABLE_NAME;
        }

        public string GetQueryCommand()
        {
            string query = "SELECT * FROM {0} WHERE {1}={2}";
            return String.Format(query, TABLE_NAME, USER_ID, userId);
        }

        public string GetUpdateDataCommand()
        {
            string query = "UPDATE {0} SET {1}={2}, {3}={4}, {5}={6} WHERE {7}={8};";
            return String.Format(query, TABLE_NAME,
                GRID_X, gridX,
                GRID_Y, gridY,
                APPLY_SNAP, isSnap ? 1 : 0,
                USER_ID, userId);
        }
    }
}
