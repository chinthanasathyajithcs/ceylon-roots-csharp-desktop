using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public static class DbHelper
    {
        // =========================================================================
        // ORIGINAL AZURE CONNECTION STRING (Preserved for reference):
        // public static string AzureConnection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=*****;Connect Timeout=30;Encrypt=True";
        // =========================================================================

        private static string dbFilePath = Path.Combine(Application.StartupPath, "app_data.db");
        public static string ConnectionString = $"Data Source={dbFilePath};Version=3;";

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        public static void InitializeDatabase()
        {
            try
            {
                if (!File.Exists(dbFilePath))
                {
                    SQLiteConnection.CreateFile(dbFilePath);
                }

                using (var conn = GetConnection())
                {
                    conn.Open();

                    // 1. Categories Table
                    string createCategories = @"
                        CREATE TABLE IF NOT EXISTS categories (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            productid TEXT DEFAULT '001',
                            productname TEXT DEFAULT 'Custom Tote Bag',
                            category TEXT NOT NULL,
                            status TEXT DEFAULT 'Available',
                            date_insert TEXT
                        );";
                    using (var cmd = new SQLiteCommand(createCategories, conn)) { cmd.ExecuteNonQuery(); }

                    // Ensure status, productid, productname columns exist in existing database files
                    try
                    {
                        using (var alterCmd = new SQLiteCommand("ALTER TABLE categories ADD COLUMN status TEXT DEFAULT 'Available';", conn))
                        {
                            alterCmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    try
                    {
                        using (var alterCmd = new SQLiteCommand("ALTER TABLE categories ADD COLUMN productid TEXT DEFAULT '001';", conn))
                        {
                            alterCmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    try
                    {
                        using (var alterCmd = new SQLiteCommand("ALTER TABLE categories ADD COLUMN productname TEXT DEFAULT 'Custom Tote Bag';", conn))
                        {
                            alterCmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    // 2. Products Table
                    string createProducts = @"
                        CREATE TABLE IF NOT EXISTS products (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            productid TEXT,
                            productname TEXT NOT NULL,
                            category TEXT,
                            price REAL,
                            stock INTEGER,
                            image TEXT,
                            status TEXT DEFAULT 'Available',
                            date_insert TEXT
                        );";
                    using (var cmd = new SQLiteCommand(createProducts, conn)) { cmd.ExecuteNonQuery(); }

                    // 3. Orders Table
                    string createOrders = @"
                        CREATE TABLE IF NOT EXISTS orders (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            customerId TEXT,
                            productids TEXT,
                            quantities TEXT,
                            prices TEXT,
                            total REAL,
                            date_order TEXT
                        );";
                    using (var cmd = new SQLiteCommand(createOrders, conn)) { cmd.ExecuteNonQuery(); }

                    // 4. CustomerDelivery Table
                    string createDelivery = @"
                        CREATE TABLE IF NOT EXISTS CustomerDelivery (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            productID TEXT DEFAULT '001',
                            productName TEXT DEFAULT 'Custom Tote Bag',
                            FullName TEXT,
                            PassportNIC TEXT,
                            Email TEXT,
                            Mobile TEXT,
                            HotelName TEXT,
                            RoomNumber TEXT,
                            StreetAddress TEXT,
                            City TEXT,
                            NearestLandmark TEXT,
                            PreferredDeliveryDate TEXT,
                            LeaveAtReception INTEGER DEFAULT 0,
                            ImagePath TEXT,
                            BagSize TEXT
                        );";
                    using (var cmd = new SQLiteCommand(createDelivery, conn)) { cmd.ExecuteNonQuery(); }

                    // Migration: Ensure ImagePath, BagSize, productID, and productName columns exist if table was created previously
                    try
                    {
                        using (var cmd = new SQLiteCommand("ALTER TABLE CustomerDelivery ADD COLUMN ImagePath TEXT;", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    try
                    {
                        using (var cmd = new SQLiteCommand("ALTER TABLE CustomerDelivery ADD COLUMN BagSize TEXT;", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    try
                    {
                        using (var cmd = new SQLiteCommand("ALTER TABLE CustomerDelivery ADD COLUMN productID TEXT DEFAULT '001';", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    try
                    {
                        using (var cmd = new SQLiteCommand("ALTER TABLE CustomerDelivery ADD COLUMN productName TEXT DEFAULT 'Custom Tote Bag';", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    // Seed default categories if empty
                    string countCat = "SELECT COUNT(*) FROM categories";
                    using (var cmd = new SQLiteCommand(countCat, conn))
                    {
                        long count = Convert.ToInt64(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            string seedCat = @"
                                INSERT INTO categories (category, date_insert) VALUES ('Handmade Bags', datetime('now'));
                                INSERT INTO categories (category, date_insert) VALUES ('Traditional Crafts', datetime('now'));
                                INSERT INTO categories (category, date_insert) VALUES ('Textiles', datetime('now'));";
                            using (var seedCmd = new SQLiteCommand(seedCat, conn)) { seedCmd.ExecuteNonQuery(); }
                        }
                    }

                    // Update prices for existing sample products if price is 0
                    string updatePrices = @"
                        UPDATE products SET price = 25.00 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Handwoven Tote%';
                        UPDATE products SET price = 18.50 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Batik Patchwork Canvas%';
                        UPDATE products SET price = 12.00 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Elephant Print Cotton%';
                        UPDATE products SET price = 6.50  WHERE (price IS NULL OR price = 0) AND productname LIKE '%Jute Zip Pouch%';
                        UPDATE products SET price = 14.00 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Dumbara Weave Envelope%';
                        UPDATE products SET price = 24.00 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Batik Cotton Sarong%';
                        UPDATE products SET price = 30.00 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Floral Silk Shawl%';
                        UPDATE products SET price = 16.00 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Elephant Table Runner%';
                        UPDATE products SET price = 19.50 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Cotton Cushion Cover%';
                        UPDATE products SET price = 22.00 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Brass Elephant%';
                        UPDATE products SET price = 9.00  WHERE (price IS NULL OR price = 0) AND productname LIKE '%Coconut Shell Trinket%';
                        UPDATE products SET price = 28.00 WHERE (price IS NULL OR price = 0) AND productname LIKE '%Wooden Peacock Mask%';
                        UPDATE products SET price = 8.50  WHERE (price IS NULL OR price = 0) AND productname LIKE '%Cinnamon Wood Coaster%';
                    ";
                    using (var priceCmd = new SQLiteCommand(updatePrices, conn)) { priceCmd.ExecuteNonQuery(); }

                    // Map products 014-018 to existing images in products_directory
                    string fixImages = @"
                        UPDATE products SET image = 'products_directory\010.jpg' WHERE productid = '014' OR (image LIKE '%014.jpg%');
                        UPDATE products SET image = 'products_directory\011.jpg' WHERE productid = '015' OR (image LIKE '%015.jpg%');
                        UPDATE products SET image = 'products_directory\008.jpg' WHERE productid = '016' OR (image LIKE '%016.jpg%');
                        UPDATE products SET image = 'products_directory\006.jpg' WHERE productid = '017' OR (image LIKE '%017.jpg%');
                        UPDATE products SET image = 'products_directory\001.png' WHERE productid = '018' OR (image LIKE '%018.jpg%');
                    ";
                    using (var imgCmd = new SQLiteCommand(fixImages, conn)) { imgCmd.ExecuteNonQuery(); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing local SQLite database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
