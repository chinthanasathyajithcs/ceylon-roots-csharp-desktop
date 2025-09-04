using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace WindowsFormsApp1
{

    internal class productList
    {
        string connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\OneDrive\Desktop\project\WindowsFormsApp1\WindowsFormsApp1\Database1.mdf;Integrated Security=True";

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
                productsList pData = new productsList();
                pData.ID = (int)reader["id"];
                pData.productID = reader["productid"].ToString();
                pData.productName = reader["productname"].ToString();
                pData.category = reader["category"].ToString();
                pData.stock = reader["stock"].ToString();
                pData.price = reader["price"].ToString();
                pData.status = reader["status"].ToString();
                pData.image = reader["image"].ToString();

                // Date fields (check for DBNull to avoid exceptions)
                pData.DateInsert = reader["date_insert"] != DBNull.Value
                    ? ((DateTime)reader["date_insert"]).ToString("MM-dd-yyyy")
                    : null;
                pData.DateUpdate = reader["date_update"] != DBNull.Value
                    ? ((DateTime)reader["date_update"]).ToString("MM-dd-yyyy")
                    : null;

                listData.Add(pData);
            }
        }
    }

    return listData;
}


    }

    public class productsList
    {
    }
}
