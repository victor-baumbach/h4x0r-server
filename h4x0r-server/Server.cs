using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Data.SQLite;
using FlatBuffers;
using h4x0r.MessagesInternal;
using System;

namespace h4x0r
{
    class Server
    {
        public static void Initialise()
        {
            Logger.Write(Logger.Level.Info, "Server initialising...");

            m_Database = new Database();
            m_Clients = new List<Client>();
            m_SocketToClient = new Dictionary<Socket, Client>();
            m_SocketListener = new AsyncSocketListener();
            AsyncSocketListener.OnConnectionAccepted = OnConnectionAccepted;
            AsyncSocketListener.OnConnectionLost = OnConnectionLost;
            AsyncSocketListener.OnMessageReceived = OnMessageReceived;
            AsyncSocketListener.StartListening();

            m_Initialised = true;
        }

        public static void Shutdown()
        {
            if (!m_Initialised)
            {
                return;
            }

            AsyncSocketListener.StopListening();

            m_Database.Shutdown();

            Logger.Write(Logger.Level.Info, "Server shutdown.");
            m_Initialised = false;
        }

        public static void OnConnectionAccepted(Socket socket)
        {
            Client client = new Client(socket);
            m_Clients.Add(client);
            m_SocketToClient[socket] = client;
            OnClientAdded(client);
            Logger.Write(Logger.Level.Info, "Connection accepted from {0}.", client.GetFriendlyAddress());
        }

        public static void OnConnectionLost(Socket socket)
        {
            IPEndPoint remoteEndpoint = (IPEndPoint)socket.RemoteEndPoint;
            Client client = m_SocketToClient[socket];
            Logger.Write(Logger.Level.Info, "Connection lost from {0}.", client.GetFriendlyAddress());
            m_Clients.Remove(client);
            m_SocketToClient.Remove(socket);
            OnClientRemoved(client);
        }

        public static bool OnMessageReceived(Socket handler, byte[] buffer)
        {
            // The messages always have the same structure:
            // A MessageBase which contains an union of all the valid messages.
            ByteBuffer bb = new ByteBuffer(buffer);

            MessageBase messageBase = MessageBase.GetRootAsMessageBase(bb);
            switch (messageBase.DataType)
            {
                case MessageContainer.CreateAccountMessage:
                    {
                        CreateAccountMessage? message = messageBase.Data<CreateAccountMessage>();
                        if (message == null) return false;

                        Messages.CreateAccountResult result = CreateAccount(message.Value.Username, message.Value.Email, message.Value.Password);
                        AsyncSocketListener.Send(handler, Messages.CreateAccountResultMessage(result));

                        break;
                    }
                case MessageContainer.LoginMessage:
                    {
                        LoginMessage? message = messageBase.Data<LoginMessage>();
                        if (message == null) return false;

                        Messages.LoginResult result = TryLogin(message.Value.Username, message.Value.Password);
                        AsyncSocketListener.Send(handler, Messages.LoginResultMessage(result));

                        if (result == Messages.LoginResult.Success)
                        {
                            // TODO: a second find shouldn't be needed, as we're doing one in the login
                            Account account = Account.Find(message.Value.Username);

                            Client client = m_SocketToClient[handler];
                            client.AssociateAccount(account);
                            OnClientLogin(client);

                            AsyncSocketListener.Send(handler, Messages.UpdateAddressMessage(client.Node.NodeAddress.Value, account.Username));
                            AsyncSocketListener.Send(handler, Messages.UpdateCreditsMessage(account.Credits));
                            AsyncSocketListener.Send(handler, Messages.UpdateReputationMessage(account.Reputation));

                            SendAllKnownAddresses(client);
                        }

                        break;
                    }
                case MessageContainer.NodeConnectMessage:
                    {
                        NodeConnectMessage? message = messageBase.Data<NodeConnectMessage>();
                        if (message == null || message.Value.RouteLength == 0) return false;

                        List<string> route = new List<string>();
                        for (int i = 0; i < message.Value.RouteLength; ++i)
                        {
                            route.Add(message.Value.Route(i));
                        }

                        int nodeErrorIndex;
                        Messages.NodeConnectResult result = NodeConnect(route, out nodeErrorIndex);
                        AsyncSocketListener.Send(handler, Messages.NodeConnectResultMessage(result, nodeErrorIndex));

                        break;
                    }

                case MessageContainer.PurchaseSoftwareMessage:
                    {

                        PurchaseSoftwareMessage? message = messageBase.Data<PurchaseSoftwareMessage>();
                        if (message == null) return false;

                        Client client = m_SocketToClient[handler];
                        File.Type softwareToBuy = (File.Type)message.Value.Software;
                        Messages.PurchaseSoftwareResult result = TryPurchaseSoftware(client, softwareToBuy);
                        AsyncSocketListener.Send(handler, Messages.PurchaseSoftwareResultMessage(result));
                        break;
                    }
                default:
                    return false;
            }

            return true;
        }

        public static Messages.CreateAccountResult CreateAccount(string username, string email, string password)
        {
            Account account = Account.Find(username);
            if (account != null)
            {
                Logger.Write(Logger.Level.Info, "Couldn't create account '{0}', already exists", username);
                return Messages.CreateAccountResult.AlreadyExists;
            }
            else
            {
                Logger.Write(Logger.Level.Info, "Created account '{0}' ({1})", username, email);
                Account.Create(username, email, password);
                return Messages.CreateAccountResult.Success;
            }
        }

        public static Messages.LoginResult TryLogin(string username, string password)
        {
            Account account = Account.Find(username);
            if (account != null && account.Password == password)
            {
                if (account.Banned)
                {
                    Logger.Write(Logger.Level.Warning, "Login failed for user '{0}' (banned)", username);
                    return Messages.LoginResult.Banned;
                }
                else
                {
                    Logger.Write(Logger.Level.Info, "Login successful for user '{0}'", username);
                    return Messages.LoginResult.Success;
                }
            }
            else
            {
                Logger.Write(Logger.Level.Warning, "Login failed for user '{0}' (mismatching username / password)", username);
                return Messages.LoginResult.Failed;
            }
        }

        public static Messages.NodeConnectResult NodeConnect(List<string> route, out int nodeErrorIndex)
        {
            string nodeSql = "SELECT * from Nodes WHERE Address = @address";
            for (int i = 0; i < route.Count; ++i)
            {
                bool isBounce = (i != route.Count - 1);
                string address = route[i];
                SQLiteCommand command = new SQLiteCommand(nodeSql, Database.Connection);
                command.Parameters.AddWithValue("@address", address);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // The black market always rejects bounces, as the players can never see its logs.
                    Common.NodeType type = (Common.NodeType)reader.GetInt32(reader.GetOrdinal("Type"));
                    if (type == Common.NodeType.Blackmarket && isBounce)
                    {
                        nodeErrorIndex = i;
                        return Messages.NodeConnectResult.ConnectionRejected;
                    }

                    // TODO: Create log entry
                }
                else
                {
                    nodeErrorIndex = i;
                    return Messages.NodeConnectResult.Timeout;
                }
            }

            nodeErrorIndex = -1;
            return Messages.NodeConnectResult.Success;
        }

        private static void SendAllKnownAddresses(Client client)
        {
            string addressesSql = "SELECT * from KnownAddresses WHERE NodeID = @nodeid;";
            try
            {
                SQLiteCommand command = new SQLiteCommand(addressesSql, Database.Connection);
                command.Parameters.AddWithValue("@nodeid", client.Node.ID);

                // TODO: This really should send a single message which contains all the addresses.
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string address = reader.GetString(reader.GetOrdinal("Address"));
                    string hostname = reader.GetString(reader.GetOrdinal("Hostname"));
                    Common.NodeType type = (Common.NodeType)reader.GetInt32(reader.GetOrdinal("Type"));

                    AsyncSocketListener.Send(client.GetSocket(), Messages.UpdateKnownAddressMessage(address, hostname, type));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static Messages.PurchaseSoftwareResult TryPurchaseSoftware(Client client, File.Type softwareToBuy)
        {
            return Messages.PurchaseSoftwareResult.Success;
        }

        public delegate void ClientAddedDelegate(Client client);
        public static ClientAddedDelegate OnClientAdded;

        public delegate void ClientRemovedDelegate(Client client);
        public static ClientRemovedDelegate OnClientRemoved;

        public delegate void ClientLoginDelegate(Client client);
        public static ClientLoginDelegate OnClientLogin;

        public static Database Database { get { return m_Database; } }

        private static bool m_Initialised;
        private static AsyncSocketListener m_SocketListener;
        private static List<Client> m_Clients;
        private static Dictionary<Socket, Client> m_SocketToClient;
        private static Database m_Database;
    }
}
