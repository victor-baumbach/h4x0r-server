using System;
using System.Data.SQLite;
using System.Threading;

namespace h4x0r_server
{
    class Database
    {
        public Database()
        {
            string filename = @"C:\Users\Valman\Dropbox\Dev\h4x0r\database\database.sqlite3";
            m_DatabaseConnection = new SQLiteConnection("Data Source=" + filename + ";Version=3;");
            try
            {
                m_DatabaseConnection.Open();
                Logger.Write("Connection to database estabilished.");
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

        public Account GetAccount(string username)
        {
            string accountsSql = "SELECT * from Accounts WHERE Username = @username;";
            try
            {
                SQLiteCommand command = new SQLiteCommand(accountsSql, m_DatabaseConnection);
                command.Parameters.AddWithValue("@username", username);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Account account = new Account();
                    account.Username = reader.GetString(reader.GetOrdinal("Username"));
                    account.Email = reader.GetString(reader.GetOrdinal("Email"));
                    account.Password = reader.GetString(reader.GetOrdinal("Password"));
                    account.Reputation = reader.GetInt64(reader.GetOrdinal("Reputation"));
                    account.Credits = reader.GetInt64(reader.GetOrdinal("Credits"));
                    account.Banned = reader.GetBoolean(reader.GetOrdinal("Banned"));

                    // This row can be null if the player hasn't been assigned a gateway yet.
                    int gatewayIDRow = reader.GetOrdinal("GatewayID");
                    if (reader.IsDBNull(gatewayIDRow) == false)
                    {
                        account.GatewayID = reader.GetString(gatewayIDRow);
                    }

                    return account;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public Node CreateNode(Node.Type type)
        {
            m_NodesTableMutex.WaitOne();

            Node.Address address = CreateAddress();
            Node node = new Node(type, address);

            // TODO: Insert node into Nodes table

            m_NodesTableMutex.ReleaseMutex();

            return node;
        }

        private Node.Address CreateAddress()
        {
            while (true)
            {
                Node.Address address = new Node.Address();

                string accountsSql = "SELECT * from Nodes WHERE Address = @address;";
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

        private SQLiteConnection m_DatabaseConnection;
        private Mutex m_NodesTableMutex;
    }
}
