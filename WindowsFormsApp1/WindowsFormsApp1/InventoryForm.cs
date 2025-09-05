using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class InventoryForm : UserControl
    {
        string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=CSpassword2025;Connect Timeout=30;Encrypt=True";
        public InventoryForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void inventory_add_Click(object sender, EventArgs e)
        {
            if (inventory_productID.Text == "" || inventory_productName.Text == "" || inventory_category.SelectedIndex == -1 || inventory_stock.Text == "" || inventory_price.Text == "" || inventory_status.Text == "" || pictureBox1.Image == null)
            {
                MessageBox.Show("Empty Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                using (SqlConnection connect = new SqlConnection(connection)) 
                {
                    connect.Open();

                    string checkProductId = "SELECT * FROM products WHERE productid = @prodid";

                    using (SqlCommand checkProdId = new SqlCommand(checkProductId, connect))
                    {
                        checkProdId.Parameters.AddWithValue("@prodid", inventory_productID.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(checkProdId);
                        DataTable table = new DataTable();

                        adapter.Fill(table);
                        if (table.Rows.Count != 0)
                        {
                            MessageBox.Show($"{inventory_productID.Text.Trim()} is existing already", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else 
                        {
                            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                            string insertData = "INSERT INTO products (productid, productname, category, stock, price, status, image,date_import) " +
                                "VALUES(@productid, @productname, @category, @stock, @price, @status,  @image, @date)";

                            string relativepath = Path.Combine("products_directory", inventory_productID.Text.Trim() + "jpg");
                            string path = Path.Combine(baseDirectory, relativepath);

                            string directoryPath = Path.GetDirectoryName(path);

                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            File.Copy(pictureBox1.ImageLocation, path, true);

                            using (SqlCommand cmd = new SqlCommand(insertData, connect))
                            {
                                cmd.Parameters.AddWithValue("@productid", inventory_productID.Text.Trim());
                                cmd.Parameters.AddWithValue("@productname", inventory_productName.Text.Trim());
                                cmd.Parameters.AddWithValue("@category", inventory_category.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@stock", inventory_stock.Text.Trim());
                                cmd.Parameters.AddWithValue("@price", inventory_price.Text.Trim());
                                cmd.Parameters.AddWithValue("@status", inventory_status.Text.Trim());
                                cmd.Parameters.AddWithValue("@image", path);
                                

                                DateTime today = DateTime.Now;
                                cmd.Parameters.AddWithValue("@date", today);

                                cmd.ExecuteNonQuery();

                                MessageBox.Show("Added successfully", "information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                clearFields();



                            }
                        }
                    }
                }
                    }

                        
        }

        public void displayCategories()
        {

        }

        private void inventory_import_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "image Foles(*.jpg, *.png|*.jpg;*.png";

                string imagePath = "";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dialog.FileName;
                    pictureBox1.ImageLocation = imagePath;
                }
            }
            catch (Exception ex)
            {

            }
        }

        void clearFields() 
        {
            inventory_productID.Clear();
            inventory_productName.Clear();
            inventory_category.SelectedIndex = -1;
            inventory_stock.Clear();
            inventory_price.Clear();
            inventory_status.SelectedIndex = -1;
            pictureBox1.Image = null;
        }
        
        
        
        private void InventoryForm_Load(object sender, EventArgs e)
        {

        }

        private void inventory_clear_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private int getID = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                getID = (int)row.Cells[0].Value;
                inventory_productID.Text = row.Cells[1].Value.ToString();
                inventory_productName.Text = row.Cells[2].Value.ToString();
                inventory_category.Text = row.Cells[3].Value.ToString();
                inventory_stock.Text = row.Cells[4].Value.ToString();
                inventory_price.Text = row.Cells[5].Value.ToString();
                inventory_status.Text = row.Cells[6].Value.ToString();
                

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
