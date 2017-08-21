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

        public void AssociateAccount(Account acccount)
        {
            Database db = Server.GetDatabase();
            //db.GetAccount()
        }

        public string ConnectedAddress { get { return m_ConnectedToAddress; } }
        public string ConnectedHostname { get { return m_ConnectedToHostname; } }
        public Int64 Credits { get { return m_Account.Credits; } }
        public Int64 Reputation { get { return m_Account.Reputation; } }

        Socket m_Socket;
        Account m_Account;
        string m_ConnectedToAddress;
        string m_ConnectedToHostname;
    }
}
