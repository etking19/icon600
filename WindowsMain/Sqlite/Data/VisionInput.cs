using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class VisionInput : ISqlData
    {
        public const string TABLE_NAME = "vision_input";

        public const string VISION_TABLE_ID = "vision_table_id";
        public const string VISION_WINDOW = "window";
        public const string VISION_INPUT = "input";
        public const string VISION_OSD = "on_screen_display";

        public int Id { get; set; }
        public string Window { get; set; }
        public string Input { get; set; }
        public string OSD { get; set; }

        public string GetCreateCommand()
        {
            string query = @"CREATE TABLE IF NOT EXISTS {0} (
                {1} INTEGER PRIMARY KEY AUTOINCREMENT, 
                {2} VARCHAR NOT NULL, 
                {3} VARCHAR NOT NULL,
                {4} VARCHAR NOT NULL)";

            return String.Format(query, 
                TABLE_NAME,
                VISION_TABLE_ID, VISION_WINDOW, VISION_INPUT, VISION_OSD);
        }

        public string GetAddCommand()
        {
            string query = @"INSERT INTO {0} ({1}, {2}, {3}) VALUES ('{4}','{5}','{6}')";

            return String.Format(query, TABLE_NAME, VISION_WINDOW, VISION_INPUT, VISION_OSD, Window, Input, OSD);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1} = {2}";
            return String.Format(query, TABLE_NAME, VISION_TABLE_ID, Id);
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
            string query = "UPDATE {0} SET {1}='{2}', {3}='{4}', {5}='{6}' WHERE {7}={8};";
            return String.Format(query, TABLE_NAME,
                VISION_WINDOW, Window,
                VISION_INPUT, Input,
                VISION_OSD, OSD,
                VISION_TABLE_ID, Id);
        }
    }
}
