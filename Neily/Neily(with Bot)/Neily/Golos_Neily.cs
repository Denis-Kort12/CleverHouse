using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Neily
{
    class Golos_Neily
    {
        
        static public void gls(string a)
        {
            try
            {

                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.SelectVoice("IVONA 2 Tatyana OEM");//Microsoft Irina Desktop
                ss.Rate = 1;
                ss.SpeakAsync(a);
            }
            catch
            {                
            }
        }

        static string[] answer_on = new string[] {"Мир стал светлей на одну комнату", "Так намного лучше",
                                                  "У вашей мамы нету столько денег на электричество","Теперь светло"};
        static string[] answer_off = new string[] { "Надеюсь вы найдете путь к холодильнику","Если у вас украдут что-нибудь я не виновата"
                                                    ,"Теперь темно и страшно", "Только пожалуйста, не выключайте меня"};

        static public string rand_on()
        {
            try
            {
                //int kol = answer_on.Length;
                Random random = new Random();
                int randomNumber_on = random.Next(0, answer_on.Length);
                return answer_on[randomNumber_on];
            }
            catch
            {
                return null;
            }
        }

        static public string rand_off()
        {
            try
            {
                //int kol = answer_on.Length;
                Random random = new Random();
                int randomNumber_off = random.Next(0, answer_off.Length);
                return answer_off[randomNumber_off];
            }
            catch
            {
                return null;
            }
        }

    }
}
