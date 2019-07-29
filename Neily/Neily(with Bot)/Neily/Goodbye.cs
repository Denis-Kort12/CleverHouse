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
    public partial class Goodbye : Form
    {
        public Goodbye()
        {
            try
            {
                InitializeComponent();
                timer1.Start();

                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                label1.BackColor = label1.BackColor = label1.BackColor = Color.Transparent;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.Opacity == 1)
                {
                    Thread.Sleep(3000);

                    while (this.Opacity != 0)
                    {
                        Thread.Sleep(10);
                        this.Opacity -= 0.009;
                    }
                    Environment.Exit(0);
                }
                else this.Opacity += 0.03;
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show(ex.Message);
                    Application.Exit();
                }
                catch
                {
                }
            }
        }
    }
}
