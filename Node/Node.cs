using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h4x0r_server
{
    partial class Node
    {
        public enum Type
        {
            Gateway,
            Server,
            Terminal,
            Mainframe,
            Blackmarket,
            Decoy
        }

        public UInt64 ID { get; set; }
        public Address NodeAddress { get; set; }
        public Type NodeType { get; set; }
        public bool Terminated { get; set; }
    }
}

