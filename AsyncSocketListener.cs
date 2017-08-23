﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using FlatBuffers;
using h4x0r.MessagesInternal;

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
    }

    public class AsyncSocketListener
    {
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

                Logger.Write(Logger.Level.Info, "Server listening for connections on port {0}.", localEndPoint.Port);

                // Start an asynchronous socket to listen for connections.  
                m_Listener.BeginAccept(new AsyncCallback(AcceptCallback), m_Listener);
            }
            catch (Exception e)
            {
                Logger.Write(Logger.Level.Error, e.ToString());
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
                Logger.Write(Logger.Level.Info, "Server no longer listening for connections.");
            }
        }

        private static void AcceptCallback(IAsyncResult ar)
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
            catch (ObjectDisposedException)
            {
                // The ObjectDisposedException is triggered if our listener socket has been closed.
                // This is expected behaviour.
            }

        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            try
            {
                int bytesRead = handler.EndReceive(ar);
                bool terminateConnection = (bytesRead > 0);
                if (!terminateConnection)
                {
                    // Every message is prefixed with the length of the flatbuffer.
                    // It is possible for multiple messages to be concatenated.
                    int bytesToProcess = bytesRead;
                    while (bytesToProcess > 0)
                    {
                        int messageLength = h4x0r.Messages.GetMessageLength(state.buffer);
                        bytesToProcess -= 2; // length of the prefix, 2 bytes (short)
                        
                        // Malformed package, this should never happen.
                        if (messageLength <= 0 || messageLength > bytesToProcess)
                        {
                            Logger.Write(Logger.Level.Error, "Message of invalid size received, terminating connection.");
                            terminateConnection = true;
                            break;
                        }

                        // Create a message buffer which does not contain the length prefix
                        // so it can be cleanly evaluated by code which expects a flatbuffer.
                        byte[] messageBuffer = new byte[messageLength];
                        Buffer.BlockCopy(state.buffer, 2, messageBuffer, 0, messageLength);

                        // We expect anything being read from this socket to be a message.
                        // If it isn't, kill the connection.
                        if (OnMessageReceived(handler, messageBuffer) == false)
                        {
                            Logger.Write(Logger.Level.Error, "Invalid message received, terminating connection.");
                            terminateConnection = true;
                            break;
                        }

                        bytesToProcess -= messageLength;
                    }
                }

                if (terminateConnection)
                {
                    handler.Shutdown(SocketShutdown.Both);
                    OnConnectionLost(handler);
                    handler.Close();
                }
                else
                {
                    // Keep receiving data...
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                OnConnectionLost(handler);
            }
        }

        public static void Send(Socket handler, byte[] message)
        {
            // Begin sending the data to the remote device.  
            handler.BeginSend(message, 0, message.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                handler.EndSend(ar);
            }
            catch (Exception e)
            {
                Logger.Write(Logger.Level.Error, e.ToString());
            }
        }

        public delegate void ConnectionAcceptedDelegate(Socket socket);
        public static ConnectionAcceptedDelegate OnConnectionAccepted;

        public delegate void ConnectionLostDelegate(Socket socket);
        public static ConnectionLostDelegate OnConnectionLost;

        public delegate bool MessageReceivedDelegate(Socket socket, byte[] message);
        public static MessageReceivedDelegate OnMessageReceived;

        private static Socket m_Listener;
    }
}
