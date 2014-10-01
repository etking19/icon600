using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Data
{
    public class RemoteVnc : ISqlData
    {
        public const string TABLE_NAME = "remote_vnc";

        public const string REMOTEVNC_ID = "remote_id";
        public const string NAME = "label";
        public const string REMOTE_IP = "remote_ip";
        public const string REMOTE_PORT = "remote_port";

        public int id { get; set; }
        public string name { get; set; }
        public string remoteIp { get; set; }
        public int remotePort { get; set; }


        public string GetCreateCommand()
        {
            string query = "CREATE TABLE IF NOT EXISTS {0} ({1} INTEGER PRIMARY KEY AUTOINCREMENT, {2} VARCHAR(100) NOT NULL, {3} VARCHAR(100) NOT NULL, {4} INTEGER NOT NULL DEFAULT 0)";
            return String.Format(query,
                TABLE_NAME,
                REMOTEVNC_ID, NAME, REMOTE_IP, REMOTE_PORT);
        }

        public string GetAddCommand()
        {
            string query = "INSERT INTO {0} ({1}, {2}, {3}) VALUES ('{4}', '{5}', {6})";
            return String.Format(query, TABLE_NAME,
                NAME, REMOTE_IP, REMOTE_PORT,
                name, remoteIp, remotePort);
        }

        public string GetRemoveCommand()
        {
            string query = "DELETE FROM {0} WHERE {1} = {2}";
            return String.Format(query, TABLE_NAME, REMOTEVNC_ID, id);
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
            string query = "UPDATE {0} SET {1}='{2}', {3}='{4}', {5}={6} WHERE {7}={8}";
            return String.Format(query, TABLE_NAME,
                NAME, name,
                REMOTE_IP, remoteIp,
                REMOTE_PORT, remotePort,
                REMOTEVNC_ID, id);
        }
    }
}
