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
    public partial class Main_menu : Form
    {
        public Main_menu()
        {
            InitializeComponent();

            timer1.Start();

            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            label1.BackColor = label2.BackColor = label3.BackColor = Color.Transparent;
        }        

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.Font = new Font("Times New Roman", 30, FontStyle.Bold);
            label1.Cursor = Cursors.Hand;
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.Font = new Font("Times New Roman", 30, FontStyle.Bold);
            label2.Cursor = Cursors.Hand;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {           
            label1.Font = new Font("Times New Roman", 22, FontStyle.Regular);
            label1.Cursor = Cursors.Default;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.Font = new Font("Times New Roman", 22, FontStyle.Regular);
            label2.Cursor = Cursors.Default;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity == 1)
            {      
                timer1.Stop();
            }
            else this.Opacity += 0.03;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            while (this.Opacity != 0)
            {
                Thread.Sleep(10);
                this.Opacity -= 0.009;
            }

            this.Hide();

            Golos_work frm = new Golos_work();
            frm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            while (this.Opacity != 0)
            {
                Thread.Sleep(10);
                this.Opacity -= 0.009;
            }

            this.Hide();

            Win_form_menu frm = new Win_form_menu();
            frm.Show();
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.Red;
        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.White;
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.Font = new Font("Times New Roman", 30, FontStyle.Bold);
            label3.Cursor = Cursors.Hand;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.Font = new Font("Times New Roman", 22, FontStyle.Regular);
            label3.Cursor = Cursors.Default;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            while (this.Opacity != 0)
            {
                Thread.Sleep(10);
                this.Opacity -= 0.009;
            }

            this.Hide();

            Goodbye frm = new Goodbye();
            frm.Show();         
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            label1.ForeColor = Color.White;
        }

        private void label3_MouseUp(object sender, MouseEventArgs e)
        {
            label3.ForeColor = Color.White;
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            label1.ForeColor = Color.Red;
        }

        private void label3_MouseDown(object sender, MouseEventArgs e)
        {
            label3.ForeColor = Color.Red;
        }

        private void label4_MouseDown(object sender, MouseEventArgs e)
        {
            label4.ForeColor = Color.Red;
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.Font = new Font("Times New Roman", 30, FontStyle.Bold);
            label4.Cursor = Cursors.Hand;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.Font = new Font("Times New Roman", 22, FontStyle.Regular);
            label4.Cursor = Cursors.Default;
        }

        private void label4_MouseUp(object sender, MouseEventArgs e)
        {
            label4.ForeColor = Color.White;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            while (this.Opacity != 0)
            {
                Thread.Sleep(10);
                this.Opacity -= 0.009;
            }

            this.Hide();

            ShowInWf frm = new ShowInWf();
            frm.Show();

        }

        private void label5_MouseDown(object sender, MouseEventArgs e)
        {
            label5.ForeColor = Color.Red;
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.Font = new Font("Times New Roman", 30, FontStyle.Bold);
            label5.Cursor = Cursors.Hand;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Vk_Neily_Bot frm = new Vk_Neily_Bot();
            frm.Show();
            this.Close();

        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.Font = new Font("Times New Roman", 22, FontStyle.Regular);
            label5.Cursor = Cursors.Default;
        }

        private void label5_MouseUp(object sender, MouseEventArgs e)
        {
            label5.ForeColor = Color.White;
        }
    }
    
}
