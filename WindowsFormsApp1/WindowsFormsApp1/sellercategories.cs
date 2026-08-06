using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class sellercategories : UserControl
    {
        // OLD AZURE CONNECTION STRING (Preserved as requested):
        // string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=CSpassword2025;Connect Timeout=30;Encrypt=True";
        public sellercategories()
        {
            InitializeComponent();
            displayCategories();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (categories_category.Text == "" || categories_status.SelectedIndex == -1)
            {
                MessageBox.Show("Empty fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
                {
                    connect.Open();

                    string selectCategory = "SELECT * FROM categories WHERE category = @cat";

                    using (System.Data.SQLite.SQLiteCommand checkCat = new System.Data.SQLite.SQLiteCommand(selectCategory, connect))
                    {
                        checkCat.Parameters.AddWithValue("@cat", categories_category.Text.Trim());

                        System.Data.SQLite.SQLiteDataAdapter adapter = new System.Data.SQLite.SQLiteDataAdapter(checkCat);
                        DataTable table = new DataTable();

                        adapter.Fill(table);

                        if (table.Rows.Count > 0)
                        {
                            MessageBox.Show(categories_category.Text.Trim() + " is existing already", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            string insertData = "INSERT INTO categories (category, status, date_insert) VALUES(@cat, @status, @date)";

                            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(insertData, connect))
                            {
                                cmd.Parameters.AddWithValue("@cat", categories_category.Text.Trim());
                                cmd.Parameters.AddWithValue("@status", categories_status.SelectedItem.ToString());

                                DateTime today = DateTime.Now;
                                cmd.Parameters.AddWithValue("@date", today.ToString("yyyy-MM-dd HH:mm:ss"));

                                cmd.ExecuteNonQuery();

                                MessageBox.Show("Added Successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                clearfields();
                            }
                        }
                    }
                }
            }
            displayCategories();
        }

        void clearfields()
        {
            categories_category.Clear();
            categories_status.SelectedIndex = -1;
        }

        private void categories_clearBtn_Click(object sender, EventArgs e)
        {
            clearfields();
        }

        public void displayCategories()
        {
            categoriesList cData = new categoriesList();
            List<categoriesList> list = cData.categoriesListData();

            dataGridView1.DataSource = list;
        }

        private void inventory_status_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private int getID = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                getID = Convert.ToInt32(row.Cells[0].Value);
                categories_category.Text = row.Cells[1].Value?.ToString();
                categories_status.Text = row.Cells[2].Value?.ToString();
            }
        }

        private void categories_updateBtn_Click(object sender, EventArgs e)
        {
            if (getID == 0)
            {
                MessageBox.Show("Select item first", "Error Messsage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show($"Are you sure you want to update this ID: {getID}", "Comfirmation Message",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
                    {
                        connect.Open();

                        string updateData = "UPDATE categories SET category = @cat, status = @status WHERE ID = @id";

                        using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(updateData, connect))
                        {
                            cmd.Parameters.AddWithValue("@cat", categories_category.Text.Trim());
                            cmd.Parameters.AddWithValue("@status", categories_status.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@id", getID);

                            cmd.ExecuteNonQuery();
                            clearfields();
                            displayCategories();

                            MessageBox.Show("Updated Successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            displayCategories();
        }

        private void categories_deleteBtn_Click(object sender, EventArgs e)
        {
            if (getID == 0)
            {
                MessageBox.Show("Select item first", "Error Messsage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show($"Are you sure you want to delete this ID: {getID}", "Comfirmation Message",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
                    {
                        connect.Open();

                        string updateData = "DELETE FROM categories WHERE id = @id";

                        using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(updateData, connect))
                        {
                            cmd.Parameters.AddWithValue("@id", getID);

                            cmd.ExecuteNonQuery();
                            clearfields();
                            displayCategories();

                            MessageBox.Show("Deleted Successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            displayCategories();
        }
    }
}
