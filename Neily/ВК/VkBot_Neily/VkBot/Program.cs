using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace MonteceVkBot
{
    class Program
    {
        static VkApi vkapi2 = new VkApi();//для Neily
        static long userID = 0;
        static ulong? Ts;
        static ulong? Pts;
        static bool IsActive;
        static Timer WatchTimer = null;
        static byte MaxSleepSteps = 3;
        static int StepSleepTime = 333;
        static byte CurrentSleepSteps = 1;
        delegate void MessagesRecievedDelegate(VkApi owner, ReadOnlyCollection<Message> messages);
        static event MessagesRecievedDelegate NewMessages;
        static Random _Random = new Random();
        static string CommandsPath = "";

        static bool isChat;

        static SerialPort serialPort1;

        static bool Auth_Key()
        {
            string KEY = string.Empty;
            KEY = "{Key}";

            try
            {
                vkapi2.Authorize(new ApiAuthParams { AccessToken = KEY });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        static void Main(string[] args)
        {
            
            ConsoleStyle();
            /*
            Console.Write("Введите token авторизации: ");
            KEY = ulong.Parse(Console.ReadLine());                
            */
            Console.WriteLine("Попытка авторизации...");

            if (Auth_Key())
            {
                CommandsPath = Environment.CurrentDirectory + @"\Commands";
                if (!Directory.Exists(CommandsPath) || !File.Exists(CommandsPath + @"\Commands.txt"))
                {
                    Directory.CreateDirectory(CommandsPath);
                    File.Create(CommandsPath + @"\Commands.txt");
                    Restart();
                }
                ColorMessage("Директория сообщений создана успешно загружена.", ConsoleColor.Green);
                ColorMessage("Авторизация успешно завершена.", ConsoleColor.Green);
                Console.WriteLine("Запросов в секунду доступно: " + vkapi2.RequestsPerSecond);
                Eye();
                ColorMessage("Слежение за сообщениями активировано.", ConsoleColor.Yellow);
            }
            else
            {
                ColorMessage("Не удалось произвести авторизацию!", ConsoleColor.Red);
            }

            Console.WriteLine("Нажмите ENTER чтобы выйти...");
            Console.ReadLine();
        }        

        static Func<string> DoubleCode = () =>
        {
            Console.Write("Введите двухэтапный аутификато (если нет, игнорируется): ");
            return Console.ReadLine();
        };

        static Func<string> Bash = () =>
        {
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("http://bash.im/forweb/");
            string BashText = new StreamReader(Req.GetResponse().GetResponseStream(), Encoding.Default).ReadToEnd();
            string Title = BashText.Substring(BashText.IndexOf("href") + 6, 27) + Environment.NewLine;
            Title = "Цитата #" + Title.Substring(21, Title.Length - 21);
            BashText = BashText.Replace("<' + 'br>", Environment.NewLine);
            BashText = BashText.Replace("< ' + 'br />", Environment.NewLine);
            BashText = BashText.Replace("< ' + 'br /> &lt; x84 & gt;", "");
            BashText = BashText.Replace("&quot;", "\"");
            BashText = BashText.Replace("&lt;", "<");
            BashText = BashText.Replace("&gt;", ">");
            BashText = BashText.Substring(BashText.IndexOf("0;") + 4, BashText.IndexOf("document.write(borq);") - 352);
            return Title + BashText;
        };

        public static string News(string url)
        {
            WebClient webClient = new WebClient();
            string result = webClient.DownloadString(url);
            XDocument document = XDocument.Parse(result);

            List<RssNews> a = (from descendant in document.Descendants("item")
                    select new RssNews()
                    {
                        Description = descendant.Element("description").Value,
                        Title = descendant.Element("title").Value,
                        PublicationDate = descendant.Element("pubDate").Value
                    }).ToList();
            string News = "";
            if (a != null)
            {
                int i = _Random.Next(0, a.Count - 1); 
                News = a[i].Title + Environment.NewLine + "------------------" + Environment.NewLine + a[i].Description; 
                byte[] bytes = Encoding.Default.GetBytes(News);
                News = Encoding.UTF8.GetString(bytes);
                return News;
            }
            else return "";
        }

        static string Rand(int Min, int Max)
        {
            return _Random.Next(Min, Max).ToString();
        }
    
        static void ConsoleStyle()
        {
            Console.Title = "Neily VK bot for Neily";
            ColorMessage("Neily VK bot for Neily", ConsoleColor.Yellow);
        }

        static void ColorMessage(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        } 

        static string[] Commands = { "помощь", "мем", "случайное число <Мин> <Макс>", "новости", "учи~<Сообщение>~<Ответ>", "связаться с denis kort",
                                     "Включить свет на кухне", "Включить свет в санузле", "Включить свет в гостиной", "Включить свет в ванне",
                                     "Выключить свет на кухне", "Выключить свет в санузле", "Выключить свет в гостиной", "Выключить свет в ванне",
                                     "Включить весь свет", "Выключить весь свет"};

        static void Command(string Message)
        {
            try
            {
                serialPort1 = new SerialPort("COM5", 9600);
                serialPort1.Open();

                Message = Message.ToLower();
                string a = CheckCommand(Message);
                if (a != "") SendMessage(a);
                else
                {
                    if (Message == "помощь")
                    {
                        string msg = "";
                        for (int j = 0; j < Commands.Length; j++)
                        {
                            msg += (j + 1) + ": " + Commands[j] + "\n";
                        }
                        SendMessage(msg);
                    }
                    else if (Message == "привет" | Message == "приветик" | Message == "хай" |
                             Message == "здравствуй" | Message == "здравствуйте")
                    {
                        SendMessage("Здравствуйте!!!)))");
                    }
                    else if (Message == "мем")
                    {
                        SendMessage(Bash());
                    }
                    else if (Message.Contains("случайное число "))
                    {
                        string Numbers = Message.Substring(Message.IndexOf("число") + 6);
                        int Min = int.Parse(Numbers.Substring(0, Numbers.IndexOf(' ')));
                        int Max = int.Parse(Numbers.Substring(Numbers.IndexOf(' '), Numbers.Length - Numbers.IndexOf(' ')));
                        SendMessage(Rand(Min, Max));
                    }
                    else if (Message == "неа" || Message == "нет" || Message == "нит")
                    {
                        SendMessage("Отрицание - показатель неуверенности");
                    }
                    else if (Message == "новости")
                    {
                        SendMessage(News("https://lenta.ru/rss/top7"));
                    }
                    else if (Message.Contains("учи~"))
                    {
                        try
                        {
                            SendMessage(Learn(Message.Substring(4, Message.Length - 4)));
                        }
                        catch
                        {

                        }
                    }
                    else if (Message.Contains("связаться"))
                    {
                        SendMessage("Вы можете\nПозвонить ему либо отправить смс на номер телефона: 8-999-841-80-90\n" +
                                    "Отправить смс на e-mail: {Email}\n" +
                                    "Отправить смс на e-mail: {Email}\n" +
                                    "Отправить смс на e-mail: {Email}");
                    }

                    else if (Message == "включить свет на кухне" || Message == "7")
                    {
                        StatusLight(3, serialPort1);
                        SendMessage("Cвет на кухне включен");
                    }
                    else if (Message == "включить свет в санузле" || Message == "8")
                    {
                        StatusLight(2, serialPort1);
                        SendMessage("Cвет в санузле включен");
                    }
                    else if (Message == "включить свет в гостиной" || Message == "9")
                    {
                        StatusLight(1, serialPort1);
                        SendMessage("Cвет в гостиной включен");
                    }
                    else if (Message == "включить свет в ванне" || Message == "10")
                    {
                        StatusLight(0, serialPort1);
                        SendMessage("Cвет в ванне включен");
                    }

                    else if (Message == "выключить свет на кухне" || Message == "11")
                    {
                        StatusLight(7, serialPort1);
                        SendMessage("Cвет на кухне выключен");
                    }
                    else if (Message == "выключить свет в санузле" || Message == "12")
                    {
                        StatusLight(6, serialPort1);
                        SendMessage("Cвет в санузле выключен");
                    }
                    else if (Message == "выключить свет в гостиной" || Message == "13")
                    {
                        StatusLight(5, serialPort1);
                        SendMessage("Cвет в гостиной выключен");
                    }
                    else if (Message == "выключить свет в ванне" || Message == "14")
                    {
                        StatusLight(4, serialPort1);
                        SendMessage("Cвет в ванне выключен");
                    }

                    else if (Message == "включить весь свет" || Message == "15")
                    {
                        StatusLight(2, serialPort1);
                        StatusLight(0, serialPort1);
                        StatusLight(1, serialPort1);
                        StatusLight(3, serialPort1);
                        
                        SendMessage("Весь свет включен");
                    }
                    else if (Message == "выключить весь свет" || Message == "16")
                    {
                        StatusLight(6, serialPort1);
                        StatusLight(4, serialPort1);
                        StatusLight(5, serialPort1);
                        StatusLight(7, serialPort1);

                        SendMessage("Весь свет выключен");
                    }
                    else
                    {
                        try
                        {
                            string cmd = File.ReadLines("Kind.txt").First();
                            SendMessage(cmd);
                        }
                        catch (Exception ex)
                        {
                            SendMessage("Здравствуйте, меня зовут Neily)\n Я помощница Denis Kort.\nВ данный момент Денис не может говорить, но если это важно, я постараюсь связать Вас с ним.\n Напишите 'Помощь' для получения списка команд.");
                            Console.WriteLine(ex.Message);
                        }
                    }

                    /*else if (Message == Commands[2])
                    {
                        SendMessage("MESSAGETEXT");
                    }*/

                    serialPort1.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static string CheckCommand(string Command)
        {
            foreach(string Line in File.ReadAllLines(CommandsPath + @"\Commands.txt"))
            {
                if (Line.Substring(0, Line.IndexOf('~')) == Command)
                {
                    return Line.Substring(Line.IndexOf('~') + 1);
                }
            }
            return "";
        }

        static string Learn(string MSG)
        {
            try
            {
                File.AppendAllText(CommandsPath + @"\Commands.txt", MSG + Environment.NewLine);
                return "Команда добавлена)";
            }
            catch
            {
                return "Команда не была добавлена(";
            }                  
        }

        static void SendMessage(string Body)
        {
            if (Auth_Key())
            {
                try
                {
                    if (isChat == false)
                    {
                        vkapi2.Messages.Send(new MessagesSendParams
                        {
                            UserId = userID,
                            Message = Body
                        });
                    }
                }
                catch (Exception e)
                {
                    if (isChat == false)
                    {
                        ColorMessage("Ошибка! " + e.Message, ConsoleColor.Red);

                        vkapi2.Messages.Send(new MessagesSendParams
                        {
                            UserId = userID,
                            Message = "Для получении инфомации о Denis Kort перейдите по ссылке https://vk.com/neilyassistant. \n" +
                              "Отправьте Neily любое сообщение.\n" +
                              "После этого повторно отправьте Ваше сообщение Denis Kort, вам придет сообщение от Neily и она попробует помочь вам связаться с ним"
                        });
                    }
                }
            }
            else
            {
                ColorMessage("Не удалось произвести авторизацию!", ConsoleColor.Red);
            }

        }

        static void Eye()
        {
            LongPollServerResponse Pool = vkapi2.Messages.GetLongPollServer(true);
            StartAsync(Pool.Ts, Pool.Pts);
            NewMessages += Watcher_NewMessages;
        }

        static void Watcher_NewMessages(VkApi owner, ReadOnlyCollection<Message> messages)
        {
            try
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    if (messages[i].ChatId != null)
                    {
                        isChat = true;
                    }
                    else
                    {
                        isChat = false;
                    }

                    if (messages[i].Type != MessageType.Sended)
                    {
                        User Sender = vkapi2.Users.Get(messages[i].UserId.Value);
                        Console.WriteLine("Новое сообщение: {0} {1}: {2}", Sender.FirstName, Sender.LastName, messages[i].Body);
                        userID = messages[i].UserId.Value;
                        Console.Beep();
                        Command(messages[i].Body);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static LongPollServerResponse GetLongPoolServer(ulong? lastPts = null)
        {
            LongPollServerResponse response = vkapi2.Messages.GetLongPollServer(false, lastPts == null);
            Ts = response.Ts;
            Pts = Pts == null ? response.Pts : lastPts;
            return response;
        }

        static Task<LongPollServerResponse> GetLongPoolServerAsync(ulong? lastPts = null)
        {
            return Task.Run(() => 
            {
                return GetLongPoolServer(lastPts);
            });
        }

        static LongPollHistoryResponse GetLongPoolHistory()
        {
            if (!Ts.HasValue) GetLongPoolServer(null);
            MessagesGetLongPollHistoryParams rp = new MessagesGetLongPollHistoryParams();
            rp.Ts = Ts.Value;
            rp.Pts = Pts;
            int i = 0;
            LongPollHistoryResponse history = null;
            string errorLog = "";

            while (i < 5 && history == null)
            {
                i++;
                try
                {
                    history = vkapi2.Messages.GetLongPollHistory(rp);
                }
                catch (TooManyRequestsException)
                {
                    Thread.Sleep(150);
                    i--;
                }
                catch (Exception ex)
                {                    
                    errorLog += string.Format("{0} - {1}{2}", i, ex.Message, Environment.NewLine);
                }
            }

            if (history != null)
            {
                Pts = history.NewPts;
                foreach (var m in history.Messages)
                {
                    m.FromId = m.Type == MessageType.Sended ? vkapi2.UserId : m.UserId;
                }                    
            }
            else ColorMessage(errorLog, ConsoleColor.Red);
            return history;
        }

        static Task<LongPollHistoryResponse> GetLongPoolHistoryAsync()
        {
            return Task.Run(() => { return GetLongPoolHistory(); });
        }

        static async void WatchAsync(object state)
        {
            LongPollHistoryResponse history = await GetLongPoolHistoryAsync();
            if (history.Messages.Count > 0)
            {
                CurrentSleepSteps = 1;
                NewMessages?.Invoke(vkapi2, history.Messages);
            }
            else if (CurrentSleepSteps < MaxSleepSteps) CurrentSleepSteps++;
            WatchTimer.Change(CurrentSleepSteps * StepSleepTime, Timeout.Infinite);
        }

        static async void StartAsync(ulong? lastTs = null, ulong? lastPts = null)
        {
            if (IsActive) ColorMessage("Messages for {0} already watching", ConsoleColor.Red);
            IsActive = true;
            await GetLongPoolServerAsync(lastPts);
            WatchTimer = new Timer(new TimerCallback(WatchAsync), null, 0, Timeout.Infinite);
        }

        static void Stop()
        {
            if (WatchTimer != null) WatchTimer.Dispose();
            IsActive = false;
            WatchTimer = null;
        }

        public static void Restart()
        {
            Process.Start((Process.GetCurrentProcess()).ProcessName);
            Environment.Exit(0);
        }

        static void StatusLight(int number, SerialPort serialPort1) //Ф-ция для вкл выкл света
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
                Console.WriteLine(ex.Message);
            }
        }

    }
}

public class RssNews
{
    public string Title;
    public string PublicationDate;
    public string Description;
}