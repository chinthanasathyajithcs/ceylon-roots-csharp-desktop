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
        // OLD AZURE CONNECTION STRING (Preserved as comment):
        // string connection = @"Data Source=csharpproject2025.database.windows.net;Initial Catalog=csharpproject2025;User ID=csharpproject2025;Password=*****;Connect Timeout=30;Encrypt=True";

        public int ID { set; get; }
        public int categoryID { set; get; }
        public string category { set; get; }
        public string status { set; get; }
        public string DateInsert { set; get; }

        public List<categoriesList> categoriesListData()
        {
            List<categoriesList> listData = new List<categoriesList>();

            using (System.Data.SQLite.SQLiteConnection connect = DbHelper.GetConnection())
            {
                connect.Open();

                string selectData = "SELECT * FROM categories";

                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(selectData, connect))
                {
                    System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        categoriesList cData = new categoriesList();
                        cData.ID = Convert.ToInt32(reader["id"]);
                        cData.categoryID = cData.ID;
                        cData.category = reader["category"] != DBNull.Value ? reader["category"].ToString() : "";
                        
                        int statusIdx = -1;
                        try { statusIdx = reader.GetOrdinal("status"); } catch { statusIdx = -1; }
                        cData.status = (statusIdx >= 0 && reader[statusIdx] != DBNull.Value) ? reader[statusIdx].ToString() : "Available";

                        int dateIdx = -1;
                        try { dateIdx = reader.GetOrdinal("date_insert"); } catch { dateIdx = -1; }
                        cData.DateInsert = (dateIdx >= 0 && reader[dateIdx] != DBNull.Value) ? reader[dateIdx].ToString() : "";

                        listData.Add(cData);
                    }
                }
            }
            return listData;
        }
    }
}
