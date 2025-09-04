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


    }

    public class productsList
    {
    }
}
