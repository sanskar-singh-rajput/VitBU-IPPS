using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OtpNet;
using MySql.Data.MySqlClient;

namespace Capstone_Project_ToTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Timer timer = new Timer();
            timer.Interval = (2000); // 10 secs
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            label1.Visible = false;
            dataGridView1.Rows.Clear();
           
            string connection_s = @"server=localhost;userid=root;password=root;database=capstone";
            var con = new MySqlConnection(connection_s);
            con.Open();
            string query = $"select name, totp_secret from entity;";

            List<string> ent_name = new List<string>();
            List<string> ent_totp = new List<string>();

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ent_name.Add(reader.GetString("name"));
                ent_totp.Add(reader.GetString("totp_secret"));
            }
            reader.Close();

            for (int i = 0; i < ent_totp.Count; i++)
            {
                Totp totp = new Totp(Base32Encoding.ToBytes(ent_totp[i]));
                dataGridView1.Rows.Add(ent_name[i], totp.ComputeTotp(), totp.RemainingSeconds());
            }
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Clipboard.SetText(dataGridView1[1, e.RowIndex].Value.ToString());
            label1.Visible = true;
        }
    }
}
