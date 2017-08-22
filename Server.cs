﻿using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using FlatBuffers;
using h4x0r.MessagesInternal;

namespace h4x0r_server
{
    class Server
    {
        public static void Initialise()
        {
            Logger.Write(Logger.Level.Info, "Server initialising...");

            m_Database = new Database();
            m_Clients = new List<Client>();
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

            IPEndPoint remoteEndpoint = (IPEndPoint)socket.RemoteEndPoint;
            Logger.Write(Logger.Level.Info, "Connection accepted from {0}.", remoteEndpoint.Address);
        }

        public static void OnConnectionLost(Socket socket)
        {
            IPEndPoint remoteEndpoint = (IPEndPoint)socket.RemoteEndPoint;
            Logger.Write(Logger.Level.Info, "Connection lost from {0}.", remoteEndpoint.Address);

            foreach (Client client in m_Clients)
            {
                if (client.GetSocket() == socket)
                {
                    m_Clients.Remove(client);
                    break;
                }
            }
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

                        h4x0r.Messages.CreateAccountResult result = CreateAccount(message.Value.Username, message.Value.Email, message.Value.Password);
                        AsyncSocketListener.Send(handler, h4x0r.Messages.CreateAccountResultMessage(result));

                        break;
                    }
                case MessageContainer.LoginMessage:
                    {
                        LoginMessage? message = messageBase.Data<LoginMessage>();
                        if (message == null) return false;

                        h4x0r.Messages.LoginResult result = TryLogin(message.Value.Username, message.Value.Password);
                        AsyncSocketListener.Send(handler, h4x0r.Messages.LoginResultMessage(result));

                        if (result == h4x0r.Messages.LoginResult.Success)
                        {
                            // TODO: a second find shouldn't be needed, as we're doing one in the login
                            Account account = Account.Find(message.Value.Username);

                            // TODO: map of socket to client?
                            foreach (Client client in m_Clients)
                            {
                                if (client.GetSocket() == handler)
                                {
                                    client.AssociateAccount(account);
                                    break;
                                }
                            }
                        }

                        break;
                    }
                default:
                    return false;
            }

            return true;
        }

        public static h4x0r.Messages.CreateAccountResult CreateAccount(string username, string email, string password)
        {
            Account account = Account.Find(username);
            if (account != null)
            {
                Logger.Write(Logger.Level.Info, "Couldn't create account '{0}', already exists", username);
                return h4x0r.Messages.CreateAccountResult.AlreadyExists;
            }
            else
            {
                Logger.Write(Logger.Level.Info, "Created account '{0}' ({1})", username, email);
                Account.Create(username, email, password);
                return h4x0r.Messages.CreateAccountResult.Success;
            }
        }

        public static h4x0r.Messages.LoginResult TryLogin(string username, string password)
        {
            Account account = Account.Find(username);
            if (account != null && account.Password == password)
            {
                if (account.Banned)
                {
                    Logger.Write(Logger.Level.Warning, "Login failed for user '{0}' (banned)", username);
                    return h4x0r.Messages.LoginResult.Banned;
                }
                else
                {
                    Logger.Write(Logger.Level.Info, "Login successful for user '{0}'", username);
                    return h4x0r.Messages.LoginResult.Success;
                }
            }
            else
            {
                Logger.Write(Logger.Level.Warning, "Login failed for user '{0}' (mismatching username / password)", username);
                return h4x0r.Messages.LoginResult.Failed;
            }
        }

        public static Database Database { get { return m_Database; } }

        private static bool m_Initialised;
        private static AsyncSocketListener m_SocketListener;
        private static List<Client> m_Clients;
        private static Database m_Database;
    }
}
