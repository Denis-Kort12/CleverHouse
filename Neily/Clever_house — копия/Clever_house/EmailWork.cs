using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Clever_house
{
    class EmailWork
    {
        static string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Clever_house; Integrated Security=True";


        int port = 587; // порт smtp сервера, в случае mail.ru это 587
        bool enableSSL = true;

        static string emailFrom = "{Email}"; // адрес почты отправителя письма
        static string password = "{Password}"; // пароль почты отправителя письма
        static string emailTo = "{Email}"; // адрес почты получателя письма
        static string subject = "Neily"; // тема письма
        static string body = ""; // текст письма
        static string smtpAddress = "smtp.mail.ru"; // адрес stmp сервера

        MailMessage mail = new MailMessage();

        public void SendEmail(long number, bool status, string name)
        {
            bool prov = ProvRecord(number);

            if (prov == true && status == false)
            {
                body = "Привет, в квартиру вошел пользователь <" + name + "> в " + DateTime.Now;

                SendEmail(body);
            }

            if (prov == true && status == true)
            {
                body = "Привет, из квартиры вышел пользователь <" + name + "> в " + DateTime.Now;

                SendEmail(body);
            }

            if (prov == false)
            {
                body = "Привет, была использована магнитная карта, которой нет в базе <" + number + "> в " + DateTime.Now;

                SendEmail(body);
            }
        }

        private void SendEmail(string text)
        {
            mail.From = new MailAddress(emailFrom);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = text;
            mail.IsBodyHtml = true; // можно поставить false, если отправляется только текст

            //mail.Attachments.Add(new Attachment(@"C:\Users\" + Environment.UserName + @"\Pictures\Saved Pictures\image1000.jpg")); // если нужно прикрепить текстовый файл 
            mail.Attachments.Add(new Attachment(@"C:\Users\" + Environment.UserName + @"\Pictures\Saved Pictures\image"+Schet.i_img+".jpg")); // если нужно прикрепить текстовый файл 

            SmtpClient smtp = new SmtpClient(smtpAddress, port);

            smtp.Credentials = new NetworkCredential(emailFrom, password);
            smtp.EnableSsl = enableSSL;
            try
            {
                smtp.Send(mail); // отправка сообщения
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString()); // для вывода ошибки в консоль
            }
        }

        private bool ProvRecord(long number)
        {
            try
            {
                string sqlExpression = "SELECT COUNT(*) FROM Users Where ID_rfid = '" + number + "'";

                int u = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    SqlCommand com = new SqlCommand(sqlExpression, con);
                    u = Convert.ToInt32(com.ExecuteScalar());
                }

                if (u != 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

    }
}
