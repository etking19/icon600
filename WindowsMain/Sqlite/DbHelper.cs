using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Database.Data;
using System.Diagnostics;
using System.Data;

namespace Database
{
    public class DbHelper
    {
        private SQLiteConnection m_dbConnection;

        private static DbHelper sSQLiteHelper;
        private const string DB_PASSWORD = "123$%^";

        private DbHelper()
        {
        }

        public static DbHelper GetInstance()
        {
            if(sSQLiteHelper == null)
            {
                sSQLiteHelper = new DbHelper();
            }
            return sSQLiteHelper;
        }

        public bool Initialize(string dbName)
        {
            //m_dbConnection = new SQLiteConnection(String.Format("Data Source={0}; Version=3; Synchronous=Full; Password={1}", dbName, DB_PASSWORD));
            m_dbConnection = new SQLiteConnection(String.Format("Data Source={0}; Version=3; Synchronous=Full; foreign keys=True ;", dbName));

            try
            {
                m_dbConnection.Open();
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                Trace.WriteLine(e);
                return false;
            }

            return true;
        }

        public void Shutdown()
        {
            m_dbConnection.Close();
        }

        public bool CreateTable(ISqlData data)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(data.GetCreateCommand(), m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return false;
            }

            return true;
        }

        public bool AddData(ISqlData data)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(data.GetAddCommand(), m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return false;
            }
            return true;
        }

        public bool RemoveData(ISqlData data)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(data.GetRemoveCommand(), m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return false;
            }
            return true;
        }

        public bool UpdateData(ISqlData data)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(data.GetUpdateDataCommand(), m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return false;
            }

            return true;
        }

        public DataTable ReadData(ISqlData data)
        {
            SQLiteDataAdapter ad = null;
            DataTable dt = new DataTable();
            try
            {
                SQLiteCommand command = new SQLiteCommand(data.GetQueryCommand(), m_dbConnection);
                ad = new SQLiteDataAdapter(command);
                ad.Fill(dt); //fill the datasource
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            return dt;
        }
    }
}
