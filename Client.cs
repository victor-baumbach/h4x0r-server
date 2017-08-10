using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace h4x0r_server
{
    class Client
    {
        public Client(Socket socket)
        {
            m_Socket = socket;
        }

        public Socket GetSocket()
        {
            return m_Socket;
        }

        private Socket m_Socket;
    }
}
