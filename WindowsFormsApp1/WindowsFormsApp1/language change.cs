using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class language_change : Form
    {
        public language_change()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            History HistoryFormone = new History(); // <-- use your form class
            HistoryFormone.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetLanguage("de");

            History HistoryFormone = new History(); // <-- use your form class
            HistoryFormone.Show();
            this.Hide();

        }

        private void SetLanguage(string langCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCode);

            // reload the form to apply language
            this.Controls.Clear();
            InitializeComponent();
        }
    }


}
