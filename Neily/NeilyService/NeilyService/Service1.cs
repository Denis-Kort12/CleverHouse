using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Data.SqlClient;
using System.Speech.Synthesis;
using System.Net;
using System.Threading;
using MySql.Data.MySqlClient;

namespace NeilyService
{
    public partial class NeilyService : ServiceBase
    {
        string Connect = "Server=neily.ru;Port=3306;Database=clever_house;User Id=id5542083_kort;Password={Password};SslMode = none";
        string CommandText = "Select *from Light_status";

        static int provtoilet;
        static int provbath;
        static int provsittingroom;
        static int provkitchen;

        static int toilet;
        static int bath;
        static int sittingroom;
        static int kitchen;

        bool zahod;

        SerialPort serialPort1;

        public NeilyService()
        {
            InitializeComponent();
        }

        private System.Timers.Timer timer1;

        protected override void OnStart(string[] args)
        {
            try
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStart.txt");

                File.Create(AppDomain.CurrentDomain.BaseDirectory + "Portopen.txt");

                try
                {
                    Read_Light_from_bd(out provtoilet, out provbath, out provsittingroom, out provkitchen);
                }
                catch (Exception ex)
                {
                    file(ex.Message);
                }
               
                //Создаем таймер и выставляем его параметры
                this.timer1 = new System.Timers.Timer();
                this.timer1.Enabled = true;
                //Интервал 10000мс - 10с.
                this.timer1.Interval = 4000;
                this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
                this.timer1.AutoReset = true;
                this.timer1.Start();
            }
            catch
            {                
            }
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Read_Light_from_bd(out toilet, out bath, out sittingroom, out kitchen);

                if (toilet == provtoilet && bath == provbath &&
                    sittingroom == provsittingroom && kitchen == provkitchen)
                {
                    file("Нет изменений");
                }
                else
                {
                    file("!!туалет " + toilet + " ванна " + bath + " гостинная " + sittingroom + " кухня " + kitchen);

                    provbath = bath;
                    provkitchen = kitchen;
                    provsittingroom = sittingroom;
                    provtoilet = toilet;

                    try
                    {
                        serialPort1 = new SerialPort("COM5", 9600);
                        serialPort1.Open();

                        if (toilet == 1)
                        {
                            StatusLight(2, serialPort1);
                        }
                        else
                        {
                            StatusLight(6, serialPort1);
                        }

                        if (bath == 1)
                        {
                            StatusLight(0, serialPort1);
                        }
                        else
                        {
                            StatusLight(4, serialPort1);
                        }

                        if (sittingroom == 1)
                        {
                            StatusLight(1, serialPort1);
                        }
                        else
                        {
                            StatusLight(5, serialPort1);
                        }

                        if (kitchen == 1)
                        {
                            StatusLight(3, serialPort1);
                        }
                        else
                        {
                            StatusLight(7, serialPort1);
                        }

                        file("Туалет " + toilet);
                        file("Ванная " + bath);
                        file("Кухня " + kitchen);
                        file("Гостинная " + sittingroom);

                        serialPort1.Close();
                    }
                    catch(Exception ex)
                    {
                        file(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                file(ex.Message);


                /* try
                 {
                     serialPort1.Close();
                 }
                 catch
                 { }*/
            }


        }

        protected override void OnStop()
        {
            try
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStop.txt");
            }
            catch
            { }
        }


        static public void file(string text)
        {
            try
            {
                System.IO.StreamWriter textFile = new System.IO.StreamWriter(@"c:\\write\\write.txt", true);
                textFile.WriteLine(text);
                textFile.Close();
            }
            catch
            {
            }

        }

        void StatusLight(int number, SerialPort serialPort1) //Ф-ция для вкл выкл света
        {
            try
            {
                switch (number)
                {
                    case 0:

                        serialPort1.Write("q"); //Включение в ванне
                        break;

                    case 1:

                        serialPort1.Write("w"); //Включение в гостинной
                        break;

                    case 2:

                        serialPort1.Write("e"); //Включение в туалете
                        break;

                    case 3:

                        serialPort1.Write("r"); //Включение на кухне
                        break;

                    case 4:

                        serialPort1.Write("a"); ///Выключение в ванне
                        break;

                    case 5:

                        serialPort1.Write("s"); //Выключение в гостинной
                        break;

                    case 6:

                        serialPort1.Write("d"); ///Выключение в туалете
                        break;

                    case 7:

                        serialPort1.Write("f"); //Выключение на кухне
                        break;
                }
            }
            catch (Exception ex)
            {
                file(ex.Message);
            }
        }

        void Read_Light_from_bd(out int toilet,out int bath,out int sittingroom,out int kitchen)
        {
            toilet = bath = sittingroom = kitchen = 0;

            try
            {
                MySqlConnection myConnection = new MySqlConnection(Connect);
                MySqlCommand myCommand = new MySqlCommand(CommandText, myConnection);

                myConnection.Open();
                Console.WriteLine("Подключение произошло успешно");


                MySqlDataReader reader = myCommand.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0}\t{1}", reader.GetName(0), reader.GetName(1));

                    while (reader.Read()) // построчно считываем данные
                    {
                        string name = reader.GetString(0);
                        int status = reader.GetInt32(1);

                        switch (name)
                        {
                            case "Toilet":
                                {
                                    toilet = status;
                                    break;
                                }
                            case "Bath":
                                {
                                    bath = status;
                                    break;
                                }
                            case "Kitchen":
                                {
                                    kitchen = status;
                                    break;
                                }
                            case "Sitting":
                                {
                                    sittingroom = status;
                                    break;
                                }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                file(ex.Message);
            }
        }

    }

}
