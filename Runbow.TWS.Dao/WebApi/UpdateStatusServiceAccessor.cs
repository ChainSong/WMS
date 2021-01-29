using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WebApi;

namespace Runbow.TWS.Dao.WebApi
{
    public class UpdateStatusServiceAccessor
    {
        private string connStr = ConfigurationManager.ConnectionStrings["WMS"].ConnectionString.ToString();
        public string GetUpdateStatus(string OrderKey)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
               
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_OrderHeader_Update_Status", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderKey", OrderKey);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 25;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[1].Size = 10;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                   
                }
                catch
                {  }
            }
            return message;
        }
    }
}