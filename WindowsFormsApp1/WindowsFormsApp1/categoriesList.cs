using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class categoriesList
    {
        string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=CSpassword2025;Connect Timeout=30;Encrypt=True";

        public int ID { set; get; }
        public string category { set; get; }
        public string status { set; get; }
        public string DateInsert { set; get; }

        public List<categoriesList> categoriesListData()
        {
            List<categoriesList> listData = new List<categoriesList>();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();

                string selectData = "SELECT * FROM categories";

                using (SqlCommand cmd = new SqlCommand(selectData, connect))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        categoriesList cData = new categoriesList();
                        cData.ID = (int)reader["ID"];
                        cData.category = reader["category"].ToString();
                        cData.status = reader["status"].ToString();
                        cData.DateInsert = ((DateTime)reader["date_insert"]).ToString("MM/dd/yyyy");

                        listData.Add(cData);
                    }
                }
            }
            return listData;
        }
    }
}
