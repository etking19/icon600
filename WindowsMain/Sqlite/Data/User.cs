using System;

namespace Database.Data
{
    public class User : ISqlData
    {
        public const string TABLE_NAME = "users";

        public const string USER_ID = "user_id";
        public const string LABEL = "label";
        public const string USERNAME = "username";
        public const string PASSWORD = "password";
        public const string GROUP_ID = "group_id";

        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public string label { get; set; }

        public int group { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY AUTOINCREMENT, {2} VARCHAR(100) UNIQUE NOT NULL, {3} VARCHAR(100) NOT NULL, {4} VARCHAR(100) NOT NULL, {5} INTEGER, FOREIGN KEY({5}) REFERENCES {6}({7}) ON DELETE CASCADE)";
            return String.Format(query, TABLE_NAME, USER_ID, USERNAME, PASSWORD, LABEL, GROUP_ID, Group.TABLE_NAME, Group.GROUP_ID);
        }

        string ISqlData.GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {3}, {4}) VALUES ('{5}', '{6}', '{7}', {8})";
            return String.Format(query, TABLE_NAME,
                LABEL, USERNAME, PASSWORD, GROUP_ID,
                label, username, password, group);
        }

        string ISqlData.GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1}={2}";
            return String.Format(query, TABLE_NAME, USER_ID, id);
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
            string query = "UPDATE {0} SET {1}='{2}', {3}='{4}', {5}='{6}', {7}={8} WHERE {9}={10};";
            return String.Format(query, TABLE_NAME, 
                PASSWORD, password, 
                LABEL, label, 
                USERNAME, username,
                GROUP_ID, group,
                USER_ID, id);
        }
    }
}
