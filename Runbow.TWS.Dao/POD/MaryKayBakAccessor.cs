using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Dao.POD
{
    public  class MaryKayBakAccessor : BaseAccessor
    {
        /// 执行非查询存储过程和SQL语句
        /// 增、删、改
        /// </summary>
        /// <param name="strSQL">要执行的SQL语句</param>
        /// <param name="paras">参数列表，没有参数填入null</param>
        /// <param name="cmdType">Command类型</param>
        /// <returns>返回影响行数</returns>
        public  int ExcuteSQL(string strSQL, SqlParameter[] paras, CommandType cmdType)
        {
            int i = 0;
            using (SqlConnection conn = new SqlConnection(BaseAccessor.DMSSqlConnection))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    cmd.CommandType = cmdType;
                    if (paras != null)
                    {
                        cmd.Parameters.AddRange(paras);
                    }
                    cmd.Transaction = transaction;
                    i = cmd.ExecuteNonQuery();
                    transaction.Commit();
                    conn.Close();

                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return i;
        }

        /// <summary>
        /// 执行查询，返回DataSet对象
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="pas">参数数组</param>
        /// <param name="cmdtype">Command类型</param>
        /// <returns>DataSet对象</returns>
        public  DataSet GetDataSet(string strSQL, SqlParameter[] pas, CommandType cmdtype)
        {
            DataSet dt = new DataSet(); ;
            using (SqlConnection conn = new SqlConnection(BaseAccessor.DMSSqlConnection))
            {
                SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
                da.SelectCommand.CommandType = cmdtype;
                if (pas != null)
                {
                    da.SelectCommand.Parameters.AddRange(pas);
                }
                da.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 删除物流跟踪信息
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteTrackInfoByID(string Ids)
        {
            string strSql = " DELETE FROM tbl_PODTrack WHERE colID IN(" + Ids + ") ";
            return ExcuteSQL(strSql, null, CommandType.Text) > 0 ? true : false;
        }

        /// <summary>
        /// 根据订单号更新运单号
        /// </summary>
        /// <param name="strSql">拼接的sql</param>
        public bool UpdateTrackNum(string strSql)
        {
            return ExcuteSQL(strSql, null, CommandType.Text) > 0 ? true : false;
        }

         /// <summary>
        /// 导出物流跟踪信息
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <returns></returns>
        public DataTable MaryKayExportTrack(string SqlWhere)
        {
            DataTable mydt = new DataTable();
            using (SqlConnection conn = new SqlConnection(BaseAccessor.DMSSqlConnection))
            {
                try
                {
                    SqlCommand mycommd = new SqlCommand("Proc_MaryKay_TrackExportM", conn); //Proc_MaryKay_TrackExport 查询每条订单号对应的最新的物流跟踪信息
                    mycommd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParme = mycommd.Parameters.Add("@SqlWhere", SqlDbType.NVarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = SqlWhere.Trim();


                    conn.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = mycommd;
                        sda.Fill(mydt);
                    }
                    conn.Close();

                    return mydt;
                }
                catch
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// 查询物流跟踪信息
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public DataTable GetMaryKayGetTrack(string SqlWhere, int? PageIndex, int PageSize, out int RowCount)
        {
            DataTable mydt = new DataTable();
            using (SqlConnection conn = new SqlConnection(BaseAccessor.DMSSqlConnection))
            {
                try
                {
                    SqlCommand mycommd = new SqlCommand("Proc_MaryKay_TrackInfo", conn);
                    mycommd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParme = mycommd.Parameters.Add("@SqlWhere", SqlDbType.NVarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = SqlWhere.Trim();

                    sqlParme = mycommd.Parameters.Add("@PageIndex", SqlDbType.Int);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = PageIndex;

                    sqlParme = mycommd.Parameters.Add("@PageSize", SqlDbType.Int);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = PageSize;

                    sqlParme = mycommd.Parameters.Add("RowCount", SqlDbType.Int);
                    sqlParme.Direction = ParameterDirection.Output;
                    sqlParme.Value = PageSize;

                    conn.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = mycommd;
                        sda.Fill(mydt);
                    }
                    conn.Close();


                    RowCount = (int)mycommd.Parameters[3].Value;
                    return mydt;
                }
                catch
                {
                    RowCount = 0;
                    return null;
                }
               
            }
        }
    }
}
