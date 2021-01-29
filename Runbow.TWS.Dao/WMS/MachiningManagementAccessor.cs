using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.ASNs;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Machining;


namespace Runbow.TWS.Dao.WMS
{
    public class MachiningManagementAccessor : BaseAccessor
    {
        /// <summary>
        /// 查询显示 分页查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<WMS_MachiningHeaderAndDetail> GetMachiningByCondition(MachiningSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {

            string sqlWhere = this.GenGetWhere(SearchCondition);

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetMachiningByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<WMS_MachiningHeaderAndDetail>();
        }

        public IEnumerable<WMS_MachiningHeaderAndDetail> GetMachiningByID(long ID)
        {

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetMachiningByID", dbParams);
            return ds.Tables[0].ConvertToEntityCollection<WMS_MachiningHeaderAndDetail>();
        }

        public string MachiningDelete(long ID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_MachiningDeleteByID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

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
                    return message + "(" + ex.Message + ")";
                }
            }
        }

        public int CheckMachiningNumber(string MachiningNumber)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@MachiningNumber", DbType.String, MachiningNumber, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetMachiningByMachiningNumber", dbParams);
            return dt.Rows.Count.ObjectToInt32();
        }

        public IEnumerable<Inventorys> GetInventoryBySearchCondition(MachiningSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetInventory(SearchCondition);
            
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetAreaInventoryBySearchCondition", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<Inventorys>();
        }

        public IEnumerable<Inventorys> GetLittleInventoryBySearchCondition(MachiningSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetInventory(SearchCondition);

            //int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
                //new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                //new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                //new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetLittleInventoryBySearchCondition", dbParams);
            //RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<Inventorys>();
        }

        /// <summary>
        /// 条件拼接
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private string GenGetWhere(MachiningSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(SearchCondition.MachiningNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.MachiningNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.MachiningNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.MachiningNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.MachiningNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and MachiningNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and MachiningNumber  like '%" + SearchCondition.MachiningNumber.Trim() + "%' ");
                }
                //sb.Append(" AND a.ASNNumber ='").Append(SearchCondition.ASNNumber).Append("' ");
            }

            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND CustomerID='").Append(SearchCondition.CustomerID).Append("' ");
            }

            if (SearchCondition.ExpectDate != null)
            {
                sb.Append(" AND ExpectDate >='").Append(SearchCondition.ExpectDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndExpectDate != null)
            {
                sb.Append(" AND a.ExpectDate <='").Append(SearchCondition.ExpectDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.MachiningType))
            {
                sb.Append(" AND MachiningType ='").Append(SearchCondition.MachiningType).Append("' ");
            }
            return sb.ToString();
        }

        public string GenGetInventory(MachiningSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND CustomerID = ").Append(SearchCondition.CustomerID);
            }
            
            if (!string.IsNullOrEmpty(SearchCondition.Location))
            {               
                 sb.Append(" AND Location LIKE '%" + SearchCondition.Location.Trim() + "%' ");                
            }            
            return sb.ToString();
        }

        public string SaveMachining(IEnumerable<WMS_MachiningHeaderAndDetail> MachiningList, string Creator)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {                 
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_SaveMachining", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Machining", MachiningList.Select(p => new WMSMachiningToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size =50;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                  
                    return message;
                }
                catch (Exception e)
                {

                    throw;
                }

            }
        }

        public string AddMachining(IEnumerable<WMS_MachiningHeaderAndDetail> MachiningList, string Creator, string IDS,string IDDS)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddMachining", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Machining", MachiningList.Select(p => new WMSMachiningToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 50;
                    cmd.Parameters.AddWithValue("@IDS", IDS);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters.AddWithValue("@IDDS", IDDS);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Size = 100;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();

                    return message;
                }
                catch (Exception e)
                {

                    throw;
                }

            }
        }

        public string BucketOutMachining(IEnumerable<WMS_MachiningHeaderAndDetail> MachiningList, string Creator, string IDS)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_BucketOutMachining", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Machining", MachiningList.Select(p => new WMSMachiningToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 50;
                    cmd.Parameters.AddWithValue("@IDS", IDS);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();

                    return message;
                }
                catch (Exception e)
                {

                    throw;
                }

            }
        }

        public string BucketInMachining(IEnumerable<WMS_MachiningHeaderAndDetail> MachiningList, string Creator, string IDS)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_BucketInMachining", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Machining", MachiningList.Select(p => new WMSMachiningToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 50;
                    cmd.Parameters.AddWithValue("@IDS", IDS);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();

                    return message;
                }
                catch (Exception e)
                {

                    throw;
                }

            }
        }

        public IEnumerable<WMS_MachiningHeaderAndDetail> GetPrintMachining(string id)
        {
            string Where = this.GenGetPrintOrder(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetPrintMachining", dbParams);

            return dt.ConvertToEntityCollection<WMS_MachiningHeaderAndDetail>();
        }

        public string GenGetPrintOrder(string id)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append("AND h.ID IN (").Append(id).Append(")");
            }
            return sb.ToString();
        }
    }
}
