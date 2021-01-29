using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Dao
{
    public class ReceiptManagementAccessor : BaseAccessor
    {
        public GetReceiptDetailByConditionResponse GetReceiptByCondition(ReceiptSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string sqlWhere = this.GenGetReceiptWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetReceiptByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            response.ReceiptDetailCollection2 = ds.Tables[2].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public GetReceiptDetailByConditionResponse GetShelvesByIDs(string IDs)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            //string sqlWhere = this.GenGetReceiptWhere(SearchCondition);
            //int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String, IDs, ParameterDirection.Input)
            };
            DataSet ds = new DataSet();
            try
            {
                ds = this.ExecuteDataSet("Proc_WMS_GetShelvesByIDs", dbParams);
            }
            catch (Exception e)
            {

                throw e;
            }


            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            //response.ReceiptDetailCollection2 = ds.Tables[2].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public GetReceiptDetailByConditionResponse GetShelvesByIDs2(string IDs, string ProdName)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            //string sqlWhere = this.GenGetReceiptWhere(SearchCondition);
            //int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String, IDs, ParameterDirection.Input)
            };
            DataSet ds = new DataSet();
            try
            {
                ds = this.ExecuteDataSet(ProdName, dbParams);
            }
            catch (Exception e)
            {

                throw e;
            }


            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            //response.ReceiptDetailCollection2 = ds.Tables[2].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public GetReceiptDetailByConditionResponse GetReceiptByIDs(string IDs)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            //string sqlWhere = this.GenGetReceiptWhere(SearchCondition);
            //int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String, IDs, ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetReceiptByIDs", dbParams);

            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            //response.ReceiptDetailCollection2 = ds.Tables[2].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public GetReceiptDetailByConditionResponse GetImportReceiptByCondition(ReceiptSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string sqlWhere = this.GenGetReceiptWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetImportReceiptByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            response.ReceiptDetailCollection2 = ds.Tables[2].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public GetReceiptDetailByConditionResponse GetImportReceiptByCondition2(ReceiptSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount, string prodName)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string sqlWhere = this.GenGetReceiptWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet(prodName, dbParams);
            rowCount = (int)dbParams[3].Value;
            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            response.ReceiptDetailCollection2 = ds.Tables[2].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        //入库报表
        public GetReceiptDetailByConditionResponse GetReceiptForRPTByCondition(ReceiptSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string sqlWhere = this.GenGetReceiptRPTWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output),
                new DbParam("@CustomerID", DbType.Int32, SearchCondition.CustomerID, ParameterDirection.Input),
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetReceiptForRPT", dbParams);
            rowCount = (int)dbParams[3].Value;
            // ReportReceiptReport
            response.ReceiptDetailCollection3 = ds.Tables[0].ConvertToEntityCollection<ReportReceiptReport>();
            return response;
        }
        /// <summary>
        /// 来货预检差异扫描
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public GetReceiptDetailByConditionResponse GetAsnScanDiffForRPTByCondition(ReceiptSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string sqlWhere = this.GenGetAsnScanDiffRPTWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetAsnScanDiffForRPT", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.ReceiptDetailCollection3 = ds.Tables[0].ConvertToEntityCollection<ReportReceiptReport>();
            return response;
        }

        public GetReceiptDetailByConditionResponse ExportReceiptForRPTByCondition(ReceiptSearchCondition SearchCondition)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string sqlWhere = this.GenGetReceiptRPTWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.Int32, SearchCondition.CustomerID, ParameterDirection.Input),
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_ExportGetReceiptBySearchCondition_Report", dbParams);
            //  rowCount = (int)dbParams[3].Value;
            // ReportReceiptReport
            response.ReceiptDetailCollection3 = ds.Tables[0].ConvertToEntityCollection<ReportReceiptReport>();
            return response;
        }
        public GetReceiptDetailByConditionResponse ExportAsnScanDiffForRPTByCondition(ReceiptSearchCondition SearchCondition)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string sqlWhere = this.GenGetAsnScanDiffRPTWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_ExportGetAsnScanDiffBySearchCondition_Report", dbParams);
            response.ReceiptDetailCollection3 = ds.Tables[0].ConvertToEntityCollection<ReportReceiptReport>();
            return response;
        }
        public GetReceiptDetailByConditionResponse GetReceiptDetailByCondition(ReceiptDetailSearchCondition SearchCondition, string ID)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string RID = "and a.RID='" + ID + "'";
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.String, RID, ParameterDirection.Input)

            };

            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetReceiptDetailByCondition", dbParams);
            DataTable dt2 = new DataTable();

            if (dt.Rows.Count > 0)
            {
                dt2 = dt.Clone();
                DataRow dr = dt2.NewRow();
                dr.ItemArray = dt.Rows[0].ItemArray;
                dt2.Rows.Add(dr);
                response.Receipt = dt2.ConvertToEntity<Receipt>();
                response.ReceiptDetailCollection = dt.ConvertToEntityCollection<ReceiptDetail>();
            }
            return response;

        }

        public IEnumerable<ASN> ASNQuery(ASNSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenGetASNWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetASNByCondition_Receipt", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ASN>();
        }

        public GetAsnOrReceiptOrDetailByConditionResponse ASNDetailQuery(GetAsnOrReceiptOrDetailByConditionRequest SearchCondition, long ID)
        {
            GetAsnOrReceiptOrDetailByConditionResponse response = new GetAsnOrReceiptOrDetailByConditionResponse();
            string prc = "";
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)

            };
            prc = "Proc_WMS_GetASNOrASNDetailByCondition_Receipt";
            DataSet ds = this.ExecuteDataSet(prc, dbParams);
            response.dtAsn = ds.Tables[0];
            response.dtAsnDetail = ds.Tables[1];
            //response.Receipt = ds.Tables[0].ConvertToEntity<Receipt>();
            //response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public GetAsnOrReceiptOrDetailByConditionResponse ReceiptDetailQuery(GetAsnOrReceiptOrDetailByConditionRequest SearchCondition, long ID)
        {
            GetAsnOrReceiptOrDetailByConditionResponse response = new GetAsnOrReceiptOrDetailByConditionResponse();
            string prc = "";
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
            };
            prc = "Proc_WMS_GetReceiptOrReceiptDetailByCondition_Receipt";
            DataSet ds = this.ExecuteDataSet(prc, dbParams);
            response.Receipt = ds.Tables[0].ConvertToEntity<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public GetAsnOrReceiptOrDetailByConditionResponse ReceiptDetailAndBarCodeQuery(GetAsnOrReceiptOrDetailByConditionRequest SearchCondition, long ID)
        {
            GetAsnOrReceiptOrDetailByConditionResponse response = new GetAsnOrReceiptOrDetailByConditionResponse();
            string prc = "";
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
            };
            prc = "Proc_WMS_GetReceiptAndReceiptDetailAndBarCodeByID";
            DataSet ds = this.ExecuteDataSet(prc, dbParams);
            response.Receipt = ds.Tables[0].ConvertToEntity<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public string ReceiptDelete(string ReceiptNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_DeleteReceiptByID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReceiptNumber", ReceiptNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 50;
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
                    //cmd.ExecuteNonQuery();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }

        }
        public GetReceiptDetailByConditionResponse GetReceiptDetailByIDS(string IDS)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetReceiptDetailByIDS", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", IDS.Split(',').Select(p => new IdsForInt32(Convert.ToInt32(p))));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.CommandTimeout = 300;
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    response.ReceiptDetailCollection = ds.Tables[0].ConvertToEntityCollection<ReceiptDetail>();
                    conn.Close();
                }
                catch (Exception ex)
                {

                }

            }

            return response;
        }
        /// <summary>
        /// 下发上架任务
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public string ReceiptTask(string IDS,string Name)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ReceiptTask", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", IDS.Split(',').Select(p => new IdsForInt32(Convert.ToInt32(p))));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
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
        private string GenGetReceiptWhere(ReceiptSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.ReceiptNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ReceiptNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ReceiptNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ReceiptNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ReceiptNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and ReceiptNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and ReceiptNumber  like '%" + SearchCondition.ReceiptNumber.Trim() + "%' ");
                }
                //sb.Append(" AND a.ReceiptNumber ='").Append(SearchCondition.ReceiptNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternReceiptNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ExternReceiptNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ExternReceiptNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ExternReceiptNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ExternReceiptNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and ExternReceiptNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and ExternReceiptNumber  like '%" + SearchCondition.ExternReceiptNumber.Trim() + "%' ");
                }
                //sb.Append(" AND a.ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ASNNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ASNNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ASNNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ASNNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ASNNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and ASNNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and ASNNumber  like '%" + SearchCondition.ASNNumber.Trim() + "%' ");
                }
                //sb.Append(" AND a.ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND a.CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (SearchCondition.CustomerID == 0 && SearchCondition.CustomerIDs != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                numbers = SearchCondition.CustomerIDs.Split(',').Select(s => { return s.Trim(); });
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                sb.Append(" and a.CustomerID in ( ");
                foreach (string s in numbers)
                {
                    sb.Append("'").Append(s).Append("',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ) ");
            }
            if (SearchCondition.WarehouseID != 0)
            {
                sb.Append(" AND a.WarehouseID=").Append(SearchCondition.WarehouseID).Append(" ");
            }
            else
            {
                sb.Append(" AND a.WarehouseName in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.WarehouseName.Trim()).Append(")) ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ReceiptType))
            {
                sb.Append(" AND a.ReceiptType='").Append(SearchCondition.ReceiptType).Append("' ");
            }
            if (SearchCondition.Status != 0)
            {
                sb.Append(" AND a.Status='").Append(SearchCondition.Status).Append("' ");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND a.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:ss.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND a.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:59.99")).Append("' ");
            }
            if (SearchCondition.StartReceiptDate != null)
            {
                sb.Append(" AND a.ReceiptDate >='").Append(SearchCondition.StartReceiptDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndReceiptDate != null)
            {
                sb.Append(" AND a.ReceiptDate <='").Append(SearchCondition.EndReceiptDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartCompleteDate != null)
            {
                sb.Append(" AND a.CompleteDate >='").Append(SearchCondition.StartCompleteDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCompleteDate != null)
            {
                sb.Append(" AND a.CompleteDate <='").Append(SearchCondition.EndCompleteDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str1))
            {
                sb.Append(" and str1 like '%" + SearchCondition.str1 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str2))
            {
                sb.Append(" and str2 like '%" + SearchCondition.str2 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {
                sb.Append(" and str3 like '%" + SearchCondition.str3 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str4))
            {
                sb.Append(" and str4 like '%" + SearchCondition.str4 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str5))
            {
                sb.Append(" and str5 like '%" + SearchCondition.str5 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str6))
            {
                sb.Append(" and str6 like '%" + SearchCondition.str6 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str7))
            {
                sb.Append(" and str7 like '%" + SearchCondition.str7 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str8))
            {
                sb.Append(" and str8 like '%" + SearchCondition.str8 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str9))
            {
                sb.Append(" and str9 like '%" + SearchCondition.str9 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str10))
            {
                sb.Append(" and str10 like '%" + SearchCondition.str10 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str11))
            {
                sb.Append(" and str11 like '%" + SearchCondition.str11 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str12))
            {
                sb.Append(" and str12 like '%" + SearchCondition.str12 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str13))
            {
                sb.Append(" and str13 like '%" + SearchCondition.str13 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str14))
            {
                sb.Append(" and str14 like '%" + SearchCondition.str14 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str15))
            {
                sb.Append(" and str15 like '%" + SearchCondition.str15 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str16))
            {
                sb.Append(" and str16 like '%" + SearchCondition.str16 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str17))
            {
                sb.Append(" and str17 like '%" + SearchCondition.str17 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str18))
            {
                sb.Append(" and str18 like '%" + SearchCondition.str18 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str19))
            {
                sb.Append(" and str19 like '%" + SearchCondition.str19 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str20))
            {
                sb.Append(" and str20 like '%" + SearchCondition.str20 + "%' ");
            }
            if (SearchCondition.StartDateTime1 != null)
            {
                sb.Append(" AND a.DateTime1 >='").Append(SearchCondition.StartDateTime1.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime1 != null)
            {
                sb.Append(" AND a.DateTime1 <='").Append(SearchCondition.EndDateTime1.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartDateTime2 != null)
            {
                sb.Append(" AND a.DateTime2 >='").Append(SearchCondition.StartDateTime2.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime2 != null)
            {
                sb.Append(" AND a.DateTime2 <='").Append(SearchCondition.EndDateTime2.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartDateTime3 != null)
            {
                sb.Append(" AND a.DateTime3 >='").Append(SearchCondition.StartDateTime3.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime3 != null)
            {
                sb.Append(" AND a.DateTime3 <='").Append(SearchCondition.EndDateTime3.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartDateTime4 != null)
            {
                sb.Append(" AND a.DateTime4 >='").Append(SearchCondition.StartDateTime4.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime4 != null)
            {
                sb.Append(" AND a.DateTime4 <='").Append(SearchCondition.EndDateTime4.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartDateTime5 != null)
            {
                sb.Append(" AND a.DateTime5 >='").Append(SearchCondition.StartDateTime5.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime5 != null)
            {
                sb.Append(" AND a.DateTime5 <='").Append(SearchCondition.EndDateTime5.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.Int1 != null)
            {
                sb.Append(" AND a.Int1=").Append(SearchCondition.Int1).Append(" ");
            }
            if (SearchCondition.Int2 != null)
            {
                sb.Append(" AND a.Int2=").Append(SearchCondition.Int2).Append(" ");
            }
            if (SearchCondition.Int3 != null)
            {
                sb.Append(" AND isnull(a.Int3,'0')=").Append(SearchCondition.Int3).Append(" ");
                //sb.Append(" AND a.Int3=").Append(SearchCondition.Int3).Append(" ");
            }
            if (SearchCondition.Int4 != null)
            {
                sb.Append(" AND a.Int4=").Append(SearchCondition.Int4).Append(" ");
            }
            if (SearchCondition.Int5 != null)
            {
                sb.Append(" AND a.Int5=").Append(SearchCondition.Int5).Append(" ");
            }
            return sb.ToString();
        }

        //入库报表
        private string GenGetReceiptRPTWhere(ReceiptSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.ReceiptNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ReceiptNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ReceiptNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ReceiptNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ReceiptNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and   ReceiptNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and   ReceiptNumber  like '%" + SearchCondition.ReceiptNumber.Trim() + "%' ");
                }
                //sb.Append(" AND   ReceiptNumber ='").Append(SearchCondition.ReceiptNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternReceiptNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ExternReceiptNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ExternReceiptNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ExternReceiptNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ExternReceiptNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and   ExternReceiptNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and   ExternReceiptNumber  like '%" + SearchCondition.ExternReceiptNumber.Trim() + "%' ");
                }
                //sb.Append(" AND   ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.ASNNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ASNNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ASNNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ASNNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ASNNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and   ASNNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and   ASNNumber  like '%" + SearchCondition.ASNNumber.Trim() + "%' ");
                }
                //sb.Append(" AND   ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND   CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (SearchCondition.CustomerID == null && SearchCondition.CustomerIDs != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                numbers = SearchCondition.CustomerIDs.Split(',').Select(s => { return s.Trim(); });
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                sb.Append(" and   CustomerID in ( ");
                foreach (string s in numbers)
                {
                    sb.Append("'").Append(s).Append("',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ) ");
            }
            if (SearchCondition.WarehouseID != 0)
            {
                sb.Append(" AND   WarehouseID=").Append(SearchCondition.WarehouseID).Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Area))
                sb.Append(" AND str5='").Append(SearchCondition.Area).Append("'");
            if (!string.IsNullOrEmpty(SearchCondition.ReceiptType))
            {
                sb.Append(" AND   ReceiptType='").Append(SearchCondition.ReceiptType).Append("' ");
            }
            if (SearchCondition.Status != 0)
            {
                sb.Append(" AND   Status='").Append(SearchCondition.Status).Append("' ");
            }
            if (SearchCondition.StartReceiptDate != null)
            {
                sb.Append(" AND   ReceiptDate >='").Append(SearchCondition.StartReceiptDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndReceiptDate != null)
            {
                sb.Append(" AND   ReceiptDate <='").Append(SearchCondition.EndReceiptDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartCompleteDate != null)
            {
                sb.Append(" AND   CompleteDate >='").Append(SearchCondition.StartCompleteDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCompleteDate != null)
            {
                sb.Append(" AND   CompleteDate <='").Append(SearchCondition.EndCompleteDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }


            //根据入库单号(收货单号)进行查询
            if (SearchCondition.ReceiptNumber != null)
            {
                sb.Append("AND ReceiptNumber like '%" + SearchCondition.ReceiptNumber.Trim() + "%'");
            }
            //if (SearchCondition.ReceiptNumber != null)
            //{
            //    sb.Append(" AND ReceiptNumber='").Append(SearchCondition.ReceiptNumber).Append("'");
            //}
            


            //根据外部单号进行查询
            if (SearchCondition.ExternReceiptNumber != null)
            {
                sb.Append("AND ExternReceiptNumber like '%" + SearchCondition.ExternReceiptNumber.Trim() + "%'");
            }


            //根据库位进行查询
            if (SearchCondition.Location != null)
            {
                sb.Append("AND Location like '%" + SearchCondition.Location.Trim() + "%'");
            }


            //根据SKU进行查询
            if (SearchCondition.SKU != null)
            {
                sb.Append("AND SKU like '%" + SearchCondition.SKU.Trim() + "%'");
            }

            //根据Article进行查询
            if (SearchCondition.Article != null)
            {
                sb.Append("AND Article like '%" + SearchCondition.Article.Trim() + "%'");
            }

           

            if (!string.IsNullOrEmpty(SearchCondition.str1))
            {
                sb.Append(" and str1 like '%" + SearchCondition.str1 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str2))
            {
                sb.Append(" and str2 like '%" + SearchCondition.str2 + "%' ");
            }

            //根据门店代码进行查询
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {
                sb.Append(" and str3 like '%" + SearchCondition.str3 + "%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.str4))
            {
                sb.Append(" and str4 like '%" + SearchCondition.str4 + "%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.str5))
            {
                sb.Append(" and str5 like '%" + SearchCondition.str5 + "%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.str6))
            {
                sb.Append(" and str6 like '%" + SearchCondition.str6 + "%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.str7))
            {
                sb.Append(" and str7 like '%" + SearchCondition.str7 + "%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.str8))
            {
                sb.Append(" and str8 like '%" + SearchCondition.str8 + "%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.str9))
            {
                sb.Append(" and str9 like '%" + SearchCondition.str9 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str10))
            {
                sb.Append(" and str10 like '%" + SearchCondition.str10 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str11))
            {
                sb.Append(" and str11 like '%" + SearchCondition.str11 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str12))
            {
                sb.Append(" and str12 like '%" + SearchCondition.str12 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str13))
            {
                sb.Append(" and str13 like '%" + SearchCondition.str13 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str14))
            {
                sb.Append(" and str14 like '%" + SearchCondition.str14 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str15))
            {
                sb.Append(" and str15 like '%" + SearchCondition.str15 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str16))
            {
                sb.Append(" and str16 like '%" + SearchCondition.str16 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str17))
            {
                sb.Append(" and str17 like '%" + SearchCondition.str17 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str18))
            {
                sb.Append(" and str18 like '%" + SearchCondition.str18 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str19))
            {
                sb.Append(" and str19 like '%" + SearchCondition.str19 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str20))
            {
                sb.Append(" and str20 like '%" + SearchCondition.str20 + "%' ");
            }
            if (SearchCondition.StartDateTime1 != null)
            {
                sb.Append(" AND   DateTime1 >='").Append(SearchCondition.StartDateTime1.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime1 != null)
            {
                sb.Append(" AND   DateTime1 <='").Append(SearchCondition.EndDateTime1.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartDateTime2 != null)
            {
                sb.Append(" AND   DateTime2 >='").Append(SearchCondition.StartDateTime2.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime2 != null)
            {
                sb.Append(" AND   DateTime2 <='").Append(SearchCondition.EndDateTime2.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartDateTime3 != null)
            {
                sb.Append(" AND   DateTime3 >='").Append(SearchCondition.StartDateTime3.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime3 != null)
            {
                sb.Append(" AND   DateTime3 <='").Append(SearchCondition.EndDateTime3.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartDateTime4 != null)
            {
                sb.Append(" AND   DateTime4 >='").Append(SearchCondition.StartDateTime4.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime4 != null)
            {
                sb.Append(" AND   DateTime4 <='").Append(SearchCondition.EndDateTime4.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartDateTime5 != null)
            {
                sb.Append(" AND   DateTime5 >='").Append(SearchCondition.StartDateTime5.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime5 != null)
            {
                sb.Append(" AND   DateTime5 <='").Append(SearchCondition.EndDateTime5.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.Int1 != null)
            {
                sb.Append(" AND   Int1=").Append(SearchCondition.Int1).Append(" ");

            }
            if (SearchCondition.Int2 != null)
            {
                sb.Append(" AND   Int2=").Append(SearchCondition.Int2).Append(" ");

            }
            if (SearchCondition.Int3 != null)
            {
                sb.Append(" AND   Int3=").Append(SearchCondition.Int3).Append(" ");

            }
            if (SearchCondition.Int4 != null)
            {
                sb.Append(" AND   Int4=").Append(SearchCondition.Int4).Append(" ");

            }
            if (SearchCondition.Int5 != null)
            {
                sb.Append(" AND   Int5=").Append(SearchCondition.Int5).Append(" ");

            }
            return sb.ToString();
        }
        private string GenGetAsnScanDiffRPTWhere(ReceiptSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (SearchCondition.StartReceiptDate != null)
            {
                sb.Append(" AND   a.ExpectDate >='").Append(SearchCondition.StartReceiptDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndReceiptDate != null)
            {
                sb.Append(" AND   a.ExpectDate <='").Append(SearchCondition.EndReceiptDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            //门店代码查询
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.str3.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.str3.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.str3.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.str3.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and  a.str3 in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and  a.str3  like '%" + SearchCondition.str3.Trim() + "%' ");
                }
            }


            return sb.ToString();
        }

        private string GenGetASNWhere(ASNSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.ASNNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ASNNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ASNNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ASNNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ASNNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and ASNNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and ASNNumber  like '%" + SearchCondition.ASNNumber.Trim() + "%' ");
                }
            }
            if (SearchCondition.CustomerID != 0 && SearchCondition.CustomerID != null)
            {
                sb.Append(" AND a.CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 新增订单及明细
        /// </summary>
        public string AddReceiptAndReceiptDetail(AddReceiptAndReceiptDetailRequest rece)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Receipt> result = new List<Receipt>();
                    IList<ReceiptDetail> receiptDetail = new List<ReceiptDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddReceiptANDReceiptDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Receipt", rece.Receipts.Select(receipt => new WMSReceiptToDb(receipt)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ReceiptDetail", rece.ReceiptDetails.Select(receiptDetali => new WMSReceiptDetailToDb(receiptDetali)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 50;
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

        public string EditReceiptAndReceiptDetail(AddReceiptAndReceiptDetailRequest rece)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Receipt> result = new List<Receipt>();
                    IList<ReceiptDetail> receiptDetail = new List<ReceiptDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_EditReceiptORReceiptDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Receipt", rece.Receipts.Select(receipt => new WMSReceiptToDb(receipt)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ReceiptDetail", rece.ReceiptDetails.Select(receiptDetali => new WMSReceiptDetailToDb(receiptDetali)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 50;
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

        public string EditReceiptAndReceiptDetail_ImportPatch(AddReceiptAndReceiptDetailRequest rece)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Receipt> result = new List<Receipt>();
                    IList<ReceiptDetail> receiptDetail = new List<ReceiptDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_EditReceiptORReceiptDetail_ImportPatch", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Receipt", rece.Receipts.Select(receipt => new WMSReceiptToDb(receipt)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ReceiptDetail", rece.ReceiptDetails.Select(receiptDetali => new WMSReceiptDetailToDb(receiptDetali)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = int.MaxValue;
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

        public string ReceiptStatusBack(AddReceiptAndReceiptDetailRequest request, int ToStatus)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    if (request.Receipts.Count() == 1)
                    {
                        SqlCommand cmd = new SqlCommand("Proc_WMS_ReceiptStatusBack", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", request.Receipts.Select(m => m.ID).FirstOrDefault().ObjectToInt64());
                        cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                        cmd.Parameters.AddWithValue("@ToStatus", ToStatus);
                        cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                        cmd.Parameters.AddWithValue("@message", message);
                        cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                        cmd.Parameters[2].Direction = ParameterDirection.Output;
                        cmd.Parameters[2].Size = 500;
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
                    else
                    {
                        SqlCommand cmd = new SqlCommand("Proc_WMS_ReceiptStatusBack_Batch", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Receipt", request.Receipts.Select(receipt => new WMSReceiptToDb(receipt)));
                        cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                        cmd.Parameters.AddWithValue("@ToStatus", ToStatus);
                        cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                        cmd.Parameters.AddWithValue("@message", message);
                        cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                        cmd.Parameters[2].Direction = ParameterDirection.Output;
                        cmd.Parameters[2].Size = 500;
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
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }

        }

        public GetReceiptAndReceriptDetailsResponse PrintShelves(string rid, int Flag)
        {
            GetReceiptAndReceriptDetailsResponse response = new GetReceiptAndReceriptDetailsResponse();
            string where = this.GetId(rid);
            string sqlWhere = this.GetPrintShelves(rid);

            DbParam[] dbParams = new DbParam[]{
                 new DbParam("@ID", DbType.String,rid.Split(',')[0],ParameterDirection.Input),
                new DbParam("@Where", DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@sqlWhere", DbType.String,where,ParameterDirection.Input),
                 new DbParam("@Flag", DbType.Int32,Flag,ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintShelves", dbParams);
            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public GetReceiptAndReceriptDetailsResponse PrintShelvesYXDR(string rid, int Flag)
        {
            GetReceiptAndReceriptDetailsResponse response = new GetReceiptAndReceriptDetailsResponse();
            string where = this.GetId(rid);
            string sqlWhere = this.GetPrintShelves(rid);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@sqlWhere", DbType.String,where,ParameterDirection.Input),
                 new DbParam("@Flag", DbType.Int32,Flag,ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintShelvesYXDR", dbParams);
            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }

        public GetReceiptAndReceriptDetailsResponse PrintShelvesYFBLD(string rid, int Flag)
        {
            GetReceiptAndReceriptDetailsResponse response = new GetReceiptAndReceriptDetailsResponse();
            string where = this.GetId(rid);
            string sqlWhere = this.GetPrintShelves(rid);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@sqlWhere", DbType.String,where,ParameterDirection.Input),
                 new DbParam("@Flag", DbType.Int32,Flag,ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintShelvesYFBLD", dbParams);
            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;

        }

        public GetReceiptAndReceriptDetailsResponse PrintShelvesNike(string rid, int Flag)
        {
            GetReceiptAndReceriptDetailsResponse response = new GetReceiptAndReceriptDetailsResponse();
            string where = this.GetId(rid);
            string sqlWhere = this.GetPrintShelves(rid);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.String,rid.Split(',')[0],ParameterDirection.Input),
                new DbParam("@Where", DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@sqlWhere", DbType.String,where,ParameterDirection.Input),
                 new DbParam("@Flag", DbType.Int32,Flag,ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintShelves_Nike", dbParams);
            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;

        }

        public int GetSkuTotal(string ID, string SKU, string BoxNumber, string Batchs)
        {
            int m = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.String,ID,ParameterDirection.Input),
                new DbParam("@SKU", DbType.String,SKU==null?"":SKU,ParameterDirection.Input),
                new DbParam("@BoxNumber", DbType.String,BoxNumber==null?"":BoxNumber,ParameterDirection.Input),
                new DbParam("@Batchs", DbType.String,Batchs==null?"":Batchs,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetSKUTotal", dbParams);
            if (ds.Tables[0].Rows.Count > 0)
            {
                m = ds.Tables[0].Rows[0][0].ObjectToInt32();
            }

            return m;

        }

        public string GetPrintShelves(string rid)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(rid))
            {
                sb.Append(" AND rd.RID IN (").Append(rid).Append(")");
            }

            return sb.ToString();
        }

        public string GetId(string rid)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(rid))
            {
                sb.Append(" AND ID IN (").Append(rid).Append(")");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 爱库存加入库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool AddInventoryAKC(string id, string UserName, out string msg)
        {
            msg = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AddInventoryAKC]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@Message", msg);
                cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                msg = sda.SelectCommand.Parameters["@Message"].Value.ToString();
                conn.Close();
                if (msg == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// HABA更新体积
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool UpdateReceiptVolume(string id,string Volume, string UserName, out string msg)
        {
            msg = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateReceiptVolume", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@Volume", Volume);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@Message", msg);
                cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                cmd.Parameters[3].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                msg = sda.SelectCommand.Parameters["@Message"].Value.ToString();
                conn.Close();
                if (msg == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string BackClsoeBox(string ExternReceiptNumber)
        {
           string  msg = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WMS_BackClsoeBox_RF", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExternReceiptNumber", ExternReceiptNumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@Message", msg);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                msg = sda.SelectCommand.Parameters["@Message"].Value.ToString();
                conn.Close();
                return msg;
            }
        }
        
        /// <summary>
        /// 根据id查询订单头信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<Receipt> GetRceiptInfoByIDs(string ids)
        {
            try
            {
                string sql = @"SELECT ID,ReceiptNumber,ExternReceiptNumber,ASNID,ASNNumber,CustomerID,CustomerName,WarehouseID,WarehouseName,ReceiptDate,Status,ReceiptType
                            ,Creator,CreateTime,CompleteDate, str1, str2, str3, str4, str5, str6, str7,str8, str9, str10, str11, str12, str13, 
                            str14, str15, str16, str17, str18, str19, str20, DateTime1, DateTime2,DateTime3, DateTime4, DateTime5, Int1, Int2, Int3, Int4, Int5
                            FROM dbo.WMS_Receipt WHERE ID IN ("+ids+")";
                return this.ExecuteDataTableBySqlString(sql).ConvertToEntityCollection<Receipt>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 吉特上架单
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public GetReceiptAndReceriptDetailsResponse PrintShelves_JT(string rid, int Flag)
        {
            GetReceiptAndReceriptDetailsResponse response = new GetReceiptAndReceriptDetailsResponse();
            string where = this.GetId(rid);
            string sqlWhere = this.GetPrintShelves(rid);

            DbParam[] dbParams = new DbParam[]{
                 new DbParam("@ID", DbType.String,rid.Split(',')[0],ParameterDirection.Input),
                new DbParam("@Where", DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@sqlWhere", DbType.String,where,ParameterDirection.Input),
                 new DbParam("@Flag", DbType.Int32,Flag,ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintShelves", dbParams);
            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }


    }
}
