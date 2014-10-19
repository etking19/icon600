using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class PresetVnc : ISqlData
    {
        public const string TABLE_NAME = "preset_vnc";

        public const string PRESET_VNC_ID = "preset_vnc_id";
        public const string PRESET_NAME_ID = "preset_name_id";
        public const string VNC_ID = "vnc_id";
        public const string VNC_LATEST_LEFT = "vnc_left";
        public const string VNC_LATEST_TOP = "vnc_top";
        public const string VNC_LATEST_RIGHT = "vnc_right";
        public const string VNC_LATEST_BOTTOM = "vnc_bottom";

        public int preset_name_id { get; set; }
        public int preset_vnc_id { get; set; }
        public int vnc_latest_pos_left { get; set; }
        public int vnc_latest_pos_top { get; set; }
        public int vnc_latest_pos_right { get; set; }
        public int vnc_latest_pos_bottom { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} REFERENCES {2}({3}) ON DELETE CASCADE, {4} REFERENCES {5}({6}) ON DELETE CASCADE, {7} INTEGER PRIMARY KEY AUTOINCREMENT, {8} INTEGER NOT NULL DEFAULT 0, {9} INTEGER NOT NULL DEFAULT 0, {10} INTEGER NOT NULL DEFAULT 0, {11} INTEGER NOT NULL DEFAULT 0)";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME_ID, PresetName.TABLE_NAME, PresetName.PRESET_ID,
                VNC_ID, RemoteVnc.TABLE_NAME, RemoteVnc.REMOTEVNC_ID,
                PRESET_VNC_ID,
                VNC_LATEST_LEFT, VNC_LATEST_TOP, VNC_LATEST_RIGHT, VNC_LATEST_BOTTOM);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {5}, {6}, {7}, {8}) VALUES ({3}, {4}, {9}, {10}, {11}, {12})";
            return String.Format(query, TABLE_NAME,
                PRESET_NAME_ID, VNC_ID,
                preset_name_id, preset_vnc_id,
                VNC_LATEST_LEFT, VNC_LATEST_TOP, VNC_LATEST_RIGHT, VNC_LATEST_BOTTOM,
                vnc_latest_pos_left, vnc_latest_pos_top, vnc_latest_pos_right, vnc_latest_pos_bottom);
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
