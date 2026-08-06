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
                string imagePath = row.Cells["ImagePath"]?.Value?.ToString();
                string customerName = row.Cells["FullName"]?.Value?.ToString() ?? "Customer";

                if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                {
                    MessageBox.Show($"No image file found for {customerName}.\nFile Path: {imagePath}", "Image Preview", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (Form previewForm = new Form())
                {
                    previewForm.Text = $"Customer Design Preview - {customerName}";
                    previewForm.Size = new Size(700, 600);
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
                        Text = $"🎨 Customer Artwork: {customerName}",
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        ForeColor = Color.FromArgb(238, 217, 196),
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    topPanel.Controls.Add(lblTitle);
                    previewForm.Controls.Add(topPanel);

                    PictureBox picView = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.White,
                        BorderStyle = BorderStyle.FixedSingle
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
                        Padding = new Padding(30, 20, 30, 80),
                        BackColor = Color.FromArgb(78, 52, 46)
                    };
                    cardPanel.Controls.Add(picView);
                    previewForm.Controls.Add(cardPanel);

                    Button btnOpenExternal = new Button
                    {
                        Text = "🖼️ OPEN FULL IMAGE / PRINT",
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        BackColor = Color.FromArgb(238, 217, 196),
                        ForeColor = Color.FromArgb(78, 52, 46),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Size = new Size(260, 45),
                        Location = new Point(previewForm.ClientSize.Width - 290, previewForm.ClientSize.Height - 60),
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
            string query = "SELECT * FROM CustomerDelivery"; 

            using (System.Data.SQLite.SQLiteConnection conn = DbHelper.GetConnection())
            {
                System.Data.SQLite.SQLiteDataAdapter adapter = new System.Data.SQLite.SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
    }
}
