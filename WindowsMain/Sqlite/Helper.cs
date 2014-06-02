using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Sqlite.Data;

namespace Sqlite
{
    public class Helper
    {
        private SQLiteConnection m_dbConnection;

        private static Helper sSQLiteHelper;

        private Helper()
        {
        }

        public static Helper GetInstance()
        {
            if(sSQLiteHelper == null)
            {
                sSQLiteHelper = new Helper();
            }
            return sSQLiteHelper;
        }

        public void Initialize(string dbName)
        {
            m_dbConnection = new SQLiteConnection(String.Format("{0}{1}{2}", "Data Source=", dbName, ";Version=3;"));

            try
            {
                m_dbConnection.Open();
            }
            catch (System.Data.SQLite.SQLiteException)
            {
            }
        }

        public void CreateTable(ISqlData data)
        {
            SQLiteCommand command = new SQLiteCommand(data.GetCreateCommand(), m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void AddData(ISqlData data)
        {
            SQLiteCommand command = new SQLiteCommand(data.GetAddCommand(), m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void RemoveData(ISqlData data)
        {
            SQLiteCommand command = new SQLiteCommand(data.GetRemoveCommand(), m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void UpdateData(ISqlData data)
        {
            SQLiteCommand command = new SQLiteCommand(data.GetUpdateDataCommand(), m_dbConnection);
            command.ExecuteNonQuery();
        }

        public SQLiteDataReader ReadData(ISqlData data)
        {
            SQLiteCommand command = new SQLiteCommand(data.GetQueryCommand(), m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            return reader;
        }
    }
}
