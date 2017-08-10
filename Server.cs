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

            m_Clients = new List<Client>();
            m_SocketListener = new AsyncSocketListener();
            AsyncSocketListener.OnConnectionAccepted = OnConnectionAccepted;
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

            Logger.Write("Server shutdown.");
            m_Initialised = false;
        }

        public static void OnConnectionAccepted(Socket socket)
        {
            Client client = new Client(socket);
            m_Clients.Add(client);
            Logger.Write("Connection accepted.");
        }

        private static bool m_Initialised;
        private static AsyncSocketListener m_SocketListener;
        private static List<Client> m_Clients;
    }
}
