using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

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
            Log("Server initialising...");
            string filename = @"C:\Users\Valman\Dropbox\Dev\h4x0r\database\database.sqlite3";
            m_DatabaseConnection = new SQLiteConnection("Data Source=" + filename + ";Version=3;");
            //string accountsSql = "select * from Accounts;";
            try
            {
                m_DatabaseConnection.Open();
                //DataSet ds = new DataSet();
                //var da = new SQLiteDataAdapter(accountsSql, m_DatabaseConnection);
                //da.Fill(ds);
                //dataGridView1.DataSource = ds.Tables[0].DefaultView;
                Log("Connection to database estabilished.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private SQLiteConnection m_DatabaseConnection;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_DatabaseConnection.Close();
        }

        private void Log(string text)
        {
            DateTime d = DateTime.Now;
            textBoxLog.Text += "[" + d.ToShortDateString() + " " + d.ToShortTimeString() + "]: " + text + Environment.NewLine;
        }
    }
}
