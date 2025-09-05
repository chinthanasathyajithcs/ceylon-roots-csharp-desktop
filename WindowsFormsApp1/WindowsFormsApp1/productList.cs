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
        string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=CSpassword2025;Connect Timeout=30;Encrypt=True";

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

            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();

                string selectData = "SELECT * FROM products";

                using (SqlCommand cmd = new SqlCommand(selectData, connect))
                {
                    SqlDataReader reader = cmd.ExecuteReader();



                    while (reader.Read())
                    {
                        productsList pList = new productsList();

                        pList.ID = (int)reader["ID"];
                        pList.productID = reader["productid"].ToString();
                        pList.productName = reader["productname"].ToString();
                        pList.category = reader["category"].ToString();
                        pList.stock = reader["stock"].ToString();
                        pList.price = reader["price"].ToString();
                        pList.status = reader["status"].ToString();
                        pList.image = reader["image"].ToString();
                        pList.DateInsert = ((DateTime)reader["date_insert"]).ToString("MM-dd-yyyy");
                        pList.DateUpdate = reader["date_update"] != DBNull.Value 
    ? ((DateTime)reader["date_update"]).ToString("MM-dd-yyyy") 
    : "";

                        listData.Add(pList);

                    }


                }
                return listData;

            }



        }
    }
   

   
}
