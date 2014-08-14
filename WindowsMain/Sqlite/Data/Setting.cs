using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class Setting : ISqlData
    {
        public const string TABLE_NAME = "settings";

        public const string ID = "setting_id";
        public const string PORT_START = "port_start";
        public const string PORT_END = "port_end";
        public const string COL = "matrix_col";
        public const string ROW = "matrix_row";
        public const string VNC_PATH = "vnc_path";

        public int port_start { get; set; }
        public int port_end { get; set; }
        public int matrix_col { get; set; }
        public int matrix_row { get; set; }
        public string vnc_path { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY, {2} INTEGER NOT NULL DEFAULT 0, {3} INTEGER NOT NULL DEFAULT 0, {4} INTEGER NOT NULL DEFAULT 0, {5} INTEGER NOT NULL DEFAULT 0, {6} VARCHAR(100) NOT NULL)";
            return String.Format(query, ID, TABLE_NAME, PORT_START, PORT_END, COL, ROW, VNC_PATH);
        }

        public string GetAddCommand()
        {
            string query = "INSERT OR REPLACE INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}) VALUES ({7}, {8}, {9}, {10}, {11}, '{12}')";
            return String.Format(query, TABLE_NAME,
                ID, PORT_START, PORT_END, COL, ROW, VNC_PATH,
                0, port_start, port_end, matrix_col, matrix_row, vnc_path);
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
