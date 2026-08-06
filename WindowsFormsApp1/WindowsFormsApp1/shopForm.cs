using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace WindowsFormsApp1
{
    public partial class shopForm : UserControl
    {
        // OLD AZURE CONNECTION STRING (Preserved as requested):
        // string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=CSpassword2025;Connect Timeout=30;Encrypt=True";

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

                int availableStock = 0;
                int.TryParse(selectedCard.productStock, out availableStock);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["id"].Value != null && (int)row.Cells["id"].Value == selectedCard.id)
                    {
                        int currentQty = 0;
                        int.TryParse(row.Cells["QTY"].Value?.ToString(), out currentQty);

                        int addQty = 1;
                        int.TryParse(selectedCard.productQuantity, out addQty);

                        int newQty = currentQty + addQty;

                        if (newQty > availableStock)
                        {
                            MessageBox.Show($"Cannot add more than available stock ({availableStock}).", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        decimal getprice = Convert.ToDecimal(selectedCard.productPrice.Replace("$", ""));
                        row.Cells["Price"].Value = getprice * newQty;
                        row.Cells["QTY"].Value = newQty;
                        row.Cells["prodName"].Value = selectedCard.productName;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    int getQuantity = 1;
                    int.TryParse(selectedCard.productQuantity, out getQuantity);
                    if (getQuantity > availableStock)
                    {
                        MessageBox.Show($"Cannot add more than available stock ({availableStock}).", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    decimal getprice = Convert.ToDecimal(selectedCard.productPrice.Replace("$", ""));
                    dataGridView1.Rows.Add(selectedCard.id, selectedCard.productName, getQuantity, getprice * getQuantity);
                }
                updateTotalprice();
            };
        }

        public void loadProducts()
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
                {
                    connect.Open();

                    string selectData = "SELECT * FROM products WHERE status = 'Available'";
                    using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(selectData, connect))
                    {
                        System.Data.SQLite.SQLiteDataAdapter adapter = new System.Data.SQLite.SQLiteDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        flowLayoutPanel1.Controls.Clear();

                        foreach (DataRow row in table.Rows)
                        {
                            int id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                            string productname = row["productname"] != DBNull.Value ? row["productname"].ToString() : "N/A";
                            string stock = row["stock"] != DBNull.Value ? row["stock"].ToString() : "0";
                            string price = row["price"] != DBNull.Value ? $"${Convert.ToDecimal(row["price"]):0.00}" : "$0.00";
                            string productId = row["productid"] != DBNull.Value ? row["productid"].ToString() : "N/A";
                            string category = row["category"] != DBNull.Value ? row["category"].ToString() : "N/A";

                            Image image = null;
                            if (row["image"] != DBNull.Value)
                            {
                                string imagePath = row["image"].ToString();
                                if (!string.IsNullOrEmpty(imagePath))
                                {
                                    if (!Path.IsPathRooted(imagePath))
                                    {
                                        imagePath = Path.Combine(Application.StartupPath, imagePath);
                                    }
                                    if (File.Exists(imagePath))
                                    {
                                        try
                                        {
                                            image = Image.FromFile(imagePath);
                                        }
                                        catch
                                        {
                                            image = null;
                                        }
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
                MessageBox.Show($"Error: {ex.Message}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        bool check = false;

        private bool ValidateAndCalculateAmount()
        {
            try
            {
                if (decimal.TryParse(shop_total.Text.Replace("$", "").Trim(), out decimal getTotal) &&
                    decimal.TryParse(textBox1.Text.Replace("$", "").Trim(), out decimal getAmount))
                {
                    if (getAmount >= getTotal)
                    {
                        check = true;
                        shop_amount.Text = $"${getAmount - getTotal:0.00}";
                        return true;
                    }
                }
            }
            catch { }
            check = false;
            return false;
        }

        private void shop_placeOrderBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateAndCalculateAmount())
            {
                MessageBox.Show("Invalid: Insufficient Amount. Please enter an amount equal to or greater than the Total.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to proceed?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
                {
                    connect.Open();

                    string countData = "SELECT COUNT(*) FROM orders";
                    int count = 1;

                    using (System.Data.SQLite.SQLiteCommand cData = new System.Data.SQLite.SQLiteCommand(countData, connect))
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
                    using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(insertData, connect))
                    {
                        cmd.Parameters.AddWithValue("@cid", $"CID-{count}");
                        cmd.Parameters.AddWithValue("@pid", productIdsStr);
                        cmd.Parameters.AddWithValue("@qty", quantitiesStr);
                        cmd.Parameters.AddWithValue("@price", pricesStr);
                        cmd.Parameters.AddWithValue("@total", totalAmount);

                        DateTime today = DateTime.Now;
                        cmd.Parameters.AddWithValue("@date", today.ToString("yyyy-MM-dd HH:mm:ss"));

                        int rowAffected = cmd.ExecuteNonQuery();

                        if (rowAffected > 0)
                        {
                            for (int q = 0; q < productIds.Count; q++)
                            {
                                string updateData = "UPDATE products SET stock = stock - @qty WHERE id = @id";
                                using (System.Data.SQLite.SQLiteCommand updateCmd = new System.Data.SQLite.SQLiteCommand(updateData, connect))
                                {
                                    updateCmd.Parameters.AddWithValue("@qty", quantities[q]);
                                    updateCmd.Parameters.AddWithValue("@id", productIds[q]);
                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                            MessageBox.Show("Order placed successfully! You can now click the RECEIPT button to view or print your receipt.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Order placement failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private int rowIndex = 0;
        private void shop_receiptBtn_Click(object sender, EventArgs e)
        {
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(printDocument1_BeginPrint);

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
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
            ValidateAndCalculateAmount();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!ValidateAndCalculateAmount())
                {
                    MessageBox.Show("Invalid: Insufficient Amount", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Font titleFont = new Font("Arial", 18, FontStyle.Bold);
            Font subTitleFont = new Font("Arial", 11, FontStyle.Italic);
            Font headerFont = new Font("Arial", 11, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 10, FontStyle.Regular);
            Font summaryFont = new Font("Arial", 11, FontStyle.Bold);

            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            float rightMargin = e.MarginBounds.Right;
            float contentWidth = e.MarginBounds.Width;

            float currentY = topMargin;

            StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center };
            StringFormat rightFormat = new StringFormat { Alignment = StringAlignment.Far };
            StringFormat leftFormat = new StringFormat { Alignment = StringAlignment.Near };

            // 1. Title & Header
            g.DrawString("PURCHASE RECEIPT", titleFont, Brushes.DarkSlateGray, leftMargin + (contentWidth / 2), currentY, centerFormat);
            currentY += 30;

            g.DrawString("Traditional Handicrafts & Fabric Store", subTitleFont, Brushes.Black, leftMargin + (contentWidth / 2), currentY, centerFormat);
            currentY += 25;

            g.DrawString($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", bodyFont, Brushes.Black, leftMargin, currentY, leftFormat);
            currentY += 20;

            Pen gridPen = new Pen(Color.LightGray, 1);
            Pen thickPen = new Pen(Color.Black, 1.5f);

            g.DrawLine(thickPen, leftMargin, currentY, rightMargin, currentY);
            currentY += 8;

            // 2. Table Headers
            float colIdX = leftMargin + 10;
            float colNameX = leftMargin + 70;
            float colQtyX = leftMargin + 340;
            float colPriceX = rightMargin - 10;

            g.DrawString("ID", headerFont, Brushes.Black, colIdX, currentY, leftFormat);
            g.DrawString("Product Name", headerFont, Brushes.Black, colNameX, currentY, leftFormat);
            g.DrawString("Qty", headerFont, Brushes.Black, colQtyX, currentY, centerFormat);
            g.DrawString("Price ($)", headerFont, Brushes.Black, colPriceX, currentY, rightFormat);

            currentY += 24;
            g.DrawLine(thickPen, leftMargin, currentY, rightMargin, currentY);
            currentY += 6;

            // 3. Draw Grid Rows
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                string idStr = row.Cells["id"].Value?.ToString() ?? "";
                string nameStr = row.Cells["prodName"].Value?.ToString() ?? "";
                string qtyStr = row.Cells["QTY"].Value?.ToString() ?? "1";

                decimal priceVal = 0;
                if (row.Cells["Price"].Value != null)
                {
                    decimal.TryParse(row.Cells["Price"].Value.ToString().Replace("$", ""), out priceVal);
                }
                string priceStr = $"${priceVal:0.00}";

                g.DrawString(idStr, bodyFont, Brushes.Black, colIdX, currentY, leftFormat);
                g.DrawString(nameStr, bodyFont, Brushes.Black, colNameX, currentY, leftFormat);
                g.DrawString(qtyStr, bodyFont, Brushes.Black, colQtyX, currentY, centerFormat);
                g.DrawString(priceStr, bodyFont, Brushes.Black, colPriceX, currentY, rightFormat);

                currentY += 22;
                g.DrawLine(gridPen, leftMargin, currentY, rightMargin, currentY);
                currentY += 6;
            }

            currentY += 10;
            g.DrawLine(thickPen, leftMargin, currentY, rightMargin, currentY);
            currentY += 15;

            // 4. Totals Summary
            string totalStr = shop_total.Text.StartsWith("$") ? shop_total.Text : $"${shop_total.Text}";
            string amountStr = textBox1.Text.StartsWith("$") ? textBox1.Text : $"${textBox1.Text}";
            string changeStr = shop_amount.Text.StartsWith("$") ? shop_amount.Text : $"${shop_amount.Text}";

            g.DrawString($"Total Price:  {totalStr}", summaryFont, Brushes.Black, colPriceX, currentY, rightFormat);
            currentY += 22;

            g.DrawString($"Amount Paid:  {amountStr}", bodyFont, Brushes.Black, colPriceX, currentY, rightFormat);
            currentY += 20;

            g.DrawString($"Change Due:   {changeStr}", summaryFont, Brushes.DarkGreen, colPriceX, currentY, rightFormat);
            currentY += 35;

            // 5. Footer
            g.DrawLine(gridPen, leftMargin, currentY, rightMargin, currentY);
            currentY += 10;
            g.DrawString("Thank you for shopping with us! Please come again.", subTitleFont, Brushes.DimGray, leftMargin + (contentWidth / 2), currentY, centerFormat);

            e.HasMorePages = false;
        }
        

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sOrder_Click(object sender, EventArgs e)
        {
            /*Form2 Form2 = new Form2();
            Form2.Show();
            this.Hide();*/
            foreach (Form form in Application.OpenForms)
            {
                if (form is customerinterface)
                {
                    form.Close();
                    break;
                }
            }

            
            var Form2 = new Form2();
            Form2.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
    }



