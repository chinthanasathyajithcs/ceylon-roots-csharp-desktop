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
            string f = @"C:\\Users\\USER\\Documents\\Vageesha\\First Year second sem\\C#\\My programmes\\My app\\Git desktop clone\\winforms-starter\\WindowsFormsApp1\\WindowsFormsApp1\\bin\\Debug\\videos\\Fabric_video.mp4";
            axWindowsMediaPlayer1.URL = f;
            //axWindowsMediaPlayer1.Ctlcontrols.play();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            historytwo HistoryFormtwo = new historytwo(); // <-- use your form class
            HistoryFormtwo.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new customerinterface().Show();
            this.Hide();
        }
    }
}
