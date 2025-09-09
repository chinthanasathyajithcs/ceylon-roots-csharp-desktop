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
    public partial class historytwo : Form
    {
        public historytwo()
        {
            InitializeComponent();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            History HistoryFormone = new History(); // <-- use your form class
            HistoryFormone.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Historythree HistoryFormthree = new Historythree(); // <-- use your form class
            HistoryFormthree.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
