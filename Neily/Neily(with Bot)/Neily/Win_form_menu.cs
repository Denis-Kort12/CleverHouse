using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neily
{
    public partial class Win_form_menu : Form
    {
        public Win_form_menu()
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
                timer1.Stop();
            }
            else this.Opacity += 0.03;
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            label1.ForeColor = Color.Red;
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.Font = new Font("Times New Roman", 30, FontStyle.Bold);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.Font = new Font("Times New Roman", 24, FontStyle.Regular);
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            label1.ForeColor = Color.White;
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.Red;
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.Font = new Font("Times New Roman", 30, FontStyle.Bold);
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.Font = new Font("Times New Roman", 24, FontStyle.Regular);
        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.White;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            while (this.Opacity != 0)
            {
                Thread.Sleep(10);
                this.Opacity -= 0.009;
            }

            this.Hide();

            Win_form_work frm = new Win_form_work();
            frm.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            while (this.Opacity != 0)
            {
                Thread.Sleep(10);
                this.Opacity -= 0.009;
            }

            this.Hide();

            User_form_add frm = new User_form_add();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main_menu frm = new Main_menu();
            frm.Show();

            this.Close();
        }
    }
}
