using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neily
{
    public partial class User_form_add : Form
    {
        public User_form_add()
        {
            InitializeComponent();

            timer1.Start();

        }


        List<For_list_collection> data = new List<For_list_collection>();

        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Clever_house;Integrated Security=True";
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity == 1)
            {
                timer1.Stop();
            }
            else this.Opacity += 0.03;
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            int number = 0;

            try
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users Where User_name ='" + textBox1.Text + "' or Nick = '" + textBox2.Text + "' or ID_rfid = '" + Convert.ToInt64(textBox3.Text) + "'", connection);
                        number = Convert.ToInt32(command.ExecuteScalar());
                    }

                    if (number == 0)
                    {
                        string sqlExpression = "INSERT INTO Users (User_name, Nick, ID_rfid, Status) VALUES ('" + textBox1.Text + "','" + textBox2.Text + "','" + Convert.ToInt64(textBox3.Text) + "' , 'false')";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Запись успешно добавлена");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Запись с такими значениями уже существует");
                    }
                }
                else
                {
                    MessageBox.Show("Не все поля заполнены");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //------------------------------------------------------------------------------------------------------------

        private void comboBox1_Click(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Items.Clear();
                data.Clear();

                string sqlExpression = "SELECT * FROM Users";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {

                        while (reader.Read()) // построчно считываем данные
                        {

                            data.Add(new For_list_collection() { name = reader.GetString(0), nick = reader.GetString(1), id_rfid = reader.GetInt64(2) });

                        }
                    }

                    reader.Close();
                }

                foreach (For_list_collection s in data)
                {
                    comboBox1.Items.Add(s.nick);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int number = 0;

            try
            {
                if (comboBox1.Text != "" && textBox4.Text != "" && textBox5.Text != "")
                {

                    foreach (For_list_collection s in data)
                    {
                        if (s.nick != comboBox1.Text)
                        {
                            if (s.name == textBox5.Text || s.id_rfid == Convert.ToInt64(textBox4.Text))
                            {
                                MessageBox.Show("Такой пользователь с такими данными существует");
                                return;
                            }
                        }
                    }

                    if (number == 0)
                    {
                        string sqlExpression = "UPDATE Users SET User_name ='" + textBox5.Text + "', ID_rfid = '" + Convert.ToInt64(textBox4.Text) + "' WHERE Nick = '" + comboBox1.Text + "'";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Запись успешно изменена");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Запись с такими значениями не существует");
                    }
                }
                else
                {
                    MessageBox.Show("Не все поля заполнены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //------------------------------------------------------------------------------

        private void comboBox2_Click(object sender, EventArgs e)
        {
            try
            {
                comboBox2.Items.Clear();

                string sqlExpression = "SELECT * FROM Users";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {

                        while (reader.Read()) // построчно считываем данные
                        {
                            string name = reader.GetString(1);

                            comboBox2.Items.Add(name);
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.Text != "")
                {
                    string sqlExpression = "Delete from Users Where Nick = '" + comboBox2.Text + "'";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Запись успешно удалена");
                    }
                }
                else
                {
                    MessageBox.Show("Не выбран Nick пользователя");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            Main_menu frm = new Main_menu();
            frm.Show();

            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Main_menu frm = new Main_menu();
            frm.Show();

            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Main_menu frm = new Main_menu();
            frm.Show();

            this.Close();
        }
    }
}
