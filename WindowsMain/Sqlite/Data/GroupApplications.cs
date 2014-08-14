using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class GroupApplications : ISqlData
    {
        public const string TABLE_NAME = "group_applications";

        public const string GROUP_ID = "group_id";
        public const string APPLICATION_ID = "application_id";

        public int application_id { get; set; }
        public int group_id { get; set; }

        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} REFERENCES {2}({3}) ON DELETE CASCADE, {4} REFERENCES {5}({6}) ON DELETE CASCADE, PRIMARY KEY ({1}, {4}))";
            return String.Format(query, TABLE_NAME,
                GROUP_ID, Group.TABLE_NAME, Group.GROUP_ID,
                APPLICATION_ID, Application.TABLE_NAME, Application.APPLICATION_ID);            
        }

        /// <summary>
        /// add application to certain group
        /// </summary>
        /// <returns></returns>
        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}) VALUES ({3}, {4})";
            return String.Format(query, 
                TABLE_NAME, 
                GROUP_ID, 
                APPLICATION_ID,
                group_id,
                application_id);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1}={2}";
            return String.Format(query, TABLE_NAME, GROUP_ID, group_id);
        }

        public string GetTableName()
        {
            return TABLE_NAME;
        }

        public string GetQueryCommand()
        {
            string query = "SELECT * FROM {0} WHERE {1}={2}";
            return String.Format(query, TABLE_NAME, GROUP_ID, group_id);
        }

        public string GetUpdateDataCommand()
        {
            throw new NotImplementedException();
        }
    }
}
