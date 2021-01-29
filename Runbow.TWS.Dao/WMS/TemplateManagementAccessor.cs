using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.Template;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts;


namespace Runbow.TWS.Dao.WMS
{
    public class TemplateManagementAccessor : BaseAccessor
    {
        /// <summary>
        /// 查询显示 分页查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<TableColumn> GetTemplateByCondition(SearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {

            string sqlWhere = this.GenGetWhere(SearchCondition);

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetTemplateByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<TableColumn>();
        }

        public IEnumerable<TableColumn> GetTemplateDetailByCondition(SearchCondition SearchCondition)
        {
            //string sqlWhere = this.GenGetWhereTemplateDetail(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ProjectID", DbType.Int32, SearchCondition.ProjectID, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.Int32, SearchCondition.CustomerID, ParameterDirection.Input),
                new DbParam("@TableName", DbType.String, SearchCondition.TableName.Trim(), ParameterDirection.Input)
                //new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetTemplateDetailByCondition", dbParams);
            return dt.ConvertToEntityCollection<TableColumn>();
        }

        public string EditTemplateDetail(GetTemplateByConditionRequest rece)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Receipt> result = new List<Receipt>();
                    IList<ReceiptDetail> receiptDetail = new List<ReceiptDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_EditTemplateDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Templates", rece.tableColumns.Select(m => new WMSTemplateToDb(m)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ProjectID", rece.SearchCondition.ProjectID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@CustomerID", rece.SearchCondition.CustomerID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@TableName", rece.SearchCondition.TableName);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Size = 100;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 100;
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

        private string GenGetWhere(SearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" AND c.ProjectID is NULL  AND c.CustomerID is NULL").Append(" ");
            return sb.ToString();
        }

        private string GenGetWhereTemplateDetail(SearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (SearchCondition.ProjectID != 0 && SearchCondition.ProjectName!="==请选择==")
            {
                sb.Append(" AND c.ProjectID=").Append(SearchCondition.ProjectID).Append(" ");
            }
            if (SearchCondition.ProjectID != 0 && SearchCondition.ProjectName == "==请选择==")
            {
                sb.Append(" AND c.ProjectID is　NULL ").Append(" ");
            }
            if (SearchCondition.ProjectID == 0)
            {
                sb.Append(" AND c.ProjectID is null ");
            }
            if (SearchCondition.CustomerID != 0 && SearchCondition.CustomerName != "==请选择==")
            {
                sb.Append(" AND c.CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (SearchCondition.CustomerID != 0 && SearchCondition.CustomerName == "==请选择==")
            {
                sb.Append(" AND c.CustomerID is　NULL ").Append(" ");
            }
            if (SearchCondition.CustomerID == 0)
            {
                sb.Append(" AND c.CustomerID is null ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.TableName))
            {
                sb.Append(" AND c.TableName='").Append(SearchCondition.TableName).Append("' ");
            }
            return sb.ToString();
        }
    }
}
