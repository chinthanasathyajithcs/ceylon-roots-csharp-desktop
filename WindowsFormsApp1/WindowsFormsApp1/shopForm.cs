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
using System.IO;

namespace WindowsFormsApp1
{
    public partial class shopForm : UserControl
    {
        string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=CSpassword2025;Connect Timeout=30;Encrypt=True";

        public shopForm()
        {
            InitializeComponent();
            loadProducts();
        }

        public void cardItems(int id, string productname, string stock, string price, Image image, string productId, string category, string quantity)
        {
            var card = new cardProduct()
            {
                id = id,
                productName = productname,
                productStock = stock,
                productPrice = price,
                productId = productId,
                category = category,
                productImage = image,
                productQuantity = quantity
            };

            flowLayoutPanel1.Controls.Add(card);
            card.selectCard += (q, w) =>
            {
                var selectedCard = (cardProduct)q;
                bool flag = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["id"].Value != null && (int)row.Cells["id"].Value == selectedCard.id)
                    {
                        
                        int currentQty = 0;
                        int.TryParse(row.Cells["QTY"].Value?.ToString(), out currentQty);
                        int addQty = 1;
                        int.TryParse(selectedCard.productQuantity, out addQty);
                        row.Cells["QTY"].Value = currentQty + addQty;
                        row.Cells["prodName"].Value = selectedCard.productName;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    int qty = 1;
                    int.TryParse(selectedCard.productQuantity, out qty);
                    dataGridView1.Rows.Add(selectedCard.id, selectedCard.productName, qty);
                }
            };
        }

        public void loadProducts()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();

                    string selectData = "SELECT * FROM products WHERE status = 'Available'";
                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        flowLayoutPanel1.Controls.Clear();

                        foreach (DataRow row in table.Rows)
                        {
                            int id = row["id"] != DBNull.Value ? (int)row["id"] : 0;
                            string productname = row["productname"] != DBNull.Value ? row["productname"].ToString() : "N/A";
                            string stock = row["stock"] != DBNull.Value ? row["stock"].ToString() : "0";
                            string price = row["price"] != DBNull.Value ? $"{row["price"]:0.00}" : "0.00";
                            string productId = row["productid"] != DBNull.Value ? row["productid"].ToString() : "N/A";
                            string category = row["category"] != DBNull.Value ? row["category"].ToString() : "N/A";

                            Image image = null;
                            if (row["image"] != DBNull.Value)
                            {
                                string imagePath = row["image"].ToString();
                                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                                {
                                    try
                                    {
                                        image = Image.FromFile(imagePath);
                                    }
                                    catch (Exception ex)
                                    {
                                        image = null;
                                    }
                                }
                            }

                            cardItems(id, productname, stock, price, image, productId, category, "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
