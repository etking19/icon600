using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class PresetVisionInput : ISqlData
    {
        public const string TABLE_NAME = "preset_vision";

        public const string PRESET_VISION_ID = "preset_vision_id";
        public const string PRESET_NAME_ID = "preset_name_id";
        public const string VISION_ID = "vision_id";
        public const string VISION_LATEST_LEFT = "vision_left";
        public const string VISION_LATEST_TOP = "vision_top";
        public const string VISION_LATEST_RIGHT = "vision_right";
        public const string VISION_LATEST_BOTTOM = "vision_bottom";

        public int preset_name_id { get; set; }
        public int preset_vision_id { get; set; }
        public int vision_latest_pos_left { get; set; }
        public int vision_latest_pos_top { get; set; }
        public int vision_latest_pos_right { get; set; }
        public int vision_latest_pos_bottom { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} REFERENCES {2}({3}) ON DELETE CASCADE, {4} REFERENCES {5}({6}) ON DELETE CASCADE, {7} INTEGER PRIMARY KEY AUTOINCREMENT, {8} INTEGER NOT NULL DEFAULT 0, {9} INTEGER NOT NULL DEFAULT 0, {10} INTEGER NOT NULL DEFAULT 0, {11} INTEGER NOT NULL DEFAULT 0)";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME_ID, PresetName.TABLE_NAME, PresetName.PRESET_ID,
                VISION_ID, VisionInput.TABLE_NAME, VisionInput.VISION_TABLE_ID,
                PRESET_VISION_ID,
                VISION_LATEST_LEFT, VISION_LATEST_TOP, VISION_LATEST_RIGHT, VISION_LATEST_BOTTOM);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {5}, {6}, {7}, {8}) VALUES ({3}, {4}, {9}, {10}, {11}, {12})";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME_ID, VISION_ID,
                preset_name_id, preset_vision_id,
                VISION_LATEST_LEFT, VISION_LATEST_TOP, VISION_LATEST_RIGHT, VISION_LATEST_BOTTOM,
                vision_latest_pos_left, vision_latest_pos_top, vision_latest_pos_right, vision_latest_pos_bottom);
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
