using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Inventory;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.Entity.DataBaseEntity;
namespace Runbow.TWS.Dao.WMS
{
    public class AdjustmentManagementAccessor : BaseAccessor
    {
        /// <summary>
        /// 库存变更数据查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public GetAdjustmentDetailByConditionResponse GetadjustByCondition(AdjustmentSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetAdjustmentDetailByConditionResponse response = new GetAdjustmentDetailByConditionResponse();
            string sqlWhere = this.GenGetAdjustWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetAdjustmentByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.AdjustmentCollection = ds.Tables[0].ConvertToEntityCollection<Adjustment>();
            //response.AdjustmentDetailCollection = ds.Tables[1].ConvertToEntityCollection<AdjustmentDetail>();
            return response;
        }

        /// <summary>
        /// 导出库存变更数据
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public GetAdjustmentDetailByConditionResponse ExportadjustByCondition(AdjustmentSearchCondition SearchCondition)
        {
            GetAdjustmentDetailByConditionResponse response = new GetAdjustmentDetailByConditionResponse();
            string sqlWhere = this.GenGetAdjustWhere(SearchCondition);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_ExportAdjustmentByCondition", dbParams);
            response.AdjustmentCollection = ds.Tables[0].ConvertToEntityCollection<Adjustment>();
            response.AdjustmentDetailCollection = ds.Tables[1].ConvertToEntityCollection<AdjustmentDetail>();
            return response;
        }



        private string GenGetAdjustWhere(AdjustmentSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.AdjustmentNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.AdjustmentNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.AdjustmentNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.AdjustmentNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.AdjustmentNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and a.AdjustmentNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and a.AdjustmentNumber  like '%" + SearchCondition.AdjustmentNumber.Trim() + "%' ");
                }
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND a.CustomerID = ").Append(SearchCondition.CustomerID);
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
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND a.Warehouse in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.Warehouse.Trim()).Append(")) ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.AdjustmentType))
            {
                sb.Append(" AND a.AdjustmentType='").Append(SearchCondition.AdjustmentType).Append("' ");
            }
            if (SearchCondition.Status != 0)
            {
                sb.Append(" AND a.Status='").Append(SearchCondition.Status).Append("' ");
            }
            if (SearchCondition.StartAdjustmentDate != null)
            {
                sb.Append(" AND a.AdjustmentTime >='").Append(SearchCondition.StartAdjustmentDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndAdjustmentDate != null)
            {
                sb.Append(" AND a.AdjustmentTime <='").Append(SearchCondition.EndAdjustmentDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.IsHold != 0)
            {
                sb.Append(" AND a.IsHold ='").Append(SearchCondition.IsHold).Append("' ");
            }
            //if (SearchCondition.AdjustmentReason != null)
            //{
            //    sb.Append(" and a.AdjustmentReason  like '%" + SearchCondition.AdjustmentReason.Trim() + "%' ");
            //}
            if (!string.IsNullOrEmpty(SearchCondition.AdjustmentReason))
            {
                sb.Append(" AND a.AdjustmentReason='").Append(SearchCondition.AdjustmentReason).Append("' ");
            }
            //备注
            if (!string.IsNullOrEmpty(SearchCondition.Remark))
            {
                sb.Append(" and a.Remark  like '%" + SearchCondition.Remark.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str1))
            {
                sb.Append(" AND a.str1='").Append(SearchCondition.str1).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str2))
            {
                sb.Append(" AND a.str2='").Append(SearchCondition.str2).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {
                sb.Append(" AND a.str3='").Append(SearchCondition.str3).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str4))
            {
                sb.Append(" AND a.str4='").Append(SearchCondition.str4).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str5))
            {
                sb.Append(" AND a.str5='").Append(SearchCondition.str5).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str6))
            {
                sb.Append(" AND a.str6='").Append(SearchCondition.str6).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str7))
            {
                sb.Append(" AND a.str7='").Append(SearchCondition.str7).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str8))
            {
                sb.Append(" AND a.str8=").Append(SearchCondition.str8).Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str9))
            {
                sb.Append(" AND a.str9='").Append(SearchCondition.str9).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str10))
            {
                sb.Append(" AND a.str10='").Append(SearchCondition.str10).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str11))
            {
                sb.Append(" AND a.str11='").Append(SearchCondition.str11).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str12))
            {
                sb.Append(" AND a.str12='").Append(SearchCondition.str12).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str13))
            {
                sb.Append(" AND a.str13='").Append(SearchCondition.str13).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str14))
            {
                sb.Append(" AND a.str14='").Append(SearchCondition.str14).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str15))
            {
                sb.Append(" AND a.str15='").Append(SearchCondition.str15).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str16))
            {
                sb.Append(" AND a.str16='").Append(SearchCondition.str16).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str17))
            {
                sb.Append(" AND a.str17='").Append(SearchCondition.str17).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str18))
            {
                sb.Append(" AND b.SKU='").Append(SearchCondition.str18).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str19))
            {
                sb.Append(" AND b.FromArea=(SELECT TOP 1 AreaName FROM dbo.WMS_Warehouse_Area WHERE ID=" + SearchCondition.str19 + ")").Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str20))
            {
                sb.Append(" AND b.ToLocation=(SELECT TOP 1 Location FROM  dbo.WMS_Warehouse_Location  WHERE Location='" + SearchCondition.str20 + "')").Append(" ");
            }
            if (SearchCondition.Int1 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int1.ToString())))
            {
                sb.Append(" AND a.Int1=").Append(SearchCondition.Int1).Append(" ");
            }
            if (SearchCondition.Int2 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int2.ToString())))
            {
                sb.Append(" AND a.Int2=").Append(SearchCondition.Int2).Append(" ");
            }
            if (SearchCondition.Int3 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int3.ToString())))
            {
                sb.Append(" AND a.Int3=").Append(SearchCondition.Int3).Append(" ");
            }
            if (SearchCondition.Int4 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int4.ToString())))
            {
                sb.Append(" AND a.Int4=").Append(SearchCondition.Int4).Append(" ");
            }
            if (SearchCondition.Int5 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int5.ToString())))
            {
                sb.Append(" AND a.Int5=").Append(SearchCondition.Int5).Append(" ");
            }
            return sb.ToString();
        }

        //点击新增  查看  编辑
        public AdjustmentAndAdjustmentDetail GetAdjustmentInfos(int ID)
        {
            AdjustmentAndAdjustmentDetail request = new AdjustmentAndAdjustmentDetail();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int32,ID, ParameterDirection.Input),
            };
            DataSet ds = base.ExecuteDataSet("Proc_WMS_GetAdjustmentInfos", dbParams);
            request.adjustment = ds.Tables[0].ConvertToEntity<Adjustment>();
            request.adjustmentDetails = ds.Tables[1].ConvertToEntityCollection<AdjustmentDetail>();
            return request;
        }
        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="ASNNumber"></param>
        /// <returns></returns>
        public bool Cancel(int id)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@ID",DbType.Int32,id,ParameterDirection.Input),
                };
                base.ExecuteNoQuery("Proc_WMS_CancelByID", dbPatams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //单条完成
        public string Complets(int id, string type)
        {
            //using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            //{
            //    string message = "";
            //    try
            //    {
            //        IList<Adjustment> result = new List<Adjustment>();
            //        IList<AdjustmentDetail> receiptDetail = new List<AdjustmentDetail>();
            //        SqlCommand cmd = new SqlCommand("Proc_WMS_CompletsByID", conn);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@ID", id);
            //        cmd.Parameters[0].SqlDbType = SqlDbType.Int;
            //        cmd.Parameters.AddWithValue("@type", type);
            //        cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
            //        cmd.Parameters.AddWithValue("@message", message);
            //        cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
            //        cmd.Parameters[2].Direction = ParameterDirection.Output;
            //        cmd.Parameters[2].Size = 500;
            //        cmd.CommandTimeout = 300;
            //        conn.Open();

            //        DataSet ds = new DataSet();
            //        SqlDataAdapter sda = new SqlDataAdapter();
            //        sda.SelectCommand = cmd;
            //        sda.Fill(ds);
            //        message = sda.SelectCommand.Parameters["@message"].Value.ToString();
            //        conn.Close();
            //        return message;
            //    }
            //    catch (Exception ex)
            //    {
            //        return message + "(" + ex.Message + ")";
            //    }
            //}


            var sql = this.ExecuteDataTableBySqlString(@"select * from WMS_Adjustment where id=" + id).ConvertToEntityCollection<Adjustment>();



            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Adjustment> result = new List<Adjustment>();
                    IList<AdjustmentDetail> receiptdetail = new List<AdjustmentDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_wms_updateandinsertinventory_" + sql.FirstOrDefault().CustomerID, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@Inventorytype", type);
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
        /// <summary>
        /// 批量取消操作
        /// </summary>
        /// <param name="asnnumberlist"></param>
        /// <returns></returns>
        public bool Cancels(string asnid)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@IDs",DbType.String,asnid,ParameterDirection.Input),
                };
                base.ExecuteNoQuery("Proc_WMS_CancelsByIDs", dbPatams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //批量完成操作
        public string PLComplet(string ID, string type)
        {
            //using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            //{
            //    string message = "";
            //    try
            //    {
            //        IList<Adjustment> result = new List<Adjustment>();
            //        IList<AdjustmentDetail> receiptDetail = new List<AdjustmentDetail>();
            //        SqlCommand cmd = new SqlCommand("Proc_WMS_PLCompletByIDs", conn);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@IDs", ID);
            //        cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
            //        cmd.Parameters.AddWithValue("@type", type);
            //        cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
            //        cmd.Parameters.AddWithValue("@message", message);
            //        cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
            //        cmd.Parameters[2].Direction = ParameterDirection.Output;
            //        cmd.Parameters[2].Size = 500;
            //        cmd.CommandTimeout = 300;
            //        conn.Open();

            //        DataSet ds = new DataSet();
            //        SqlDataAdapter sda = new SqlDataAdapter();
            //        sda.SelectCommand = cmd;
            //        sda.Fill(ds);
            //        message = sda.SelectCommand.Parameters["@message"].Value.ToString();
            //        conn.Close();
            //        return message;
            //    }
            //    catch (Exception ex)
            //    {
            //        return message + "(" + ex.Message + ")";
            //    }
            //}

            var sql = this.ExecuteDataTableBySqlString(@"select * from WMS_Adjustment where id in (" + ID + ")").ConvertToEntityCollection<Adjustment>();

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    foreach (var item in sql)
                    {

                        IList<Adjustment> result = new List<Adjustment>();
                        IList<AdjustmentDetail> receiptDetail = new List<AdjustmentDetail>();
                        SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateAndInsertInventory_" + item.CustomerID, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", item.ID);
                        cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                        cmd.Parameters.AddWithValue("@Inventorytype", type);
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
                    }
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }

        }
        //解冻
        public bool Unfreeze(int ID)
        {
            try
            {
                DbParam[] dbPatams = new DbParam[] {
                    new  DbParam("@ID",DbType.String,ID,ParameterDirection.Input),
                };
                base.ExecuteNoQuery("Proc_WMS_Unfreeze", dbPatams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //库位检索
        public IEnumerable<LocationInfo> GetLocationList()
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, "", ParameterDirection.Input),
            };

            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetLocationList", dbParams);
            return dt.ConvertToEntityCollection<LocationInfo>();
        }
        //根据库位查询sku  货品等级  货品描述  可调整库存  调整数量  调整等级
        public IEnumerable<Inventorys> GetInventoryLocationList(string location, string warehouse, string Customer,string StoreCode)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Location", DbType.String, location, ParameterDirection.Input),
                new DbParam("@warehouse", DbType.String, warehouse, ParameterDirection.Input) ,
            new DbParam("@customerid", DbType.String, Customer, ParameterDirection.Input),
            new DbParam("@StoreCode", DbType.String, StoreCode, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventoryLocationList", dbParams);
            return dt.ConvertToEntityCollection<Inventorys>();
        }
        /// <summary>
        /// 暂存订单及明细
        /// </summary>                           
        public string AddAdjustmentANDAdjustmentDetail(AddAdjustmentandAdjustmentDetailRequest rece)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Adjustment> result = new List<Adjustment>();
                    IList<AdjustmentDetail> receiptDetail = new List<AdjustmentDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddAdjustmentANDAdjustmentDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Adjustment", rece.adjustment.Select(adjustment => new WMSAdjutmentToDb(adjustment)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@AdjustmentDetail", rece.adjustmentDetails.Select(adjustmentDetails => new WMSAdjutmentDetailToDb(adjustmentDetails)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 50;
                    cmd.Parameters.AddWithValue("@AdID", rece.AdID);
                    cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
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
        /// <summary>
        /// 验证调整数据是否合法
        /// </summary>
        /// <param name="rece"></param>
        /// <returns></returns>
        public string CheckAdjustData(AddAdjustmentandAdjustmentDetailRequest rece)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    var a = rece.adjustment.Select(adjustment => new WMSAdjutmentToDb(adjustment));
                    IList<Adjustment> result = new List<Adjustment>();
                    IList<AdjustmentDetail> receiptDetail = new List<AdjustmentDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_CheckAdjustData", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@Adjustment", rece.adjustment.Select(adjustment => new WMSAdjutmentToDb(adjustment)));
                    cmd.Parameters.AddWithValue("@Adjustment", a);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@AdjustmentDetail", rece.adjustmentDetails.Select(adjustmentDetails => new WMSAdjutmentDetailToDb(adjustmentDetails)));
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
        //修改暂存主子表   
        public string UpdateAdjustmentANDAdjustmentDetail(AddAdjustmentandAdjustmentDetailRequest rece)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Adjustment> result = new List<Adjustment>();
                    IList<AdjustmentDetail> receiptDetail = new List<AdjustmentDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateAdjustmentANDAdjustmentDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Adjustment", rece.adjustment.Select(adjustment => new WMSAdjutmentToDb(adjustment)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@AdjustmentDetail", rece.adjustmentDetails.Select(adjustmentDetails => new WMSAdjutmentDetailToDb(adjustmentDetails)));
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
        /// <summary>
        /// 提交操作
        /// </summary>
        /// <param name="rece"></param>
        /// <returns></returns>
        public string UpdateAndInsertInventory(AddAdjustmentandAdjustmentDetailRequest rece, string Inventorytype)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Inventorys> result = new List<Inventorys>();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateAndInsertInventory", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdjustmentDetail", rece.adjustmentDetails.Select(inventory => new WMSAdjutmentDetailToDb(inventory)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 50;
                    cmd.Parameters.AddWithValue("@Inventorytype", Inventorytype);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Input;
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
    }
}
