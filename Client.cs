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

        // Should be once after the Client has logged in.
        public void AssociateAccount(Account account)
        {
            m_Account = account;
        }

        public Account Account { get { return m_Account; } }
        public Node Node { get { return m_Node; } }
        public string ConnectedAddress { get { return m_ConnectedToAddress; } }
        public string ConnectedHostname { get { return m_ConnectedToHostname; } }
        public Int64 Credits { get { return m_Account == null ? 0 : m_Account.Credits; } }
        public Int64 Reputation { get { return m_Account == null ? 0 : m_Account.Reputation; } }

        Socket m_Socket;
        Account m_Account = null;
        Node m_Node = null;
        string m_ConnectedToAddress;
        string m_ConnectedToHostname;
    }
}
