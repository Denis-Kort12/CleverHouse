using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using System.IO.Ports;

namespace TelegBot
{
    public partial class Form1 : Form
    {
        private static Telegram.Bot.TelegramBotClient BOT;

        static SerialPort serialPort1 = new SerialPort();

        public Form1()
        {
            InitializeComponent();  
        }

        private void button1_Click(object sender, EventArgs e)
        {         
            try
            {
                BOT = new Telegram.Bot.TelegramBotClient("{КЛЮЧ_API}");
                BOT.OnMessage += BotOnMessageReceived;
                BOT.StartReceiving(new UpdateType[] { UpdateType.MessageUpdate });
                button1.Enabled = false;

                this.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            try
            {
                serialPort1.PortName = "COM5";
                serialPort1.BaudRate = 9600;
                serialPort1.DtrEnable = true;
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                Telegram.Bot.Types.Message msg = messageEventArgs.Message;

                if (msg == null || msg.Type != MessageType.TextMessage) return;

                string Answer = "";

                switch (msg.Text)
                {
                    case "/start": Answer = "/sittingroomon - Включить свет в гостиной\r\n/kitchenon - Включить свет на кухне\r\n/toileton - Включить свет в санузле\r\n/bathroomon - Включить свет в ванне\r\n\r\n/sittingroomoff - Выключить свет в гостинной\r\n/kitchenoff - Выключить свет на кухне\r\n/toiletoff - Выключить свет в санузле\r\n/bathroomoff - Выключить свет в ванне"; break;
                    case "/sittingroomon": Answer = "Включен свет в гостинной"; serialPort1.Write("w"); break;
                    case "/kitchenon": Answer = "Включен свет на кухне"; serialPort1.Write("r"); break;
                    case "/toileton": Answer = "Включен свет в туалете"; serialPort1.Write("e"); break;
                    case "/bathroomon": Answer = "Включен свет в ванне"; serialPort1.Write("q"); break;

                    case "/sittingroomoff": Answer = "Выключен свет в гостинной"; serialPort1.Write("s"); break;
                    case "/kitchenoff": Answer = "Выключен свет на кухне"; serialPort1.Write("f"); break;
                    case "/toiletoff": Answer = "Выключен свет в туалете"; serialPort1.Write("d"); break;
                    case "/bathroomoff": Answer = "Выключен свет в ванне"; serialPort1.Write("a"); break;


                    default: Answer = "Не знаю такой команды"; break;

                }

                await BOT.SendTextMessageAsync(msg.Chat.Id, Answer);

                try
                {
                    serialPort1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
