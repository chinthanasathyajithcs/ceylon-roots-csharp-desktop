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
        string imagePath; // Add this line

        public customer()
        {
            InitializeComponent();
            // Set default delivery date since DateTimePicker is removed
            deliveryDate = DateTime.Now;
        }

        private void customer_Load(object sender, EventArgs e)
        {
            // DateTimePicker removed, so nothing to do here
            // deliveryDate is already set in the constructor
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
            // DateTimePicker removed, so set deliveryDate to now
            deliveryDate = DateTime.Now;

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

        private void ValidateFields()
        {
            // Check text fields
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox9.Text))
            {
                throw new Exception("All fields must be filled out.");
            }

            // Check image box
            if (picture.Image == null)
            {
                throw new Exception("An image must be selected.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        { }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                ValidateFields();

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();

                    // Ensure the table exists
                    string ensureSql = @"
IF OBJECT_ID(N'dbo.CustomerDelivery', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[CustomerDelivery] (
        ID INT IDENTITY(1,1) PRIMARY KEY,
        FullName VARCHAR(100),
        PassportNIC VARCHAR(50),
        Email VARCHAR(100),
        Mobile VARCHAR(20),
        HotelName VARCHAR(100),
        RoomNumber VARCHAR(20),
        StreetAddress VARCHAR(200),
        City VARCHAR(50),
        NearestLandmark VARCHAR(200),
        LeaveAtReception BIT,
        image NVARCHAR(300)
    );
END";
                    using (var cmdEnsure = new SqlCommand(ensureSql, conn))
                        cmdEnsure.ExecuteNonQuery();

                    // Insert form data
                    string query = @"
INSERT INTO [dbo].[CustomerDelivery]
(FullName, PassportNIC, Email, Mobile, HotelName, RoomNumber, StreetAddress, City, NearestLandmark, LeaveAtReception, image)
VALUES
(@FullName, @PassportNIC, @Email, @Mobile, @HotelName, @RoomNumber, @StreetAddress, @City, @NearestLandmark, @LeaveAtReception, @image)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", textBox1.Text);
                        cmd.Parameters.AddWithValue("@PassportNIC", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Mobile", textBox4.Text);
                        cmd.Parameters.AddWithValue("@HotelName", textBox5.Text);
                        cmd.Parameters.AddWithValue("@RoomNumber", textBox8.Text);
                        cmd.Parameters.AddWithValue("@StreetAddress", textBox6.Text);
                        cmd.Parameters.AddWithValue("@City", textBox9.Text);
                        cmd.Parameters.AddWithValue("@NearestLandmark", textBox7.Text);
                        cmd.Parameters.AddWithValue("@LeaveAtReception", checkBox1.Checked);
                        cmd.Parameters.AddWithValue("@image", imagePath ?? (object)DBNull.Value);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                            MessageBox.Show("Data Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("No data was saved!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dialog.FileName;
                    picture.ImageLocation = imagePath;
                }
            }
            catch (Exception ex)
            {
                // Optionally handle exception
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            // DateTimePicker removed, so set deliveryDate to now
            deliveryDate = DateTime.Now;
            picture.Image = null;
        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
    }

