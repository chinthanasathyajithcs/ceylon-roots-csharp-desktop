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
        // OLD AZURE CONNECTION STRING (Preserved as comment):
        // string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=*****;Connect Timeout=30;Encrypt=True";
        public InventoryForm()
        {
            InitializeComponent();
            displayCategories();
        }

        private void displayproducts()
        {
            productsList pList = new productsList();
            List<productsList> listData = new productsList().productListData();
            dataGridView2.DataSource = listData;

            // Clean modern grid styling with scrollbars
            dataGridView2.BackgroundColor = System.Drawing.Color.White;
            dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.Both;

            if (dataGridView2.Columns.Contains("ID"))
            {
                dataGridView2.Columns["ID"].Visible = false;
            }
            if (dataGridView2.Columns.Contains("DateUpdate"))
            {
                dataGridView2.Columns["DateUpdate"].Visible = false;
            }
            if (dataGridView2.Columns.Contains("date_update"))
            {
                dataGridView2.Columns["date_update"].Visible = false;
            }
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
                using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
                {
                    connect.Open();

                    string checkProductId = "SELECT * FROM products WHERE productid = @prodid";

                    using (System.Data.SQLite.SQLiteCommand checkProdId = new System.Data.SQLite.SQLiteCommand(checkProductId, connect))
                    {
                        checkProdId.Parameters.AddWithValue("@prodid", inventory_productID.Text.Trim());

                        System.Data.SQLite.SQLiteDataAdapter adapter = new System.Data.SQLite.SQLiteDataAdapter(checkProdId);
                        DataTable table = new DataTable();

                        adapter.Fill(table);
                        if (table.Rows.Count != 0)
                        {
                            MessageBox.Show($"{inventory_productID.Text.Trim()} is existing already", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            string relativePath = "";
                            if (!string.IsNullOrEmpty(pictureBox1.ImageLocation) && File.Exists(pictureBox1.ImageLocation))
                            {
                                string ext = Path.GetExtension(pictureBox1.ImageLocation);
                                string fileName = inventory_productID.Text.Trim() + ext;
                                string productsDir = Path.Combine(Application.StartupPath, "products_directory");
                                Directory.CreateDirectory(productsDir);
                                string targetFullPath = Path.Combine(productsDir, fileName);
                                File.Copy(pictureBox1.ImageLocation, targetFullPath, true);
                                relativePath = Path.Combine("products_directory", fileName);
                            }

                            string insertData = "INSERT INTO products (productid, productname, category, stock, price, status, image, date_insert) " +
                                "VALUES(@productid, @productname, @category, @stock, @price, @status, @image, @date)";

                            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(insertData, connect))
                            {
                                cmd.Parameters.AddWithValue("@productid", inventory_productID.Text.Trim());
                                cmd.Parameters.AddWithValue("@productname", inventory_productName.Text.Trim());
                                cmd.Parameters.AddWithValue("@category", inventory_category.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@stock", inventory_stock.Text.Trim());
                                cmd.Parameters.AddWithValue("@price", inventory_price.Text.Trim());
                                cmd.Parameters.AddWithValue("@status", inventory_status.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@image", relativePath);

                                DateTime today = DateTime.Now;
                                cmd.Parameters.AddWithValue("@date", today.ToString("yyyy-MM-dd HH:mm:ss"));

                                cmd.ExecuteNonQuery();

                                MessageBox.Show("Added successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                clearFields();
                                displayproducts();
                            }
                        }
                    }
                }
                displayCategories();
            }
        }

        public void displayCategories()
        {
            inventory_category.Items.Clear();

            using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
            {
                connect.Open();

                string selectcat = "SELECT * FROM categories";
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(selectcat, connect))
                {
                    System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string category = reader["category"].ToString();
                        inventory_category.Items.Add(category);
                    }
                }
            }
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
            getID = 0;
        }
        
        
        
        private void InventoryForm_Load(object sender, EventArgs e)
        {
            displayproducts();
        }

        private void inventory_clear_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private int getID = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView2.Rows.Count)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                var idVal = row.Cells["ID"]?.Value ?? row.Cells[0]?.Value;
                if (idVal != null && int.TryParse(idVal.ToString(), out int parsedId))
                {
                    getID = parsedId;
                }

                inventory_productID.Text = row.Cells["productID"]?.Value?.ToString() ?? row.Cells[1]?.Value?.ToString() ?? "";
                inventory_productName.Text = row.Cells["productName"]?.Value?.ToString() ?? row.Cells[2]?.Value?.ToString() ?? "";
                inventory_category.Text = row.Cells["category"]?.Value?.ToString() ?? row.Cells[3]?.Value?.ToString() ?? "";
                inventory_stock.Text = row.Cells["stock"]?.Value?.ToString() ?? row.Cells[4]?.Value?.ToString() ?? "";
                inventory_price.Text = row.Cells["price"]?.Value?.ToString() ?? row.Cells[5]?.Value?.ToString() ?? "";
                inventory_status.Text = row.Cells["status"]?.Value?.ToString() ?? row.Cells[6]?.Value?.ToString() ?? "";

                string imagePath = row.Cells["image"]?.Value?.ToString() ?? (row.Cells.Count > 7 ? row.Cells[7]?.Value?.ToString() : "");

                try
                {
                    if (!string.IsNullOrWhiteSpace(imagePath) && System.IO.File.Exists(imagePath))
                    {
                        pictureBox1.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                }
                catch
                {
                    pictureBox1.Image = null;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void inventory_update_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to update ID {getID}?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (getID == 0)
                {
                    MessageBox.Show("Empty fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
                    {
                        connect.Open();

                        string checkProductId = "SELECT * FROM products WHERE productid = @prodid";

                        using (System.Data.SQLite.SQLiteCommand checkProd = new System.Data.SQLite.SQLiteCommand(checkProductId, connect))
                        {
                            checkProd.Parameters.AddWithValue("@prodid", inventory_productID.Text.Trim());

                            System.Data.SQLite.SQLiteDataAdapter adapter = new System.Data.SQLite.SQLiteDataAdapter(checkProd);
                            DataTable table = new DataTable();

                            adapter.Fill(table);

                            if (table.Rows.Count >= 2)
                            {
                                MessageBox.Show(inventory_productID.Text.Trim() + " was existing already", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string updateData = "UPDATE products SET productid = @prodid, productname = @prodname, category = @cat, " +
                                    "stock = @stock, price = @price, status = @status WHERE id = @id";

                                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(updateData, connect))
                                {
                                    cmd.Parameters.AddWithValue("@prodid", inventory_productID.Text.Trim());
                                    cmd.Parameters.AddWithValue("@prodname", inventory_productName.Text.Trim());
                                    cmd.Parameters.AddWithValue("@cat", inventory_category.SelectedItem.ToString());
                                    cmd.Parameters.AddWithValue("@stock", inventory_stock.Text.Trim());
                                    cmd.Parameters.AddWithValue("@price", inventory_price.Text.Trim());
                                    cmd.Parameters.AddWithValue("@status", inventory_status.SelectedItem.ToString());
                                    cmd.Parameters.AddWithValue("@id", getID);

                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("Updated successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                }
            }
            displayproducts();
        }

        private void inventory_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete ID {getID}?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (getID == 0)
                {
                    MessageBox.Show("Empty fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
                    {
                        connect.Open();

                        string updateData = "DELETE FROM products WHERE id = @id";

                        using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(updateData, connect))
                        {
                            cmd.Parameters.AddWithValue("@id", getID);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Deleted successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clearFields();
                        }
                    }
                }
                displayproducts();
            }
        }
    }
}
