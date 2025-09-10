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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is customerinterface)
                    {
                        form.Close();
                        break;
                    }
                }
                paint_interface paint_interface = new paint_interface();  // Create an object of Form2
                paint_interface.Show();               // Show Form2 (non-blocking)
                this.Hide();                // Hide Form1 (optional)
            }
        }
    }
}
