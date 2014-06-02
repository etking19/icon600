using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite.Data
{
    public class User : ISqlData
    {
        private const string TABLE_NAME = "clients";

        private string mCreateCommand = "CREATE TABLE {0} (username VARCHAR(20) PRIMARY KEY, password VARCHAR(20))";
        private string mRemoveCommand = "DELETE FROM {0} WHERE username = '{1}'";
        private string mAddCommand = "INSERT INTO {0} (username, password) VALUES ('{1}', '{2}')";
        private string mQueryCommand = "SELECT * FROM {0}";
        private string mUpdateCommand = "UPDATE {0} SET password = '{1}' WHERE username = '{2}';";
        
        private string mUsername;
        private string mPassword;

        public User()
        {

        }

        public User(string username, string password)
        {
            mUsername = username;
            mPassword = password;
        }

        public User(string usernameToBeRemove)
        {
            mUsername = usernameToBeRemove;
        }

        string ISqlData.GetAddCommand()
        {
            return String.Format(mAddCommand, TABLE_NAME, mUsername, mPassword);
        }

        string ISqlData.GetRemoveCommand()
        {
            return String.Format(mRemoveCommand, TABLE_NAME, mUsername);
        }

        public string GetTableName()
        {
            return TABLE_NAME;
        }

        public string GetCreateCommand()
        {
            return String.Format(mCreateCommand, TABLE_NAME);
        }


        public string GetQueryCommand()
        {
            return String.Format(mQueryCommand, TABLE_NAME);
        }


        public string GetUpdateDataCommand()
        {
            return String.Format(mUpdateCommand, TABLE_NAME, mPassword, mUsername);
        }
    }
}
