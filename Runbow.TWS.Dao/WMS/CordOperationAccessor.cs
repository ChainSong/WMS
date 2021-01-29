using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.Entity.DataBaseEntity;
using System.Data;
using System.Data.SqlClient;

namespace Runbow.TWS.Dao.WMS
{
    public class CordOperationAccessor : BaseAccessor
    {
        public string AddCordOperation(IEnumerable<WMS_Cord_Operation> list)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WMS_CordOperation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CordOperation", list.Select(i => new CordOperationToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Size = 50;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                message = cmd.Parameters["@message"].Value.ToString();
                conn.Close();

                return message;
            }
        }
    }
}
