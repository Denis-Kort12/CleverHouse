using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO.Ports;

namespace Neily
{
    public partial class Golos_work : Form
    {
        static Label l;
        static NumericUpDown nod;
        static string raspoz_command;

        SerialPort serialPort1 = new SerialPort();

        public Golos_work()
        {
            try
            {
                InitializeComponent();

                timer1.Start();

                serialPort1.PortName = "COM5";
                serialPort1.BaudRate = 9600;
                serialPort1.DtrEnable = true;
                serialPort1.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
  

        void recEnginer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            try
            {
                l.Text = e.Result.Text + e.Result.Confidence.ToString();

                try
                {
                    if (e.Result.Confidence > Convert.ToSingle(nod.Value))
                    {
                        switch (e.Result.Text)
                        {
                            case "Включи свет в санузле":
                                // Golos_Neily.gls("Так намного лучше");
                                Golos_Neily.gls(Golos_Neily.rand_on());

                                serialPort1.Write("e");

                                break;

                            case "Включи свет в ванне":
                                //Golos_Neily.gls("Теперь темно и страшно");
                                Golos_Neily.gls(Golos_Neily.rand_off());
                                serialPort1.Write("q");

                                break;

                            case "Включи свет в гостинной":
                                // Golos_Neily.gls("Так намного лучше");
                                Golos_Neily.gls(Golos_Neily.rand_on());

                                serialPort1.Write("w");

                                break;

                            case "Включи свет на кухне":
                                //Golos_Neily.gls("Теперь темно и страшно");
                                Golos_Neily.gls(Golos_Neily.rand_off());
                                serialPort1.Write("r");

                                break;

                            case "Выключи свет в санузле":
                                // Golos_Neily.gls("Так намного лучше");
                                Golos_Neily.gls(Golos_Neily.rand_on());

                                serialPort1.Write("d");

                                break;

                            case "Выключи свет в ванне":
                                //Golos_Neily.gls("Теперь темно и страшно");
                                Golos_Neily.gls(Golos_Neily.rand_off());
                                serialPort1.Write("a");

                                break;

                            case "Выключи свет в гостинной":
                                // Golos_Neily.gls("Так намного лучше");
                                Golos_Neily.gls(Golos_Neily.rand_on());

                                serialPort1.Write("s");

                                break;

                            case "Выключи свет на кухне":
                                //Golos_Neily.gls("Теперь темно и страшно");
                                Golos_Neily.gls(Golos_Neily.rand_off());
                                serialPort1.Write("f");

                                break;

                            case "Включи весь свет":
                                //Golos_Neily.gls("Теперь темно и страшно");
                                Golos_Neily.gls(Golos_Neily.rand_off());
                                serialPort1.Write("z");

                                break;

                            case "Выключи весь свет":
                                //Golos_Neily.gls("Теперь темно и страшно");
                                Golos_Neily.gls(Golos_Neily.rand_off());
                                serialPort1.Write("x");

                                break;
                        }

                    }
                }
                catch
                {
                    Golos_Neily.gls("Помоги, не могу это сделать");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                l = label1;
                nod = numericUpDown1;

                Work_Neily m = new Work_Neily();

                int kol = m.kol_of_command; //Колличество комманд в таблице "Computer_mode"
                string[] command_for_computer_work = new string[kol]; //Объявление массива команд таблицы "Computer_mode"

                m.Read_Work_Command(out command_for_computer_work); //Вызов метода из класса Work_Neily
                                                                    //для считывания и записи команд из 
                                                                    //таблицы "Computer_mode" в массив.

                CultureInfo ci = new CultureInfo("ru-ru");
                SpeechRecognitionEngine recEnginer = new SpeechRecognitionEngine(ci);

                Choices commands = new Choices();
                commands.Add(command_for_computer_work); //Команды для Нейли в режиме работы

                GrammarBuilder gBuilder = new GrammarBuilder();
                gBuilder.Append(commands);

                Grammar grammar = new Grammar(gBuilder);
                recEnginer.LoadGrammar(grammar);

                recEnginer.SetInputToDefaultAudioDevice();

                recEnginer.RecognizeAsync(RecognizeMode.Multiple);

                recEnginer.SpeechRecognized += recEnginer_SpeechRecognized;
                //recEnginer.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                serialPort1.Write("1");
                serialPort1.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

                if (this.Opacity == 1)
                {
                    timer1.Stop();
                }
                else this.Opacity += 0.03;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Main_menu frm = new Main_menu();
                frm.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    class Work_Neily //Класс для чтения комманд из БД "Neily_info"
    {
        string connectionString = "Data Source= .\\SQLEXPRESS; Initial Catalog=Clever_house; Integrated Security=True";
        string sqlExpression_Computer_mode = "SELECT * FROM Computer_mode";
        /*string sqlExpression_Home_mode_light_on = "SELECT * FROM Answer_for_light_on";
        string sqlExpression_Home_mode_light_off = "SELECT * FROM Answer_for_light_off";*/

        public int kol_of_command;
        
        public Work_Neily() //Метод для определения колличества записей в таблицах БД "Neily_info"
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand com = new SqlCommand("SELECT count(*) FROM Computer_mode", connection);
                    kol_of_command = Convert.ToInt32(com.ExecuteScalar());
                }

                /* using (SqlConnection connection = new SqlConnection(connectionString))
                 {
                     connection.Open();

                     SqlCommand com = new SqlCommand("SELECT count(*) FROM Answer_for_light_on", connection);
                     kol_of_command = Convert.ToInt32(com.ExecuteScalar());
                 }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Read_Work_Command(out string[] command_for_work) //Метод для чтения и записии команд из таблиц в БД
        {
            try
            {
                command_for_work = new string[kol_of_command];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression_Computer_mode, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        int i = 0;

                        while (reader.Read()) // построчно считываем данные
                        {
                            command_for_work[i] = Convert.ToString(reader.GetValue(0));
                            i++;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                command_for_work = null;
                MessageBox.Show(ex.Message);  
            }
        }

    }


}

