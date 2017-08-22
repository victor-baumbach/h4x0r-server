using System;
using System.Data.SQLite;

namespace h4x0r_server
{
    class Database
    {
        public Database()
        {
            string filename = @"database\database.sqlite3";
            m_DatabaseConnection = new SQLiteConnection("Data Source=" + filename + ";Version=3;");
            try
            {
                m_DatabaseConnection.Open();
                Logger.Write(Logger.Level.Info, "Connection to database estabilished.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Shutdown()
        {
            m_DatabaseConnection.Close();
        }

        public SQLiteConnection Connection { get { return m_DatabaseConnection; } }

        private SQLiteConnection m_DatabaseConnection;
    }
}
