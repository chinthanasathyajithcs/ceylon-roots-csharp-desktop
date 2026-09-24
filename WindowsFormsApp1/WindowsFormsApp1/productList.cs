using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace WindowsFormsApp1
{

    internal class productsList
    {
        // OLD AZURE CONNECTION STRING (Preserved as comment):
        // string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=*****;Connect Timeout=30;Encrypt=True";

        public int ID { set; get; }

        public string productID { set; get; }

        public string productName { set; get; }

        public string category { set; get; }

        public string stock { set; get; }

        public string price { set; get; }

        public string status { set; get; }

        public string image { set; get; }

        public string DateInsert { set; get; }

        public string DateUpdate { set; get; }

        public List<productsList> productListData()
        {
            List<productsList> listData = new List<productsList>();

            using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
            {
                connect.Open();

                string selectData = "SELECT * FROM products";

                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(selectData, connect))
                {
                    System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        productsList pList = new productsList();

                        pList.ID = Convert.ToInt32(reader["id"]);
                        pList.productID = reader["productid"] != DBNull.Value ? reader["productid"].ToString() : "";
                        pList.productName = reader["productname"] != DBNull.Value ? reader["productname"].ToString() : "";
                        pList.category = reader["category"] != DBNull.Value ? reader["category"].ToString() : "";
                        pList.stock = reader["stock"] != DBNull.Value ? reader["stock"].ToString() : "";
                        pList.price = reader["price"] != DBNull.Value ? reader["price"].ToString() : "";
                        pList.status = reader["status"] != DBNull.Value ? reader["status"].ToString() : "";
                        pList.image = reader["image"] != DBNull.Value ? reader["image"].ToString() : "";
                        pList.DateInsert = reader["date_insert"] != DBNull.Value ? reader["date_insert"].ToString() : "";
                        pList.DateUpdate = "";

                        listData.Add(pList);
                    }
                }
                return listData;
            }
        }
    }
   

   
}
