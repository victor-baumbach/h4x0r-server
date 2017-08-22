using System;
using System.Drawing;
using System.Windows.Forms;

namespace h4x0r_server
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_Logger = new Logger();
            Logger.AddTarget(LogToListView);

            Server.Initialise();
            Server.OnClientAdded = OnClientAdded;
            Server.OnClientRemoved = OnClientRemoved;
            Server.OnClientLogin = OnClientLogin;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.RemoveTarget(LogToListView);
            Server.Shutdown();
        }

        private void LogToListView(Logger.Level level, string text)
        {
            // Handle case of LogToListView being called outside the main thread.
            if (InvokeRequired)
            {
                Invoke(new Action<Logger.Level, string>(LogToListView), new object[] { level, text });
                return;
            }

            // Prevent the list from growing forever.
            if (listViewLog.Items.Count > 200)
            {
                listViewLog.Items.RemoveAt(0);
            }

            // Colour based on warning level
            ListViewItem item = new ListViewItem(text);
            if (level == Logger.Level.Warning)
            {
                item.BackColor = Color.Orange;
            }
            else if (level == Logger.Level.Error)
            {
                item.BackColor = Color.Red;
            }

            listViewLog.Items.Add(item);

            // Automatically scroll to the bottom of the list as more entries are added.
            listViewLog.TopItem = listViewLog.Items[listViewLog.Items.Count - 1];
        }

        private void OnClientAdded(Client client)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Client>(OnClientAdded), new object[] { client });
                return;
            }

            ListViewItem item = new ListViewItem(new[] { client.GetFriendlyAddress(), "<not logged in>" });
            listViewClients.Items.Add(item);
        }

        private void OnClientRemoved(Client client)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Client>(OnClientRemoved), new object[] { client });
                return;
            }

            string addressToRemove = client.GetFriendlyAddress();
            for (int idx = 0; idx < listViewClients.Items.Count; ++idx)
            {
                if (listViewClients.Items[idx].SubItems[0].Text == addressToRemove)
                {
                    listViewClients.Items.RemoveAt(idx);
                    break;
                }
            }
        }

        private void OnClientLogin(Client client)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Client>(OnClientLogin), new object[] { client });
                return;
            }

            string addressToUpdate = client.GetFriendlyAddress();
            for (int idx = 0; idx < listViewClients.Items.Count; ++idx)
            {
                if (listViewClients.Items[idx].SubItems[0].Text == addressToUpdate)
                {
                    listViewClients.Items[idx].SubItems[1].Text = client.Account.Username;
                    break;
                }
            }
        }

        private Logger m_Logger;
    }
}
