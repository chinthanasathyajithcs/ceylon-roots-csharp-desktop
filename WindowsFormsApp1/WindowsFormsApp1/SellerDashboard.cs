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
    public partial class Sdash : Form
    {
        public Sdash()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            userControl12.Visible = false;
            inventoryForm1.Visible = true;
            sellercategories1.Visible = false;
            orders1.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            userControl12.Visible = true;        
            inventoryForm1.Visible = false;
            sellercategories1.Visible = false;
            orders1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            userControl12.Visible = false;
            inventoryForm1.Visible = true;
            sellercategories1.Visible = false;
            orders1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            userControl12.Visible = false;
            inventoryForm1.Visible = false; 
            sellercategories1.Visible = true;
            orders1.Visible = false;
        }

        private void sellercategories1_Load(object sender, EventArgs e)
        {

        }

        private void ordersBtn_Click(object sender, EventArgs e)
        {
            userControl12.Visible = false;
            inventoryForm1.Visible = false;
            sellercategories1.Visible = false;
            orders1.Visible = true;


        }

        private void orders1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Welcome nextForm = new Welcome();
            nextForm.Show();
            this.Hide();
        }
    }
}
//test
