using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

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
            AsyncSocketListener.StartListening();

            m_Initialised = true;

            for (int i = 0; i < 5; ++i)
            {
                m_Database.CreateNode(Node.Type.Terminal);
            }
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
