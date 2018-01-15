using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace h4x0r.Run
{
    public partial class NetworkInspector : Form
    {
        public NetworkInspector()
        {
            InitializeComponent();

            m_Graphics = CreateGraphics();
            m_Font = new Font("Terminal", 8.0f);
            m_FontBrush = new SolidBrush(Color.LimeGreen);
            m_BackgroundBrush = new SolidBrush(Color.Black);
            m_NodeBrush = new SolidBrush(Color.LimeGreen);
            m_IngressNodeBrush = new SolidBrush(Color.LightSteelBlue);
            m_EgressNodeBrush = new SolidBrush(Color.LightSteelBlue);
            m_NodeLinkPen = new Pen(Color.LightGray, 2.0f);
            m_Network = null;
        }

        public void Inspect(Network network)
        {
            m_Network = network;
        }

        private void NetworkInspector_Paint(object sender, PaintEventArgs e)
        {
            m_Graphics.FillRectangle(m_BackgroundBrush, 0, 0, Width, Height);

            if (m_Network == null)
            {
                return;
            }

            int nodeSize = 48;
            int halfNodeSize = nodeSize / 2;
            for (int y = 0; y < m_Network.Size; ++y)
            {
                for (int x = 0; x < m_Network.Size; ++x)
                {
                    Network.Node node = m_Network.Nodes[x, y];
                    float cx = nodeSize * x + nodeSize / 2.0f;
                    float cy = nodeSize * y + nodeSize / 2.0f;

                    SolidBrush brush = m_NodeBrush;
                    if (node.NodeType == Network.NodeType.Ingress)
                    {
                        brush = m_IngressNodeBrush;
                    }
                    else if (node.NodeType == Network.NodeType.Egress)
                    {
                        brush = m_EgressNodeBrush;
                    }

                    // Draw the node's center
                    m_Graphics.FillEllipse(brush, cx - 4.0f, cy - 4.0f, 8.0f, 8.0f);

                    if ((node.LinkDirections & Network.LinkDirections.Left) > 0)
                    {
                        m_Graphics.DrawLine(m_NodeLinkPen, cx, cy, cx - halfNodeSize, cy);
                    }

                    if ((node.LinkDirections & Network.LinkDirections.Right) > 0)
                    {
                        m_Graphics.DrawLine(m_NodeLinkPen, cx, cy, cx + halfNodeSize, cy);
                    }

                    if ((node.LinkDirections & Network.LinkDirections.Up) > 0)
                    {
                        m_Graphics.DrawLine(m_NodeLinkPen, cx, cy, cx, cy - halfNodeSize);
                    }

                    if ((node.LinkDirections & Network.LinkDirections.Down) > 0)
                    {
                        m_Graphics.DrawLine(m_NodeLinkPen, cx, cy, cx, cy + halfNodeSize);
                    }
                }
            }
        }

        private Graphics m_Graphics;
        private Font m_Font;
        private SolidBrush m_FontBrush;
        private SolidBrush m_BackgroundBrush;
        private SolidBrush m_NodeBrush;
        private SolidBrush m_IngressNodeBrush;
        private SolidBrush m_EgressNodeBrush;
        private Pen m_NodeLinkPen;
        private Network m_Network;
    }
}
