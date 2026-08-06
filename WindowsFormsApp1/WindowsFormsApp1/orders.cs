using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class orders : UserControl
    {
        public orders()
        {
            InitializeComponent();
            LoadCustomerDeliveryData();

            if (dataGridView1 != null)
            {
                dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ShowCustomerImagePreview(e.RowIndex);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ShowCustomerImagePreview(e.RowIndex);
            }
        }

        private void ShowCustomerImagePreview(int rowIndex)
        {
            try
            {
                var row = dataGridView1.Rows[rowIndex];
                object orderIdObj = row.Cells["ID"]?.Value ?? row.Cells["id"]?.Value;
                string imagePath = row.Cells["ImagePath"]?.Value?.ToString();
                string customerName = row.Cells["FullName"]?.Value?.ToString() ?? "Customer";
                string currentBagSize = row.Cells["BagSize"]?.Value?.ToString();

                if (string.IsNullOrWhiteSpace(currentBagSize))
                {
                    currentBagSize = "Medium";
                }

                if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                {
                    MessageBox.Show($"No image file found for {customerName}.\nBag Size: {currentBagSize}\nFile Path: {imagePath}", "Image Preview", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (Form previewForm = new Form())
                {
                    previewForm.Text = $"Customer Design Preview - {customerName}";
                    previewForm.Size = new Size(800, 650);
                    previewForm.StartPosition = FormStartPosition.CenterParent;
                    previewForm.BackColor = Color.FromArgb(78, 52, 46);

                    Panel topPanel = new Panel
                    {
                        Dock = DockStyle.Top,
                        Height = 60,
                        BackColor = Color.FromArgb(60, 40, 35)
                    };

                    Label lblTitle = new Label
                    {
                        Text = $" Customer: {customerName}",
                        Font = new Font("Segoe UI", 13, FontStyle.Bold),
                        ForeColor = Color.FromArgb(238, 217, 196),
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    topPanel.Controls.Add(lblTitle);
                    previewForm.Controls.Add(topPanel);

                    PictureBox picView = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        BackColor = Color.FromArgb(60, 40, 35),
                        BorderStyle = BorderStyle.None
                    };

                    try
                    {
                        using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                        {
                            picView.Image = Image.FromStream(stream);
                        }
                    }
                    catch { }

                    Panel cardPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        Padding = new Padding(30, 10, 30, 80),
                        BackColor = Color.FromArgb(78, 52, 46)
                    };
                    cardPanel.Controls.Add(picView);
                    previewForm.Controls.Add(cardPanel);

                    Label lblSizeTag = new Label
                    {
                        Text = "Bag Size:",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(238, 217, 196),
                        AutoSize = true,
                        Location = new Point(30, previewForm.ClientSize.Height - 55),
                        Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                    };
                    previewForm.Controls.Add(lblSizeTag);
                    lblSizeTag.BringToFront();

                    ComboBox cmbBagSize = new ComboBox
                    {
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        BackColor = Color.FromArgb(238, 217, 196),
                        ForeColor = Color.FromArgb(78, 52, 46),
                        Size = new Size(120, 35),
                        Location = new Point(110, previewForm.ClientSize.Height - 58),
                        Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                    };
                    cmbBagSize.Items.AddRange(new object[] { "Small", "Medium", "Large", "Extra Large" });
                    if (cmbBagSize.Items.Contains(currentBagSize))
                        cmbBagSize.SelectedItem = currentBagSize;
                    else
                        cmbBagSize.Text = currentBagSize;

                    previewForm.Controls.Add(cmbBagSize);
                    cmbBagSize.BringToFront();

                    Button btnSaveSize = new Button
                    {
                        Text = "💾 SAVE SIZE",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        BackColor = Color.FromArgb(238, 217, 196),
                        ForeColor = Color.FromArgb(78, 52, 46),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Size = new Size(130, 35),
                        Location = new Point(240, previewForm.ClientSize.Height - 58),
                        Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                    };
                    btnSaveSize.FlatAppearance.BorderSize = 0;
                    btnSaveSize.Click += (s, e) =>
                    {
                        string newSize = cmbBagSize.SelectedItem?.ToString() ?? cmbBagSize.Text;
                        if (orderIdObj != null && int.TryParse(orderIdObj.ToString(), out int orderId))
                        {
                            try
                            {
                                using (var conn = DbHelper.GetConnection())
                                {
                                    conn.Open();
                                    string sql = "UPDATE CustomerDelivery SET BagSize = @BagSize WHERE ID = @ID";
                                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                                    {
                                        cmd.Parameters.AddWithValue("@BagSize", newSize);
                                        cmd.Parameters.AddWithValue("@ID", orderId);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                MessageBox.Show($"Bag Size updated to '{newSize}' successfully!", "Bag Size Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadCustomerDeliveryData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to update bag size: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };
                    previewForm.Controls.Add(btnSaveSize);
                    btnSaveSize.BringToFront();

                    Button btnOpenExternal = new Button
                    {
                        Text = "🖼️ OPEN FULL IMAGE / PRINT",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        BackColor = Color.FromArgb(238, 217, 196),
                        ForeColor = Color.FromArgb(78, 52, 46),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Size = new Size(230, 35),
                        Location = new Point(previewForm.ClientSize.Width - 260, previewForm.ClientSize.Height - 58),
                        Anchor = AnchorStyles.Bottom | AnchorStyles.Right
                    };
                    btnOpenExternal.FlatAppearance.BorderSize = 0;
                    btnOpenExternal.Click += (s, e) =>
                    {
                        try { Process.Start(imagePath); } catch { }
                    };
                    previewForm.Controls.Add(btnOpenExternal);
                    btnOpenExternal.BringToFront();

                    previewForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening image preview: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadCustomerDeliveryData()
        {
            string query = @"
                SELECT 
                    COALESCE(NULLIF(productID, ''), '001') AS productID, 
                    COALESCE(NULLIF(productName, ''), 'Custom Tote Bag') AS productName, 
                    FullName, 
                    COALESCE(NULLIF(BagSize, ''), 'Medium') AS BagSize, 
                    Mobile, 
                    Email, 
                    HotelName, 
                    RoomNumber, 
                    StreetAddress, 
                    City, 
                    NearestLandmark, 
                    ImagePath,
                    ID 
                FROM CustomerDelivery";

            using (System.Data.SQLite.SQLiteConnection conn = DbHelper.GetConnection())
            {
                System.Data.SQLite.SQLiteDataAdapter adapter = new System.Data.SQLite.SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

                // Clean modern grid styling with horizontal & vertical scrollbars
                dataGridView1.BackgroundColor = System.Drawing.Color.White;
                dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Both;

                if (dataGridView1.Columns.Contains("ID"))
                {
                    dataGridView1.Columns["ID"].Visible = false;
                }
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0)
            {
                MessageBox.Show("Please select an order row to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = dataGridView1.CurrentRow;
            object idObj = selectedRow.Cells["ID"]?.Value;
            string customerName = selectedRow.Cells["FullName"]?.Value?.ToString() ?? "Order";

            if (idObj != null && int.TryParse(idObj.ToString(), out int orderId))
            {
                if (MessageBox.Show($"Are you sure you want to delete order #{orderId} for {customerName}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        using (System.Data.SQLite.SQLiteConnection conn = DbHelper.GetConnection())
                        {
                            conn.Open();
                            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("DELETE FROM CustomerDelivery WHERE ID = @ID", conn))
                            {
                                cmd.Parameters.AddWithValue("@ID", orderId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        LoadCustomerDeliveryData();
                        MessageBox.Show("Order deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to delete order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("There are no orders to clear.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to CLEAR ALL CUSTOMER ORDERS? This cannot be undone.", "Confirm Clear All", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (System.Data.SQLite.SQLiteConnection conn = DbHelper.GetConnection())
                    {
                        conn.Open();
                        using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("DELETE FROM CustomerDelivery", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadCustomerDeliveryData();
                    MessageBox.Show("All orders have been cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to clear orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
