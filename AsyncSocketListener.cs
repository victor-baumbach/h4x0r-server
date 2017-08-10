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
            try
            {
                // Create a TCP/IP socket. This supports both IPv4 and IPv6 on the same socket.
                m_Listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                m_Listener.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.IPv6Any, 13370);

                // Bind the socket to the local endpoint and listen for incoming connections.  
                m_Listener.Bind(localEndPoint);
                m_Listener.Listen(100);

                Logger.Write("Server listening for connections on port {0}.", localEndPoint.Port);

                // Start an asynchronous socket to listen for connections.  
                m_Listener.BeginAccept(new AsyncCallback(AcceptCallback), m_Listener);
            }
            catch (Exception e)
            {
                Logger.Write(e.ToString());
            }
        }

        public static void StopListening()
        {
            if (m_Listener != null)
            {
                if (m_Listener.Connected)
                {
                    m_Listener.Shutdown(SocketShutdown.Both);
                }

                m_Listener.Close();
                Logger.Write("Server no longer listening for connections.");
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Get the socket that handles the client request.  
                Socket handler = m_Listener.EndAccept(ar);

                // Notify the server that we have accepted a connection.
                OnConnectionAccepted(handler);

                // Create the state object, which has the receive buffer for this socket.
                StateObject state = new StateObject();
                state.workSocket = handler;

                // Setup asynchronous reading for the client's socket.
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

                // Now that we have handled the Accept, go back to listening for additional connections.
                m_Listener.BeginAccept(new AsyncCallback(AcceptCallback), m_Listener);
            }
            catch (ObjectDisposedException e)
            {
                // The ObjectDisposedException is triggered if our listener socket has been closed.
                // This is expected behaviour.
            }

        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);
            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                Logger.Write("Read {0} bytes from socket.", bytesRead);

                // Keep receiving data...
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                // If we read 0 bytes, then we've lost our connection.
                handler.Shutdown(SocketShutdown.Both);
                OnConnectionLost(handler);
                handler.Close();
            }
        }

        public delegate void ConnectionAcceptedDelegate(Socket socket);
        public static ConnectionAcceptedDelegate OnConnectionAccepted;

        public delegate void ConnectionLostDelegate(Socket socket);
        public static ConnectionLostDelegate OnConnectionLost;

        private static Socket m_Listener;
    }
}
