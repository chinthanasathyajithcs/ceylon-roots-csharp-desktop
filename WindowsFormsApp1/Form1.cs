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
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Language_change_form langForm = new Language_change_form(); // <-- use your form class
            langForm.Show();
            this.Hide();

        }
    }
}
