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
            Logger.Write("Server initialising...");

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

            Logger.Write("Server shutdown.");
            m_Initialised = false;
        }

        public static void OnConnectionAccepted(Socket socket)
        {
            Client client = new Client(socket);
            m_Clients.Add(client);

            IPEndPoint remoteEndpoint = (IPEndPoint)socket.RemoteEndPoint;
            Logger.Write("Connection accepted from {0}.", remoteEndpoint.Address);
        }

        public static void OnConnectionLost(Socket socket)
        {
            IPEndPoint remoteEndpoint = (IPEndPoint)socket.RemoteEndPoint;
            Logger.Write("Connection lost from {0}.", remoteEndpoint.Address);

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

                        h4x0r.Messages.CreateAccountResult result = Server.CreateAccount(message.Value.Username, message.Value.Email, message.Value.Password);
                        if (result == h4x0r.Messages.CreateAccountResult.Success)
                        {
                            Logger.Write("Created account '{0}' ({1})", message.Value.Username, message.Value.Email);
                        }

                        AsyncSocketListener.Send(handler, h4x0r.Messages.CreateAccountResultMessage(result));

                        break;
                    }
                case MessageContainer.LoginMessage:
                    {
                        LoginMessage? message = messageBase.Data<LoginMessage>();
                        if (message == null) return false;
                        break;
                    }
                default:
                    return false;
            }

            return true;
        }

        public static h4x0r.Messages.CreateAccountResult CreateAccount(string username, string email, string password)
        {
            Account account = m_Database.GetAccount(username);
            if (account != null)
            {
                return h4x0r.Messages.CreateAccountResult.AlreadyExists;
            }
            else
            {
                m_Database.CreateAccount(username, email, password);
                return h4x0r.Messages.CreateAccountResult.Success;
            }
        }

        private static bool m_Initialised;
        private static AsyncSocketListener m_SocketListener;
        private static List<Client> m_Clients;
        private static Database m_Database;
    }
}
