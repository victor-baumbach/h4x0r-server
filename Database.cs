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
                    account.NodeID = (UInt64)reader.GetInt64(reader.GetOrdinal("NodeID"));

                    return account;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public Account CreateAccount(string username, string email, string password)
        {
            Node node = CreateNode(Node.Type.Gateway);

            Account account = new Account();
            account.Username = username;
            account.Email = email;
            account.Password = password;
            account.NodeID = node.ID;
            account.Reputation = 0;
            account.Credits = 1000;
            account.Banned = false;

            string accountSql = "INSERT INTO Accounts(Username, Email, Password, NodeID, Reputation, Banned, Credits) VALUES(@username, @email, @password, @nodeid, @reputation, @banned, @credits)";
            try
            {
                SQLiteCommand command = new SQLiteCommand(accountSql, m_DatabaseConnection);
                command.Parameters.AddWithValue("@username", account.Username);
                command.Parameters.AddWithValue("@email", account.Email);
                command.Parameters.AddWithValue("@password", account.Password);
                command.Parameters.AddWithValue("@nodeid", account.NodeID);
                command.Parameters.AddWithValue("@reputation", account.Reputation);
                command.Parameters.AddWithValue("@credits", account.Credits);
                command.Parameters.AddWithValue("@banned", account.Banned);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }

            return account;
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

        private SQLiteConnection m_DatabaseConnection;
        private Mutex m_NodesTableMutex;
    }
}
