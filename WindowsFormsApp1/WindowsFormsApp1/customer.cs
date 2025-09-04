using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class customer : Form
    {
        string fullName;
        string passportNumber;
        string email;
        string phoneNumber;
        string hotelNumber;
        string street;
        string city;
        string landmark;
        string roomNumber;
        DateTime deliveryDate;


        public customer()
        {
            InitializeComponent();
        }

        private void customer_Load(object sender, EventArgs e)
        {

            dateTimePicker1.Value = DateTime.Now;

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //phone number
            phoneNumber = textBox4.Text;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //reset button
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            dateTimePicker1.Value = DateTime.Now;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //check
            if (checkBox1.Checked)
            {
                MessageBox.Show("Leave package at hotel reception option selected");
            }
            else
            {
                MessageBox.Show("Leave package at hotel reception option removed.");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //name
            fullName = textBox1.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //hotel number
            hotelNumber = textBox5.Text;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //date

            deliveryDate = dateTimePicker1.Value;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //passport
            passportNumber = textBox3.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //email
            email = textBox2.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            //room number
            roomNumber = textBox8.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //street 
            street = textBox6.Text;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            //city
            city = textBox9.Text;

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //landmark
            landmark = textBox7.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //submit
            string message =
                $"Full Name: {fullName}\n" +
                $"Passport/NIC: {passportNumber}\n" +
                $"Email: {email}\n" +
                $"Phone: {phoneNumber}\n" +
                $"Hotel No: {hotelNumber}\n" +
                $"Street: {street}\n" +
                $"City: {city}\n" +
                $"Landmark: {landmark}\n" +
                $"Room No: {roomNumber}\n" +
                $"Delivery Date: {deliveryDate.ToShortDateString()}";

            MessageBox.Show(message, " Thank you for shopping with us!");
        }

    
        }
    }

