using Neily.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neily
{
    public partial class Win_form_work : Form
    {

        SerialPort serialPort1 = new SerialPort();

        bool[] command_for_my_room = new bool[4];

        string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Clever_house; Integrated Security=True";


        void chk_img(int number) //Ф-ция для изменения картинки при вкл и выкл электрики
        {
            try
            {
                

                PictureBox[] img = { pictureBox1, pictureBox2, pictureBox3, pictureBox6 };

                if (command_for_my_room[number] == false)
                {
                    img[number].Image = Resources.Neily_on;
                    command_for_my_room[number] = true;

                    switch (number)
                    {
                        case 0:

                            serialPort1.Write("q"); //Включение Света в комнате
                            break;

                        case 1:

                            serialPort1.Write("w"); //Включение настольной лампы
                            break;

                        case 2:

                            serialPort1.Write("e"); //Включение телевизора
                            break;

                        case 3:

                            serialPort1.Write("r"); //Включение телевизора
                            break;
                    }

                }
                else
                {
                    img[number].Image = Resources.Neily_off;
                    command_for_my_room[number] = false;

                    switch (number)
                    {
                        case 0:

                            serialPort1.Write("a"); //Включение Света в комнате
                            break;

                        case 1:

                            serialPort1.Write("s"); //Включение настольной лампы
                            break;

                        case 2:

                            serialPort1.Write("d"); //Включение телевизора
                            break;

                        case 3:

                            serialPort1.Write("f"); //Включение телевизора
                            break;
                    }

                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*void chk_img(int number) //Ф-ция для изменения картинки при вкл и выкл электрики
        {
            PictureBox[] img = { pictureBox1, pictureBox2, pictureBox3, pictureBox6 };

            if (command_for_my_room[number] == false)
            {
                img[number].Image = Resources.Neily_on;
                command_for_my_room[number] = true;              
            }
            else
            {
                img[number].Image = Resources.Neily_off;
                command_for_my_room[number] = false;                
            }
        }*/

         void Tempriage() //Ф-ция для определения темпиратуры
         {
            try
            {
                string sqlExpression = "SELECT * FROM Temperature";
                string sqlExpression1 = "SELECT * FROM Humidity";

                string temp = null;
                string vlag = null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            temp = reader.GetString(0);
                        }
                    }

                    reader.Close();

                    command = new SqlCommand(sqlExpression1, connection);
                    reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            vlag = reader.GetString(0);
                        }
                    }
                }

                label5.Text = temp;
                label6.Text = vlag;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //------------------------------------------------------------------------------------------------------------------------

        public Win_form_work()
        {
            try
            {
                InitializeComponent();

                label5.Text = label6.Text = "";
                timer1.Start();

                serialPort1.PortName = "COM5";
                serialPort1.BaudRate = 9600;
                serialPort1.DtrEnable = true;
                serialPort1.Open();
                //serialPort1.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity == 1)
            {               
                timer1.Stop();
            }
            else this.Opacity += 0.03;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            chk_img(0);

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            chk_img(1);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            chk_img(2);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            chk_img(3);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked == false)
                {
                    panel3.Visible = false;
                }
                else
                {
                    panel3.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Win_form_work_Load(object sender, EventArgs e)
        {
            try
            {
                PictureBox[] img = { pictureBox1, pictureBox2, pictureBox3 };

                if (checkBox1.Checked == false)
                {
                    panel3.Visible = false;
                }
                else
                {
                    panel3.Visible = true;
                }

                if (checkBox2.Checked == false)
                {
                    panel4.Visible = false;
                }
                else
                {
                    panel4.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
     
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //Tempriage();            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox2.Checked == false)
                {
                    panel4.Visible = false;
                    //timer2.Stop();
                }
                else
                {
                    panel4.Visible = true;
                    //timer2.Start();
                    Tempriage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Main_menu frm = new Main_menu();
            frm.Show();

            this.Close();
            
        }
    }
}
