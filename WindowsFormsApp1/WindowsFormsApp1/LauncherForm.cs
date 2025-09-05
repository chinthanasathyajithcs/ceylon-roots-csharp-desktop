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
    public partial class LauncherForm : Form
    {
        public LauncherForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Welcome().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Sdash().Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new History().Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Sinventory_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            new customer().Show();
        }

        private void shop_Click(object sender, EventArgs e)
        {
            new customerinterface().Show();
        }
    }
}
