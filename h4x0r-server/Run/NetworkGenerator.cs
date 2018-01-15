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
            Left = 1,
            Right = 2,
            Up = 4,
            Down = 8,
            All = Left | Right | Up | Down
        }

        public class Node
        {
            public Node(int x, int y)
            {
                NodeType = NodeType.Generic;
                LinkDirections = LinkDirections.None;
                X = x;
                Y = y;
            }

            public NodeType NodeType { get; set; }
            public LinkDirections LinkDirections { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

        public Network(NetworkGenerator.Settings generatorSettings)
        {
            m_NetworkGenerator = new NetworkGenerator(generatorSettings);
            Size = generatorSettings.Size;
            Nodes = m_NetworkGenerator.Nodes;
        }

        public int Size { get; }
        public Node[,] Nodes { get; }
        private NetworkGenerator m_NetworkGenerator;
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
            m_Settings = settings;

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

            List<Network.Node> ingressNodesToProcess = new List<Network.Node>();
            ingressNodesToProcess.Add(ingressNode);
            List<Network.Node> egressNodesToProcess = new List<Network.Node>();
            egressNodesToProcess.Add(egressNode);
            while (undiscoveredSet.Count > 0 && ingressNodesToProcess.Count != 0 && egressNodesToProcess.Count != 0)
            {
                ProcessNode(ingressNodesToProcess, ingressSet, undiscoveredSet);
                ProcessNode(egressNodesToProcess, egressSet, undiscoveredSet);
            }
        }

        private void ProcessNode(List<Network.Node> setToProcess, List<Network.Node> setToAddTo, List<Network.Node> undiscoveredSet)
        {
            if (setToProcess.Count == 0)
            {
                return;
            }

            Network.Node node = setToProcess[0];
            setToProcess.RemoveAt(0);

            // TODO: Ensure all nodes have at least one link established.

            if (m_RandomGenerator.Next() % 2 == 0 && (node.X - 1) >= 0 && undiscoveredSet.Contains(Nodes[node.X - 1, node.Y]))
            {
                Network.Node otherNode = Nodes[node.X - 1, node.Y];
                node.LinkDirections |= Network.LinkDirections.Left;
                otherNode.LinkDirections |= Network.LinkDirections.Right;
                undiscoveredSet.Remove(otherNode);
                setToAddTo.Add(otherNode);
                setToProcess.Add(otherNode);
            }

            if (m_RandomGenerator.Next() % 2 == 0 && (node.X + 1) < m_Settings.Size && undiscoveredSet.Contains(Nodes[node.X + 1, node.Y]))
            {
                Network.Node otherNode = Nodes[node.X + 1, node.Y];
                node.LinkDirections |= Network.LinkDirections.Right;
                otherNode.LinkDirections |= Network.LinkDirections.Left;
                undiscoveredSet.Remove(otherNode);
                setToAddTo.Add(otherNode);
                setToProcess.Add(otherNode);
            }

            if (m_RandomGenerator.Next() % 2 == 0 && (node.Y - 1) >= 0 && undiscoveredSet.Contains(Nodes[node.X, node.Y - 1]))
            {
                Network.Node otherNode = Nodes[node.X, node.Y - 1];
                node.LinkDirections |= Network.LinkDirections.Up;
                otherNode.LinkDirections |= Network.LinkDirections.Down;
                undiscoveredSet.Remove(otherNode);
                setToAddTo.Add(otherNode);
                setToProcess.Add(otherNode);
            }

            if (m_RandomGenerator.Next() % 2 == 0 && (node.Y + 1) < m_Settings.Size && undiscoveredSet.Contains(Nodes[node.X, node.Y + 1]))
            {
                Network.Node otherNode = Nodes[node.X, node.Y + 1];
                node.LinkDirections |= Network.LinkDirections.Down;
                otherNode.LinkDirections |= Network.LinkDirections.Up;
                undiscoveredSet.Remove(otherNode);
                setToAddTo.Add(otherNode);
                setToProcess.Add(otherNode);
            }
        }

        public Network.Node[,] Nodes { get; set; }

        private Random m_RandomGenerator;
        private Settings m_Settings;
    }
}
