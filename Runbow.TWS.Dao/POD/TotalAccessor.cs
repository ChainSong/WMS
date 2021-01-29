using Runbow.TWS.Entity;
using Runbow.TWS.Entity.POD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
namespace Runbow.TWS.Dao.POD
{
    public class TotalAccessor : BaseAccessor
    {
        public DataTable GetMessageHistoryInfo(string SqlWhere, int? PageIndex, int PageSize, out int RowCount)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor.SMSSqlConnection))
            {
               
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_Total_MessageHistoryQuery", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SqlWhere", SqlWhere);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;

                cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;

                cmd.Parameters.AddWithValue("@PageSize", PageSize);
                cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                cmd.Parameters.AddWithValue("@RowCount", 0);
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                cmd.Parameters[3].SqlDbType = SqlDbType.Int;
               
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                RowCount =Convert.ToInt32(cmd.Parameters[3].Value);
                return dtable;
            }


        }




        public DataTable GetMessageHistoryInfoReport(string SqlWhere)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor.SMSSqlConnection))
            {
               
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_Total_MessageHistoryQuery_ExprotReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SqlWhere", SqlWhere);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;

                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
               
                return dtable;
            }


        }

        public IEnumerable<Pod> GetPODTotalInfoReport(TotalPODEntity SearchCondition, int PageIndex,
      int PageSize, out  int RowCount)
        {
            string sqlWhere = this.GenPodTotalSql(SearchCondition);
            using (SqlConnection conn = new SqlConnection(BaseAccessor.SMSSqlConnection))
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("proc_Total_QueryPodHasTracks", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Where", sqlWhere);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;

                cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;

                cmd.Parameters.AddWithValue("@PageSize", PageSize);
                cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                cmd.Parameters.AddWithValue("@RowCount", 0);
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                conn.Open();
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dt);
                RowCount = Convert.ToInt32(cmd.Parameters[3].Value);


                return dt.ConvertToEntityCollection<Pod>();
            }
        }
        private string GenPodTotalSql(TotalPODEntity SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("and CustomerID=14");
            if (SearchCondition.StatUpLoadTime != null)
            {
                sb.Append("and ActualDeliveryDate>='" + SearchCondition.StatUpLoadTime + "'");
            }
            if (SearchCondition.EndUpLoadTime != null)
            {
                sb.Append("and ActualDeliveryDate<='" + SearchCondition.EndUpLoadTime + "'");
            }
            if (SearchCondition.StateID.HasValue)
            {
                if (SearchCondition.StateID == 0)
                {
                    sb.Append(" and PodTrack.CreateTime is null");
                }
                if (SearchCondition.StateID == 1)
                {
                    sb.Append(" and PodTrack.CreateTime is not null");
                }
            }
            return sb.ToString();
        }
    }
}
