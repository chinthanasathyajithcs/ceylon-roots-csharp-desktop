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
    public partial class sellerform : Form
    {
        public sellerform()
        {
            InitializeComponent();
        }

        private void D(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "4321")
            {
                
                Sdash dashboard = new Sdash();
                dashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid code. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
