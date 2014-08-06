using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class Application : ISqlData
    {
        public const string TABLE_NAME = "applications";

        public const string APPLICATION_ID = "application_id";

        public const string LABEL = "label";
        public const string PATH = "executable_path";
        public const string ARGUMENTS = "arguments";

        public const string SHOWING_LEFT = "show_left";
        public const string SHOWING_TOP = "show_top";
        public const string SHOWING_RIGHT = "show_right";
        public const string SHOWING_BOTTOM = "show_bottom";


        public int id { get; set; }
        public string label { get; set; }
        public string path { get; set; }
        public string arguments { get; set; }
        public int pos_left { get; set; }
        public int pos_top { get; set; }
        public int pos_right { get; set; }
        public int pos_bottom { get; set; }


        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY AUTOINCREMENT, {2} VARCHAR(100) UNIQUE NOT NULL, {3} VARCHAR(100) NOT NULL, {4} VARCHAR(100) NOT NULL, {5} INTEGER NOT NULL DEFAULT 0, {6} INTEGER NOT NULL DEFAULT 0, {7} INTEGER NOT NULL DEFAULT 0, {8} INTEGER NOT NULL DEFAULT 0)";
            return String.Format(query, 
                TABLE_NAME,
                APPLICATION_ID,
                LABEL, PATH, ARGUMENTS, SHOWING_LEFT, SHOWING_TOP, SHOWING_RIGHT, SHOWING_BOTTOM
                );
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}) VALUES ('{8}', '{9}', '{10}', {11}, {12}, {13}, {14})";
            return String.Format(query,
                TABLE_NAME,
                LABEL, PATH, ARGUMENTS, SHOWING_LEFT, SHOWING_TOP, SHOWING_RIGHT, SHOWING_BOTTOM,
                label, path, arguments, pos_left, pos_top, pos_right, pos_bottom);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1} = {2}";
            return String.Format(query, TABLE_NAME, APPLICATION_ID, id);
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
            string query = "UPDATE {0} SET {1}='{2}', {3}={4}, {5}={6}, {7}={8}, {9}={10}, {11}={12}, {13}={14} WHERE {15}={16};";
            return String.Format(query, TABLE_NAME,
                LABEL, label,
                PATH, path,
                ARGUMENTS, arguments,
                SHOWING_LEFT, pos_left,
                SHOWING_TOP, pos_top,
                SHOWING_RIGHT, pos_right,
                SHOWING_BOTTOM, pos_bottom);
        }
    }
}
