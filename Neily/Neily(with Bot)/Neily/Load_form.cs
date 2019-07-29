using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neily_menu = System.Threading;


namespace Neily
{
    public partial class Load_form : Form
    {
        bool bl = true;

        public Load_form()
        {
            try
            {
                InitializeComponent();

                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                label1.BackColor = Color.Transparent;

                timer1.Start();
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
                Label_color();
                bl = false;

                timer1.Stop();
            }
            else this.Opacity += 0.01;
        }  

        int dR, dG, dB, sign;

        private void Load_form_Load(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"Доп\Clever_house\Clever_house.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Label_color()
        {
            dR = label1.BackColor.R - label1.ForeColor.R;
            dG = label1.BackColor.G - label1.ForeColor.G;
            dB = label1.BackColor.B - label1.ForeColor.B;
            sign = 1;
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (Math.Abs(label1.ForeColor.R - label1.BackColor.R) < Math.Abs(dR / 10))
            {
                sign *= -1;
                label1.Text = "Neily";
            }
            label1.ForeColor = Color.FromArgb(255, label1.ForeColor.R + sign * dR / 10, label1.ForeColor.G + sign * dG / 10, label1.ForeColor.B + sign * dB / 10);
            if (label1.BackColor.R == label1.ForeColor.R + dR)
            {
                Neily_menu.Thread.Sleep(2000);



                Main_menu frm = new Main_menu();
                frm.Show();

                while (this.Opacity != 0)
                {
                    Neily_menu.Thread.Sleep(10);
                    this.Opacity -= 0.009;
                }

                ((Timer)sender).Stop();

                this.Hide();
            }
        }

    }
}
