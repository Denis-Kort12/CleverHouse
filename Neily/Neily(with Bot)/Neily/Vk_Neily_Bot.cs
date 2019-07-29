using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neily
{
    public partial class Vk_Neily_Bot : Form
    {
        public Vk_Neily_Bot()
        {
            InitializeComponent();
        }

        string kindString;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText("Kind.txt", kindString);
                MessageBox.Show("Режим изменен)");
            }
            catch
            {
                MessageBox.Show("Произошла ошибка в изменении режима");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                kindString = listBox1.SelectedItem.ToString();

                switch (kindString)
                {
                    case "Занят":
                        {
                            kindString = "Здравствуйте, меня зовут Neily) Я помощница Denis Kort. В данный момент Денис не может говорить, но если это важно, я постараюсь связать Вас с ним. Напишите 'Помощь' для получения списка команд.";
                            break;
                        }
                    case "Работа":
                        {
                            kindString = "Здравствуйте, меня зовут Neily) Я помощница Denis Kort. В данный момент Денис работает, но если это важно, я постараюсь связать Вас с ним. Напишите 'Помощь' для получения списка команд.";
                            break;
                        }
                    case "Разработа Neily":
                        {
                            kindString = "Здравствуйте, меня зовут Neily) Я помощница Denis Kort. В данный момент Денис работает надо мной, но если это важно, я постараюсь связать Вас с ним. Напишите 'Помощь' для получения списка команд.";
                            break;
                        }
                    case "Тренируюсь":
                        {
                            kindString = "Здравствуйте, меня зовут Neily) Я помощница Denis Kort. В данный момент Денис тренируется, но если это важно, я постараюсь связать Вас с ним. Напишите 'Помощь' для получения списка команд.";
                            break;
                        }
                    case "Сплю":
                        {
                            kindString = "Здравствуйте, меня зовут Neily) Я помощница Denis Kort. В данный момент Денис спит, но если это важно, я постараюсь связать Вас с ним. Напишите 'Помощь' для получения списка команд.";
                            break;
                        }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    Process.Start(@"Доп\Вк\Montece VkBot.exe");
                }
                else
                {
                    try
                    {
                        foreach (Process proc in Process.GetProcessesByName("Montece VkBot"))
                        {
                            proc.Kill();
                        }

                        /*foreach (Process proc in Process.GetProcessesByName("Montece VkBot1"))
                        {
                            proc.Kill();
                        }*/
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Main_menu frm = new Main_menu();
                frm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
