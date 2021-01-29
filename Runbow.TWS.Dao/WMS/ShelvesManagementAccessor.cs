using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.Shelves;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.Shelves;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Entity.RabbitMQ;

namespace Runbow.TWS.Dao.WMS
{
    public class ShelvesManagementAccessor : BaseAccessor
    {
        /// <summary>
        /// 查询上架信息（上架表没有上架单）
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public GetShelvesByConditionResponse GetShelves(ReceiptReceivingSearchCondition Request)
        {
            GetShelvesByConditionResponse ConditionResponse = new GetShelvesByConditionResponse();
            string sqlWhere = SearchConditionsqlWhere(Request);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@WhereSql", DbType.String,Request.RID, ParameterDirection.Input),
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetShelves", dbParams);
            ConditionResponse.storesByGetReceipt = ds.Tables[0].ConvertToEntity<StoresByGetReceipt>();
            ConditionResponse.Shelves = ds.Tables[1].ConvertToEntityCollection<Shelves>();
            return ConditionResponse;
        }
        /// <summary>
        /// 查询上架信息（上架表有上架信息）
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        //public GetShelvesByConditionResponse GetReceiptReceiving(ReceiptReceivingSearchCondition Request)
        //{
        //    GetShelvesByConditionResponse ConditionResponse = new GetShelvesByConditionResponse();
        //    string sqlWhere = SearchConditionsqlWhere(Request);
        //    DbParam[] dbParams = new DbParam[]{
        //        new DbParam("@WhereSql", DbType.String,Request.RID, ParameterDirection.Input),
        //    };
        //    DataSet ds = this.ExecuteDataSet("Proc_GetReceiptReceiving", dbParams);
        //    ConditionResponse.storesByGetReceipt = ds.Tables[0].ConvertToEntity<StoresByGetReceipt>();
        //    ConditionResponse.Shelves = ds.Tables[1].ConvertToEntityCollection<Shelves>();
        //    return ConditionResponse;
        //}
        private string SearchConditionsqlWhere(ReceiptReceivingSearchCondition Condition)
        {
            StringBuilder sb = new StringBuilder();
            if (Condition.RID != 0)
            {
                sb.Append("and r.id='" + Condition.RID + "'");
            }
            return sb.ToString();
        }

        public bool InsertReceiptReceiving(IEnumerable<ReceiptReceiving> Request, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AddReceiptReceiving]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Receipt", Request.Select(p => new ReceiptReceivingToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Execl批量上架
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public IEnumerable<ReceiptReceiving> InsertReceiptReceivingExecl(IEnumerable<ReceiptReceiving> Request, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_InsertReceiptReceivingExecl]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Receipt", Request.Select(p => new ReceiptReceivingToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].Size = 500;
                cmd.CommandTimeout = 300;//超时时间
                conn.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<ReceiptReceiving>();
                //return ds.ConvertToEntityCollection<ReceiptReceiving>(); 
            }
        }
        /// <summary>
        /// 加入库存
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool AddshelvesAndInventory(IEnumerable<ReceiptReceiving> Request, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AddshelvesAndInventory]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Receipt", Request.Select(p => new ReceiptReceivingToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 上架单列表
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        /// 
        public string AddInstructions(string IDs, string WorkStation, string ReleatedType, int Priority, string UserName)
        {
            //Request.Ids.Split(',').Select(c => new )IdsForInt64(c.ObjectToInt64()));
            string message = "";
            IEnumerable<InstructionInfo> info = null;

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddInstructions_Receipt", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ids", IDs.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@WorkStation", WorkStation);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@ReleatedType", ReleatedType);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Priority", Priority);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[5].Direction = ParameterDirection.Output;
                    cmd.Parameters[5].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    //info = dt.ConvertToEntityCollection<InstructionInfo>();

                    // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                    return message;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }

        public IEnumerable<StoresByGetReceipt> GetReceipt(GetReceiptbyCondition Request, int pageIndex, int pageSize, out int rowCount)
        {

            string WhereSql = "";
            // string WhereSqlRRTime = "";
            if (Request != null)
            {
                WhereSql = SKUSearchCondition(Request);
                //WhereSqlRRTime = SKUSearchConditionRRTime(Request);
            }
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@WhereSql", DbType.String, WhereSql, ParameterDirection.Input),
              //  new DbParam("@WhereSqlRRTime", DbType.String, WhereSql, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetReceipt]", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<StoresByGetReceipt>();
        }

        public IEnumerable<StoresByGetReceipt> GetReceiptExport(GetReceiptbyCondition Request)
        {

            string WhereSql = "";
            // string WhereSqlRRTime = "";
            if (Request != null)
            {
                WhereSql = ExportSearchCondition(Request);
                //WhereSqlRRTime = SKUSearchConditionRRTime(Request);
            }
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@WhereSql", DbType.String, WhereSql, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_CarryOutShelves]", dbParams);
            return dt.ConvertToEntityCollection<StoresByGetReceipt>();
        }

        //private string SKUSearchConditionRRTime(GetReceiptbyCondition SearchCondition)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (SearchCondition.StartShelvesTime != null)
        //    {
        //        sb.Append("and RR.CreateTime>='" + SearchCondition.StartShelvesTime + "'");
        //    }
        //    if (SearchCondition.EndShelvesTime != null)
        //    {
        //        sb.Append("and RR.CreateTime<'" + SearchCondition.EndShelvesTime.DateTimeToString() + "23:59'");
        //    }
        //    return sb.ToString();
        //}

        /// <summary>
        /// 导出上架单
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public IEnumerable<StoresByGetReceipt> GetReceiptExecl(GetReceiptbyCondition Request)
        {
            string WhereSql = "";
            if (Request != null)
            {
                WhereSql = SKUSearchCondition(Request);
            }
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@WhereSql", DbType.String, WhereSql, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetReceiptExecl]", dbParams);
            return dt.ConvertToEntityCollection<StoresByGetReceipt>();
        }

        public IEnumerable<StoresByGetReceipt> GetShelvesByIDs(string IDs)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String, IDs, ParameterDirection.Input)
            };
            DataSet ds = new DataSet();
            try
            {
                ds = this.ExecuteDataSet("Proc_WMS_CarryOutShelvesByIDs", dbParams);
            }
            catch (Exception e)
            {
                throw e;
            }
            return ds.Tables[0].ConvertToEntityCollection<StoresByGetReceipt>();
        }

        private string SKUSearchCondition(GetReceiptbyCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.CustomerIDs))
            {
                sb.Append(" and R.CustomerID in (" + SearchCondition.CustomerIDs + ")");
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
                    sb.Append(" and  R.ExternReceiptNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and R.ExternReceiptNumber  like '%" + SearchCondition.ExternReceiptNumber.Trim() + "%' ");
                }
                //  sb.Append("and R.ExternReceiptNumber='" + SearchCondition.ExternReceiptNumber+"'");
            }
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
                    sb.Append(" and R.ReceiptNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and R.ReceiptNumber  like '%" + SearchCondition.ReceiptNumber.Trim() + "%' ");
                }

            }
            if (SearchCondition.ShelvesState != null)
            {
                sb.Append("and  R.[Status]='" + SearchCondition.ShelvesState + "'");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append("and R.CreateTime>='" + SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append("and R.CreateTime<'" + SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartStorageTime != null)
            {
                sb.Append("and R.ReceiptDate>='" + SearchCondition.StartStorageTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndStorageTime != null)
            {
                sb.Append("and R.ReceiptDate<'" + SearchCondition.EndStorageTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.WarehouseID != 0)
            {
                sb.Append("and  R.WarehouseID=" + SearchCondition.WarehouseID + " ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.WarehouseIDs))
            {
                sb.Append("and  R.WarehouseID in (" + SearchCondition.WarehouseIDs + ") ");
            }
            if ((SearchCondition.CustomerID == null ? 0 : SearchCondition.CustomerID) != 0)
            {
                sb.Append("and  R.CustomerID=" + SearchCondition.CustomerID);
            }

            //门店代码   在此处添加查询条件，可以根据门店代码进行查询上架单数据
            if (SearchCondition.str3 != null)
            {
                sb.Append(" and R.str3 like '%" + SearchCondition.str3.Trim() + "%'");
            }
            return sb.ToString();
        }

        private string ExportSearchCondition(GetReceiptbyCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.CustomerIDs))
            {
                sb.Append(" and R.CustomerID in (" + SearchCondition.CustomerIDs + ")");
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
                    sb.Append(" and  R.ExternReceiptNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and R.ExternReceiptNumber  like '%" + SearchCondition.ExternReceiptNumber.Trim() + "%' ");
                }
                //  sb.Append("and R.ExternReceiptNumber='" + SearchCondition.ExternReceiptNumber+"'");
            }
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
                    sb.Append(" and R.ReceiptNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and R.ReceiptNumber  like '%" + SearchCondition.ReceiptNumber.Trim() + "%' ");
                }

            }
            if (SearchCondition.ShelvesState != null)
            {
                sb.Append("and  R.[Status]='" + SearchCondition.ShelvesState + "'");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append("and R.CreateTime>='" + SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append("and R.CreateTime<'" + SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartStorageTime != null)
            {
                sb.Append("and R.ReceiptDate>='" + SearchCondition.StartStorageTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndStorageTime != null)
            {
                sb.Append("and R.ReceiptDate<'" + SearchCondition.EndStorageTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.WarehouseID != 0)
            {
                sb.Append("and  R.WarehouseID=" + SearchCondition.WarehouseID + " ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.WarehouseIDs))
            {
                sb.Append("and  R.WarehouseID in (" + SearchCondition.WarehouseIDs + ") ");
            }
            if ((SearchCondition.CustomerID == null ? 0 : SearchCondition.CustomerID) != 0)
            {
                sb.Append("and  R.CustomerID=" + SearchCondition.CustomerID);
            }
            return sb.ToString();
        }

        public string UpdateInventoryType(string receiptnumber, string action)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string Message = "";
                SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateInventoryType_Bridge", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@receiptnumber", receiptnumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@Message", Message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                Message = sda.SelectCommand.Parameters["@Message"].Value.ToString();
                conn.Close();
                return Message;
            }
        }
        /// <summary>
        /// Nike差异加入库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public string AddInventoryWithFreeze(string id, string UserName)
        {
            try
            {
                string sql = "select * from [wms_receiptreceiving] where rid = " + id;
                var receiptreceiving = this.ExecuteDataTableBySqlString(sql).ConvertToEntityCollection<ReceiptReceiving>();
                if (receiptreceiving == null)
                {
                    return "未上架";
                }
                if (receiptreceiving.FirstOrDefault().Status == 9)
                {
                    return "订单已入库，请刷新界面";
                }
                if (receiptreceiving.Where(a => a.Location.Length < 2).Count() > 0)
                {
                    return "添加失败,库区和库位不能为空，请检查上架单";
                }
                //string WarehousId = receiptreceiving.FirstOrDefault().Warehouse;
                string CustomerId = receiptreceiving.FirstOrDefault().CustomerID.ToString();
                string AddInventory = @"DECLARE @Flag INT; 
      DECLARE @Sql VARCHAR(max); 
      DECLARE @SkuLineNumber NVARCHAR(50); 
      DECLARE @SUN INT; 
      DECLARE @NULLCount INT; 
      DECLARE @SuccessCount INT; 
	  DECLARE @Customerid  NVARCHAR(50); 
      BEGIN try 
          BEGIN TRANSACTION; 
         INSERT INTO wms_inventory_" + CustomerId + @"
                      (rrid, 
                       [receiptnumber], 
                       [warehouse], 
                       [area], 
                       [location], 
                       superid, 
                       [customerid], 
                       [customername], 
                       [sku], 
                       [goodsname], 
                       [goodstype], 
                       [qty], 
                       [inventorytype], 
                       batchnumber, 
                       [creator], 
                       [createtime], 
                       boxnumber, 
                       [unit], 
                       [specifications], 
                       datetime1, 
                       datetime2,str1) 
          (SELECT id, 
                  r2.[receiptnumber], 
                  warehouse, 
                  area, 
                  location, 
                  0,  
                  customerid, 
                  customername, 
                  r2.sku, 
                  goodsname, 
                  r2.goodstype, 
                  qtyreceived, 
                  1, 
                  batchnumber, 
                  '" + UserName + @"', 
                  Getdate(), 
                  boxnumber, 
                  [unit], 
                  [specifications], 
                  r2.datetime1, 
                  r2.datetime2 ,
                  r2.str1
           FROM   [dbo].wms_receiptreceiving r2  
           WHERE  1 = 1 
                  AND rid = " + id + @") 
	    INSERT INTO dbo.WMS_Adjustment
	    ( AdjustmentNumber ,CustomerID ,CustomerName ,Warehouse ,Status ,AdjustmentType ,AdjustmentReason ,AdjustmentTime ,IsHold ,Creator , CreateTime ,
	      Updator ,UpdateTime ,Remark)
	    SELECT ReceiptNumber,CustomerID,CustomerName,WarehouseName,1,'库存冻结单','入库差异冻结',GETDATE(),1,'system',GETDATE(),'system'
	    ,GETDATE(),Remark FROM dbo.WMS_Receipt WHERE id= " + id + @"

	    INSERT INTO dbo.WMS_AdjustmentDetail
	            ( AID ,AdjustmentNumber ,CustomerID ,CustomerName ,SKU ,UPC ,BatchNumber ,BoxNumber ,
	              GoodsName , FromWarehouse ,ToWarehouse ,FromArea ,ToArea ,FromLocation ,ToLocation ,FromQty ,
	              ToQty ,FromGoodsType ,ToGoodsType ,Unit ,Specifications ,IsHold ,AdjustmentReason ,
	              Creator ,CreateTime ,Updator ,UpdateTime  
	            )
       	    select (SELECT TOP(1) ID FROM dbo.WMS_Adjustment WHERE AdjustmentNumber=rd.receiptnumber), rd.receiptnumber,rd.CustomerID,
		    rd.CustomerName, rd.SKU,rd.UPC,rd.BatchNumber,rd.BoxNumber,rd.GoodsName,r.WarehouseName,r.WarehouseName,
		   (SELECT TOP(1)Area FROM dbo.WMS_ReceiptReceiving WHERE RDID=rd.ID),(SELECT TOP(1)Area FROM dbo.WMS_ReceiptReceiving WHERE RDID=rd.ID),
		    (SELECT TOP(1)Location FROM dbo.WMS_ReceiptReceiving WHERE RDID=rd.ID),(SELECT TOP(1)Location FROM dbo.WMS_ReceiptReceiving WHERE RDID=rd.ID), QtyExpected-ISNULL((select top 10000 sum(rr.QtyReceived) from WMS_ReceiptReceiving rr 
              WHERE rr.RID=rd.RID and rr.sku=rd.sku and rr.LineNumber=rd.LineNumber group by rr.RID,rr.sku,rr.LineNumber),0) QtyDiff,
              QtyExpected-ISNULL((select top 10000 sum(rr.QtyReceived) from WMS_ReceiptReceiving rr 
              WHERE rr.RID=rd.RID and rr.sku=rd.sku and rr.LineNumber=rd.LineNumber group by rr.RID,rr.sku,rr.LineNumber),0) QtyDiff2,'A品','A品',
              rd.Unit,rd.Specifications,1,'入库差异冻结','system',GETDATE(),'system',GETDATE()
              FROM wms_receiptdetail rd LEFT JOIN dbo.WMS_Receipt r ON r.ID=rd.RID
              LEFT JOIN dbo.WMS_Product p ON rd.SKU=p.SKU AND rd.CustomerID=p.StorerID
      where RID= " + id + @" and (isnull((select top 10000 sum(rr.QtyReceived) from WMS_ReceiptReceiving rr 
   WHERE rr.RID=rd.RID and rr.sku=rd.sku and rr.LineNumber=rd.LineNumber group by rr.RID,rr.sku,rr.LineNumber),0)-QtyExpected)<>0

   INSERT INTO dbo.WMS_Inventory_" + CustomerId + @"
           ( RRID ,ReceiptNumber ,Warehouse ,Area ,Location ,SuperID ,CustomerID ,CustomerName ,SKU ,UPC ,GoodsName ,GoodsType ,
             Qty ,InventoryType ,RelatedID ,BoxNumber ,BatchNumber ,Unit ,Specifications ,Creator ,CreateTime ,Updator ,UpdateTime ,
             str1 ,str2 ,str3 , str4 ,str5 ,str6 ,str7 ,str8 ,str9 ,str10 ,str11 ,str12 ,str13 ,str14 ,str15 ,str16 ,str17 ,str18 ,
             str19 ,str20 ,DateTime1 ,DateTime2 ,DateTime3 ,DateTime4 , DateTime5 ,Int1 ,Int2 ,Int3 ,Int4 ,Int5
           )
   select rd.id, rd.receiptnumber,r.WarehouseName,(SELECT TOP(1)Area FROM dbo.WMS_ReceiptReceiving WHERE RDID=rd.ID),(SELECT TOP(1)Location FROM dbo.WMS_ReceiptReceiving WHERE RDID=rd.ID),0,rd.CustomerID,
		    rd.CustomerName, rd.SKU,rd.UPC,rd.GoodsName,'A品',QtyExpected-ISNULL((select top 10000 sum(rr.QtyReceived) from WMS_ReceiptReceiving rr 
              WHERE rr.RID=rd.RID and rr.sku=rd.sku and rr.LineNumber=rd.LineNumber group by rr.RID,rr.sku,rr.LineNumber),0) QtyDiff,
		    3,0,rd.BoxNumber,rd.BatchNumber,rd.Unit,rd.Specifications,'system',GETDATE(),'system',GETDATE(), 
              rd.str1 ,rd.str2 ,rd.str3 , rd.str4 ,rd.str5 ,rd.str6 ,rd.str7 ,rd.str8 ,rd.str9 ,rd.str10 ,rd.str11 ,rd.str12 ,rd.str13 ,rd.str14 ,rd.str15 ,rd.str16 ,rd.str17 ,rd.str18 ,
              rd.str19 ,rd.str20 ,rd.DateTime1 ,rd.DateTime2 ,rd.DateTime3 ,rd.DateTime4 , rd.DateTime5 ,rd.Int1 ,rd.Int2 ,rd.Int3 ,rd.Int4 ,rd.Int5
              FROM wms_receiptdetail rd LEFT JOIN dbo.WMS_Receipt r ON r.ID=rd.RID
      where RID= " + id + @" and (isnull((select top 10000 sum(rr.QtyReceived) from WMS_ReceiptReceiving rr 
   WHERE rr.RID=rd.RID and rr.sku=rd.sku and rr.LineNumber=rd.LineNumber group by rr.RID,rr.sku,rr.LineNumber),0)-QtyExpected)<>0

         UPDATE dbo.WMS_ReceiptDetail SET QtyReceived=s.QtyReceived
FROM dbo.WMS_ReceiptDetail r RIGHT JOIN (SELECT sku,SUM(QtyReceived) AS QtyReceived FROM dbo.WMS_ReceiptReceiving WHERE RID= " + id + @" GROUP BY SKU) s
ON r.SKU=s.SKU WHERE  rid =  " + id + @"; 
          UPDATE wms_asndetail 
          SET    qtyreceived = Isnull(Ad.qtyreceived, 0) 
                               + Isnull(Rd.qtyreceived, 0) 
          FROM   [wms_receiptdetail] Rd 
                 LEFT JOIN wms_asndetail Ad 
                        ON Ad.asnid = Rd.asnid 
                           AND Ad.sku = Rd.sku 
                           AND Isnull(Ad.batchnumber, '') = 
                               Isnull(Rd.batchnumber, '') 
          WHERE  Rd.rid = " + id + @"; 
          UPDATE [dbo].wms_receiptreceiving 
          SET    [status] = 9 
          WHERE  1 = 1 
                 AND rid =  " + id + @"; 
          UPDATE [dbo].[wms_receipt] 
          SET    [status] = 9, 
                 completedate = Getdate() 
          WHERE  1 = 1 
                 AND id = " + id + @"; 
          SELECT @SUN = Count(*) 
          FROM   wms_asndetail 
          WHERE  asnid = (SELECT asnid 
                          FROM   wms_receipt 
                          WHERE  id = " + id + @") 
                 AND qtyreceived < qtyexpected; 
        IF ( @SUN = 0 ) 
            BEGIN 
                UPDATE wms_asn 
                SET    completedate = Getdate(), 
                       [status] = 9 
                WHERE  id = (SELECT TOP(1) asnid 
                             FROM   wms_receipt 
                             WHERE  id = " + id + @"); 
            END; 
          SELECT @SuccessCount = Count(*) 
          FROM   [dbo].[wms_inventory_" + CustomerId + @"] i 
                 LEFT JOIN wms_receiptreceiving r2 
                        ON i.rrid = r2.id 
          WHERE  r2.rid = " + id + @"; 
          COMMIT; 
      END try 
      BEGIN catch 
          IF Xact_state() <> 0 
            BEGIN 
                ROLLBACK TRANSACTION; 
            END; 
      END catch;";
                this.ScanExecuteNonQuery(AddInventory);
                string SuccessSql = @" SELECT i.ID 
            FROM[dbo].[wms_inventory_" + CustomerId + @"] i 
            LEFT JOIN wms_receiptreceiving r2 ON i.rrid = r2.id 
            WHERE r2.rid = " + id;
                var SuccessCount = this.ExecuteDataTableBySqlString(SuccessSql).ConvertToEntityCollection<ReceiptReceiving>();
                if (SuccessCount.Count() > 0)
                {
                    return "成功";
                }
                return "失败,请检查上架单";
            }
            catch (Exception e)
            {

                return "添加失败" + e.Message;
            }
        }


        public string AddInventory(string id, string UserName,string ProcName)
        {
            try
            {
                string sql = "select * from [wms_receiptreceiving] where rid = " + id;
                var receiptreceiving = this.ExecuteDataTableBySqlString(sql).ConvertToEntityCollection<ReceiptReceiving>();
                if (receiptreceiving == null)
                {
                    return "未上架";
                }
                if (receiptreceiving.FirstOrDefault().Status == 9)
                {
                    return "订单已入库，请刷新界面";
                }
                if (receiptreceiving.Where(a => a.Location.Length < 2).Count() > 0)
                {
                    return "添加失败,库区和库位不能为空，请检查上架单";
                }
                //string WarehousId = receiptreceiving.FirstOrDefault().Warehouse;
                string CustomerId = receiptreceiving.FirstOrDefault().CustomerID.ToString();

                #region 验证是否取消
                string externreceiptnumber = receiptreceiving.FirstOrDefault().ExternReceiptNumber.ToString();
                string warehouse = receiptreceiving.FirstOrDefault().Warehouse.ToString();
                string result = new DeliverConfirmAccessor().ValidOrderCancel(externreceiptnumber, Convert.ToInt64(CustomerId), ProcName, warehouse, 3);
                if(!string.IsNullOrEmpty(result))
                {
                    return "已取消，无法加入库存";
                }
                #endregion

                string AddInventory = @" DECLARE @Flag INT; 
      DECLARE @Sql VARCHAR(max); 
      DECLARE @SkuLineNumber NVARCHAR(50); 
      DECLARE @SUN INT; 
      DECLARE @NULLCount INT; 
      DECLARE @SuccessCount INT; 
	  DECLARE @Customerid  NVARCHAR(50); 
      BEGIN try 
          BEGIN TRANSACTION; 
         INSERT INTO wms_inventory_" + CustomerId + @"
                      (rrid, 
                       [receiptnumber], 
                       [warehouse], 
                       [area], 
                       [location], 
                       superid, 
                       [customerid], 
                       [customername], 
                       [sku], 
                       [goodsname], 
                       [goodstype], 
                       [qty], 
                       [inventorytype], 
                       batchnumber, 
                       [creator], 
                       [createtime], 
                       boxnumber, 
                       [unit], 
                       [specifications], 
                       datetime1, 
                       datetime2,str1,str3,str2,str4) 
          (SELECT id, 
                  r2.[receiptnumber], 
                  warehouse, 
                  area, 
                  location, 
                  0,  
                  customerid, 
                  customername, 
                  r2.sku, 
                  goodsname, 
                  case r2.goodstype when 'C品' then 'C品' else 'A品' end, 
                  qtyreceived, 
                  1, 
                  batchnumber, 
                  '" + UserName + @"', 
                  Getdate(), 
                  boxnumber, 
                  [unit], 
                  [specifications], 
                  r2.datetime1, 
                  r2.datetime2 ,
                  r2.str1,
                  (SELECT  r.str3 FROM dbo.WMS_Receipt r WHERE r.ID=" + id + @"),r2.str2,str4
           FROM   [dbo].wms_receiptreceiving r2  
           WHERE  1 = 1 
                  AND rid = " + id + @") 
          UPDATE [wms_receiptdetail] 
          SET    qtyreceived = (SELECT TOP 1 Isnull(Sum(qtyreceived), 0) 
                                FROM   wms_receiptreceiving 
                                WHERE  rdid = rd.id 
                                GROUP  BY rid) 
          FROM   [wms_receiptdetail] rd 
          WHERE  rid = " + id + @"; 
          UPDATE wms_asndetail 
          SET    qtyreceived = Isnull(Ad.qtyreceived, 0) 
                               + Isnull(Rd.qtyreceived, 0) 
          FROM   [wms_receiptdetail] Rd 
                 LEFT JOIN wms_asndetail Ad 
                        ON Ad.asnid = Rd.asnid 
                           AND Ad.sku = Rd.sku 
                           AND Ad.LineNumber=Rd.LineNumber
                           AND Isnull(Ad.batchnumber, '') = 
                               Isnull(Rd.batchnumber, '') 
          WHERE  Rd.rid = " + id + @"; 
          UPDATE [dbo].wms_receiptreceiving 
          SET    [status] = 9 
          WHERE  1 = 1 
                 AND rid = " + id + @"; 
          UPDATE [dbo].[wms_receipt] 
          SET    [status] = 9, 
                 completedate = Getdate() 
          WHERE  1 = 1 
                 AND id = " + id + @"; 
          SELECT @SUN = Count(*) 
          FROM   wms_asndetail 
          WHERE  asnid = (SELECT asnid 
                          FROM   wms_receipt 
                          WHERE  id = " + id + @") 
                 AND qtyreceived < qtyexpected; 
        IF ( @SUN = 0 ) 
            BEGIN 
                UPDATE wms_asn 
                SET    completedate = Getdate(), 
                       [status] = 9 
                WHERE  id = (SELECT TOP(1) asnid 
                             FROM   wms_receipt 
                             WHERE  id = " + id + @"); 
            END; 
          SELECT @SuccessCount = Count(*) 
          FROM   [dbo].[wms_inventory_" + CustomerId + @"] i 
                 LEFT JOIN wms_receiptreceiving r2 
                        ON i.rrid = r2.id 
          WHERE  r2.rid = " + id + @"; 
          COMMIT; 
      END try 
      BEGIN catch 
          IF Xact_state() <> 0 
            BEGIN 
                ROLLBACK TRANSACTION; 
            END; 
      END catch;";
                this.ScanExecuteNonQuery(AddInventory);
                string SuccessSql = @" SELECT i.ID 
            FROM[dbo].[wms_inventory_" + CustomerId + @"] i 
            LEFT JOIN wms_receiptreceiving r2 ON i.rrid = r2.id 
            WHERE r2.rid = " + id;
                var SuccessCount = this.ExecuteDataTableBySqlString(SuccessSql).ConvertToEntityCollection<ReceiptReceiving>();
                if (SuccessCount.Count() > 0)
                {
                    return "成功";
                }
                return "失败,请检查上架单";
            }
            catch (Exception e)
            {

                return "添加失败" + e.Message;
            }
        }
        /// <summary>
        /// 导出差异
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ReceiptDetail> CheckReceiving(string id)
        {

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@id", DbType.String, id, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetReceivingDifferent]", dbParams);
            return dt.ConvertToEntityCollection<ReceiptDetail>().ToList();
        }

        public List<ReceiptDetail> CheckRFReceiving(string id)
        {

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@id", DbType.String, id, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetReceivingDifferentRF]", dbParams);
            return dt.ConvertToEntityCollection<ReceiptDetail>().ToList();
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
                        SqlCommand cmd = new SqlCommand("[Proc_WMS_ReceiptReceivingStatusBack]", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", request.Receipts.Select(m => m.ID).FirstOrDefault().ObjectToInt64());
                        cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                        cmd.Parameters.AddWithValue("@ToStatus", ToStatus);
                        cmd.Parameters[1].SqlDbType = SqlDbType.Int;
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
                    else
                    {
                        SqlCommand cmd = new SqlCommand("[Proc_WMS_ReceiptReceivingStatusBack_Batch]", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Receipt", request.Receipts.Select(receipt => new WMSReceiptToDb(receipt)));
                        cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                        cmd.Parameters.AddWithValue("@ToStatus", ToStatus);
                        cmd.Parameters[1].SqlDbType = SqlDbType.Int;
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
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }

        }
    }
}
