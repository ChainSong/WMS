using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.SettlementManagement;
using Runbow.TWS.MessageContracts;
using System.Runtime.InteropServices;

namespace Runbow.TWS.Dao
{
    public class SettlementAccessor : BaseAccessor
    {
        public IEnumerable<Settlement> GetSettlement(SettlementSearchCondition wc, out int rowCount)
        {
            #region 查询条件
            StringBuilder sb = new StringBuilder(); //普通结算
            StringBuilder sbbd = new StringBuilder(); //变动结算
            int receipt = 0;
            int order = 0;
            int move = 0;
            int adjust = 0;

            if (!string.IsNullOrEmpty(wc.CustomerID.ToString().Trim()))
            {
                sb.Append(" AND a.CustomerID='").Append(wc.CustomerID.ToString().Trim()).Append("' ");
            }
            if (!string.IsNullOrEmpty(wc.WarehouseID.ToString().Trim()))
            {
                sb.Append(" AND ww.WarehouseID='").Append(wc.WarehouseID.ToString().Trim()).Append("' ");
            }
            
            #endregion
            string sqlWhere = sb.ToString();
            int tempRowCount = 0;

            DataTable dt = new DataTable();
            if (wc.Type == 5)
            {
                //差异结算
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@Receipt", DbType.Int32,receipt, ParameterDirection.Input),
                new DbParam("@Order", DbType.Int32,order, ParameterDirection.Input),
                new DbParam("@Move", DbType.Int32, move, ParameterDirection.Input),
                new DbParam("@Adjust", DbType.Int32, adjust, ParameterDirection.Input),
                new DbParam("@BeginTime", DbType.String, wc.StartSettlementdate, ParameterDirection.Input),
                new DbParam("@EndTime", DbType.String, wc.EndSettlementdate, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                dt = this.ExecuteDataTable("Proc_WMS_GetSettlementByCondition5", dbParams);
                rowCount = (int)dbParams[1].Value;

            }
            else
            {
                //条件结算
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                dt = this.ExecuteDataTable("Proc_WMS_GetSettlementByConditions", dbParams);
                rowCount = (int)dbParams[1].Value;
            }


            return dt.ConvertToEntityCollection<Settlement>();
        }

        public IEnumerable<SettlementDetail> GetSettlementNew(SettlementSearchCondition wc, out int rowCount)
        {
            #region 查询条件
            StringBuilder sb = new StringBuilder(); 
            if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
            {
                sb.Append(" AND c.CustomerID=").Append(wc.CustomerID.ToString()).Append(" ");
            }
            if (!string.IsNullOrEmpty(wc.WarehouseID.ToString()))
            {
                sb.Append(" AND c.WarehouseID=").Append(wc.WarehouseID.ToString()).Append(" ");
            }
            if (wc.CompleteDate != null)
            {
                sb.Append(" AND c.CompleteDate >='").Append(wc.CompleteDate.DateTimeToString("d")).Append(" 00:00:00'");
            }
            if (wc.StartCompleteDate != null)
            {
                sb.Append(" AND c.CompleteDate >='").Append(wc.StartCompleteDate.DateTimeToString("d")).Append(" 00:00:00'");//开始时间没取到
            }
            if (wc.EndCompleteDate != null)
            {
                sb.Append(" AND c.CompleteDate <='").Append(wc.EndCompleteDate.DateTimeToString("d")).Append(" 23:59:59'");//结束时间没取到
            }
            #endregion
            string sqlWhere = sb.ToString();
            int tempRowCount = 0;
            DataTable dt = new DataTable();
            //查询出库单
            DbParam[] dbParams = new DbParam[]{
            new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            new DbParam("@PageIndex", DbType.String, wc.PageIndex, ParameterDirection.Input),
            new DbParam("@PageSize", DbType.String, wc.PageSize, ParameterDirection.Input),
            new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                };
            dt = this.ExecuteDataTable("Proc_WMS_GetSettlementOrderByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<SettlementDetail>();
        }

        public string GetSettlementSave(SettlementSearchCondition wc)
        {
            string Mess = string.Empty;
            try
            {
                //保存结算主子表
                string Roles = string.Empty;
                DataTable dt = new DataTable();
                #region 查询条件
                StringBuilder sb = new StringBuilder(); 
                if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
                {
                    sb.Append(" AND c.CustomerID=").Append(wc.CustomerID.ToString()).Append(" ");
                }
                if (!string.IsNullOrEmpty(wc.WarehouseID.ToString()))
                {
                    sb.Append(" AND c.WarehouseID=").Append(wc.WarehouseID.ToString()).Append(" ");
                }
                if (wc.CompleteDate != null)
                {
                    sb.Append(" AND c.CompleteDate >='").Append(wc.CompleteDate.DateTimeToString("d")).Append(" 00:00:00'");
                }
                if (wc.StartCompleteDate != null)
                {
                    sb.Append(" AND c.CompleteDate >='").Append(wc.StartCompleteDate.DateTimeToString("d")).Append(" 00:00:00'");//开始时间没取到
                }
                if (wc.EndCompleteDate != null)
                {
                    sb.Append(" AND c.CompleteDate <='").Append(wc.EndCompleteDate.DateTimeToString("d")).Append(" 23:59:59'");//结束时间没取到
                }
                #endregion
                string sqlWhere = sb.ToString();
                int tempRowCount = 0;
                DbParam[] dbParams = new DbParam[]{
                    
                    new DbParam("@SettlementNumber", DbType.String, wc.SettlementNumber, ParameterDirection.Input),
                    //new DbParam("@ExternNumber", DbType.String, wc.ExternNumber, ParameterDirection.Input),
                    new DbParam("@SettlementMonth", DbType.String,DateTime.Parse( wc.StartCompleteDate.DateTimeToString("d")).Month.ToString(), ParameterDirection.Input),//月份不对
                    new DbParam("@CustomerID", DbType.Int64, wc.CustomerID, ParameterDirection.Input),
                    new DbParam("@CustomerName", DbType.String, wc.CustomerName, ParameterDirection.Input),
                    new DbParam("@WarehouseID", DbType.Int64, wc.WarehouseID, ParameterDirection.Input),
                    new DbParam("@WarehouseName", DbType.String, wc.WarehouseName, ParameterDirection.Input),
                    new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                    new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                };
               
                //条件结算 
                dt = this.ExecuteDataTable("Proc_WMS_SettlementSave", dbParams);
                if (dt.Rows[0][0].ToString().Equals("1"))
                {
                    Mess = "1";
                }
                else
                {
                    Mess = "操作失败(" + dt.Rows[0][0].ToString() + ")";
                }
                return Mess;
            }
            catch (Exception ex)
            {
                return "操作失败(" + ex.Message + ")";
            }
        }

        public GetSettlementByConditionResponse GetSettlementPreview(SettlementSearchCondition wc)
        {
            string Mess = string.Empty;
            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            DataTable dt = new DataTable();
            try
            {
                string Roles = string.Empty;
                #region 查询条件
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
                {
                    sb.Append(" AND c.CustomerID=").Append(wc.CustomerID.ToString()).Append(" ");
                }
                if (!string.IsNullOrEmpty(wc.WarehouseID.ToString()))
                {
                    sb.Append(" AND c.WarehouseID=").Append(wc.WarehouseID.ToString()).Append(" ");
                }
                if (wc.StartCompleteDate != null)
                {
                    sb.Append(" AND c.CompleteDate >='").Append(wc.StartCompleteDate.DateTimeToString("d")).Append(" 00:00:00'");//开始时间没取到
                }
                if (wc.EndCompleteDate != null)
                {
                    sb.Append(" AND c.CompleteDate <='").Append(wc.EndCompleteDate.DateTimeToString("d")).Append(" 23:59:59'");//结束时间没取到
                }
                #endregion
                string sqlWhere = sb.ToString();
                DbParam[] dbParams = new DbParam[]{
                    new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                };
                //条件结算预览
                dt = this.ExecuteDataTable("Proc_WMS_SettlementPreview", dbParams);
                response.SettlementCollection = dt.ConvertToEntityCollection<Settlement>();
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public GetSettlementByConditionResponse GetSettlementList(SettlementSearchCondition wc, out int rowCount)
        {
            string Mess = string.Empty;
            rowCount = 0;
            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            try
            {
                //保存结算主子表
                DataTable dt = new DataTable();
                #region 查询条件
                //string sb = string.Empty;
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
                {
                    sb.Append(" and c.CustomerID =" + wc.CustomerID.ToString().Trim() + " ");
                }
                if (!string.IsNullOrEmpty(wc.WarehouseID.ToString()))
                {
                    sb.Append(" and c.WarehouseID =" + wc.WarehouseID.ToString().Trim() + " ");
                }
                if (wc.StartSettlementdate != null)
                {
                    sb.Append(" AND c.CreateTime >='").Append(wc.StartSettlementdate.DateTimeToString("d")).Append(" 00:00:00'");
                }
                if (wc.EndSettlementdate != null)
                {
                    sb.Append(" AND c.CreateTime <='").Append(wc.EndSettlementdate.DateTimeToString("d")).Append(" 23:59:59'");
                }
                if (!string.IsNullOrEmpty(wc.SettlementNumber))
                {
                    sb.Append(" and c.SettlementNumber like '%" + wc.SettlementNumber.Trim() + "%' ");
                }
                if (!string.IsNullOrEmpty(wc.ExternNumber))
                {
                    sb.Append(" and c.ExternNumber like '%" + wc.ExternNumber.Trim() + "%' ");
                }
                if (!string.IsNullOrEmpty(wc.Month))
                {
                    sb.Append(" and c.SettlementMonth = '" + wc.Month.Trim() + "' ");
                }
                //if (!string.IsNullOrEmpty(wc.WarehouseName) && wc.WarehouseName != "==请选择==")
                //{
                //sb += " and c.warehouse= " + wc.Warehouse;
                //switch (wc.Warehouse)
                //{
                //    default:
                //        break;
                //    case "20":
                //        wc.Warehouse = "NIKE-OSR广州仓";
                //        break;
                //    case "15":
                //        wc.Warehouse = "NIKE-OSR北京仓";
                //        break;
                //    case "22":
                //        wc.Warehouse = "NIKE-NFS广州仓";
                //        break;
                //    case "21":
                //        wc.Warehouse = "NIKE-NFS北京仓";
                //        break;
                //}
                //    sb.Append(" and c.WarehouseID= '").Append(wc.WarehouseID.ToString().Trim()).Append("' ");
                //}
                //if (!string.IsNullOrEmpty(wc.Area) && wc.Area != "==请选择==")
                //{
                //    //sb += " and ae.id='" + wc.Area + "'";
                //    sb.Append(" and ae.ID=" + wc.Area.Trim() + " ");
                //}
                //if (wc.Type.ToString() != "0")
                //{
                //    //sb += " and c.Type='" + wc.Type.ToString() + "'";
                //    sb.Append(" and c.Type='" + wc.Type.ToString().Trim() + "' ");
                //}
                #endregion
                int tempRowCount = 0;
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, wc.PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, wc.PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                DataSet ds = ExecuteDataSet("Proc_WMS_GetSettlementByCondition", dbParams);
                rowCount = (int)dbParams[3].Value;
                response.SettlementCollection = ds.Tables[0].ConvertToEntityCollection<Settlement>();
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public GetSettlementByConditionResponse GetSettlementListPay(SettlementSearchCondition wc, out int rowCount)
        {
            string Mess = string.Empty;
            rowCount = 0;
            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            try
            {
                //保存结算主子表
                DataTable dt = new DataTable();
                #region 查询条件
                //string sb = string.Empty;
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
                {
                    sb.Append(" and c.CustomerID =" + wc.CustomerID.ToString().Trim() + " ");
                }
                if (!string.IsNullOrEmpty(wc.WarehouseID.ToString()))
                {
                    sb.Append(" and c.WarehouseID =" + wc.WarehouseID.ToString().Trim() + " ");
                }
                if (wc.StartSettlementdate != null)
                {
                    sb.Append(" AND c.CreateTime >='").Append(wc.StartSettlementdate.DateTimeToString("d")).Append(" 00:00:00'");
                }
                if (wc.EndSettlementdate != null)
                {
                    sb.Append(" AND c.CreateTime <='").Append(wc.EndSettlementdate.DateTimeToString("d")).Append(" 23:59:59'");
                }
                if (!string.IsNullOrEmpty(wc.SettlementNumber))
                {
                    sb.Append(" and c.SettlementNumber like '%" + wc.SettlementNumber.Trim() + "%' ");
                }
                if (!string.IsNullOrEmpty(wc.ExternNumber))
                {
                    sb.Append(" and c.ExternNumber like '%" + wc.ExternNumber.Trim() + "%' ");
                }
                if (!string.IsNullOrEmpty(wc.Month))
                {
                    sb.Append(" and c.SettlementMonth = '" + wc.Month.Trim() + "' ");
                }
                //if (!string.IsNullOrEmpty(wc.WarehouseName) && wc.WarehouseName != "==请选择==")
                //{
                //sb += " and c.warehouse= " + wc.Warehouse;
                //switch (wc.Warehouse)
                //{
                //    default:
                //        break;
                //    case "20":
                //        wc.Warehouse = "NIKE-OSR广州仓";
                //        break;
                //    case "15":
                //        wc.Warehouse = "NIKE-OSR北京仓";
                //        break;
                //    case "22":
                //        wc.Warehouse = "NIKE-NFS广州仓";
                //        break;
                //    case "21":
                //        wc.Warehouse = "NIKE-NFS北京仓";
                //        break;
                //}
                //    sb.Append(" and c.WarehouseID= '").Append(wc.WarehouseID.ToString().Trim()).Append("' ");
                //}
                //if (!string.IsNullOrEmpty(wc.Area) && wc.Area != "==请选择==")
                //{
                //    //sb += " and ae.id='" + wc.Area + "'";
                //    sb.Append(" and ae.ID=" + wc.Area.Trim() + " ");
                //}
                //if (wc.Type.ToString() != "0")
                //{
                //    //sb += " and c.Type='" + wc.Type.ToString() + "'";
                //    sb.Append(" and c.Type='" + wc.Type.ToString().Trim() + "' ");
                //}
                #endregion
                int tempRowCount = 0;
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, wc.PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, wc.PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                DataSet ds = ExecuteDataSet("Proc_WMS_GetSettlementByConditionPay", dbParams);
                rowCount = (int)dbParams[3].Value;
                response.SettlementCollection = ds.Tables[0].ConvertToEntityCollection<Settlement>();
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public GetSettlementByConditionResponse GetSettlementBySettlementNumber(SettlementSearchCondition wc)
        {
            string Mess = string.Empty;

            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            try
            {
                //保存结算主子表
                DataTable dt = new DataTable();
                #region 查询条件
                string sb = string.Empty;
                if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
                {
                    sb += " and  c.CustomerID='" + wc.CustomerID.ToString().Trim() + "' ";
                }
                if (!string.IsNullOrEmpty(wc.WarehouseID.ToString()))
                {
                    sb += " and  c.WarehouseID='" + wc.WarehouseID.ToString().Trim() + "' ";
                }
                if (!string.IsNullOrEmpty(wc.SettlementNumber))
                {
                    sb += " and  c.SettlementNumber='" + wc.SettlementNumber.Trim() + "' ";
                }
                #endregion
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb, ParameterDirection.Input)
            };
                DataSet ds = this.ExecuteDataSet("Proc_WMS_GetSettlementBySettlementNumber", dbParams);

                response.SettlementCollection = ds.Tables[0].ConvertToEntityCollection<Settlement>();
                response.SettlementDetailCollection = ds.Tables[1].ConvertToEntityCollection<SettlementDetail>();
                return response;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public GetSettlementByConditionResponse ExportSettlementBySettlementNumber(SettlementSearchCondition wc)
        {
            string Mess = string.Empty;

            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            try
            {
                //保存结算主子表
                DataTable dt = new DataTable();
                #region 查询条件
                string sb = string.Empty;
                if (!string.IsNullOrEmpty(wc.SettlementNumber))
                {
                    sb += " and  SettlementNumber='" + wc.SettlementNumber.Trim() + "' ";
                }
                #endregion
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb, ParameterDirection.Input)
            };
                DataSet ds = this.ExecuteDataSet("Proc_WMS_ExportSettlementBySettlementNumber", dbParams);
                response.SettlementDetailCollection = ds.Tables[0].ConvertToEntityCollection<SettlementDetail>();
                return response;
            }
            catch (Exception)
            {
                return null;
            }

        }
        
        public string SaveSettlementBySettlementNumber(SettlementSearchCondition wc)
        {
            string Mess = string.Empty;
            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            try
            {
                //string[] qtysum = wc.ActulQtyargs;
                //if (qtysum.Length > 0)
                //{
                    string str = string.Empty;
                    str = " and  SettlementNumber='" + wc.SettlementNumber.Trim() + "' ";

                    DbParam[] dbParams = new DbParam[]{
                            new DbParam("@Where", DbType.String,str,ParameterDirection.Input)
                        };
                    DataTable dt = this.ExecuteDataSet("Proc_WMS_GetSettlementBySettlementNumber", dbParams).Tables[1];
                    //if (dt.Rows.Count == qtysum.Length)
                    //{
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DbParam[] dbUpdate = new DbParam[]{
                            //new DbParam("@ActualQty", DbType.String,qtysum[i].ToString(),ParameterDirection.Input),
                            new DbParam("@ID", DbType.Int32,int.Parse(dt.Rows[i]["ID"].ToString()),ParameterDirection.Input)};

                            this.ExecuteScalar("Proc_WMS_SaveSettlementBySettlementNumber", dbUpdate);
                        }
                    //}
                //}
                return "更新成功";
            }
            catch (Exception)
            {
                return "更新失败";
            }

        }
        
        public string GetSettlementDelete(SettlementSearchCondition wc)
        {
            string Mess = string.Empty;
            try
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.Int64,wc.CustomerID,ParameterDirection.Input),
                new DbParam("@WarehouseID", DbType.Int64,wc.WarehouseID,ParameterDirection.Input),
                new DbParam("@SettlementNumber", DbType.String,wc.SettlementNumber,ParameterDirection.Input)
            };
                this.ExecuteScalar("Proc_WMS_GetSettlementDelete", dbParams);
                return "删除成功";
            }
            catch (Exception ex)
            {
                return "删除失败";
            }

        }
        
        public string GetSettlementDone(SettlementSearchCondition wc)
        {
            string message = "";
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                conn.Open();
                SqlTransaction sqlTransaction = conn.BeginTransaction();//事物
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetSettlementDone", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SettlementNumber", wc.SettlementNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 50;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 500;
                    cmd.CommandTimeout = 30;
                    cmd.Transaction = sqlTransaction;//绑定事物
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    //conn.Close();

                    #region 推送数据
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        using (SqlConnection connn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TWSS"].ConnectionString.ToString()))
                        {
                            connn.Open();
                            try
                            {
                                string sql =string.Format("INSERT INTO dbo.CTRSettledStorage(ProjectID,PodID,SettledNumber,SettledType, " +
                                    "CustomerOrShipperID,CustomerOrShipperName ,Boxnumber ," +
                                    "TotalAmt ," +
                                    "Str1,Str2,Str3,Creator ,CreateTime ,RelatedCustomerID) " +

                                    "SELECT 136,{0},'{1}',{2}, " +
                                    "{3},'{4}',{5}," +
                                    "{6}," +
                                    "{7},'{8}','{9}','{10}','{11}',{12} "
                                    , ds.Tables[0].Rows[0]["ID"], ds.Tables[0].Rows[0]["SettlementNumber"].ToString(), ds.Tables[0].Rows[0]["SettlementType"].ToString()
                                    , ds.Tables[0].Rows[0]["CustomerID"], ds.Tables[0].Rows[0]["CustomerName"].ToString(), ds.Tables[0].Rows[0]["Qty"].ToString()
                                    , ((Double.Parse( ds.Tables[0].Rows[0]["RentalTotalBilling"].ToString())
                                    + Double.Parse(ds.Tables[0].Rows[0]["FacilityTotalBilling"].ToString())
                                    + Double.Parse(ds.Tables[0].Rows[0]["OBHandlingTotalBilling"].ToString())
                                    + Double.Parse(ds.Tables[0].Rows[0]["GOHHandlingTotalBilling"].ToString())
                                    + Double.Parse(ds.Tables[0].Rows[0]["SecurityTagTotalBilling"].ToString())))
                                    , ds.Tables[0].Rows[0]["SettlementMonth"], ds.Tables[0].Rows[0]["WarehouseName"].ToString(), ds.Tables[0].Rows[0]["SettlementDay"], ds.Tables[0].Rows[0]["Settler"].ToString(), ds.Tables[0].Rows[0]["SettlementTime"].ToString(), ds.Tables[0].Rows[0]["WarehouseID"].ToString()
                                    );
                                //"SELECT 136,ID,SettlementNumber,SettlementType, " +
                                //"CustomerID,CustomerName,Qty,(RentalTotalBilling+FacilityTotalBilling+OBHandlingTotalBilling+GOHHandlingTotalBilling+SecurityTagTotalBilling)," +
                                //"SettlementMonth,WarehouseName,Settler,SettlementTime,WarehouseID "
                                SqlCommand cmdd = new SqlCommand(sql, connn);
                                //cmdd.Transaction = sqlTransaction;//再次绑定事物
                                int rows = cmdd.ExecuteNonQuery();
                                if (rows > 0)
                                {
                                    sqlTransaction.Commit();//提交事物
                                } else
                                {
                                    message = "插入数据失败，请重试！";
                                    sqlTransaction.Rollback();//回滚事物
                                }
                            }
                            catch (Exception ex)
                            {
                                message = ex.Message;
                                sqlTransaction.Rollback();//回滚事物
                            }
                            connn.Close();
                        }
                    }
                    else
                    {
                        message = "推送数据为空，请重试！";
                        sqlTransaction.Rollback();//回滚事物
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    sqlTransaction.Rollback();//回滚事物
                }
                conn.Close();
            }

            return message;
        }

        //批量导出
        public DataSet SummaryExportSettlementBySettlementNumber_b(SettlementSearchCondition wc)
        {
            string Mess = string.Empty;

            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            try
            {
                //保存结算主子表
                DataTable dt = new DataTable();
                #region 查询条件
                string sb = string.Empty;
                if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
                {
                    sb += " and  c.CustomerID='" + wc.CustomerID.ToString().Trim() + "' ";
                }
                if (!string.IsNullOrEmpty(wc.WarehouseID.ToString()))
                {
                    sb += " and  c.WarehouseID='" + wc.WarehouseID.ToString().Trim() + "' ";
                }
                if (!string.IsNullOrEmpty(wc.SettlementNumber))
                {
                    if (wc.SettlementNumber.Contains(","))
                    {
                        sb += " and  c.SettlementNumber in (";
                        string[] strs = wc.SettlementNumber.Split(',');
                        foreach (var str in strs)
                        {
                            sb += "'" + str + "',";
                        }
                        sb = sb.Substring(0, sb.Length - 1);
                        sb += ")";
                    }
                    else
                    {
                        sb += " and  c.SettlementNumber='" + wc.SettlementNumber.Trim() + "' ";
                    }
                }
                #endregion
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb, ParameterDirection.Input)
                };
                DataSet ds = this.ExecuteDataSet("Proc_WMS_GetSettlementBySettlementNumber_Export", dbParams);
                return ds;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }


        //导出
        public DataSet ExportSettlementBySettlementNumber_b(SettlementSearchCondition wc)
        {
            string Mess = string.Empty;

            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            try
            {
                //保存结算主子表
                DataTable dt = new DataTable();
                #region 查询条件
                string sb = string.Empty;
                if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
                {
                    sb += " and  c.CustomerID='" + wc.CustomerID.ToString().Trim() + "' ";
                }
                if (!string.IsNullOrEmpty(wc.WarehouseID.ToString()))
                {
                    sb += " and  c.WarehouseID='" + wc.WarehouseID.ToString().Trim() + "' ";
                }
                if (!string.IsNullOrEmpty(wc.SettlementNumber))
                {
                    sb += " and  c.SettlementNumber='" + wc.SettlementNumber.Trim() + "' ";
                }
                #endregion
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb, ParameterDirection.Input)
                };
                DataSet ds = this.ExecuteDataSet("Proc_WMS_GetSettlementBySettlementNumber_Export", dbParams);
                return ds;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public GetSettlementByConditionResponse GetPrintSettlementBySettlementNumber(string Settlementnumber)
        {
            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();

            string str = " AND SettlementNumber = '" + Settlementnumber + "'";
            DbParam[] dbparams = new DbParam[]{
                new DbParam("@Where",DbType.String,str,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("[Proc_WMS_GetPrintSettlementBySettlementNumber]", dbparams);

            response.Settlement = ds.Tables[0].ConvertToEntity<Settlement>();
            response.SettlementDetailCollection = ds.Tables[1].ConvertToEntityCollection<SettlementDetail>();

            return response;
        }

        public GetSettlementByConditionResponse GetPrintSettlementBySettlementNumberNike(string Settlementnumber)
        {
            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();

            string str = " AND SettlementNumber = '" + Settlementnumber + "'";
            DbParam[] dbparams = new DbParam[]{
                new DbParam("@Where",DbType.String,str,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("[Proc_WMS_GetPrintSettlementBySettlementNumberNike]", dbparams);

            response.Settlement = ds.Tables[0].ConvertToEntity<Settlement>();
            response.SettlementDetailCollection = ds.Tables[1].ConvertToEntityCollection<SettlementDetail>();

            return response;

        }

        public void AddInfoHiltiNewTab(WMS_HiltibjSettled cb)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddHilti", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WarehouseID", cb.WarehouseID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@WarehouseName", cb.WarehouseName);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@SettlementNumber", cb.SettlementNumber);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@OutsourcingTotalSum", cb.OutsourcingTotalSum);
                cmd.Parameters[3].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@OutsourcingAveragecost", cb.OutsourcingAveragecost);
                cmd.Parameters[4].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@Number1", cb.Number1);
                cmd.Parameters[5].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Number2", cb.Number2);
                cmd.Parameters[6].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Number3", cb.Number3);
                cmd.Parameters[7].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Number4", cb.Number4);
                cmd.Parameters[8].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@OperationCost1", cb.OperationCost1);
                cmd.Parameters[9].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@OperationCost2", cb.OperationCost2);
                cmd.Parameters[10].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@OperationCost3", cb.OperationCost3);
                cmd.Parameters[11].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@OperationCost4", cb.OperationCost4);
                cmd.Parameters[12].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@OperationTotal", cb.OperationTotal);
                cmd.Parameters[13].SqlDbType = SqlDbType.Decimal;

                cmd.Parameters.AddWithValue("@Num1", cb.Num1);
                cmd.Parameters[14].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Num2", cb.Num2);
                cmd.Parameters[15].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Num3", cb.Num3);
                cmd.Parameters[16].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Num4", cb.Num4);
                cmd.Parameters[17].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Num5", cb.Num5);
                cmd.Parameters[18].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Num6", cb.Num6);
                cmd.Parameters[19].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Num7", cb.Num7);
                cmd.Parameters[20].SqlDbType = SqlDbType.BigInt;

                cmd.Parameters.AddWithValue("@Cost1", cb.Cost1);
                cmd.Parameters[21].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@Cost2", cb.Cost2);
                cmd.Parameters[22].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@Cost3", cb.Cost3);
                cmd.Parameters[23].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@Cost4", cb.Cost4);
                cmd.Parameters[24].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@Cost5", cb.Cost5);
                cmd.Parameters[25].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@Cost6", cb.Cost6);
                cmd.Parameters[26].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@Cost7", cb.Cost7);
                cmd.Parameters[27].SqlDbType = SqlDbType.Decimal;
                cmd.Parameters.AddWithValue("@TotalCost", cb.TotalCost);
                cmd.Parameters[28].SqlDbType = SqlDbType.Decimal;
                cmd.CommandTimeout = 2000;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            string message = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TWSS"].ConnectionString.ToString()))
            {
                conn.Open();
                //  SqlTransaction sqlTransaction = conn.BeginTransaction();//事物
                try
                {
                    if (cb.WarehouseID == 53)
                    {
                        decimal? OperationCost2 = 0;
                        decimal? OperationCost4 = 0;
                        decimal? Cost2 = 0;
                        decimal? Cost4 = 0;
                        decimal? Cost5 = 0;
                        decimal? Cost6 = 0;
                        decimal? totalcoat = 0;
                        OperationCost2=cb.OperationCost2;      //仓库收入
                        OperationCost4=cb.OperationCost4;      //入库管理费
                        Cost2=cb.Cost2;               //门店出库管理费
                        Cost4=cb.Cost4;               //门店出库理货费
                        Cost5=cb.Cost5;               //电商出货拣货费
                        Cost6=cb.Cost6;               //门店出库拣货费
                        totalcoat=cb.TotalCost;       //总费用
                        string sql = $"INSERT INTO dbo.CTRSettledStorage values(53,'" + cb.SettlementNumber.ToString() + "' ,0 ,null , null ,0 ,98 ,'HABA',null ,null,null,null ,null , null ,null ,null,null , null , null,0, null , null ,null ,null ," + OperationCost2 + " ," + OperationCost4 + " ,null , null ," + Cost2 + "," + Cost4 + "," + Cost5 + "," + Cost6 + ", 0 ," + totalcoat + ", 1, 'HABA上海仓', '"+ DateTime.Now.ToString("yyyy-MM-dd")+ "', null, null, null, null,'haba','"+ DateTime.Now.ToString("yyyy-MM-dd")+"', 0, 98, 1, 0 )";
                        SqlCommand cmdd = new SqlCommand(sql, conn);
                        int rows = cmdd.ExecuteNonQuery();
                    }
                    else
                    {

                    
                    int? number = cb.Number1 + cb.Number2;
                    decimal? total = cb.OutsourcingAveragecost + cb.OperationTotal + cb.TotalCost;
                    decimal? outsourcing = 0;
                    decimal? averagecost = 0;
                    decimal? operationtotal = 0;
                    decimal? totalcoat = 0;
                    outsourcing = cb.OutsourcingTotalSum;
                    averagecost = cb.OutsourcingAveragecost;
                    operationtotal = cb.OperationTotal;
                    totalcoat = cb.TotalCost;
                    string sql = $"INSERT INTO dbo.CTRSettledStorage values(50,'" + cb.SettlementNumber.ToString() + "' ,0 ,null , null ,0 ,96 ,'喜利得北京',null , null,null,null ,null , null ,null ,null,null , null , null," + number + ", null , null ,null ,null , 0 , 0 ,null , null ," + outsourcing + "," + averagecost + "," + operationtotal + "," + totalcoat + ", 0 ," + total + ", 1, '喜利得北京仓', '"+DateTime.Now.ToString("yyyy-MM-dd")+"', null, null, null, null,'hilti','"+DateTime.Now.ToString("yyyy-MM-dd")+"', 0, 96, 1, 0 )";
                    SqlCommand cmdd = new SqlCommand(sql, conn);
                    //cmdd.Transaction = sqlTransaction;//再次绑定事物
                    int rows = cmdd.ExecuteNonQuery();
                        //if (rows > 0)
                        //{
                        //    sqlTransaction.Commit();//提交事物
                        //}
                        //else
                        //{
                        //    message = "插入数据失败，请重试！";
                        //    sqlTransaction.Rollback();//回滚事物
                        //}
                    }
                   
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    // sqlTransaction.Rollback();//回滚事物
                }
                conn.Close();
            }

        }

        public IEnumerablerResult Count(string creatdate,int CustomerID)
        {
            string where = creatdate.Split(' ')[0];
            IEnumerablerResult response = new IEnumerablerResult();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TWS"].ConnectionString.ToString()))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("GetReAndOrCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Where", where);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                if (ds != null)
                {
                    if (CustomerID == 96)
                    {
                        response.A = ds.Tables[0].ConvertToEntityCollection<HiltiCount>();
                        response.B = ds.Tables[1].ConvertToEntityCollection<HiltiCount>();
                        return response;
                    }
                    else
                    {
                        response.C = ds.Tables[2].ConvertToEntityCollection<HabalClass>();
                        response.D = ds.Tables[3].ConvertToEntityCollection<HabalClass>();
                       
                    }
                   
                }
                return response;
            }
        }

        public GetSettlementByConditionResponse GetHiltiList(WMS_HiltibjSettled wc, DateTime? StartSettlementdate, DateTime? EndSettlementdate, string DateTime1,int Cid, out int rowCount)
        {
            string Mess = string.Empty;
            rowCount = 0;
            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            try
            {

                DataTable dt = new DataTable();
                #region 查询条件
                //string sb = string.Empty;
                StringBuilder sb = new StringBuilder();

                if (Cid !=0)
                {
                    sb.Append(" and c.WarehouseID="+Cid+"");
                }

                if (StartSettlementdate != null)
                {
                    sb.Append(" AND c.CreateTime >='").Append(StartSettlementdate.DateTimeToString("d")).Append(" 00:00:00'");
                }
                if (EndSettlementdate != null)
                {
                    sb.Append(" AND c.CreateTime <='").Append(EndSettlementdate.DateTimeToString("d")).Append(" 23:59:59'");
                }
                if (!string.IsNullOrEmpty(wc.SettlementNumber))
                {
                    sb.Append(" and c.SettlementNumber like '%" + wc.SettlementNumber.Trim() + "%' ");
                }
                if (!string.IsNullOrEmpty(DateTime1))
                {
                    sb.Append(" and c.str1 ='" + DateTime1.Trim() + "' ");
                }

                #endregion
                int tempRowCount = 0;
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, wc.PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, wc.PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                DataSet ds = ExecuteDataSet("Proc_GetHiltiByCondition", dbParams);
                rowCount = (int)dbParams[3].Value;
                response.HilSettlementCollection = ds.Tables[0].ConvertToEntityCollection<WMS_HiltibjSettled>();
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable ExportSettlement(WMS_HiltibjSettled wc, DateTime? StartSettlementdate, DateTime? EndSettlementdate, string DateTime1, out int rowCount)
        {
            string Mess = string.Empty;
            rowCount = 0;
            GetSettlementByConditionResponse response = new GetSettlementByConditionResponse();
            try
            {

                DataTable dt = new DataTable();
                #region 查询条件
                //string sb = string.Empty;
                StringBuilder sb = new StringBuilder();



                if (StartSettlementdate != null)
                {
                    sb.Append(" AND c.CreateTime >='").Append(StartSettlementdate.DateTimeToString("d")).Append(" 00:00:00'");
                }
                if (EndSettlementdate != null)
                {
                    sb.Append(" AND c.CreateTime <='").Append(EndSettlementdate.DateTimeToString("d")).Append(" 23:59:59'");
                }
                if (!string.IsNullOrEmpty(wc.SettlementNumber))
                {
                    sb.Append(" and c.SettlementNumber like '%" + wc.SettlementNumber.Trim() + "%' ");
                }
                if (!string.IsNullOrEmpty(DateTime1))
                {
                    sb.Append(" and c.str1 ='" + DateTime1.Trim() + "' ");
                }

                #endregion
                int tempRowCount = 0;
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, wc.PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, wc.PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                return base.ExecuteDataTable("Proc_GetHiltiByCondition", dbParams);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //删除
        public int DeleteHiltibjSettled(string ID)
        {
            int id = Convert.ToInt32(ID);
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql = "delete WMS_HiltibjSettled where ID=" + id;
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
