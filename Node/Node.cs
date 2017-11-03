using System;
using System.Data.SQLite;
using System.Threading;

namespace h4x0r_server
{
    partial class Node
    {
        public enum Type
        {
            Invalid = -1,
            Gateway = 0,
            Server,
            Terminal,
            Mainframe,
            Blackmarket,
            Decoy,
            Home
        }

        static public Node Create(Type type)
        {
            if (m_NodesTableMutex == null)
            {
                m_NodesTableMutex = new Mutex();
            }

            m_NodesTableMutex.WaitOne();
            Address address = CreateAddress();

            string nodeSql = "INSERT INTO Nodes(Address, Type, Terminated) VALUES(@address, @type, @terminated)";
            try
            {
                SQLiteCommand command = new SQLiteCommand(nodeSql, Server.Database.Connection);
                command.Parameters.AddWithValue("@address", address.Value);
                command.Parameters.AddWithValue("@type", (int)type);
                command.Parameters.AddWithValue("@terminated", 0);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }

            // Retrieve the automatically generated ID from the database for this new entry
            UInt64 id = 0;
            string nodesSql = "SELECT * from Nodes WHERE Address = @address;";
            try
            {
                SQLiteCommand command = new SQLiteCommand(nodesSql, Server.Database.Connection);
                command.Parameters.AddWithValue("@address", address.Value);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    id = (UInt64)reader.GetInt64(reader.GetOrdinal("ID"));
                }
            }
            catch (Exception)
            {
                throw;
            }

            m_NodesTableMutex.ReleaseMutex();

            return new Node(id, CreateAddress(), type, false);
        }

        static public Node Find(UInt64 id)
        {
            string nodesSql = "SELECT * from Nodes WHERE ID = @id;";
            try
            {
                SQLiteCommand command = new SQLiteCommand(nodesSql, Server.Database.Connection);
                command.Parameters.AddWithValue("@id", id);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Node(
                            (UInt64)reader.GetInt64(reader.GetOrdinal("ID")),
                            new Address(reader.GetString(reader.GetOrdinal("Address"))),
                            (Type)reader.GetInt64(reader.GetOrdinal("Type")),
                            reader.GetBoolean(reader.GetOrdinal("Terminated")));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        static private Address CreateAddress()
        {
            while (true)
            {
                Address address = new Address();

                string accountsSql = "SELECT * FROM Nodes WHERE Address = @address;";
                try
                {
                    SQLiteCommand command = new SQLiteCommand(accountsSql, Server.Database.Connection);
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

        public UInt64 ID { get { return m_ID; } }
        public Address NodeAddress { get { return m_Address; } }
        public Type NodeType { get { return m_NodeType; } }
        public bool Terminated { get { return m_Terminated; } }

        private Node(UInt64 id, Address address, Type nodeType, bool terminated)
        {
            m_ID = id;
            m_Address = address;
            m_NodeType = nodeType;
            m_Terminated = terminated;
        }

        private UInt64 m_ID;
        private Address m_Address;
        private Type m_NodeType;
        private bool m_Terminated;

        static private Mutex m_NodesTableMutex = null;
    }
}

