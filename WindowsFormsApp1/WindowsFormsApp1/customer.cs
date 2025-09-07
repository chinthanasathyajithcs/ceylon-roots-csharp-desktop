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
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class customer : Form
    {
        string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=CSpassword2025;Connect Timeout=30;Encrypt=True";
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
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();

                    // Ensure the table exists
                    string ensureSql = @"
IF OBJECT_ID(N'dbo.CustomerDelivery', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[CustomerDelivery] (
        ID INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(100),
        PassportNIC NVARCHAR(50),
        Email NVARCHAR(100),
        Mobile NVARCHAR(20),
        HotelName NVARCHAR(100),
        RoomNumber NVARCHAR(20),
        StreetAddress NVARCHAR(200),
        City NVARCHAR(50),
        NearestLandmark NVARCHAR(200),
        PreferredDeliveryDate DATE,
        LeaveAtReception BIT
    );
END";
                    using (var cmdEnsure = new SqlCommand(ensureSql, conn))
                        cmdEnsure.ExecuteNonQuery();

                    // Insert form data
                    string query = @"
INSERT INTO [dbo].[CustomerDelivery]
(FullName, PassportNIC, Email, Mobile, HotelName, RoomNumber, StreetAddress, City, NearestLandmark, PreferredDeliveryDate, LeaveAtReception)
VALUES
(@FullName, @PassportNIC, @Email, @Mobile, @HotelName, @RoomNumber, @StreetAddress, @City, @NearestLandmark, @PreferredDeliveryDate, @LeaveAtReception)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@PassportNIC", passportNumber);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Mobile", phoneNumber);
                        cmd.Parameters.AddWithValue("@HotelName", hotelNumber);
                        cmd.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        cmd.Parameters.AddWithValue("@StreetAddress", street);
                        cmd.Parameters.AddWithValue("@City", city);
                        cmd.Parameters.AddWithValue("@NearestLandmark", landmark);
                        cmd.Parameters.AddWithValue("@PreferredDeliveryDate", deliveryDate);
                        cmd.Parameters.AddWithValue("@LeaveAtReception", checkBox1.Checked);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                            MessageBox.Show("Data Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("No data was saved!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    }

