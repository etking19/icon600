using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class PresetApplications : ISqlData
    {
        public const string TABLE_NAME = "preset_application";

        public const string PRESET_APPLICATION_ID = "preset_application_id";
        public const string PRESET_NAME_ID = "preset_name_id";
        public const string APPLICATION_ID = "application_id";
        public const string APPLICATION_LATEST_LEFT = "application_left";
        public const string APPLICATION_LATEST_TOP = "application_top";
        public const string APPLICATION_LATEST_RIGHT = "application_right";
        public const string APPLICATION_LATEST_BOTTOM = "application_bottom";


        public int preset_name_id { get; set; }
        public int preset_application_id { get; set; }

        public int app_latest_pos_left { get; set; }
        public int app_latest_pos_top { get; set; }
        public int app_latest_pos_right { get; set; }
        public int app_latest_pos_bottom { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} REFERENCES {2}({3}) ON DELETE CASCADE, {4} REFERENCES {5}({6}) ON DELETE CASCADE, {7} INTEGER PRIMARY KEY AUTOINCREMENT, {8} INTEGER NOT NULL DEFAULT 0, {9} INTEGER NOT NULL DEFAULT 0, {10} INTEGER NOT NULL DEFAULT 0, {11} INTEGER NOT NULL DEFAULT 0)";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME_ID, PresetName.TABLE_NAME, PresetName.PRESET_ID,
                APPLICATION_ID, Application.TABLE_NAME, Application.APPLICATION_ID,
                PRESET_APPLICATION_ID,
                APPLICATION_LATEST_LEFT, APPLICATION_LATEST_TOP, APPLICATION_LATEST_RIGHT, APPLICATION_LATEST_BOTTOM);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {5}, {6}, {7}, {8}) VALUES ({3}, {4}, {9}, {10}, {11}, {12})";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME_ID, APPLICATION_ID,
                preset_name_id, preset_application_id,
                APPLICATION_LATEST_LEFT, APPLICATION_LATEST_TOP, APPLICATION_LATEST_RIGHT, APPLICATION_LATEST_BOTTOM,
                app_latest_pos_left, app_latest_pos_top, app_latest_pos_right, app_latest_pos_bottom);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1}={2}";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME_ID, preset_name_id);
        }

        public string GetTableName()
        {
            return TABLE_NAME;
        }

        /// <summary>
        /// get the data for giving preset name table id
        /// </summary>
        /// <returns></returns>
        public string GetQueryCommand()
        {
            string query = "SELECT * FROM {0} where {1}={2}";
            return String.Format(query, TABLE_NAME, PRESET_NAME_ID, preset_name_id);
        }

        public string GetUpdateDataCommand()
        {
            throw new NotImplementedException();
        }
    }
}
