using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h4x0r_server
{
    partial class Node
    {
        public Node(Type type, Address address)
        {
            m_Type = type;
            m_Address = address;
            m_Terminated = false;
        }

        public enum Type
        {
            Gateway,
            Server,
            Terminal,
            Mainframe,
            Blackmarket,
            Decoy
        }

        UInt64 m_ID;
        Address m_Address;
        Type m_Type;
        bool m_Terminated;
    }
}

