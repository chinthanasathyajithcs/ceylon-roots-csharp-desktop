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
        // OLD AZURE CONNECTION STRING (Preserved as comment):
        // string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=*****;Connect Timeout=30;Encrypt=True";
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
                            bool hasCustomId = int.TryParse(categories_id.Text.Trim(), out int customId) && customId > 0;
                            string insertData = hasCustomId ?
                                "INSERT INTO categories (id, category, status, date_insert) VALUES(@id, @cat, @status, @date)" :
                                "INSERT INTO categories (category, status, date_insert) VALUES(@cat, @status, @date)";

                            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(insertData, connect))
                            {
                                if (hasCustomId)
                                    cmd.Parameters.AddWithValue("@id", customId);

                                cmd.Parameters.AddWithValue("@cat", categories_category.Text.Trim());
                                cmd.Parameters.AddWithValue("@status", categories_status.SelectedItem != null ? categories_status.SelectedItem.ToString() : categories_status.Text);

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
            categories_id.Clear();
            categories_category.Clear();
            categories_status.SelectedIndex = -1;
            getID = 0;
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

            // Clean modern grid styling with scrollbars
            dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Both;

            if (dataGridView1.Columns.Contains("ID"))
            {
                dataGridView1.Columns["ID"].Visible = false;
            }
            if (dataGridView1.Columns.Contains("categoryID"))
            {
                dataGridView1.Columns["categoryID"].HeaderText = "Category ID";
            }
            if (dataGridView1.Columns.Contains("category"))
            {
                dataGridView1.Columns["category"].HeaderText = "Category Name";
            }
            if (dataGridView1.Columns.Contains("status"))
            {
                dataGridView1.Columns["status"].HeaderText = "Status";
            }
            if (dataGridView1.Columns.Contains("DateInsert"))
            {
                dataGridView1.Columns["DateInsert"].HeaderText = "Date Inserted";
            }
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

                var idVal = row.Cells["categoryID"]?.Value ?? row.Cells["ID"]?.Value ?? row.Cells[0]?.Value;
                if (idVal != null && int.TryParse(idVal.ToString(), out int parsedId))
                {
                    getID = parsedId;
                }

                categories_id.Text = getID > 0 ? getID.ToString() : "";
                categories_category.Text = row.Cells["category"]?.Value?.ToString();
                categories_status.Text = row.Cells["status"]?.Value?.ToString();
            }
        }

        private void categories_updateBtn_Click(object sender, EventArgs e)
        {
            int targetId = getID;
            int newId = targetId;

            if (int.TryParse(categories_id.Text.Trim(), out int inputId) && inputId > 0)
            {
                newId = inputId;
                if (targetId == 0)
                    targetId = newId;
            }

            if (targetId == 0)
            {
                MessageBox.Show("Please select a category row or enter a valid Category ID to update.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to update Category ID: {targetId}?", "Confirmation Message",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
                {
                    connect.Open();

                    string statusVal = categories_status.SelectedItem != null ? categories_status.SelectedItem.ToString() : categories_status.Text;
                    string updateData = "UPDATE categories SET id = @newId, category = @cat, status = @status WHERE id = @targetId";

                    using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(updateData, connect))
                    {
                        cmd.Parameters.AddWithValue("@newId", newId);
                        cmd.Parameters.AddWithValue("@cat", categories_category.Text.Trim());
                        cmd.Parameters.AddWithValue("@status", statusVal);
                        cmd.Parameters.AddWithValue("@targetId", targetId);

                        cmd.ExecuteNonQuery();
                        clearfields();
                        displayCategories();

                        MessageBox.Show("Updated Successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            displayCategories();
        }

        private void categories_deleteBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(categories_id.Text.Trim(), out int inputId) && inputId > 0)
            {
                getID = inputId;
            }

            if (getID == 0)
            {
                MessageBox.Show("Please enter or select a valid Category ID to delete.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show($"Are you sure you want to delete Category ID: {getID}?", "Confirmation Message",
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
