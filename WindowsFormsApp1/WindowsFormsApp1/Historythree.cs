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
    public partial class Historythree : Form
    {
        public Historythree()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string f = @"C:\\Users\\Welcome\\Downloads\\Sri Lankan Handloom Industry _ Skilled weavers.mp4";
            axWindowsMediaPlayer1.URL = f;
            //axWindowsMediaPlayer1.Ctlcontrols.play();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            historytwo HistoryFormtwo = new historytwo(); // <-- use your form class
            HistoryFormtwo.Show();
            this.Hide();
        }
    }
}
