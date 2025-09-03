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
    public partial class Form3 : Form
    {
        public Form3(string imagePath)
        {
            InitializeComponent();
            this.Text = "Preview Saved Image";
            this.Width = 600;
            this.Height = 450;

            Label lbl = new Label
            {
                Text = "You are now on the next page! Image preview:",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lbl);

            PictureBox preview = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Load image into memory so Paint can close
            using (var bmpTemp = new Bitmap(imagePath))
            {
                preview.Image = new Bitmap(bmpTemp);
            }

            this.Controls.Add(preview);
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
