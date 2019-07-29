using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neily
{
    public partial class ShowInWf : Form
    {

        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Clever_house;Integrated Security=True";
        string commandString;

        public ShowInWf()
        {
            InitializeComponent();
        }

        private void ShowInWf_Load(object sender, EventArgs e)
        {
            try
            {
                commandString = "Select *from Come_in_flat";

                List<string[]> data = new List<string[]>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    SqlCommand command = new SqlCommand(commandString, con);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        data.Add(new string[4]);

                        data[data.Count - 1][0] = reader[1].ToString();
                        data[data.Count - 1][1] = reader[2].ToString();
                        data[data.Count - 1][2] = reader[3].ToString();
                        data[data.Count - 1][3] = reader[4].ToString();
                    }

                    reader.Close();

                }

                dataGridView1.ColumnCount = 4;
                dataGridView1.ColumnHeadersVisible = true;

                // Set the column header style.
                DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

                columnHeaderStyle.BackColor = Color.Beige;
                columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
                dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

                dataGridView1.Columns[0].Name = "Имя";
                dataGridView1.Columns[1].Name = "Ник";
                dataGridView1.Columns[2].Name = "Время входа";
                dataGridView1.Columns[3].Name = "Время выхода";

                foreach (string[] s in data)
                {
                    dataGridView1.Rows.Add(s);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int number = Convert.ToInt32(e.RowIndex) + 1;

                byte[] img = null;

                commandString = "Select *from Come_in_flat Where ID_come_in_flat = " + number;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    SqlCommand command = new SqlCommand(commandString, con);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (string.IsNullOrEmpty(reader.GetValue(5).ToString()) != true)
                        {
                            img = (byte[])reader.GetValue(5);
                        }
                        else
                        {
                            MessageBox.Show("Для данной записи не было сделано фотографии");
                            pictureBox1.Image = null;
                        }

                    }

                    reader.Close();
                }

                if (img != null)
                {

                    MemoryStream mStream = new MemoryStream();

                    mStream.Write(img, 0, img.Length);
                    Bitmap bm = new Bitmap(mStream, false);
                    mStream.Dispose();

                    pictureBox1.Image = bm;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main_menu frm = new Main_menu();
            frm.Show();

            this.Close();
        }
    }   
}

