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
                        decimal getprice = Convert.ToDecimal(selectedCard.productPrice.Replace("$",""));
                        int getQuantity = Convert.ToInt32(selectedCard.productQuantity);



                        int currentQty = 0;
                        int.TryParse(row.Cells["QTY"].Value?.ToString(), out currentQty);
                        int addQty = 1;
                        int.TryParse(selectedCard.productQuantity, out addQty);

                        row.Cells["Price"].Value = getprice * (currentQty + addQty);
                        row.Cells["QTY"].Value = selectedCard.productQuantity;
                        row.Cells["prodName"].Value = selectedCard.productName;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {

                    decimal getprice = Convert.ToDecimal(selectedCard.productPrice.Replace("$", ""));
                    int getQuantity = Convert.ToInt32(selectedCard.productQuantity);

                    int qty = 1;
                    int.TryParse(selectedCard.productQuantity, out qty);
                    dataGridView1.Rows.Add(selectedCard.id, selectedCard.productName, getQuantity,getprice  * getQuantity);
                }
                updateTotalprice();
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
        bool check = false;
        private void shop_placeOrderBtn_Click(object sender, EventArgs e)
        {
            if (!check)
            {
                MessageBox.Show("Invalid: Insufficient Amount", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to proceed?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection connect = new SqlConnection(connection))
                    {
                        connect.Open();

                        string countData = "SELECT COUNT(*) FROM orders";
                        int count = 1;

                        using (SqlCommand cData = new SqlCommand(countData, connect))
                        {
                            count = Convert.ToInt32(cData.ExecuteScalar()) + 1;
                        }

                        List<string> productIds = new List<string>();
                        List<string> quantities = new List<string>();
                        List<string> prices = new List<string>();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells["id"].Value != null && row.Cells["QTY"].Value != null && row.Cells["Price"].Value != null)
                            {
                                productIds.Add(row.Cells["id"].Value.ToString());
                                quantities.Add(row.Cells["QTY"].Value.ToString());
                                prices.Add(row.Cells["Price"].Value.ToString());
                            }
                        }

                        string productIdsStr = string.Join(",", productIds);
                        string quantitiesStr = string.Join(",", quantities);
                        string pricesStr = string.Join(",", prices);

                        decimal totalAmount = Convert.ToDecimal(shop_total.Text.Replace("$", ""));

                        string insertData = "INSERT INTO orders (customerId, productids, quantities, prices, total, date_order) VALUES(@cid, @pid, @qty, @price, @total, @date)";
                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@cid", $"CID-{count}");
                            cmd.Parameters.AddWithValue("@pid", productIdsStr);
                            cmd.Parameters.AddWithValue("@qty", quantitiesStr);
                            cmd.Parameters.AddWithValue("@price", pricesStr);
                            cmd.Parameters.AddWithValue("@total", totalAmount);

                            DateTime today = DateTime.Now;
                            cmd.Parameters.AddWithValue("@date", today);

                            int rowAffected = cmd.ExecuteNonQuery();

                            if (rowAffected > 0)
                            {
                                for (int q = 0; q < productIds.Count; q++)
                                {
                                    string updateData = "UPDATE products SET stock = stock - @qty WHERE id = @id";
                                    using (SqlCommand updateCmd = new SqlCommand(updateData, connect))
                                    {
                                        updateCmd.Parameters.AddWithValue("@qty", quantities[q]);
                                        updateCmd.Parameters.AddWithValue("@id", productIds[q]);
                                        updateCmd.ExecuteNonQuery();
                                    }
                                }
                                MessageBox.Show("Order placed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Order placement failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }
                        }
                    }
                }
            }
        }

        private void shop_receiptBtn_Click(object sender, EventArgs e)
        {

        }

        private void updateTotalprice()
        {
            decimal totalprice = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow && row.Cells["id"].Value != null && row.Cells["Price"].Value != null)
                {
                    decimal price = 0;
                    decimal.TryParse(row.Cells["Price"].Value.ToString(), out price);
                    totalprice += price;
                }
            }

            shop_total.Text = $"{totalprice:F2}";
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    decimal getTotal = Convert.ToDecimal(shop_total.Text.ToString().Replace("$", ""));
                    decimal getChange = Convert.ToDecimal(textBox1.Text);

                    if (getTotal > getChange)
                    {
                        check = false;
                        MessageBox.Show("Invalid: Insufficient Amount", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        check = true;
                        shop_amount.Text = $"${getChange - getTotal:0.00}";
                        e.SuppressKeyPress = true;
                    }
                }
                catch (Exception ex)
                {
                    check = false;
                    MessageBox.Show($"Error: {ex}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
