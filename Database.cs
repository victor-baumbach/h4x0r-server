using System;
using System.Data.SQLite;
using System.Threading;

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

            m_NodesTableMutex = new Mutex();
        }

        public void Shutdown()
        {
            m_DatabaseConnection.Close();
        }

        public Node CreateNode(Node.Type type)
        {
            m_NodesTableMutex.WaitOne();

            Node node = new Node();
            node.ID = 0;
            node.NodeAddress = CreateAddress();
            node.NodeType = type;
            node.Terminated = false;

            string nodeSql = "INSERT INTO Nodes(Address, Type, Terminated) VALUES(@address, @type, @terminated)";
            try
            {
                SQLiteCommand command = new SQLiteCommand(nodeSql, m_DatabaseConnection);
                command.Parameters.AddWithValue("@address", node.NodeAddress.Value);
                command.Parameters.AddWithValue("@type", (int)type);
                command.Parameters.AddWithValue("@terminated", 0);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }

            string lastRowId = "SELECT last_insert_rowid() FROM Nodes";
            try
            {
                SQLiteCommand command = new SQLiteCommand(lastRowId, m_DatabaseConnection);
                node.ID = Convert.ToUInt64(command.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }

            m_NodesTableMutex.ReleaseMutex();

            return node;
        }

        private Node.Address CreateAddress()
        {
            while (true)
            {
                Node.Address address = new Node.Address();

                string accountsSql = "SELECT * FROM Nodes WHERE Address = @address;";
                try
                {
                    SQLiteCommand command = new SQLiteCommand(accountsSql, m_DatabaseConnection);
                    command.Parameters.AddWithValue("@address", address.Value);

                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        continue;
                    }
                    else
                    {
                        return address;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public SQLiteConnection Connection { get; }

        private SQLiteConnection m_DatabaseConnection;
        private Mutex m_NodesTableMutex;
    }
}
