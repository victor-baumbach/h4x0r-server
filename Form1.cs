using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private Logger m_Logger;
    }
}
