using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h4x0r.Run
{
    public class Network
    {
        public enum NodeType
        {
            Ingress,    // A run starts at the ingress node.
            Egress,     // And must end at the egress node.
            ICE,        // ICE nodes can only be captured once the respective Icebreaker finishes running.
            Generic,    // Generic nodes must be captured.
            Link        // Can be traversed but doesn't have to be captured.
        }

        [Flags]
        public enum LinkDirections
        {
            None = 0,
            Left,
            Right,
            Up,
            Down
        }

        public class Node
        {
            public Node()
            {
                NodeType = NodeType.Generic;
                LinkDirections = LinkDirections.None;
            }

            public NodeType NodeType { get; set; }
            public LinkDirections LinkDirections { get; set; }
        }
    }

    public class NetworkGenerator
    {
        public class Settings
        {
            public Settings()
            {
                Size = 8;
            }

            public int Size { get; set; }
        }

        public NetworkGenerator(Settings settings)
        {
            Nodes = new Network.Node[settings.Size, settings.Size];
        }

        public Network.Node[,] Nodes { get; set; }
    }
}
