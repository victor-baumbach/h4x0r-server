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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_Logger = new Logger();
            Logger.AddTarget(LogToTextBox);

            Server.Initialise();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.RemoveTarget(LogToTextBox);
            Server.Shutdown();
        }

        private void LogToTextBox(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(LogToTextBox), new object[] { text });
                return;
            }

            textBoxLog.Text += text;
        }

        private Logger m_Logger;
    }
}
