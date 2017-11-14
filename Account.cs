using System;
using System.Data.SQLite;

namespace h4x0r
{
    class Account
    {
        static public Account Create(string username, string email, string password)
        {
            Node node = Node.Create(Node.Type.Gateway);

            Account account = new Account(
                username,
                email,
                password,
                node.ID,
                0,
                1000,
                false);

            string accountSql = "INSERT INTO Accounts(Username, Email, Password, NodeID, Reputation, Banned, Credits) VALUES(@username, @email, @password, @nodeid, @reputation, @banned, @credits)";
            try
            {
                SQLiteCommand command = new SQLiteCommand(accountSql, Server.Database.Connection);
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

        static public Account Find(string username)
        {
            string accountsSql = "SELECT * from Accounts WHERE Username = @username;";
            try
            {
                SQLiteCommand command = new SQLiteCommand(accountsSql, Server.Database.Connection);
                command.Parameters.AddWithValue("@username", username);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Account(
                            reader.GetString(reader.GetOrdinal("Username")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("Password")),
                            (UInt64)reader.GetInt64(reader.GetOrdinal("NodeID")),
                            reader.GetInt32(reader.GetOrdinal("Reputation")),
                            reader.GetInt64(reader.GetOrdinal("Credits")),
                            reader.GetBoolean(reader.GetOrdinal("Banned")));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public string Username { get { return m_Username; } }
        public string Email { get { return m_Email; } }
        public string Password { get { return m_Password; } }
        public UInt64 NodeID { get { return m_NodeID; } }
        public Int32 Reputation { get { return m_Reputation; } }
        public Int64 Credits { get { return m_Credits; } }
        public bool Banned { get { return m_Banned; } }

        private Account(string username, string email, string password, UInt64 nodeID, Int32 reputation, Int64 credits, bool banned)
        {
            m_Username = username;
            m_Email = email;
            m_Password = password;
            m_NodeID = nodeID;
            m_Reputation = reputation;
            m_Credits = credits;
            m_Banned = banned;
        }

        private string m_Username;
        private string m_Email;
        private string m_Password;
        UInt64 m_NodeID;
        Int32 m_Reputation;
        Int64 m_Credits;
        bool m_Banned;
    }
}
