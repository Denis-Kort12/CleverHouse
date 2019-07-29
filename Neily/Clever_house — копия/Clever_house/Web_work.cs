using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever_house
{
    class Web_work
    {
        private static FilterInfoCollection videoDevices;
        private static VideoCaptureDevice videoSource;

        static string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Clever_house; Integrated Security=True";


        static Bitmap bitmap;

        public static void Start()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);

            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.Start();

        }


        private static void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                bitmap = (Bitmap)eventArgs.Frame.Clone();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Convert.ToString(ex.Message));
            }
        }

        public static void Scrin()
        {
            //bitmap.Save(@"C:\Users\" + Environment.UserName + @"\Pictures\Saved Pictures\image1000.jpg");
            bitmap.Save(@"C:\Users\" + Environment.UserName + @"\Pictures\Saved Pictures\image" + Schet.i_img + ".jpg");
        }

        public static void AddScrinBd()
        {
            Image img = BitmapToImage(bitmap);

            byte[] scrin_from_web = ImageToByteArray(img);

            try
            {
                string sqlExpression;

                sqlExpression = "UPDATE Come_in_flat SET Img = @Img WHERE ID_come_in_flat = " + CountStrock();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.AddWithValue("@Img", scrin_from_web);

                    command.ExecuteNonQuery();
                }
            }
            catch
            { }
        }

        //конвертирует Image в массив байт
        private static byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        //конвертирует Bitmap в Image
        private static Image BitmapToImage(Bitmap map)
        {
            Stream imageStream = new MemoryStream();
            map.Save(imageStream, ImageFormat.Png);
            return Image.FromStream(imageStream);
        }

        private static int CountStrock()
        {
            string sqlExpression;

            sqlExpression = "SELECT COUNT(*) From Come_in_flat";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                int kol = Convert.ToInt32(command.ExecuteScalar());

                return kol;
            }
        }

    }
}
