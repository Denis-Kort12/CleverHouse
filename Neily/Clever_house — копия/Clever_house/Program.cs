using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clever_house
{
    class Program
    {
        //EmailWork emailWork;

        static SerialPort serialPort1;

        static string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Clever_house; Integrated Security=True";

        struct Users //При поднесении карты к Rfid-модулю ищем и записываем определенного пользователя в структуру
                     //из таблицы Users
        {
            public string User_name; //Имя пользователя
            public string Nick; //Ник пользователя
            public long ID_rfid; //Уникальный номер карты
            public bool Status; //Статус нахождения пользователя дома или его отсутствие
        }

        static string returnMessage;

        static void Read_from_users(long number, out string one, out string two, out long three, out bool four) //Ф-ция для чтения
        //и-ции о конктретном пользователе при поднесении карты к Rfid-модулю
        {
            one = "";
            two = "";
            three = 0;
            four = false;

            try
            {
                string sqlExpression = "SELECT * FROM Users Where ID_rfid =" + number;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {

                        while (reader.Read()) // построчно считываем данные
                        {
                            one = reader.GetString(0);
                            two = reader.GetString(1);
                            three = reader.GetInt64(2);
                            four = reader.GetBoolean(3);
                        }
                    }

                    reader.Close();
                }
            }
            catch
            {
            }
        }

        static void Add_to_come_in_flat(bool status, string one, string two, byte[] img) //ф-ция для добавления и-ции о том
        //какой пользователь в какое время вышел или вошел в квартиру, а также сделанное фото в этот момент сохраняется
        //хранится и-ция в таблице Come_in_flat
        {
            try
            {
                string sqlExpression;

                if (status == false) sqlExpression = "Insert INTO Come_in_flat(Name, Nick, Time_in, Img) VALUES (@name, @nick, @time_in, @img)";
                else sqlExpression = "Insert INTO Come_in_flat(Name, Nick, Time_out, Img) VALUES (@name, @nick, @time_out, @img)";

                string time = Convert.ToString(DateTime.Now);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.AddWithValue("@name", one);
                    command.Parameters.AddWithValue("@nick", two);

                    if (status == false) command.Parameters.AddWithValue("@time_in", time);
                    else command.Parameters.AddWithValue("@time_out", time);

                    command.Parameters.AddWithValue("@img", img);

                    command.ExecuteNonQuery();
                }
            }
            catch
            { }
        }

        static void Add_to_come_in_flat(bool status, string one, string two) //ф-ция для добавления и-ции о том
        //какой пользователь в какое время вышел или вошел в квартиру, а также сделанное фото в этот момент сохраняется
        //хранится и-ция в таблице Come_in_flat
        {
            try
            {
                string sqlExpression;

                if (status == false) sqlExpression = "Insert INTO Come_in_flat(Name, Nick, Time_in) VALUES (@name, @nick, @time_in)";
                else sqlExpression = "Insert INTO Come_in_flat(Name, Nick, Time_out) VALUES (@name, @nick, @time_out)";

                string time = Convert.ToString(DateTime.Now);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.AddWithValue("@name", one);
                    command.Parameters.AddWithValue("@nick", two);

                    if (status == false) command.Parameters.AddWithValue("@time_in", time);
                    else command.Parameters.AddWithValue("@time_out", time);

                    command.ExecuteNonQuery();
                }
            }
            catch
            { }
        }

        static void Change_status(bool status, long number) //ф-ция для изменения статуса пользователя в таблице Users
        {
            try
            {
                string sqlExpression;

                if (status == false)
                {
                    sqlExpression = "UPDATE Users SET Status = 1 WHERE ID_rfid = " + number;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(sqlExpression, connection);

                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    sqlExpression = "UPDATE Users SET Status = 0 WHERE ID_rfid = " + number;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(sqlExpression, connection);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            { }
        }

        //---------------------------------------------------------------------------------------------------
        static void Add_temp_and_vlag(string vlag, string temp)
        {

            try
            {
                string sqlExpression;

                sqlExpression = "Insert INTO Temperature(Temp) VALUES (@temp)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.AddWithValue("@temp", temp);

                    command.ExecuteNonQuery();
                }

                sqlExpression = "Insert INTO Humidity(Vlag) VALUES (@vlag)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.AddWithValue("@vlag", vlag);

                    command.ExecuteNonQuery();
                }
            }
            catch
            { }
        }

        //---------------------------------------------------------------------------------------------------
        static void Add_smoke(int dim)
        {

            try
            {
                string sqlExpression;

                sqlExpression = "Insert INTO Smoke(Dim) VALUES (@dim)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.AddWithValue("@dim", dim);

                    command.ExecuteNonQuery();
                }


            }
            catch
            { }
        }


        static void Com_port()
        {
            try
            {
                serialPort1.Open();

                Web_work.Start();

                Users person;
               // byte[] imageData = File.ReadAllBytes(@"C:\Users\Kort\Pictures\Camera Roll\devushki_hatsune_miku_obyatiya_prikosnoveniya_pesnya_23192_3840x2400.jpg");

                Console.WriteLine("Цикл запущен");

                for (;;)
                {

                    //if (DateTime.Now.Minute == 17)
                    //{
                    //    serialPort1.Write("t");

                    //    returnMessage = serialPort1.ReadLine();

                    //    MessageBox.Show(returnMessage);
                    //}

                    if (serialPort1.IsOpen == false)
                    { 
                        serialPort1.Open();
                    }

                    returnMessage = serialPort1.ReadLine();


                    if (returnMessage.Contains("RFID"))
                    {

                        //MessageBox.Show(returnMessage);

                        string text = returnMessage;

                        string[] words = text.Split(new char[] { ' ' });

                        string number = words[0];
                        string sensor = words[1];

                        if (sensor == "RFID")
                        {
                            Schet.i_img++;

                            EmailWork emailWork;                          

                           /* try
                            {
                                Web_work.Scrin();
                                Web_work.AddScrinBd();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }*/

                            Read_from_users(Convert.ToInt64(number), out person.User_name, out person.Nick, out person.ID_rfid, out person.Status);

                            Change_status(person.Status, person.ID_rfid);

                            // Add_to_come_in_flat(person.Status, person.User_name, person.Nick, imageData);
                            Add_to_come_in_flat(person.Status, person.User_name, person.Nick);

                            try
                            {
                                Web_work.Scrin();
                                Web_work.AddScrinBd();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            emailWork = new EmailWork();
                            emailWork.SendEmail(person.ID_rfid, person.Status, person.User_name);

                           /* Web_work.Scrin();
                            Web_work.AddScrinBd();*/
                            
                            
                            //Web_work.StopScrin();


                            if (person.Status == false)
                            {
                                Golos_Neily.gls("Здравствуй " + person.User_name); //Приветствует или прощается с пользователем
                            }
                            else
                            {
                                Golos_Neily.gls("Досвидания " + person.User_name);
                            }
                        }

                    }
                    else
                    {

                    }

                    if (DateTime.Now.Minute == 30 && DateTime.Now.Second == 1)
                    {

                        if (returnMessage.Contains("Vlag"))
                        {
                            string text = returnMessage;

                            string[] words = text.Split(new char[] { ' ' });

                            Add_temp_and_vlag(words[0], words[1]);

                        }
                        else
                        {

                        }
                    }


                    if (returnMessage.Contains("DIM"))
                    {
                        Console.Beep(1000, 1000);
                        string text = returnMessage;

                        string[] words = text.Split(new char[] { ' ' });

                        Add_smoke(Convert.ToInt32(words[0]));

                    }
                    else
                    {

                    }

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                serialPort1.Close();
            }
        }
//-----------------------------------------------------------------------------------------------------------------

        static void Main(string[] args)
        {
            try
            {
                Schet.i_img = 0;

                DirectoryInfo dirInfo = new DirectoryInfo(@"C:\Users\" + Environment.UserName + @"\Pictures\Saved Pictures");

                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }

                serialPort1 = new SerialPort("COM3", 9600);

                Thread thread1 = new Thread(Com_port);
                thread1.Start();
            }
            catch
            {
            }

        }
    }

    class Golos_Neily //Класс для синтеза речи
    {
        static public void gls(string a)
        {
            try
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.SelectVoice("IVONA 2 Tatyana");//Microsoft Irina Desktop
                ss.Rate = 1;
                ss.SpeakAsync(a);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
