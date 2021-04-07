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
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.Entity.WMS.Receipt;

namespace Runbow.TWS.Dao.WMS
{
    public class ASNManagementAccessor : BaseAccessor
    {
        /// <summary>
        /// 扫描更新数量
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="str2"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public string AsnScanQtyUpdate(string AsnNumber, string str2, string SKU, string Creator)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AsnScanQtyUpdate", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AsnNumber", AsnNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@str2", str2);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@SKU", SKU);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.Int;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
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
                    //return message + "(" + ex.Message + ")";
                    message = ex.Message;
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 根据单号查扫描情况
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public GetASNByConditionResponse GetASNScanByAsnNumber(string AsnNumber)
        {
            GetASNByConditionResponse response = new GetASNByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@AsnNumber", DbType.String, AsnNumber, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetASNDetailScanDiff", dbParams);
            response.ExpectTotalBox = ds.Tables[0].ConvertToEntity<ASNScan>();
            response.ReceiveTotalBox = ds.Tables[1].ConvertToEntity<ASNScan>();
            response.ExpectTotalSKU = ds.Tables[2].ConvertToEntity<ASNScan>();
            response.ReceiveTotalSKU = ds.Tables[3].ConvertToEntity<ASNScan>();
            response.ASNScanBoxSKUCollection = ds.Tables[4].ConvertToEntityCollection<ASNScan>();
            response.ASNScanBoxDetailSKUCollection = ds.Tables[5].ConvertToEntityCollection<ASNScan>();
            return response;
        }
        /// <summary>
        /// 检查箱差异
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="ScanBoxNumber"></param>
        /// <returns></returns>
        public string CheckDiff(string AsnNumber, string ScanBoxNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_CheckDiff", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AsnNumber", AsnNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@ScanBoxNumber", ScanBoxNumber);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
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
                    //return message + "(" + ex.Message + ")";
                    message = ex.Message;
                    throw ex;
                }
            }
        }
        public string ClearAsnBoxNumber(string AsnNumber, string ScanBoxNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ClearAsnBoxNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AsnNumber", AsnNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@ScanBoxNumber", ScanBoxNumber);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
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
                    //return message + "(" + ex.Message + ")";
                    message = ex.Message;
                    throw ex;
                }
            }
        }
        public List<ASNDetail> CheckDiffReturn(string AsnNumber, string ScanBoxNumber)
        {
            List<ASNDetail> lists = new List<ASNDetail>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_CheckDiffReturn]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AsnNumber", AsnNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@ScanBoxNumber", ScanBoxNumber);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    lists = ds.Tables[0].ConvertToEntityCollection<ASNDetail>().ToList();
                    conn.Close();
                    return lists;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    message = ex.Message;
                    throw ex;
                }
            }
        }
        public List<ASNDetail> GetAsnScanBoxSum(string AsnNumber, string ScanBoxNumber)
        {
            List<ASNDetail> lists = new List<ASNDetail>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_GetAsnScanBoxSum]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AsnNumber", AsnNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@ScanBoxNumber", ScanBoxNumber);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    lists = ds.Tables[0].ConvertToEntityCollection<ASNDetail>().ToList();
                    conn.Close();
                    return lists;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    message = ex.Message;
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 根据单号查明细--asn扫描
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public GetASNDetailByConditionResponse GetASNDetailForScanByAsnNumber(string AsnNumber)
        {
            GetASNDetailByConditionResponse response = new GetASNDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@AsnNumber", DbType.String, AsnNumber, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetASNDetailForScanByAsnNumber", dbParams);
            response.AsnDetailCollection = ds.Tables[0].ConvertToEntityCollection<ASNDetail>();
            return response;
        }

        /// <summary>
        /// 查询显示 分页查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<ASN> GetASNByCondition(ASNSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenGetWhere(SearchCondition);

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetASNByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ASN>();
        }

        public IEnumerable<ASN> GetASNByConditionSF(ASNSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenGetWhereSF(SearchCondition);

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetASNByConditionSF]", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ASN>();
        }

        /// <summary>
        /// 入库单状态统计查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<ASN> GetASNStatusByCondition(ASNSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetReceiptStatusWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),

            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetReceiptStatusByCondition", dbParams);
            return dt.ConvertToEntityCollection<ASN>();
        }
        /// <summary>
        /// 状态查询条件
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public string GenGetReceiptStatusWhere(ASNSearchCondition search)
        {
            StringBuilder sb = new StringBuilder();
            if (search.CustomerID != null)
            {
                sb.Append(" AND a.CustomerID=").Append(search.CustomerID).Append(" ");
            }
            if (search.WarehouseName != null)
            {
                sb.Append(" AND a.WarehouseName='").Append(search.WarehouseName).Append("' ");
            }
            if (search.StartCreateTime != null)
            {
                sb.Append(" AND a.CreateTime >='").Append(search.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (search.EndCreateTime != null)
            {
                sb.Append(" AND a.CreateTime <='").Append(search.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }

            return sb.ToString();
        }
        /// <summary>
        /// 查询此状态下的所有订单
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public IEnumerable<ASN> SearchReceiptOrderTotal(ASNSearchCondition SearchCondition, int type)
        {
            string sqlWhere = this.GenGetReceiptStatusTotal(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                 new DbParam("@Type",DbType.Int32,type,ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetReceiptStatusTotalByCondition", dbParams);
            return dt.ConvertToEntityCollection<ASN>();
        }

        /// <summary>
        /// 查询此状态下的所有订单
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public int TurnCG(int Id, int CustomerId)
        {
            string sqlStr = @" update  WMS_ASNSF set Status=5 where id=" + Id;
            int i = this.ScanExecuteNonQuery(sqlStr);
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            //     new DbParam("@Type",DbType.Int32,type,ParameterDirection.Input),
            //};
            //DataTable dt = this.ExecuteDataTable("Proc_WMS_GetReceiptStatusTotalByCondition", dbParams);
            //return dt.ConvertToEntityCollection<ASN>();
            return 1;
        }

        /// <summary>
        /// 查询此状态下的所有订单
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public int TurnASN(int Id, int CustomerId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select* from WMS_ASNSF  where ID=" + Id);

            var sqlData = this.ScanDataTable(sql.ToString()).ConvertToEntity<ASN>();
            if (sqlData.Status == 9)
            {
                return 1;
            }

            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(@" 
             insert Into WMS_ASN ([ASNNumber]
             ,[ExternReceiptNumber]
             ,[CustomerID]
             ,[CustomerName]
             ,[WarehouseID]
             ,[WarehouseName]
             ,[ExpectDate]
             ,[Status]
             ,[ASNType]
             ,[Creator]
             ,[CreateTime]
             ,[Updator]
             ,[UpdateTime]
             ,[CompleteDate]
             ,[Remark]
             ,[str1]
             ,[str2]
             ,[str3]
             ,[str4]
             ,[str5]
             ,[str6]
             ,[str7]
             ,[str8]
             ,[str9]
             ,[str10]
             ,[str11]
             ,[str12]
             ,[str13]
             ,[str14]
             ,[str15]
             ,[str16]
             ,[str17]
             ,[str18]
             ,[str19]
             ,[str20]
             ,[DateTime1]
             ,[DateTime2]
             ,[DateTime3]
             ,[DateTime4]
             ,[DateTime5]
             ,[Int1]
             ,[Int2]
             ,[Int3]
             ,[Int4]
             ,[Int5])
             select [ASNNumber]
             ,[ExternReceiptNumber]
             ,[CustomerID]
             ,[CustomerName]
             ,[WarehouseID]
             ,[WarehouseName]
             ,[ExpectDate]
             ,1
             ,'补料入库-物料'
             ,[Creator]
             ,[CreateTime]
             ,[Updator]
             ,[UpdateTime]
             ,[CompleteDate]
             ,[Remark]
             ,[str1]
             ,[str2]
             ,[str3]
             ,[str4]
             ,[str5]
             ,[str6]
             ,[str7]
             ,[str8]
             ,[str9]
             ,[str10]
             ,[str11]
             ,[str12]
             ,[str13]
             ,[str14]
             ,[str15]
             ,[str16]
             ,[str17]
             ,[str18]
             ,[str19]
             ,'物料'
             ,[DateTime1]
             ,[DateTime2]
             ,[DateTime3]
             ,[DateTime4]
             ,[DateTime5]
             ,[Int1]
             ,[Int2]
             ,[Int3]
             ,[Int4]
             ,[Int5] from WMS_ASNSF where ID=" + Id + @"
             insert into  WMS_ASNDetail  ([ASNID]
             ,[ASNNumber]
             ,[ExternReceiptNumber]
             ,[CustomerID]
             ,[CustomerName]
             ,[LineNumber]
             ,[SKU]
             ,[UPC]
             ,[BoxNumber]
             ,[BatchNumber]
             ,[QtyExpected]
             ,[QtyReceived]
             ,[QtyReceipt]
             ,[GoodsType]
             ,[GoodsName]
             ,[Unit]
             ,[Specifications]
             ,[Creator]
             ,[CreateTime]
             ,[Updator]
             ,[UpdateTime]
             ,[str1]
             ,[str2]
             ,[str3]
             ,[str4]
             ,[str5]
             ,[str6]
             ,[str7]
             ,[str8]
             ,[str9]
             ,[str10]
             ,[str11]
             ,[str12]
             ,[str13]
             ,[str14]
             ,[str15]
             ,[str16]
             ,[str17]
             ,[str18]
             ,[str19]
             ,[str20]
             ,[DateTime1]
             ,[DateTime2]
             ,[DateTime3]
             ,[DateTime4]
             ,[DateTime5]
             ,[Int1]
             ,[Int2]
             ,[Int3]
             ,[Int4]
             ,[Int5])
             
             select  @@IDENTITY [ASNID]
             ,[ASNNumber]
             ,[ExternReceiptNumber]
             ,[CustomerID]
             ,[CustomerName]
             ,[LineNumber]
             ,[SKU]
             ,[UPC]
             ,[BoxNumber]
             ,[BatchNumber]
             ,[QtyExpected]
             ,[QtyReceived]
             ,[QtyReceipt]
             ,[GoodsType]
             ,[GoodsName]
             ,[Unit]
             ,[Specifications]
             ,[Creator]
             ,[CreateTime]
             ,[Updator]
             ,[UpdateTime]
             ,[str1]
             ,[str2]
             ,[str3]
             ,[str4]
             ,[str5]
             ,[str6]
             ,[str7]
             ,[str8]
             ,[str9]
             ,[str10]
             ,[str11]
             ,[str12]
             ,[str13]
             ,[str14]
             ,[str15]
             ,[str16]
             ,[str17]
             ,[str18]
             ,[str19]
             ,[str20]
             ,[DateTime1]
             ,[DateTime2]
             ,[DateTime3]
             ,[DateTime4]
             ,[DateTime5]
             ,[Int1]
             ,[Int2]
             ,[Int3]
             ,[Int4]
             ,[Int5] from  WMS_ASNDetailSF where ASNID=" + Id);
            sqlStr.Append(@"update WMS_ASNSF set Status = 9 where ID = " + Id);

            int i = this.ScanExecuteNonQuery(sqlStr.ToString());
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            //     new DbParam("@Type",DbType.Int32,type,ParameterDirection.Input),
            //};
            //DataTable dt = this.ExecuteDataTable("Proc_WMS_GetReceiptStatusTotalByCondition", dbParams);
            //return dt.ConvertToEntityCollection<ASN>();
            return 1;
        }

        /// <summary>
        /// 根据状态查询订单条件
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public string GenGetReceiptStatusTotal(ASNSearchCondition search)
        {
            StringBuilder sb = new StringBuilder();

            if (search.CustomerID != null)
            {
                sb.Append(" AND a.CustomerID=").Append(search.CustomerID).Append(" ");
            }
            if (!string.IsNullOrEmpty(search.WarehouseName))
            {
                sb.Append(" AND a.WarehouseName='").Append(search.WarehouseName).Append("' ");
            }
            if (search.StartCreateTime != null)
            {
                sb.Append(" AND a.CreateTime >='").Append(search.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (search.EndCreateTime != null)
            {
                sb.Append(" AND a.CreateTime <='").Append(search.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (search.Status != null)
            {
                sb.Append(" AND a.Status=").Append(search.Status).Append(" ");
            }
            return sb.ToString();
        }


        public GetASNDetailByConditionResponse GetASNandasndetailByCondition(ASNSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetASNDetailByConditionResponse response = new GetASNDetailByConditionResponse();
            string sqlWhere = this.GenGetWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetASNandasndetailByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.AsnCollection = ds.Tables[0].ConvertToEntityCollection<ASN>();
            response.AsnDetailCollection = ds.Tables[1].ConvertToEntityCollection<ASNDetail>();
            return response;
        }


        public GetASNDetailByConditionResponse GetASNandasndetailByConditionSF(ASNSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetASNDetailByConditionResponse response = new GetASNDetailByConditionResponse();
            string sqlWhere = this.GenGetWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("[Proc_WMS_GetASNandasndetailByConditionSF]", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.AsnCollection = ds.Tables[0].ConvertToEntityCollection<ASN>();
            response.AsnDetailCollection = ds.Tables[1].ConvertToEntityCollection<ASNDetail>();
            return response;
        }

        //导出主表和明细表
        public GetASNDetailByConditionResponse GetExportAsnandDetailByCondition(ASNSearchCondition SearchCondition)
        {
            GetASNDetailByConditionResponse response = new GetASNDetailByConditionResponse();
            string sqlWhere = this.GenGetWhere(SearchCondition);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetExportAsnandDetailByCondition", dbParams);

            response.AsnCollection = ds.Tables[0].ConvertToEntityCollection<ASN>();
            response.AsnDetailCollection = ds.Tables[1].ConvertToEntityCollection<ASNDetail>();
            return response;
        }

        //导出选中的主表和明细表
        public GetASNDetailByConditionResponse GetReceiptByIDs(string IDs)
        {
            GetASNDetailByConditionResponse response = new GetASNDetailByConditionResponse();
            //string sqlWhere = this.GenGetReceiptWhere(SearchCondition);
            //int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String, IDs, ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetASNByIDs", dbParams);

            response.AsnCollection = ds.Tables[0].ConvertToEntityCollection<ASN>();
            response.AsnDetailCollection = ds.Tables[1].ConvertToEntityCollection<ASNDetail>();
            //response.ReceiptDetailCollection2 = ds.Tables[2].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }
        //获取要上传的订单和订单明细
        public GetReceiptDetailByConditionResponse GetConfirmAsnAndAsnDetail(string strwhere)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string sqlWhere = strwhere;

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@sqlWhere", DbType.String, sqlWhere, ParameterDirection.Input)

            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetAkzoASNNormalConfirm", dbParams);

            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }
        //获取要上传的订单和订单明细
        public GetReceiptDetailByConditionResponse GetConfirmAsnAndAsnDetailByNikeCE(string strwhere)
        {
            GetReceiptDetailByConditionResponse response = new GetReceiptDetailByConditionResponse();
            string sqlWhere = strwhere;

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@sqlWhere", DbType.String, sqlWhere, ParameterDirection.Input)

            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetNikeCEASNNormalConfirm", dbParams);

            response.ReceiptCollection = ds.Tables[0].ConvertToEntityCollection<Receipt>();
            response.ReceiptDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReceiptDetail>();
            return response;
        }
        //获取要上传的取消入库单订单和订单明细
        public GetASNDetailByConditionResponse GetCancelAsnAndAsnDetail(string strwhere)
        {
            GetASNDetailByConditionResponse response = new GetASNDetailByConditionResponse();
            string sqlWhere = strwhere;

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@sqlWhere", DbType.String, sqlWhere, ParameterDirection.Input)

            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetAkzoASNNormalConfirm", dbParams);

            response.AsnCollection = ds.Tables[0].ConvertToEntityCollection<ASN>();
            response.AsnDetailCollection = ds.Tables[1].ConvertToEntityCollection<ASNDetail>();
            return response;
        }
        /// <summary>
        /// 点击编辑
        /// </summary>
        /// <param name="asnnumbner"></param>
        /// <returns></returns>
        public ASNAndASNDetail GetASNInfos(int ID)
        {
            ASNAndASNDetail request = new ASNAndASNDetail();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int32,ID, ParameterDirection.Input),
            };
            DataSet ds = base.ExecuteDataSet("Proc_WMS_GetASNInfos", dbParams);

            request.asn = ds.Tables[0].ConvertToEntity<ASN>();
            request.asnDetails = ds.Tables[1].ConvertToEntityCollection<ASNDetail>();
            return request;
        }

        public ASNAndASNDetail GetASNInfosSF(int ID)
        {
            ASNAndASNDetail request = new ASNAndASNDetail();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int32,ID, ParameterDirection.Input),
            };
            DataSet ds = base.ExecuteDataSet("[Proc_WMS_GetASNInfosSF]", dbParams);

            request.asn = ds.Tables[0].ConvertToEntity<ASN>();
            request.asnDetails = ds.Tables[1].ConvertToEntityCollection<ASNDetail>();
            return request;
        }
        /// <summary>
        /// 生成上架库位
        /// </summary>
        /// <param name="ASNNumber"></param>
        /// <returns></returns>
        public bool CreateShelfLocation(string id)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@IDS",DbType.String,id,ParameterDirection.Input),
                };
                base.ExecuteNoQuery("Proc_WMS_CreateShelfLocation_New", dbPatams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="ASNNumber"></param>
        /// <returns></returns>
        public bool ASNDelete(int id)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@ID",DbType.Int32,id,ParameterDirection.Input),
                };
                base.ExecuteNoQuery("Proc_WMS_DeleteByID", dbPatams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="ASNNumber"></param>
        /// <returns></returns>
        public bool ASNDeleteSF(int id)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@ID",DbType.Int32,id,ParameterDirection.Input),
                };
                base.ExecuteNoQuery("[Proc_WMS_DeleteByIDSF]", dbPatams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 批量取消操作
        /// </summary>
        /// <param name="asnnumberlist"></param>
        /// <returns></returns>
        public bool StatusBacks(string asnid)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@ASNID",DbType.String,asnid,ParameterDirection.Input),
                };
                base.ExecuteNoQuery("Proc_WMS_StatusBacks", dbPatams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 完成操作
        /// </summary>
        /// <param name="ASNNumber"></param>
        /// <returns></returns>
        public bool Complet(int ID)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@ID",DbType.Int32,ID,ParameterDirection.Input),
                };
                base.ExecuteNoQuery("Proc_WMS_CompletByID", dbPatams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 更新订单上传状态
        /// </summary>
        /// <param name="Type">Type 更新的订单类型  1, 入库单  2, 出库单一次回传 3 出库单二次回传 ,4 调整 5, 其他</param>
        /// <param name="IDs">要更新的订单ID集合,多个用英文逗号分割 如 122,1123,111  ,传入预入库单或预出库单ID </param>
        public void UpdateConfirmStatus(string Type, string IDs, string DocNumber)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@Type",DbType.String,Type,ParameterDirection.Input),
                     new  DbParam("@IDs",DbType.String,IDs,ParameterDirection.Input),
                     new  DbParam("@DocNumber",DbType.String,DocNumber,ParameterDirection.Input),
                };
                base.ExecuteNoQuery("Proc_WMS_UpdateConfirmStatus", dbPatams);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Receipt> CountReceipt(int ID)
        {

            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@ID",DbType.Int32,ID,ParameterDirection.Input),
                };
                DataTable dt = this.ExecuteDataTable("Proc_WMS_GetreceiptByID", dbPatams);
                return dt.ConvertToEntityCollection<Receipt>();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public int CountAsn(string ExternNumber, long customerID)
        {
            int s = 0;
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@ExternNumber",DbType.String,ExternNumber,ParameterDirection.Input),
                      new  DbParam("@customerID",DbType.Int64,customerID,ParameterDirection.Input)
                };

                DataTable dt = this.ExecuteDataTable("Proc_WMS_GetAsnByExternNumber", dbPatams);
                s = dt.Rows.Count.ObjectToInt32();
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return s;
        }
        /// <summary>
        /// 批量完成操作
        /// </summary>
        /// <param name="asnnumberlist"></param>
        /// <returns></returns>
        public bool CompletALLSelect(string asnid)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                new  DbParam("@ASNID",DbType.String,asnid,ParameterDirection.Input),
            };
                base.ExecuteNoQuery("Proc_WMS_CompletALLSelect", dbPatams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 条件拼接
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private string GenGetWhere(ASNSearchCondition SearchCondition)
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
                //sb.Append(" AND a.ASNNumber ='").Append(SearchCondition.ASNNumber).Append("' ");
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
            if (!string.IsNullOrEmpty(SearchCondition.ASNType))
            {
                sb.Append(" AND a.ASNType='").Append(SearchCondition.ASNType).Append("' ");
            }
            if (SearchCondition.Status != 0)
            {
                sb.Append(" AND a.Status='").Append(SearchCondition.Status).Append("' ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND a.CustomerID='").Append(SearchCondition.CustomerID).Append("' ");
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
            if (SearchCondition.StartExpectDate != null)
            {
                sb.Append(" AND a.ExpectDate >='").Append(SearchCondition.StartExpectDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndExpectDate != null)
            {
                sb.Append(" AND a.ExpectDate <='").Append(SearchCondition.EndExpectDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND a.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND a.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            //if (SearchCondition.StartCreateTime != null)
            //{
            //    sb.Append(" AND a.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:ss.00")).Append("' ");
            //}
            //if (SearchCondition.EndCreateTime != null)
            //{
            //    sb.Append(" AND a.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:59.99")).Append("' ");
            //}
            if (SearchCondition.WarehouseID != 0)
            {
                sb.Append(" AND a.WarehouseId='").Append(SearchCondition.WarehouseID).Append("' ");
            }
            else
            {
                sb.Append(" AND a.WarehouseName in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.WarehouseName.Trim()).Append(")) ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str1))
            {
                sb.Append(" and str1 like '%" + SearchCondition.str1.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str2))
            {
                sb.Append(" and str2 like '%" + SearchCondition.str2.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {
                sb.Append(" and str3 like '%" + SearchCondition.str3.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str4))
            {
                sb.Append(" and str4 like '%" + SearchCondition.str4.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str5))
            {
                sb.Append(" and str5 like '%" + SearchCondition.str5.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str6))
            {
                sb.Append(" and str6 like '%" + SearchCondition.str6.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str7))
            {
                sb.Append(" and str7 like '%" + SearchCondition.str7.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str8))
            {
                sb.Append(" and str8 like '%" + SearchCondition.str8.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str9))
            {
                sb.Append(" and str9 like '%" + SearchCondition.str9.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str10))
            {
                sb.Append(" and str10 like '%" + SearchCondition.str10.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str11))
            {
                sb.Append(" and str11 like '%" + SearchCondition.str11.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str12))
            {
                sb.Append(" and str12 like '%" + SearchCondition.str12.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str13))
            {
                sb.Append(" and str13 like '%" + SearchCondition.str13.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str14))
            {
                sb.Append(" and str14 like '%" + SearchCondition.str14.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str15))
            {
                sb.Append(" and str15 like '%" + SearchCondition.str15.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str16))
            {
                sb.Append(" and str16 like '%" + SearchCondition.str16.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str17))
            {
                sb.Append(" and str17 like '%" + SearchCondition.str17.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str18))
            {
                sb.Append(" and str18 like '%" + SearchCondition.str18.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str19))
            {
                sb.Append(" and str19 like '%" + SearchCondition.str19.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str20))
            {
                sb.Append(" and str20 like '%" + SearchCondition.str20.Trim() + "%' ");
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

            }
            if (SearchCondition.Int4 != null)
            {
                sb.Append(" AND a.Int4=").Append(SearchCondition.Int4).Append(" ");

            }
            if (SearchCondition.Int5 != null)
            {
                sb.Append(" AND a.Int5=").Append(SearchCondition.Int5).Append(" ");

            }
            if (!string.IsNullOrEmpty(SearchCondition.Model) && SearchCondition.Model == "产品")
            {
                sb.Append(" AND a.ASNType like '%").Append(SearchCondition.Model).Append("%' ");
            }
            else
            {
                sb.Append(" AND a.ASNType like '%物料%' ");

            }

            return sb.ToString();
        }


        /// <summary>
        /// 条件拼接
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private string GenGetWhereSF(ASNSearchCondition SearchCondition)
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
                //sb.Append(" AND a.ASNNumber ='").Append(SearchCondition.ASNNumber).Append("' ");
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
            if (!string.IsNullOrEmpty(SearchCondition.ASNType))
            {
                sb.Append(" AND a.ASNType='").Append(SearchCondition.ASNType).Append("' ");
            }
            if (SearchCondition.Status != 0)
            {
                if (SearchCondition.Status == 5)
                {
                    sb.Append(" AND a.Status>=").Append(SearchCondition.Status).Append(" ");
                }
                else
                {
                    sb.Append(" AND a.Status='").Append(SearchCondition.Status).Append("' ");
                }
            }

            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND a.CustomerID='").Append(SearchCondition.CustomerID).Append("' ");
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
            if (SearchCondition.StartExpectDate != null)
            {
                sb.Append(" AND a.ExpectDate >='").Append(SearchCondition.StartExpectDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndExpectDate != null)
            {
                sb.Append(" AND a.ExpectDate <='").Append(SearchCondition.EndExpectDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND a.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND a.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            //if (SearchCondition.StartCreateTime != null)
            //{
            //    sb.Append(" AND a.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:ss.00")).Append("' ");
            //}
            //if (SearchCondition.EndCreateTime != null)
            //{
            //    sb.Append(" AND a.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:59.99")).Append("' ");
            //}
            if (SearchCondition.WarehouseID != 0)
            {
                sb.Append(" AND a.WarehouseId='").Append(SearchCondition.WarehouseID).Append("' ");
            }
            else
            {
                sb.Append(" AND a.WarehouseName in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.WarehouseName.Trim()).Append(")) ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str1))
            {
                sb.Append(" and str1 like '%" + SearchCondition.str1.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str2))
            {
                sb.Append(" and str2 like '%" + SearchCondition.str2.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {
                sb.Append(" and str3 like '%" + SearchCondition.str3.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str4))
            {
                sb.Append(" and str4 like '%" + SearchCondition.str4.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str5))
            {
                sb.Append(" and str5 like '%" + SearchCondition.str5.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str6))
            {
                sb.Append(" and str6 like '%" + SearchCondition.str6.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str7))
            {
                sb.Append(" and str7 like '%" + SearchCondition.str7.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str8))
            {
                sb.Append(" and str8 like '%" + SearchCondition.str8.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str9))
            {
                sb.Append(" and str9 like '%" + SearchCondition.str9.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str10))
            {
                sb.Append(" and str10 like '%" + SearchCondition.str10.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str11))
            {
                sb.Append(" and str11 like '%" + SearchCondition.str11.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str12))
            {
                sb.Append(" and str12 like '%" + SearchCondition.str12.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str13))
            {
                sb.Append(" and str13 like '%" + SearchCondition.str13.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str14))
            {
                sb.Append(" and str14 like '%" + SearchCondition.str14.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str15))
            {
                sb.Append(" and str15 like '%" + SearchCondition.str15.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str16))
            {
                sb.Append(" and str16 like '%" + SearchCondition.str16.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str17))
            {
                sb.Append(" and str17 like '%" + SearchCondition.str17.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str18))
            {
                sb.Append(" and str18 like '%" + SearchCondition.str18.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str19))
            {
                sb.Append(" and str19 like '%" + SearchCondition.str19.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str20))
            {
                sb.Append(" and str20 like '%" + SearchCondition.str20.Trim() + "%' ");
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

            }
            if (SearchCondition.Int4 != null)
            {
                sb.Append(" AND a.Int4=").Append(SearchCondition.Int4).Append(" ");

            }
            if (SearchCondition.Int5 != null)
            {
                sb.Append(" AND a.Int5=").Append(SearchCondition.Int5).Append(" ");

            }
            if (!string.IsNullOrEmpty(SearchCondition.Model) && SearchCondition.Model == "产品")
            {
                sb.Append(" AND a.ASNType like '%").Append(SearchCondition.Model).Append("%' ");
            }
            else
            {
                sb.Append(" AND a.ASNType like '%物料%' ");

            }

            return sb.ToString();
        }
        /// <summary>
        /// 新增订单及明细
        /// </summary>
        public string AddasnAndasnDetail(AddASNandASNDetailRequest rece)
        {
            rece.asnDetails = ReturnNewDt(rece.asnDetails.ToDataTable()).ConvertToEntityCollection<ASNDetail>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Receipt> result = new List<Receipt>();
                    IList<ReceiptDetail> receiptDetail = new List<ReceiptDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddASNANDASNDetali", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Asn", rece.asn.Select(ASN => new WMSASNToDb(ASN)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@AsnDetali", rece.asnDetails.Select(asnDetali => new WMSASNDetailToDb(asnDetali)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
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
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 新增订单及明细
        /// </summary>
        public string AddasnAndasnDetailSF(AddASNandASNDetailRequest rece)
        {
            rece.asnDetails = ReturnNewDt(rece.asnDetails.ToDataTable()).ConvertToEntityCollection<ASNDetail>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Receipt> result = new List<Receipt>();
                    IList<ReceiptDetail> receiptDetail = new List<ReceiptDetail>();
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_AddASNANDASNDetaliSF]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Asn", rece.asn.Select(ASN => new WMSASNToDb(ASN)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@AsnDetali", rece.asnDetails.Select(asnDetali => new WMSASNDetailToDb(asnDetali)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
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
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public int ExternKeyCheck(string keys, string flag, long CustomerID)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                int message = 0;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ExternKeyCheck", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Asn", keys);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@flag", flag);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ObjectToInt32();
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

        public int ExternKeyCheckSF(string keys, string flag, long CustomerID)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                int message = 0;
                try
                {
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_ExternKeyCheckSF]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Asn", keys);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@flag", flag);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ObjectToInt32();
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
        /// <summary>
        /// 根据订单号查询订单信息并验证
        /// </summary>
        /// <param name="Orderkey"></param>
        /// <returns></returns>
        public GetOrderByConditionResponse OrderKeyCheck(string Orderkey)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_OrderCheck", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderKey", Orderkey);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                    conn.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 根据订单号查询订单信息并验证
        /// </summary>
        /// <param name="Orderkey"></param>
        /// <returns></returns>
        public GetOrderByConditionResponse OrderNumbersKeyCheck(List<OrderNumbers> numbers)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_OrderNumbersCheck", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderNumbers", numbers.Select(m => new OrderNumberToDb(m)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                    conn.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 导入包装信息
        /// </summary>
        /// <param name="PackageInfo"></param>
        /// <returns></returns>
        public string ImportPackageInfo(AddPackageAndDetailRequest request)
        {
            string error = string.Empty;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_InsertPackageHeaderAddDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PackageHeader", request.packages.Select(ASN => new WMSPackageToDb(ASN)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.Add(new SqlParameter("@Return", SqlDbType.NVarChar, 200));
                    cmd.Parameters["@Return"].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    error = cmd.Parameters["@Return"].Value.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return error;
        }
        /// <summary>
        /// 导入包装信息
        /// </summary>
        /// <param name="PackageInfo"></param>
        /// <param name="Proc">存储过程</param>
        /// <returns></returns>
        public string ImportPackageInfo(AddPackageAndDetailRequest request, string Proc)
        {
            string error = string.Empty;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(Proc, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PackageHeader", request.packages.Select(ASN => new WMSPackageToDb(ASN)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.Add(new SqlParameter("@Return", SqlDbType.NVarChar, 200));
                    cmd.Parameters["@Return"].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    error = cmd.Parameters["@Return"].Value.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return error;
        }
        /// <summary>
        /// 生成行号
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>DataTable</returns>
        public DataTable ReturnNewDt(DataTable dt)
        {
            //克隆一个新表
            //DataTable tables = dt.Clone();
            /////为新表赋值
            //foreach (DataRow item in dt.Rows)
            //{
            //    tables.Rows.Add(item.ItemArray);
            //}
            //得到有几个不同的外部单号
            //var dtt = from t in dt.AsEnumerable()
            //      group t by new { t1 = t.Field<string>("ExternReceiptNumber"), t2 = t.Field<string>("SKU"), t3 = t.Field<string>("BoxNumber"), t4 = t.Field<string>("BatchNumber"),
            //                       t5 = t.Field<string>("Qty"),
            //                       t6 = t.Field<string>("ID"),
            //                       t7 = t.Field<string>("ASNID"),
            //                       t8 = t.Field<string>("ASNNumber"),
            //                       t9 = t.Field<string>("CustomerID"),
            //                       t10 = t.Field<string>("CustomerName"),
            //                       t11 = t.Field<string>("QtyReceived"),
            //                       t12 = t.Field<string>("GoodsType"),
            //                       t13 = t.Field<string>("GoodsName"),
            //                       t14 = t.Field<string>("Creator"),
            //                       t15 = t.Field<string>("CreateTime"),
            //                       t16 = t.Field<string>("Updator"),
            //                       t17 = t.Field<string>("UpdateTime"),
            //                       t18 = t.Field<string>("str1"),
            //                       t19 = t.Field<string>("str2"),
            //                       t20 = t.Field<string>("str3"),
            //                       t21 = t.Field<string>("str4"),
            //                       t22 = t.Field<string>("str5"),
            //                       t23 = t.Field<string>("str6"),
            //                       t24 = t.Field<string>("str7"),
            //                       t25 = t.Field<string>("str8"),
            //                       t26 = t.Field<string>("str9"),
            //                       t27 = t.Field<string>("str10"),
            //                       t28 = t.Field<string>("str11"),
            //                       t29 = t.Field<string>("str12"),
            //                       t30 = t.Field<string>("str13"),
            //                       t31 = t.Field<string>("str14"),
            //                       t32 = t.Field<string>("str15"),
            //                       t33 = t.Field<string>("str16"),
            //                       t34 = t.Field<string>("str17"),
            //                       t35 = t.Field<string>("str18"),
            //                       t36 = t.Field<string>("str19"),
            //                       t37 = t.Field<string>("str20"),
            //                       t38 = t.Field<string>("DateTime1"),
            //                       t39 = t.Field<string>("DateTime2"),
            //                       t40 = t.Field<string>("DateTime3"),
            //                       t41 = t.Field<string>("DateTime4"),
            //                       t42 = t.Field<string>("DateTime5"),
            //                       t43 = t.Field<string>("Int1"),
            //                       t44 = t.Field<string>("Int2"),
            //                       t45 = t.Field<string>("Int3"),
            //                       t46 = t.Field<string>("Int4"),
            //                       t47 = t.Field<string>("Int5")
            //      } into m
            //      select new
            //      {
            //          SKU = m.Select(p => p.Field<string>("SKU")).First(),
            //          ExternReceiptNumber = m.Select(p => p.Field<string>("ExternReceiptNumber")).First(),
            //          BoxNumber = m.Select(p => p.Field<string>("BoxNumber")).First(),
            //          BatchNumber = m.Select(p => p.Field<string>("BatchNumber")).First(),
            //          QtyExpected = m.Sum(p => p.Field<decimal>("QtyExpected")),
            //          Qty = m.Select(p => p.Field<string>("Qty")).First(),
            //          ID = m.Select(p => p.Field<string>("ID")).First(),
            //          ASNID = m.Select(p => p.Field<string>("ASNID")).First(),
            //          ASNNumber = m.Select(p => p.Field<string>("ASNNumber")).First(),
            //          CustomerID = m.Select(p => p.Field<string>("CustomerID")).First(),
            //          CustomerName = m.Select(p => p.Field<string>("CustomerName")).First(),
            //          QtyReceived = m.Select(p => p.Field<string>("QtyReceived")).First(),
            //          GoodsType = m.Select(p => p.Field<string>("GoodsType")).First(),
            //          GoodsName = m.Select(p => p.Field<string>("GoodsName")).First(),
            //          Creator = m.Select(p => p.Field<string>("Creator")).First(),
            //          CreateTime = m.Select(p => p.Field<string>("CreateTime")).First(),
            //          Updator = m.Select(p => p.Field<string>("Updator")).First(),
            //          UpdateTime = m.Select(p => p.Field<string>("UpdateTime")).First(),
            //          str1 = m.Select(p => p.Field<string>("str1")).First(),
            //          str2 = m.Select(p => p.Field<string>("str2")).First(),
            //          str3 = m.Select(p => p.Field<string>("str3")).First(),
            //          str4 = m.Select(p => p.Field<string>("str4")).First(),
            //          str5 = m.Select(p => p.Field<string>("str5")).First(),
            //          str6 = m.Select(p => p.Field<string>("str6")).First(),
            //          str7 = m.Select(p => p.Field<string>("str7")).First(),
            //          str8 = m.Select(p => p.Field<string>("str8")).First(),
            //          str9 = m.Select(p => p.Field<string>("str9")).First(),
            //          str10 = m.Select(p => p.Field<string>("str10")).First(),
            //          str11 = m.Select(p => p.Field<string>("str11")).First(),
            //          str12 = m.Select(p => p.Field<string>("str12")).First(),
            //          str13 = m.Select(p => p.Field<string>("str13")).First(),
            //          str14 = m.Select(p => p.Field<string>("str14")).First(),
            //          str15 = m.Select(p => p.Field<string>("str15")).First(),
            //          str16 = m.Select(p => p.Field<string>("str16")).First(),
            //          str17 = m.Select(p => p.Field<string>("str17")).First(),
            //          str18 = m.Select(p => p.Field<string>("str18")).First(),
            //          str19 = m.Select(p => p.Field<string>("str19")).First(),
            //          str20 = m.Select(p => p.Field<string>("str20")).First(),
            //          DateTime1 = m.Select(p => p.Field<string>("DateTime1")).First(),
            //          DateTime2 = m.Select(p => p.Field<string>("DateTime2")).First(),
            //          DateTime3 = m.Select(p => p.Field<string>("DateTime3")).First(),
            //          DateTime4 = m.Select(p => p.Field<string>("DateTime4")).First(),
            //          DateTime5 = m.Select(p => p.Field<string>("DateTime5")).First(),
            //          Int1 = m.Select(p => p.Field<string>("Int1")).First(),
            //          Int2 = m.Select(p => p.Field<string>("Int2")).First(),
            //          Int3 = m.Select(p => p.Field<string>("Int3")).First(),
            //          Int4 = m.Select(p => p.Field<string>("Int4")).First(),
            //          Int5 = m.Select(p => p.Field<string>("Int5")).First()
            //      };

            List<string> exnumber = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!exnumber.Contains(dt.Rows[i]["ExternReceiptNumber"].ToString()))
                {
                    exnumber.Add(dt.Rows[i]["ExternReceiptNumber"].ToString());
                }
            }
            DataTable dsd = new DataTable();
            dsd = dt.Clone();
            foreach (var item in exnumber)
            {
                DataTable dd = new DataTable();
                dd = dt.Clone();
                DataRow[] drs = dt.Select("ExternReceiptNumber='" + item + "'");
                foreach (DataRow dr in drs)
                {
                    dd.Rows.Add(dr.ItemArray);
                }
                for (int i = 0; i < dd.Rows.Count; i++)
                {
                    //dd.Rows[i]["linenumber"] = dd.Rows[i]["linenumber"] == "" ? returnlinenumber(i + 1) : dd.Rows[i]["linenumber"];
                    dd.Rows[i]["linenumber"] = string.IsNullOrEmpty(dd.Rows[i]["linenumber"].ObjectToString()) ? returnlinenumber(i + 1) : dd.Rows[i]["linenumber"];
                    dsd.Rows.Add(dd.Rows[i].ItemArray);
                }
            }
            return dsd;
        }

        private string returnlinenumber(int row_count)
        {
            var linnumber = "";
            if (row_count < 10)
            {
                linnumber = "0000" + row_count;
            }
            if (100 > row_count && row_count >= 10)
            {
                linnumber = "000" + row_count;
            }
            if (1000 > row_count && row_count >= 100)
            {
                linnumber = "00" + row_count;
            }
            if (row_count >= 1000)
            {
                linnumber = "0" + row_count;
            }
            return linnumber;
        }
        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="rece"></param>
        /// <returns></returns>
        public bool UpdateasnAndasnDetail(AddASNandASNDetailRequest rece)
        {
            bool istrue = false;
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    IList<ASN> result = new List<ASN>();
                    IList<ASNDetail> receiptDetail = new List<ASNDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateASNANDASNDetali", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Asn", rece.asn.Select(ASN => new WMSASNToDb(ASN)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@AsnDetali", rece.asnDetails.Select(asnDetali => new WMSASNDetailToDb(asnDetali)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.Add(new SqlParameter("@return", SqlDbType.Int));
                    cmd.Parameters["@return"].Direction = ParameterDirection.ReturnValue;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    rows = Convert.ToInt32(cmd.Parameters["@return"].Value);
                    if (rows >= 1)
                    {
                        istrue = true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return istrue;
        }

        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="rece"></param>
        /// <returns></returns>
        public bool UpdateasnAndasnDetailSF(AddASNandASNDetailRequest rece)
        {
            bool istrue = false;
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    IList<ASN> result = new List<ASN>();
                    IList<ASNDetail> receiptDetail = new List<ASNDetail>();
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_UpdateASNANDASNDetaliSF]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Asn", rece.asn.Select(ASN => new WMSASNToDb(ASN)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@AsnDetali", rece.asnDetails.Select(asnDetali => new WMSASNDetailToDb(asnDetali)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.Add(new SqlParameter("@return", SqlDbType.Int));
                    cmd.Parameters["@return"].Direction = ParameterDirection.ReturnValue;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    rows = Convert.ToInt32(cmd.Parameters["@return"].Value);
                    if (rows >= 1)
                    {
                        istrue = true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return istrue;
        }
        /// <summary>
        /// 批量转入库单操作
        /// </summary>
        /// <param name="ASNIDs"></param>
        /// <returns></returns>
        public bool InsertIntoReceiptAndReceiptDetails(string ASNIDs)
        {
            bool istrue = false;
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string s = ASNIDs.Replace("'", "");

                    SqlCommand cmd = new SqlCommand("Proc_WMS_ASNTOReceipt", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ASNIDs", s);
                    //cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.Add(new SqlParameter("@return", SqlDbType.Int));
                    cmd.Parameters["@return"].Direction = ParameterDirection.ReturnValue;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    rows = Convert.ToInt32(cmd.Parameters["@return"].Value);
                    if (rows >= 1)
                    {
                        istrue = true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return istrue;
        }

        public IEnumerable<ASN> GetASNInfo()
        {
            IEnumerable<ASN> list = new List<ASN>();
            try
            {
                DbParam[] param = new DbParam[] { };
                DataSet ds = ExecuteDataSet("Proc_WMS_GetAllASNInfo", param);
                list = ds.Tables[0].ConvertToEntityCollection<ASN>();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateMDNAkzo(string DocNumber)
        {
            try
            {
                DbParam[] param = new DbParam[] {
                new DbParam("@DocNumber",DbType.String,DocNumber,ParameterDirection.Input)
            };
                ExecuteNoQuery("UpdateMDNAkzo", param);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 查询asn异常跟踪信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="rowcounts"></param>
        /// <returns></returns>
        public IEnumerable<ASNAbnormalTracking> GetASNAbnormalList(ASNAbnormalSearchCondition search, out int rowcounts)
        {
            string where = GenASNAbnormalWhere(search);
            string sql = $@"  SELECT ROW_NUMBER() OVER (ORDER BY a.ReceiptTime desc) AS TotalCount,a.*  FROM dbo.WMS_ASNAbnormalTracking a WHERE 1=1 {where}    
                              ORDER BY a.ReceiptTime desc
                              OFFSET { search.PageIndex } ROWS FETCH NEXT {search.PageSize} ROWS ONLY";
            return this.ExecuteDataTableBySqlString(sql).ConvertToEntityCollection<ASNAbnormalTracking>(out rowcounts);
        }

        /// <summary>
        /// 异常跟踪查询条件拼接
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public string GenASNAbnormalWhere(ASNAbnormalSearchCondition search)
        {
            StringBuilder sb = new StringBuilder();
            if (search.CustomerID != 0 && search.CustomerID != null)
            {
                sb.Append(" AND a.CustomerID=").Append(search.CustomerID).Append(" ");
            }
            if (!string.IsNullOrEmpty(search.ExternReceiptNumber))
            {
                sb.Append(" AND a.ExternReceiptNumber like '%").Append(search.ExternReceiptNumber.Trim()).Append("%' ");
            }
            if (!string.IsNullOrEmpty(search.BoxNumber))
            {
                sb.Append(" AND a.BoxNumber='").Append(search.BoxNumber.Trim()).Append("' ");
            }
            if (!string.IsNullOrEmpty(search.SKU))
            {
                sb.Append(" AND a.SKU='").Append(search.SKU.Trim()).Append("' ");
            }
            if (!string.IsNullOrEmpty(search.ReasonCode))
            {
                sb.Append(" AND a.ReasonCode='").Append(search.ReasonCode.Trim()).Append("' ");
            }
            if (!string.IsNullOrEmpty(search.StartCreateTime))
            {
                sb.Append(" AND a.ReceiptTime >='").Append(DateTime.Parse(search.StartCreateTime).DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (!string.IsNullOrEmpty(search.EndCreateTime))
            {
                sb.Append(" AND a.ReceiptTime <='").Append(DateTime.Parse(search.EndCreateTime).DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 新增、修改asn异常信息
        /// </summary>
        /// <param name="Abnormal"></param>
        /// <param name="type">1.新增，2.修改</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddorUpdateASNAbnormal(ASNAbnormalTracking abnormal, int type, out string msg)
        {
            msg = "";
            try
            {
                ASNDetail asn_boxDetail = new ASNDetail();//单号加箱号查询的结果
                ASNDetail asndetail = new ASNDetail();//单号加箱号加sku查询的结果
                string sql1 = "";
                string sql2 = "";
                string sql3 = "";
                if (type == 1)
                {
                    sql1 = $@"
                        SELECT ad.ID, ad.ASNID,a.ASNNumber,ad.ExternReceiptNumber,ad.CustomerID,ad.CustomerName,ad.LineNumber,ad.SKU,ad.BoxNumber,ad.QtyExpected
                         ,ad.QtyReceived,ad.QtyReceipt,ad.GoodsName,ad.str2,a.str3 ,p.str9,p.str10
                         FROM dbo.WMS_ASNDetail  ad
                         LEFT JOIN dbo.WMS_ASN a ON a.ID=ad.ASNID
                         LEFT JOIN dbo.WMS_Product p on p.StorerID=ad.CustomerID AND p.SKU=ad.SKU AND p.Str8='01'
                         WHERE a.Status!=-1 AND a.CustomerID={abnormal.CustomerID} AND  a.ExternReceiptNumber='{abnormal.ExternReceiptNumber}' AND ad.SKU='{abnormal.SKU}' AND ad.str2='{abnormal.BoxNumber}' ";
                    asndetail = this.ExecuteDataTableBySqlString(sql1).ConvertToEntity<ASNDetail>();


                    if (abnormal.ReasonCode == "多货")
                    {
                        if (asndetail == null)
                        {
                            //不存在，再通过单号加箱号查询一下
                            sql2 = $@" SELECT TOP 1 ad.ID, ad.ASNID,a.ASNNumber,ad.ExternReceiptNumber,ad.CustomerID,ad.CustomerName,ad.LineNumber,ad.SKU,ad.BoxNumber,ad.QtyExpected
                                                   ,ad.QtyReceived,ad.QtyReceipt,ad.GoodsName,ad.str2,a.str3 ,p.str9,p.str10
                                                   FROM dbo.WMS_ASNDetail  ad
                                                   LEFT JOIN dbo.WMS_ASN a ON a.ID=ad.ASNID
                                                   LEFT JOIN dbo.WMS_Product p on p.StorerID=ad.CustomerID AND p.SKU=ad.SKU AND p.Str8='01'
                                                   WHERE a.Status!=-1 AND a.CustomerID={abnormal.CustomerID} AND 
                                    a.ExternReceiptNumber='{abnormal.ExternReceiptNumber}'  AND ad.str2='{abnormal.BoxNumber}'";
                            asn_boxDetail = this.ExecuteDataTableBySqlString(sql2).ConvertToEntity<ASNDetail>();
                            if (asn_boxDetail == null)
                            {
                                msg = "未找到订单，请检查单号，箱号是否正确！";
                                return false;
                            }
                            //新增多货的情况
                            sql3 = $@"
                                    INSERT INTO dbo.WMS_ASNAbnormalTracking
                                       (ASNID ,ADID , ASNNumber , ExternReceiptNumber , CustomerID , CustomerName , BoxNo , SKU , UPC , BoxNumber ,Article , Size ,
                                       QtyExpected , QtyReceived , QtyDiff , ReasonCode , Remark , Creator , CreateTime , Updator , UpdateTime , Registrant , ReceiptTime ,
                                       StorerKey , FreeStatus , Location , ClaimNumber , FreeNumber , SurveyResult , QtyAdj , QtyAdjResult , AdjTime , SendITTime , ITReplyTime,DateTime1 )
                                       Values({asn_boxDetail.ASNID},null,'{asn_boxDetail.ASNNumber}','{asn_boxDetail.ExternReceiptNumber}','{asn_boxDetail.CustomerID}','{asn_boxDetail.CustomerName}',
                                           '{abnormal.BoxNo}','{abnormal.SKU}',null,'{asn_boxDetail.str2}',
                                           (SELECT TOP 1 str10 FROM dbo.WMS_Product p WHERE p.StorerID='{asn_boxDetail.CustomerID}' AND p.SKU='{abnormal.SKU}' AND p.Str8='01' ),
                                           (SELECT TOP 1 str9 FROM dbo.WMS_Product p WHERE p.StorerID='{asn_boxDetail.CustomerID}' AND p.SKU='{abnormal.SKU}' AND p.Str8='01' ),
                                           0,'{abnormal.QtyReceived}','{abnormal.QtyReceived - 0}','{abnormal.ReasonCode}','{abnormal.Remark}',
                                           '{abnormal.Creator}',getdate(),null,null,'{abnormal.Registrant}',{abnormal.ReceiptTime.ObjectToString2()},'{asn_boxDetail.str3}','{abnormal.FreeStatus}',
                                           '{abnormal.Location}','{abnormal.ClaimNumber}','{abnormal.FreeNumber}','{abnormal.SurveyResult}',{abnormal.QtyAdj.ObjectToString2()},{abnormal.QtyAdjResult.ObjectToString2()},
                                           {abnormal.AdjTime.ObjectToString2()},{abnormal.SendITTime.ObjectToString2()},{abnormal.ITReplyTime.ObjectToString2()},{abnormal.DateTime1.ObjectToString2()})";
                        }
                        else
                        {
                            //这种是属于订单1件，货到了2件的情况
                            //直接安排呗
                            sql3 = $@"
                                   INSERT INTO dbo.WMS_ASNAbnormalTracking
                                      ( ASNID ,ADID , ASNNumber , ExternReceiptNumber , CustomerID , CustomerName , BoxNo , SKU , UPC , BoxNumber ,Article , Size ,
                                        QtyExpected , QtyReceived , QtyDiff , ReasonCode , Remark , Creator , CreateTime , Updator , UpdateTime , Registrant , ReceiptTime ,
                                        StorerKey , FreeStatus , Location , ClaimNumber , FreeNumber , SurveyResult , QtyAdj , QtyAdjResult , AdjTime , SendITTime , ITReplyTime,DateTime1 )
                                   VALUES({asndetail.ASNID},{asndetail.ID},'{asndetail.ASNNumber}','{asndetail.ExternReceiptNumber}','{asndetail.CustomerID}','{asndetail.CustomerName}',
                                            '{abnormal.BoxNo}','{asndetail.SKU}',null,'{asndetail.str2}','{asndetail.str10}','{asndetail.str9}',
                                            '{asndetail.QtyExpected}','{abnormal.QtyReceived}',abs('{ asndetail.QtyExpected.ObjectToInt32() - abnormal.QtyReceived.ObjectToInt32()}'),'{abnormal.ReasonCode}','{abnormal.Remark}',
                                            '{abnormal.Creator}',getdate(),null,null,'{abnormal.Registrant}',{abnormal.ReceiptTime.ObjectToString2()},'{asndetail.str3}','{abnormal.FreeStatus}',
                                            '{abnormal.Location}','{abnormal.ClaimNumber}','{abnormal.FreeNumber}','{abnormal.SurveyResult}',{abnormal.QtyAdj.ObjectToString2()},{abnormal.QtyAdjResult.ObjectToString2()},
                                            {abnormal.AdjTime.ObjectToString2()},{abnormal.SendITTime.ObjectToString2()},{abnormal.ITReplyTime.ObjectToString2()},{abnormal.DateTime1.ObjectToString2()})";
                        }
                    }
                    else//少货和破损的
                    {
                        if (asndetail == null)
                        {
                            msg = "未找到订单，请检查单号，箱号，SKU是否正确！";
                            return false;
                        }
                        //新增
                        sql3 = $@"
                                   INSERT INTO dbo.WMS_ASNAbnormalTracking
                                      ( ASNID ,ADID , ASNNumber , ExternReceiptNumber , CustomerID , CustomerName , BoxNo , SKU , UPC , BoxNumber ,Article , Size ,
                                        QtyExpected , QtyReceived , QtyDiff , ReasonCode , Remark , Creator , CreateTime , Updator , UpdateTime , Registrant , ReceiptTime ,
                                        StorerKey , FreeStatus , Location , ClaimNumber , FreeNumber , SurveyResult , QtyAdj , QtyAdjResult , AdjTime , SendITTime , ITReplyTime,DateTime1 )
                                   VALUES({asndetail.ASNID},{asndetail.ID},'{asndetail.ASNNumber}','{asndetail.ExternReceiptNumber}','{asndetail.CustomerID}','{asndetail.CustomerName}',
                                            '{abnormal.BoxNo}','{asndetail.SKU}',null,'{asndetail.str2}','{asndetail.str10}','{asndetail.str9}',
                                            '{asndetail.QtyExpected}','{abnormal.QtyReceived}',abs('{ asndetail.QtyExpected.ObjectToInt32() - abnormal.QtyReceived.ObjectToInt32()}'),'{abnormal.ReasonCode}','{abnormal.Remark}',
                                            '{abnormal.Creator}',getdate(),null,null,'{abnormal.Registrant}',{abnormal.ReceiptTime.ObjectToString2()},'{asndetail.str3}','{abnormal.FreeStatus}',
                                            '{abnormal.Location}','{abnormal.ClaimNumber}','{abnormal.FreeNumber}','{abnormal.SurveyResult}',{abnormal.QtyAdj.ObjectToString2()},{abnormal.QtyAdjResult.ObjectToString2()},
                                            {abnormal.AdjTime.ObjectToString2()},{abnormal.SendITTime.ObjectToString2()},{abnormal.ITReplyTime.ObjectToString2()},{abnormal.DateTime1.ObjectToString2()})";
                    }
                }
                else//修改的
                {
                    //不管多货少货更新时不取数据库的   
                    sql3 = $@" UPDATE dbo.WMS_ASNAbnormalTracking SET Updator='{abnormal.Creator}',UpdateTime=getdate(), Remark='{abnormal.Remark}',ReceiptTime={abnormal.ReceiptTime.ObjectToString2()},FreeStatus='{abnormal.FreeStatus}',
                                    Location='{abnormal.Location}',ClaimNumber='{abnormal.ClaimNumber}',FreeNumber='{abnormal.FreeNumber}',SurveyResult='{abnormal.SurveyResult}',
                                    QtyAdj={abnormal.QtyAdj.ObjectToString2()},QtyAdjResult={abnormal.QtyAdjResult.ObjectToString2()},AdjTime={abnormal.AdjTime.ObjectToString2()},SendITTime={abnormal.SendITTime.ObjectToString2()},
                                    ITReplyTime={abnormal.ITReplyTime.ObjectToString2()},DateTime1={abnormal.DateTime1.ObjectToString2()}
                                    WHERE ID={abnormal.ID} ";
                }
                return this.ScanExecuteNonQueryBySql(sql3) > 0;

                #region 那就这样吧
                //多货有两种情况
                //1.比如订单3件，发来了4件  2.订单中这一箱没有这个sku，但是实物有
                //判断订单和箱号是否在系统中存在
                //ASNDetail asn_boxDetail = new ASNDetail();
                //ASNDetail asndetail = new ASNDetail();

                //string sql1 = "";
                //string sql2 = "";
                //sql2 = $@"
                //    SELECT ad.ID, ad.ASNID,a.ASNNumber,ad.ExternReceiptNumber,ad.CustomerID,ad.CustomerName,ad.LineNumber,ad.SKU,ad.BoxNumber,ad.QtyExpected
                //     ,ad.QtyReceived,ad.QtyReceipt,ad.GoodsName,ad.str2,a.str3 ,p.str9,p.str10
                //     FROM dbo.WMS_ASNDetail  ad
                //     LEFT JOIN dbo.WMS_ASN a ON a.ID=ad.ASNID
                //     LEFT JOIN dbo.WMS_Product p on p.StorerID=ad.CustomerID AND p.SKU=ad.SKU
                //     WHERE a.Status!=-1 AND a.CustomerID={abnormal.CustomerID} AND  a.ExternReceiptNumber='{abnormal.ExternReceiptNumber}' AND ad.SKU='{abnormal.SKU}' AND ad.str2='{abnormal.BoxNumber}' ";
                //asndetail = this.ExecuteDataTableBySqlString(sql1).ConvertToEntity<ASNDetail>();

                //if (abnormal.ReasonCode != "多货")//少货和破损
                //{
                //    if (asndetail == null)
                //    {
                //        msg = "未找到订单，请检查单号，箱号，SKU是否正确！";
                //        return false;
                //    }
                //}
                //else
                //{
                //    if (asndetail == null)
                //    {
                //        sql1 = $@" SELECT TOP 1 ad.ID, ad.ASNID,a.ASNNumber,ad.ExternReceiptNumber,ad.CustomerID,ad.CustomerName,ad.LineNumber,ad.SKU,ad.BoxNumber,ad.QtyExpected
                //                       ,ad.QtyReceived,ad.QtyReceipt,ad.GoodsName,ad.str2,a.str3 ,p.str9,p.str10
                //                       FROM dbo.WMS_ASNDetail  ad
                //                       LEFT JOIN dbo.WMS_ASN a ON a.ID=ad.ASNID
                //                       LEFT JOIN dbo.WMS_Product p on p.StorerID=ad.CustomerID AND p.SKU=ad.SKU
                //                       WHERE a.Status!=-1 AND a.CustomerID={abnormal.CustomerID} AND 
                //        a.ExternReceiptNumber='{abnormal.ExternReceiptNumber}'  AND ad.str2='{abnormal.BoxNumber}'";
                //        asn_boxDetail = this.ExecuteDataTableBySqlString(sql1).ConvertToEntity<ASNDetail>();
                //        if (asn_boxDetail == null)
                //        {
                //            msg = "未找到订单，请检查单号，箱号是否正确！";
                //            return false;
                //        }
                //    }
                //}
                ////新增
                //string sql3 = "";
                //if (type == 1)
                //{
                //    if (abnormal.ReasonCode == "多货")
                //    {
                //        sql3 = $@"
                //           INSERT INTO dbo.WMS_ASNAbnormalTracking
                //              ( ASNID ,ADID , ASNNumber , ExternReceiptNumber , CustomerID , CustomerName , BoxNo , SKU , UPC , BoxNumber ,Article , Size ,
                //       QtyExpected , QtyReceived , QtyDiff , ReasonCode , Remark , Creator , CreateTime , Updator , UpdateTime , Registrant , ReceiptTime ,
                //       StorerKey , FreeStatus , Location , ClaimNumber , FreeNumber , SurveyResult , QtyAdj , QtyAdjResult , AdjTime , SendITTime , ITReplyTime )
                //            VALUES({asndetail.ASNID},{asndetail.ID},'{asndetail.ASNNumber}','{asndetail.ExternReceiptNumber}','{asndetail.CustomerID}','{asndetail.CustomerName}',
                //                    '{abnormal.BoxNo}','{asndetail.SKU}',null,'{asndetail.str2}','{asndetail.str10}','{asndetail.str9}',
                //                    '{asndetail.QtyExpected}','{abnormal.QtyReceived}','{ asndetail.QtyExpected.ObjectToInt32() - abnormal.QtyReceived.ObjectToInt32()}','{abnormal.ReasonCode}','{abnormal.Remark}',
                //                    '{abnormal.Creator}',getdate(),null,null,'{abnormal.Registrant}',{abnormal.ReceiptTime.ObjectToString2()},'{asndetail.str3}','{abnormal.FreeStatus}',
                //                    '{abnormal.Location}','{abnormal.ClaimNumber}','{abnormal.FreeNumber}','{abnormal.SurveyResult}',{abnormal.QtyAdj.ObjectToString2()},{abnormal.QtyAdjResult.ObjectToString2()},
                //                    {abnormal.AdjTime.ObjectToString2()},{abnormal.SendITTime.ObjectToString2()},{abnormal.ITReplyTime.ObjectToString2()})";

                //    }
                //    else
                //    {
                //        sql3 = $@"
                //           INSERT INTO dbo.WMS_ASNAbnormalTracking
                //              ( ASNID ,ADID , ASNNumber , ExternReceiptNumber , CustomerID , CustomerName , BoxNo , SKU , UPC , BoxNumber ,Article , Size ,
                //       QtyExpected , QtyReceived , QtyDiff , ReasonCode , Remark , Creator , CreateTime , Updator , UpdateTime , Registrant , ReceiptTime ,
                //       StorerKey , FreeStatus , Location , ClaimNumber , FreeNumber , SurveyResult , QtyAdj , QtyAdjResult , AdjTime , SendITTime , ITReplyTime )
                //            VALUES({asndetail.ASNID},{asndetail.ID},'{asndetail.ASNNumber}','{asndetail.ExternReceiptNumber}','{asndetail.CustomerID}','{asndetail.CustomerName}',
                //                    '{abnormal.BoxNo}','{asndetail.SKU}',null,'{asndetail.str2}','{asndetail.str10}','{asndetail.str9}',
                //                    '{asndetail.QtyExpected}','{abnormal.QtyReceived}','{ asndetail.QtyExpected.ObjectToInt32() - abnormal.QtyReceived.ObjectToInt32()}','{abnormal.ReasonCode}','{abnormal.Remark}',
                //                    '{abnormal.Creator}',getdate(),null,null,'{abnormal.Registrant}',{abnormal.ReceiptTime.ObjectToString2()},'{asndetail.str3}','{abnormal.FreeStatus}',
                //                    '{abnormal.Location}','{abnormal.ClaimNumber}','{abnormal.FreeNumber}','{abnormal.SurveyResult}',{abnormal.QtyAdj.ObjectToString2()},{abnormal.QtyAdjResult.ObjectToString2()},
                //                    {abnormal.AdjTime.ObjectToString2()},{abnormal.SendITTime.ObjectToString2()},{abnormal.ITReplyTime.ObjectToString2()})";

                //    }
                //}
                //else
                //{
                //    //不管多货少货更新时不取数据库的   
                //    sql3 = $@" UPDATE dbo.WMS_ASNAbnormalTracking SET Updator='{abnormal.Creator}',UpdateTime=getdate(), Remark='{abnormal.Remark}',ReceiptTime={abnormal.ReceiptTime.ObjectToString2()},FreeStatus='{abnormal.FreeStatus}',
                //                Location='{abnormal.Location}',ClaimNumber='{abnormal.ClaimNumber}',FreeNumber='{abnormal.FreeNumber}',SurveyResult='{abnormal.SurveyResult}',
                //                QtyAdj={abnormal.QtyAdj.ObjectToString2()},QtyAdjResult={abnormal.QtyAdjResult.ObjectToString2()},AdjTime={abnormal.AdjTime.ObjectToString2()},SendITTime={abnormal.SendITTime.ObjectToString2()},ITReplyTime={abnormal.ITReplyTime.ObjectToString2()} 
                //                WHERE ID={abnormal.ID} ";
                //}
                //return this.ScanExecuteNonQueryBySql(sql3) > 0;
                #endregion
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 导出asn异常跟踪信息
        /// </summary>         
        /// <returns></returns>
        public IEnumerable<ASNAbnormalTracking> ExportASNAbnormal(ASNAbnormalSearchCondition search)
        {
            string where = GenASNAbnormalWhere(search);
            string sql = $@" SELECT * FROM dbo.WMS_ASNAbnormalTracking a WHERE 1=1 {where} ORDER BY a.ReceiptTime desc";
            return this.ExecuteDataTableBySqlString(sql).ConvertToEntityCollection<ASNAbnormalTracking>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool DeleteASNAbnormal(long ID)
        {
            string sql = "DELETE FROM dbo.WMS_ASNAbnormalTracking WHERE ID=" + ID;
            return this.ScanExecuteNonQuery(sql) > 0;
        }

        //NIKE退货仓-查询箱号
        public GetASNNewBoxLabelByConditionResponse GetASNNewBoxLabelByCondition(ASNNewBoxLabelSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetASNNewBoxLabelByConditionResponse response = new GetASNNewBoxLabelByConditionResponse();
            string sqlWhere = this.GetASNNewBoxLabelWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetASNNewBoxLabelByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.asnBoxCollection = ds.Tables[0].ConvertToEntityCollection<ASNNewBoxLabel>();
            return response;
        }

        /// <summary>
        /// NIKE退货仓-查询箱号条件拼接
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private string GetASNNewBoxLabelWhere(ASNNewBoxLabelSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (SearchCondition.CustomerID != 0 && SearchCondition.CustomerID != null)
            {
                sb.Append(" AND a.CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (SearchCondition.WarehouseID != 0 && SearchCondition.WarehouseID != null)
            {
                sb.Append(" AND a.WarehouseID=").Append(SearchCondition.WarehouseID).Append(" ");
            }
            if (SearchCondition.BoxNumber != "" && SearchCondition.BoxNumber != null)
            {
                sb.Append(" AND a.BoxNumber='").Append(SearchCondition.BoxNumber.Trim()).Append("' ");
            }
            return sb.ToString();
        }
        //NIKE退货仓-
        public int Insertnewbox(string customerid, string ExternReceiptNumber, int total, string warehouseid, string GoodsType)
        {
            int s = 0;
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@CustomerID",DbType.String,customerid,ParameterDirection.Input),
                    new  DbParam("@ExternReceiptNumber",DbType.String,ExternReceiptNumber,ParameterDirection.Input),
                    new  DbParam("@WarehouseID",DbType.String,warehouseid,ParameterDirection.Input),
                    new  DbParam("@Total",DbType.Int64,total,ParameterDirection.Input),
                    new  DbParam("@GoodsType",DbType.String,GoodsType,ParameterDirection.Input),
                };

                DataTable dt = this.ExecuteDataTable("Proc_WMS_Insertnewbox", dbPatams);//Proc_WMS_Insertnewbox
                s = int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return s;
        }
        //NIKE退货仓-
        public IEnumerable<ASNNewBoxLabel> GetPrintASNNewBoxLabel(string ids, int type)
        {
            //try
            //{
            //    DbParam[] dbPatams = new DbParam[] {
            //    new  DbParam("@IDS",DbType.String,ids,ParameterDirection.Input),
            //    new  DbParam("@Type",DbType.Int32,type,ParameterDirection.Input),
            //    };
            //    return ExecuteDataTable("Proc_WMS_GetPrintASNNewBoxLabel", dbPatams).ConvertToEntityCollection<ASNNewBoxLabel>();
            //}
            //catch (Exception)
            //{
            //    return null;
            //}
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                try
                {

                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_GetPrintASNNewBoxLabel]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", ids.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                    cmd.CommandTimeout = 300;
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    return ds.Tables[0].ConvertToEntityCollection<ASNNewBoxLabel>();

                }
                catch (Exception e)
                {
                    return null;

                }

            }



        }
        //NIKE退货仓-
        public string GetLocationLabelBySKU(string AsnNumber, string ScanSKU, string GoodsType)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                new  DbParam("@AsnNumber",DbType.String,AsnNumber,ParameterDirection.Input),
                new  DbParam("@ScanSKU",DbType.String,ScanSKU,ParameterDirection.Input),
                new  DbParam("@GoodsType",DbType.String,GoodsType,ParameterDirection.Input)
                };
                return ExecuteDataTable("Proc_WMS_GetLocationLabelBySKU", dbPatams).ConvertToEntity<LocationInfo>().Location;
            }
            catch (Exception)
            {
                return null;
            }


        }

        public IEnumerable<ASNDetailLocation> GetLocationLabelByASNNumber(string AsnNumber)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                new  DbParam("@AsnNumber",DbType.String,AsnNumber,ParameterDirection.Input)
                };
                return ExecuteDataTable("Proc_WMS_GetLocationLabelByAsnNumber", dbPatams).ConvertToEntityCollection<ASNDetailLocation>();
            }
            catch (Exception)
            {
                return null;
            }


        }
        //NIKE退货仓-
        public int UDnewboxPrintedTimes(string customerid, string warehouseid, string boxids)
        {
            int s = 0;
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@CustomerID",DbType.String,customerid,ParameterDirection.Input),
                    new  DbParam("@WarehouseID",DbType.String,warehouseid,ParameterDirection.Input),
                    new  DbParam("@boxids",DbType.String,boxids,ParameterDirection.Input)
                };

                DataTable dt = this.ExecuteDataTable("Proc_WMS_UDnewboxPrintedTimes", dbPatams);//Proc_WMS_Insertnewbox
                s = int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return s;
        }

        public int ExternKeyCheck_TH(string pon, string ern, string plno, string flag, long CustomerID)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                int message = 0;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ExternKeyCheck_TH", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Pon", pon);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Ern", ern);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@PLNO", plno);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@flag", flag);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[4].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[5].SqlDbType = SqlDbType.Int;
                    cmd.Parameters[5].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ObjectToInt32();
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
