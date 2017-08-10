using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace h4x0r_server
{
    /////////////////////////////////////////////////////////////////////
    // AsyncSocketListener
    /////////////////////////////////////////////////////////////////////

    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class AsyncSocketListener
    {
        // Thread signal.  
        private static ManualResetEvent allDone = new ManualResetEvent(false);

        public AsyncSocketListener()
        {
        }

        public static void StartListening()
        {
            m_ListenThread = new Thread(Listen);
            m_ListenThread.Start();
        }

        public static void StopListening()
        {
            m_Listening = false;

            // Block until the listening thread is done.
            while (IsListening())
            {

            }
        }

        public static bool IsListening()
        {
            return (m_ListenThread != null && m_ListenThread.IsAlive);
        }

        private static void Listen()
        {
            m_Listening = true;

            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 13370);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                Logger.Write("Server listening for connections at {0}", localEndPoint);

                while (m_Listening)
                {
                    Socket socket = listener.Accept();
                    OnConnectionAccepted(socket);
                }

                Logger.Write("Server no longer listening for connections.");
            }
            catch (Exception e)
            {
                Logger.Write(e.ToString());
            }
        }

        public delegate void ConnectionAcceptedDelegate(Socket socket);
        public static ConnectionAcceptedDelegate OnConnectionAccepted;

        private static Thread m_ListenThread = null;
        private static bool m_Listening = false;
    }
}
