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
            Down,
            All = Left | Right | Up | Down
        }

        public class Node
        {
            public Node(int x, int y)
            {
                NodeType = NodeType.Generic;
                LinkDirections = LinkDirections.All;
                X = x;
                Y = y;
            }

            public NodeType NodeType { get; set; }
            public LinkDirections LinkDirections { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
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
            m_RandomGenerator = new Random();

            // Create a square network.
            Nodes = new Network.Node[settings.Size, settings.Size];
            for (int x = 0; x < settings.Size; x++)
            {
                for (int y = 0; y < settings.Size; y++)
                {
                    Nodes[x, y] = new Network.Node(x, y);
                }
            }

            // There are three sets, as the network gets generated the nodes will be 
            // moved from the undiscoveredSet to either the ingressSet or the egressSet.
            // The algorithm is based on the answer to the following question in StackOverflow:
            // https://stackoverflow.com/questions/22305644/how-to-generate-a-maze-with-more-than-one-success-full-path
            List<Network.Node> ingressSet = new List<Network.Node>();
            List<Network.Node> egressSet = new List<Network.Node>();
            List<Network.Node> undiscoveredSet = new List<Network.Node>();

            // Pick a random node on the left edge of the network as the ingress node.
            Network.Node ingressNode = Nodes[0, m_RandomGenerator.Next() % settings.Size];
            ingressNode.NodeType = Network.NodeType.Ingress;
            ingressSet.Add(ingressNode);

            // Pick a random node on the right edge of the network as the egress node.
            Network.Node egressNode = Nodes[settings.Size - 1, m_RandomGenerator.Next() % settings.Size];
            egressNode.NodeType = Network.NodeType.Egress;
            egressSet.Add(egressNode);

            // Add all other nodes to the undiscoveredSet.
            foreach (Network.Node node in Nodes)
            {
                if (node != ingressNode && node != egressNode)
                {
                    undiscoveredSet.Add(node);
                }
            }

            for (int y = 0; y < settings.Size; ++y)
            {
                for (int x = 0; x < settings.Size; ++x)
                {
                    Network.Node node = Nodes[x, y];
                    if (node.NodeType == Network.NodeType.Egress)
                    {
                        Console.Write("E ");
                    }
                    else if (node.NodeType == Network.NodeType.Ingress)
                    {
                        Console.Write("I ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }

        }

        public Network.Node[,] Nodes { get; set; }

        private Random m_RandomGenerator;
    }
}
