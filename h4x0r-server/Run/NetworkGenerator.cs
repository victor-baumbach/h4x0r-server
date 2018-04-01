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
                Size = 10;
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
                // The lists of notes to process need to be shuffled so nodes aren't discovered
                // in a biased direction.
                ShuffleList(ref ingressNodesToProcess);
                ShuffleList(ref egressNodesToProcess);

                ProcessNode(ingressNodesToProcess, ingressSet);
                ProcessNode(egressNodesToProcess, egressSet);
            }

            ConnectSets(ingressSet, egressSet);
            PrototypeOutput();
        }

        private void PrototypeOutput()
        {
            for (int x = 0; x < m_Settings.Size; x++)
            {
                for (int y = 0; y < m_Settings.Size; y++)
                {
                    if (Nodes[x, y].LinkDirections == Network.LinkDirections.All)
                    {
                        Console.WriteLine(String.Format("grid_data[{0}][{1}].set_link_directions({2})", x, y, "grid_node_script.GridLinkDirections.All"));
                    }
                    else
                    {
                        if ((Nodes[x, y].LinkDirections & Network.LinkDirections.Down) > 0)
                        {
                            Console.WriteLine(String.Format("grid_data[{0}][{1}].set_link_directions({2})", x, y, "grid_node_script.GridLinkDirections.Down"));
                        }
                        if ((Nodes[x, y].LinkDirections & Network.LinkDirections.Left) > 0)
                        {
                            Console.WriteLine(String.Format("grid_data[{0}][{1}].set_link_directions({2})", x, y, "grid_node_script.GridLinkDirections.Left"));
                        }
                        if ((Nodes[x, y].LinkDirections & Network.LinkDirections.None) > 0)
                        {
                            Console.WriteLine(String.Format("grid_data[{0}][{1}].set_link_directions({2})", x, y, "grid_node_script.GridLinkDirections.None"));
                        }
                        if ((Nodes[x, y].LinkDirections & Network.LinkDirections.Right) > 0)
                        {
                            Console.WriteLine(String.Format("grid_data[{0}][{1}].set_link_directions({2})", x, y, "grid_node_script.GridLinkDirections.Right"));
                        }
                        if ((Nodes[x, y].LinkDirections & Network.LinkDirections.Up) > 0)
                        {
                            Console.WriteLine(String.Format("grid_data[{0}][{1}].set_link_directions({2})", x, y, "grid_node_script.GridLinkDirections.Up"));
                        }
                    }
                }
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

            // Randomise new links. Always try to establish at least one.
            if (directions != Network.LinkDirections.None)
            {
                List<Network.LinkDirections> directionsList = new List<Network.LinkDirections>();
                if ((directions & Network.LinkDirections.Left) != 0) directionsList.Add(Network.LinkDirections.Left);
                if ((directions & Network.LinkDirections.Right) != 0) directionsList.Add(Network.LinkDirections.Right);
                if ((directions & Network.LinkDirections.Up) != 0) directionsList.Add(Network.LinkDirections.Up);
                if ((directions & Network.LinkDirections.Down) != 0) directionsList.Add(Network.LinkDirections.Down);

                ShuffleList(ref directionsList);

                directions = Network.LinkDirections.None;
                directions |= directionsList[0];
                if (directionsList.Count > 1 /*&& (m_RandomGenerator.Next() % 2) == 0*/) directions |= directionsList[1];

                if (directionsList.Count <= 1)
                {
                    int a = 0;
                }
            }

            return directions;
        }

        private void ShuffleList<T>(ref List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = m_RandomGenerator.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void ConnectSets(List<Network.Node> ingressSet, List<Network.Node>egressSet)
        {
            List<PotentialConnection> potentialConnections = new List<PotentialConnection>();

            // Create a grid containing all the nodes in the egress set so we can quickly
            // check nodes without having to perform a linear search.
            Network.Node[,] egressGrid = new Network.Node[m_Settings.Size, m_Settings.Size];
            foreach (Network.Node node in egressSet)
            {
                egressGrid[node.X, node.Y] = node;
            }

            foreach (Network.Node node in ingressSet)
            {
                if (node.X - 1 >= 0 && egressGrid[node.X - 1, node.Y] != null)
                {
                    potentialConnections.Add(new PotentialConnection(node, egressGrid[node.X - 1, node.Y]));
                }

                if (node.X + 1 < m_Settings.Size && egressGrid[node.X + 1, node.Y] != null)
                {
                    potentialConnections.Add(new PotentialConnection(node, egressGrid[node.X + 1, node.Y]));
                }

                if (node.Y - 1 >= 0 && egressGrid[node.X, node.Y - 1] != null)
                {
                    potentialConnections.Add(new PotentialConnection(node, egressGrid[node.X, node.Y - 1]));
                }

                if (node.Y + 1 < m_Settings.Size && egressGrid[node.X, node.Y + 1] != null)
                {
                    potentialConnections.Add(new PotentialConnection(node, egressGrid[node.X, node.Y + 1]));
                }
            }

            ShuffleList(ref potentialConnections);
            int connectionsToEstablish = Math.Min(m_RandomGenerator.Next(m_Settings.Size - 1) + 1, potentialConnections.Count);
            for (int i = 0; i < connectionsToEstablish; ++i)
            {
                EstablishConnection(potentialConnections[i]);
            }
        }

        private void EstablishConnection(PotentialConnection pc)
        {
            if (pc.NodeA.X < pc.NodeB.X)
            {
                pc.NodeA.LinkDirections |= Network.LinkDirections.Right;
                pc.NodeB.LinkDirections |= Network.LinkDirections.Left;
            }
            else if (pc.NodeA.X > pc.NodeB.X)
            {
                pc.NodeA.LinkDirections |= Network.LinkDirections.Left;
                pc.NodeB.LinkDirections |= Network.LinkDirections.Right;
            }
            else if (pc.NodeA.Y < pc.NodeB.Y)
            {
                pc.NodeA.LinkDirections |= Network.LinkDirections.Down;
                pc.NodeB.LinkDirections |= Network.LinkDirections.Up;
            }
            else if (pc.NodeA.Y > pc.NodeB.Y)
            {
                pc.NodeA.LinkDirections |= Network.LinkDirections.Up;
                pc.NodeB.LinkDirections |= Network.LinkDirections.Down;
            }
        }

        private class PotentialConnection
        {
            public PotentialConnection(Network.Node nodeA, Network.Node nodeB)
            {
                NodeA = nodeA;
                NodeB = nodeB;
            }

            public Network.Node NodeA { get; set; }
            public Network.Node NodeB { get; set; }

            public static bool operator ==(PotentialConnection pc1, PotentialConnection pc2)
            {
                return ((pc1.NodeA == pc2.NodeA || pc1.NodeA == pc2.NodeB) && (pc1.NodeB == pc2.NodeA || pc1.NodeB == pc2.NodeB));
            }

            public static bool operator !=(PotentialConnection pc1, PotentialConnection pc2)
            {
                return !((pc1.NodeA == pc2.NodeA || pc1.NodeA == pc2.NodeB) && (pc1.NodeB == pc2.NodeA || pc1.NodeB == pc2.NodeB));
            }

            public override bool Equals(object obj)
            {
                return (this == (PotentialConnection)obj);
            }

            public override int GetHashCode()
            {
                return NodeA.GetHashCode() + NodeB.GetHashCode();
            }
        }

        public Network.Node[,] Nodes { get; set; }
        private bool[,] DiscoveredState { get; set; }

        private Random m_RandomGenerator;
        private Settings m_Settings;
    }
}
