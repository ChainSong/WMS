using Runbow.TWS.Entity.WMS.WXApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
namespace Runbow.TWS.Dao
{
    public class WXAppAccessor : BaseAccessor
    {
        public DataTable GetDataTable()
        {
            string sql = string.Format(@"SELECT * FROM dbo.WMS_PreOrder WHERE CustomerID=105");
            return base.ExecuteDataTableBySqlString(sql);
        }

        public string GetSoldTrades(List<SoldTrade> soldtrade, List<SoldTradeOrder> soldtradeDetail)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddSoldTradeANDDetail", conn);//默认
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Po", soldtrade.Select(p => new SoldTradesToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Pod", soldtradeDetail.Select(p => new SoldTradesDetailToDb(p)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Creator", "WX");
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;//超时时间
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                }
                catch (Exception e)
                {
                    message = "application catch:" + e.Message;
                }
            }
            return message;
        }

        public DataSet AddSoldProductOrDetail(List<Product> ps, List<ProductSku> pds)
        {
            string message = string.Empty;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WX_AddSoldProductOrDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ps", ps.Select(p => new WXSoldProductToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@PDs", pds.Select(pd => new WXSoldProductDetailToDb(pd)));
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Size = 50;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                conn.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();

                return ds;
            }
        }

        public string AddSoldRefundDetail(List<RefundDetail> rds)
        {
            string message = string.Empty;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WX_AddSoldRefundDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RD", rds.Select(r => new WXSoldRefundDetailToDb(r)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Size = 50;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
            }

            return message;
        }
    }
}
