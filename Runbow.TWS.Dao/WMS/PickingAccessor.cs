using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;

namespace Runbow.TWS.Dao.WMS
{
    public class PickingAccessor : BaseAccessor
    {
        public void CreatePickingAndDetail(string Creator, string IDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_CreatePickingAndDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", IDs.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@Creator", Creator);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.CommandTimeout = 300;
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                conn.Close();
            }
        }
    }
}
