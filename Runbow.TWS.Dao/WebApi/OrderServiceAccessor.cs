using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WebApi;
using Runbow.TWS.MessageContracts.WebApi;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;

namespace Runbow.TWS.Dao.WebApi
{
    public class OrderServiceAccessor
    {
        private string connStr = ConfigurationManager.ConnectionStrings["TWS"].ConnectionString.ToString();
        public IEnumerable<APIAndBackSetting> GetAPISetting(string UserName)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetAPISetting", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar; 
                    conn.Open();
                    cmd.CommandTimeout = 300;
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    IEnumerable<APIAndBackSetting> list = ds.Tables[0].ConvertToEntityCollection<APIAndBackSetting>();
                    conn.Close();
                    return list;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

    }
}