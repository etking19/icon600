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

        public int preset_name_id { get; set; }
        public int preset_vision_id { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} REFERENCES {2}({3}) ON DELETE CASCADE, {4} REFERENCES {5}({6}) ON DELETE CASCADE, {7} INTEGER PRIMARY KEY AUTOINCREMENT)";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME_ID, PresetName.TABLE_NAME, PresetName.PRESET_ID,
                VISION_ID, VisionInput.TABLE_NAME, VisionInput.VISION_TABLE_ID,
                PRESET_VISION_ID);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}) VALUES ({3}, {4})";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME_ID, VISION_ID,
                preset_name_id, preset_vision_id);
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
