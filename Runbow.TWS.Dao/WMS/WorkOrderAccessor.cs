using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.WorkOrder;

namespace Runbow.TWS.Dao.WMS
{
    public class WorkOrderAccessor : BaseAccessor
    {
        /// <summary>
        /// 新增订单及明细
        /// </summary>
        public string AddOrUpdateWorkOrder(IEnumerable<WorkOrder> list)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_AddOrUpdateWorkOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@data", list.Select(c => new WorkOrderToDb(c)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;

                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 500;
                    conn.Open();
                    cmd.CommandTimeout = 300;

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
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
