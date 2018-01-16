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
            DiscoveredState = new bool[settings.Size, settings.Size];
            for (int x = 0; x < settings.Size; x++)
            {
                for (int y = 0; y < settings.Size; y++)
                {
                    Nodes[x, y] = new Network.Node(x, y);
                    DiscoveredState[x, y] = false;
                }
            }

            // The algorithm is based on the answer to the following question in StackOverflow:
            // https://stackoverflow.com/questions/22305644/how-to-generate-a-maze-with-more-than-one-success-full-path
            List<Network.Node> ingressSet = new List<Network.Node>();
            List<Network.Node> egressSet = new List<Network.Node>();

            // Pick a random node on the left edge of the network as the ingress node.
            Network.Node ingressNode = Nodes[0, m_RandomGenerator.Next() % settings.Size];
            ingressNode.NodeType = Network.NodeType.Ingress;
            ingressSet.Add(ingressNode);

            // Pick a random node on the right edge of the network as the egress node.
            Network.Node egressNode = Nodes[settings.Size - 1, m_RandomGenerator.Next() % settings.Size];
            egressNode.NodeType = Network.NodeType.Egress;
            egressSet.Add(egressNode);

            List<Network.Node> ingressNodesToProcess = new List<Network.Node>();
            ingressNodesToProcess.Add(ingressNode);
            List<Network.Node> egressNodesToProcess = new List<Network.Node>();
            egressNodesToProcess.Add(egressNode);
            while (ingressNodesToProcess.Count != 0 && egressNodesToProcess.Count != 0)
            {
                ProcessNode(ingressNodesToProcess, ingressSet);
                ProcessNode(egressNodesToProcess, egressSet);
            }
        }

        private void ProcessNode(List<Network.Node> setToProcess, List<Network.Node> setToAddTo)
        {
            if (setToProcess.Count == 0)
            {
                return;
            }

            Network.Node node = setToProcess[0];
            setToProcess.RemoveAt(0);

            // TODO: Ensure all nodes have at least one link established.
            Network.LinkDirections directions = GenerateLinkDirections(node);

            if ((directions & Network.LinkDirections.Left) != 0)
            {
                Network.Node otherNode = Nodes[node.X - 1, node.Y];
                node.LinkDirections |= Network.LinkDirections.Left;
                otherNode.LinkDirections |= Network.LinkDirections.Right;
                DiscoveredState[otherNode.X, otherNode.Y] = true;
                setToAddTo.Add(otherNode);
                setToProcess.Add(otherNode);
            }

            if ((directions & Network.LinkDirections.Right) != 0)
            {
                Network.Node otherNode = Nodes[node.X + 1, node.Y];
                node.LinkDirections |= Network.LinkDirections.Right;
                otherNode.LinkDirections |= Network.LinkDirections.Left;
                DiscoveredState[otherNode.X, otherNode.Y] = true;
                setToAddTo.Add(otherNode);
                setToProcess.Add(otherNode);
            }

            if ((directions & Network.LinkDirections.Up) != 0)
            {
                Network.Node otherNode = Nodes[node.X, node.Y - 1];
                node.LinkDirections |= Network.LinkDirections.Up;
                otherNode.LinkDirections |= Network.LinkDirections.Down;
                DiscoveredState[otherNode.X, otherNode.Y] = true;
                setToAddTo.Add(otherNode);
                setToProcess.Add(otherNode);
            }

            if ((directions & Network.LinkDirections.Down) != 0)
            {
                Network.Node otherNode = Nodes[node.X, node.Y + 1];
                node.LinkDirections |= Network.LinkDirections.Down;
                otherNode.LinkDirections |= Network.LinkDirections.Up;
                DiscoveredState[otherNode.X, otherNode.Y] = true;
                setToAddTo.Add(otherNode);
                setToProcess.Add(otherNode);
            }
        }

        // Figure out which directions this node should link to.
        // Normally a node will already have at least one direction connected (with the exception of ingress and egress nodes).
        private Network.LinkDirections GenerateLinkDirections(Network.Node node)
        {
            Network.LinkDirections directions = Network.LinkDirections.All;

            // Don't consider any pre-existing links to this node.
            if ((node.LinkDirections & Network.LinkDirections.Left) != 0) directions &= ~Network.LinkDirections.Left;
            if ((node.LinkDirections & Network.LinkDirections.Right) != 0) directions &= ~Network.LinkDirections.Right;
            if ((node.LinkDirections & Network.LinkDirections.Up) != 0) directions &= ~Network.LinkDirections.Up;
            if ((node.LinkDirections & Network.LinkDirections.Down) != 0) directions &= ~Network.LinkDirections.Down;

            // If we are at the edge of the network, don't consider links which would take us out of bounds.
            if (node.X == 0) directions &= ~Network.LinkDirections.Left;
            else if (node.X == m_Settings.Size - 1) directions &= ~Network.LinkDirections.Right;
            if (node.Y == 0) directions &= ~Network.LinkDirections.Up;
            else if (node.Y == m_Settings.Size - 1) directions &= ~Network.LinkDirections.Down;

            // Skip potential links to already discovered nodes.
            if ((directions & Network.LinkDirections.Left) != 0 && DiscoveredState[node.X - 1, node.Y]) directions &= ~Network.LinkDirections.Left;
            if ((directions & Network.LinkDirections.Right) != 0 && DiscoveredState[node.X + 1, node.Y]) directions &= ~Network.LinkDirections.Right;
            if ((directions & Network.LinkDirections.Up) != 0 && DiscoveredState[node.X, node.Y - 1]) directions &= ~Network.LinkDirections.Up;
            if ((directions & Network.LinkDirections.Down) != 0 && DiscoveredState[node.X, node.Y + 1]) directions &= ~Network.LinkDirections.Down;

            return directions;
        }

        public Network.Node[,] Nodes { get; set; }
        private bool[,] DiscoveredState { get; set; }

        private Random m_RandomGenerator;
        private Settings m_Settings;
    }
}
