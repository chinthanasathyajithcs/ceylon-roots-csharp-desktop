using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Thank_you_Form : Form
    {
        private Timer timer;
        public Thank_you_Form()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 20000; // 20 seconds = 20000 ms
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Stop the timer so it doesn't trigger again
            timer.Stop();
            OpenForm1();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Stop();
            OpenForm1();
        }
        private void OpenForm1()
        {
            this.BeginInvoke(new Action(() =>
            {
                Welcome nextForm = new Welcome();
                nextForm.Show();
                this.Hide();
            }));/* Open the next form
            Welcome nextForm = new Welcome();
            nextForm.Show();

            // Hide current form
            this.Hide();*/
        }
    }
}
