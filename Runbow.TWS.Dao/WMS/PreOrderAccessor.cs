using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.WMS.PreOrders;
using Runbow.TWS.Entity.WMS;
using System.Data.SqlClient;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.MessageContracts.WMS.Order;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Entity.WMS.NIKECE.ShipRequest;

namespace Runbow.TWS.Dao.WMS
{
    public class PreOrderAccessor : BaseAccessor
    {
        /// <summary>
        /// 查询预出库单   分页查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<PreOrderSearchCondition> GetPrdOrder(PreOrderSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GetPreOrderWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@WhereSql", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetPreOrder]", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<PreOrderSearchCondition>();
        }
        /// <summary>
        /// 查询预出库单  不分页
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<PreOrderSearchCondition> GetAllPrdOrder(PreOrderSearchCondition SearchCondition)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_WMS_GetAllPreOrder", conn);//Proc_WMS_AutomatedOutbound
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PreOrderIds", SearchCondition.Ids.Select(a => new WMSPreOrderIdsToDb(a)));//这是声明一个参数 并赋值
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;  //声明第一个参数的类型  
                cmd.CommandTimeout = 300;
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);//将得到的数据 填充到DataTable中
                conn.Close();
                return dt.ConvertToEntityCollection<PreOrderSearchCondition>();
            }
            //string sqlWhere = this.GetPreOrderWhere(SearchCondition); 
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@WhereSql", DbType.String, sqlWhere, ParameterDirection.Input),
            //};
            //DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetAllPreOrder]", dbParams); 
            //return dt.ConvertToEntityCollection<PreOrderSearchCondition>();
        }
        /// <summary>
        /// 导出预出库单
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public PreOrderResponse GetPreOrderExecl(PreOrderSearchCondition SearchCondition)
        {
            PreOrderResponse re = new PreOrderResponse();
            string sqlWhere = this.GetPreOrderWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@WhereSql", DbType.String, sqlWhere, ParameterDirection.Input)//,
                //new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                //new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                //new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataSet ds = this.ExecuteDataSet("[Proc_WMS_GetPreOrderExecl]", dbParams);
            //rowCount = (int)dbParams[3].Value;
            if (ds.Tables.Count > 1)
            {
                re.SearchCondition = ds.Tables[0].ConvertToEntityCollection<PreOrderSearchCondition>();
                re.PreOd = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            }
            return re;
        }

        /// <summary>
        /// 根据勾选的ID查询导出的预出库单
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public PreOrderResponse GetPreorderByIDs(string IDs)
        {
            PreOrderResponse response = new PreOrderResponse();
            DbParam[] dbParams = new DbParam[] {
            new DbParam("IDS",DbType.String,IDs,ParameterDirection.Input)};
            DataSet ds = this.ExecuteDataSet("Proc_GetPreOrderByIDs", dbParams);
            if (ds.Tables.Count > 1)
            {
                response.SearchCondition = ds.Tables[0].ConvertToEntityCollection<PreOrderSearchCondition>();
                response.PreOd = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            }
            return response;
        }



        /// <summary>
        /// 添加查看 
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public PreOrderAndPreOrderDetail GetPrdOrder(PreOrderSearchCondition SearchCondition)
        {
            PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
            string sqlWhere = this.GetPreOrderWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, SearchCondition.ID, ParameterDirection.Input)

            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPreOrderAndDetail", dbParams);
            PreOrderAndDetail.SearchCondition = ds.Tables[0].ConvertToEntity<PreOrderSearchCondition>();
            PreOrderAndDetail.PreOd = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            return PreOrderAndDetail;
        }

        /// <summary>
        /// 添加查看 
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public int UpdateAssociateFG(IEnumerable<PreOrderDetail> details)
        {
            StringBuilder stringBuilder = new StringBuilder();


            StringBuilder GetStr = new StringBuilder();
            GetStr.Append("select * from WMS_PreOrder where str2 IN ( select top 1 str2 from WMS_PreOrder where   ExternOrderNumber = '" + details.First().ExternOrderNumber + @"')
             and OrderType = '成品出库'");
            var GetStrData = this.ScanDataTable(GetStr.ToString()).ConvertToEntity<PreOrderDetail>();

            //判断订单以及订单状态，状态未加入库存就还可以删除
            StringBuilder GetOrderStatusStr = new StringBuilder();
            GetOrderStatusStr.Append(@" select * from  WMS_ReceiptFG where [ExternReceiptNumber]='" + GetStrData.ExternOrderNumber + "' ");
            var GetOrderStatusStrData = this.ScanDataTable(GetStr.ToString()).ConvertToEntity<Receipt>();
            if (GetOrderStatusStrData != null)
            {
                if (GetOrderStatusStrData.Status == 9)
                {
                    return 9;
                }
                else
                {
                    StringBuilder deleteStr = new StringBuilder();
                    stringBuilder.Append(@" delete from WMS_ReceiptFG where  [ExternReceiptNumber]='" + GetStrData.ExternOrderNumber + "'");
                    stringBuilder.Append(@" delete from WMS_ReceiptDetailFG where  [ExternReceiptNumber]='" + GetStrData.ExternOrderNumber + "'");
                }
            }

            stringBuilder.Append(@" insert into  WMS_ReceiptFG ( [ReceiptNumber]
           ,[ExternReceiptNumber]
           ,[CustomerID]
           ,[CustomerName]
           ,[WarehouseID]
           ,[WarehouseName]
           ,[ReceiptDate]
           ,[Status]
           ,[ReceiptType]
           ,[Creator]
           ,[CreateTime]
           ,[CompleteDate]
           ,[Remark])    select top 1 PreOrderNumber,ExternOrderNumber,CustomerID,CustomerName,(select top 1 ID from WMS_Warehouse where WarehouseName=Warehouse),Warehouse,OrderTime,1,
	       '加工入库','" + details.FirstOrDefault().Creator + "',GETDATE(),GETDATE(),'' from WMS_PreOrder " +
         "  where ExternOrderNumber='" + GetStrData.ExternOrderNumber + "' ");
            int i = 0;
            foreach (var item in details)
            {
                i++;
                stringBuilder.Append(@" insert into  WMS_ReceiptDetailFG([RID]
                ,[ReceiptNumber]
                ,[ExternReceiptNumber]
                ,[CustomerID]
                ,[CustomerName]
                ,[LineNumber]
                ,[SKU]
                ,[UPC] 
                ,[GoodsName]
                ,[GoodsType]
                ,[QtyExpected]
                ,[QtyReceived]
                , WarehouseID
                ,[WarehouseName]
                ,[Area]
                ,[Location]
                ,[Creator]
                ,[CreateTime])    select (select top 1 id from WMS_ReceiptFG where ExternReceiptNumber='" + GetStrData.ExternOrderNumber + @"'
                 ),PreOrderNumber,ExternOrderNumber,CustomerID,CustomerName,'" + i.ToString().PadLeft(5, '0') + @"' [LineNumber], SKU,UPC,
	            GoodsName,GoodsType,OriginalQty," + item.OriginalQty + @",(select top 1 ID from WMS_Warehouse where WarehouseName=Warehouse),Warehouse,'' Area, '' Location,'',GETDATE()  from WMS_PreOrderDetail  	   
	            where  ExternOrderNumber='" + GetStrData.ExternOrderNumber + "'");
                if (item.DefectQty != 0)
                {
                    i++;
                    stringBuilder.Append(@" insert into  WMS_ReceiptDetailFG([RID]
                ,[ReceiptNumber]
                ,[ExternReceiptNumber]
                ,[CustomerID]
                ,[CustomerName]
                ,[LineNumber]
                ,[SKU]
                ,[UPC] 
                ,[GoodsName]
                ,[GoodsType]
                ,[QtyExpected]
                ,[QtyReceived]
                ,WarehouseID
                ,[WarehouseName]
                ,[Area]
                ,[Location]
                ,[Creator]
                ,[CreateTime])    select (select top 1 id from WMS_ReceiptFG where ExternReceiptNumber='" + GetStrData.ExternOrderNumber + @"'
                 ),PreOrderNumber,ExternOrderNumber,CustomerID,CustomerName,'" + i.ToString().PadLeft(5, '0') + @"' [LineNumber], SKU,UPC,
	            GoodsName,GoodsType,OriginalQty," + item.DefectQty + @",(select top 1 ID from WMS_Warehouse where WarehouseName=Warehouse),Warehouse,'' Area, '' Location,'" + item.Creator + @"',GETDATE()  from WMS_PreOrderDetail  	   
	            where  ExternOrderNumber='" + GetStrData.ExternOrderNumber + "'");
                }
            }

            foreach (var item in details)
            {
                //if (item.GoodsType == "A品")
                //{
                stringBuilder.Append(" update WMS_PreOrderDetail set  OriginalQty=" + item.OriginalQty + " where ExternOrderNumber='" + item.ExternOrderNumber + "'and GoodsType='A品' and SKU ='" + item.SKU + "'  ");
                //}
                stringBuilder.Append(" update WMS_ReceiptDetailFG set  QtyReceived=" + item.OriginalQty + " where ExternReceiptNumber='" + item.ExternOrderNumber + "' and GoodsType='A品' and SKU ='" + item.SKU + "'  ");
                stringBuilder.Append(" update WMS_ReceiptDetailFG set  QtyReceived=" + item.DefectQty + " where ExternReceiptNumber='" + item.ExternOrderNumber + "' and GoodsType='B品' and SKU ='" + item.SKU + "'  ");
            }
            return this.ScanExecuteNonQuery(stringBuilder.ToString());
            //PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
            //string sqlWhere = this.GetPreOrderWhere(SearchCondition);
            //int tempRowCount = 0;
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@Where", DbType.String, SearchCondition.ID, ParameterDirection.Input)

            //};
            //DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPreOrderAndDetail", dbParams);
            //PreOrderAndDetail.SearchCondition = ds.Tables[0].ConvertToEntity<PreOrderSearchCondition>();
            //PreOrderAndDetail.PreOd = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            //return PreOrderAndDetail;
        }



        public PreOrderAndPreOrderDetail GetInventoryOfOutbound(InventoryOfOutboundRequest Request)
        {
            string message = "";
            PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                try
                {

                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_GetInventoryOfOutbound]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ids", Request.Ids.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@CustomerId", Request.CustomerId);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Warehouse", Request.Warehouse);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    //cmd.Parameters.AddWithValue("@Location", Request.Location);
                    //cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    //cmd.Parameters.AddWithValue("@GoodsType", Request.GoodsType);
                    //cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 200;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();

                    PreOrderAndDetail.SearchCondition = ds.Tables[0].ConvertToEntity<PreOrderSearchCondition>();
                    PreOrderAndDetail.PreOd = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
                    // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                    return PreOrderAndDetail;
                }
                catch (Exception e)
                {
                    return PreOrderAndDetail;

                }

            }
        }

        public PreOrderAndPreOrderDetail Allocation_GetPrdOrder(PreOrderSearchCondition SearchCondition)
        {
            PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
            string sqlWhere = this.GetPreOrderWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, SearchCondition.ID, ParameterDirection.Input)

            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_AllocationGetWMS_PreOrderAndDetail", dbParams);
            PreOrderAndDetail.SearchCondition = ds.Tables[0].ConvertToEntity<PreOrderSearchCondition>();
            PreOrderAndDetail.PreOd = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();

            return PreOrderAndDetail;
        }
        /// <summary>
        /// 查询手动分页的时候的库存表中sku的数量个，和其对应的库位
        /// </summary>
        /// <param name="订单ID"></param>
        /// <returns></returns>
        public AddInventroyRequest GetPrdOrder_distributionInventory(long PREID)
        {
            AddInventroyRequest PreOrderAndDetail = new AddInventroyRequest();
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@PREID", DbType.Int64,PREID, ParameterDirection.Input)
            };
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@CustomerID", DbType.Int64, request.CustomerID, ParameterDirection.Input), 
            //    new DbParam("@SKU", DbType.String, request.SKU, ParameterDirection.Input),
            //    new DbParam("@UPC", DbType.String, request.UPC, ParameterDirection.Input), 
            //    new DbParam("@GoodsType", DbType.String, request.GoodsType, ParameterDirection.Input), 
            //    new DbParam("@BatchNumber", DbType.String, request.BatchNumber, ParameterDirection.Input), 
            //    new DbParam("@BoxNumber", DbType.String, request.BoxNumber, ParameterDirection.Input), 
            //    new DbParam("@Warehouse", DbType.String, request.Warehouse, ParameterDirection.Input), 
            //    new DbParam("@Unit", DbType.String, request.Unit, ParameterDirection.Input), 
            //    new DbParam("@Specifications", DbType.String, request.Specifications, ParameterDirection.Input)
            //};
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetPreOrder_distribution_Inventory]", dbParams);
            PreOrderAndDetail.inventorys = dt.ConvertToEntityCollection<Inventorys>();
            return PreOrderAndDetail;
        }
        /// <summary>
        /// 查询手动分页的时候的库存表中sku的数量个，和其对应的库位（根据客户配置动态调用存过）
        /// </summary>
        /// <param name="订单ID"></param>
        /// <returns></returns>
        public AddInventroyRequest GetPrdOrder_distributionInventory(long PREID, string ProcName)
        {
            AddInventroyRequest PreOrderAndDetail = new AddInventroyRequest();
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@PREID", DbType.Int64,PREID, ParameterDirection.Input)
            };
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@CustomerID", DbType.Int64, request.CustomerID, ParameterDirection.Input), 
            //    new DbParam("@SKU", DbType.String, request.SKU, ParameterDirection.Input),
            //    new DbParam("@UPC", DbType.String, request.UPC, ParameterDirection.Input), 
            //    new DbParam("@GoodsType", DbType.String, request.GoodsType, ParameterDirection.Input), 
            //    new DbParam("@BatchNumber", DbType.String, request.BatchNumber, ParameterDirection.Input), 
            //    new DbParam("@BoxNumber", DbType.String, request.BoxNumber, ParameterDirection.Input), 
            //    new DbParam("@Warehouse", DbType.String, request.Warehouse, ParameterDirection.Input), 
            //    new DbParam("@Unit", DbType.String, request.Unit, ParameterDirection.Input), 
            //    new DbParam("@Specifications", DbType.String, request.Specifications, ParameterDirection.Input)
            //};
            DataTable dt = this.ExecuteDataTable(ProcName, dbParams);
            PreOrderAndDetail.inventorys = dt.ConvertToEntityCollection<Inventorys>();
            return PreOrderAndDetail;
        }

        public IEnumerable<Inventorys> GetBatchBySKU(string SKU, long? CustomerID, string Warehouse, string BatchNumber, string GoodsType, string BoxNumber, string Unit, string Specifications, string UPC)
        {
            //int tempRowCount = 0;
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@SKU", DbType.String, SKU, ParameterDirection.Input), 
            //    new DbParam("@BatchNumber", DbType.String, BatchNumber==""?null:BatchNumber, ParameterDirection.Input), 
            //    new DbParam("@GoodsType", DbType.String, GoodsType==""?null:GoodsType, ParameterDirection.Input), 
            //    new DbParam("@CustomerID", DbType.Int32, CustomerID, ParameterDirection.Input), 
            //    new DbParam("@Warehouse", DbType.String, Warehouse, ParameterDirection.Input), 
            //    new DbParam("@BoxNumber", DbType.String, BoxNumber==""?null:BoxNumber, ParameterDirection.Input), 
            //    new DbParam("@Unit", DbType.String, Unit==""?null:Unit, ParameterDirection.Input), 
            //    new DbParam("@Specifications", DbType.String, Specifications==""?null:Specifications, ParameterDirection.Input), 
            //    new DbParam("@UPC", DbType.String, UPC==""?null:UPC, ParameterDirection.Input), 
            //};
            //DataTable dt = this.ExecuteDataTable("Proc_WMS_GetBatchBySKU", dbParams);
            //return dt.ConvertToEntityCollection<Inventorys>();

            try
            {
                string SqlStr = @"SELECT  DISTINCT i.CreateTime,i.SKU,i.GoodsName,i.GoodsType,BatchNumber,i.Unit,i.Specifications,SUM(Qty) AS InventoryQty,BoxNumber,i.UPC

             FROM dbo.WMS_Inventory_" + CustomerID.Value + @" i LEFT JOIN (SELECT * FROM dbo.WMS_Product WHERE ISNULL(str8,'')<>'02') p ON i.CustomerID = p.StorerID AND i.SKU = p.SKU
            WHERE   i.InventoryType=1 AND  isnull(i.SKU,'')=isnull('" + SKU + @"',isnull(i.SKU,''))  and CustomerID= " + CustomerID.Value;
                if (!string.IsNullOrEmpty(GoodsType))
                {

                    SqlStr += "AND i.GoodsType='" + GoodsType + "'";
                }

                if (!string.IsNullOrEmpty(BatchNumber))
                {

                    SqlStr += "AND i.BatchNumber='" + BatchNumber + "'";
                }

                if (!string.IsNullOrEmpty(Warehouse))
                {

                    SqlStr += "AND i.Warehouse='" + Warehouse + "'";
                }

                if (!string.IsNullOrEmpty(BoxNumber))
                {

                    SqlStr += "AND i.BoxNumber='" + BoxNumber + "'";
                }

                if (!string.IsNullOrEmpty(Unit))
                {

                    SqlStr += "AND i.Unit='" + Unit + "'";
                }

                if (!string.IsNullOrEmpty(Specifications))
                {

                    SqlStr += "AND i.Specifications='" + Specifications + "'";
                }
                if (!string.IsNullOrEmpty(UPC))
                {

                    SqlStr += "AND i.UPC='" + UPC + "'";
                }
                SqlStr += @"GROUP BY  i.SKU,i.GoodsName,i.GoodsType,BatchNumber,i.CreateTime,BoxNumber,i.Unit,i.Specifications ,UPC
                            ORDER BY CreateTime ";
                return this.ExecuteDataTableBySqlString(SqlStr).ConvertToEntityCollection<Inventorys>();
                //response.PageIndex = PageIndex;
                //response.PageCount = TotalCount;
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {

            }
            return null;



        }

        public IEnumerable<DistributionInformation> ManualAllocationJson(IEnumerable<PreOrderDetail> PreOrderList, long? ID, string Creator, string CustomerId, string Criterion, string SqlProc)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(SqlProc, conn);//Proc_WMS_ManualAllocation_Total
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pod", PreOrderList.Select(p => new WMSPreOrderDetailInfoToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerId);
                cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Creator", Creator);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Criterion", Criterion);
                cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[5].Direction = ParameterDirection.Output;
                cmd.Parameters[5].Size = 1000;
                cmd.CommandTimeout = 600;
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                // return message;
                return ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<DistributionInformation>();
                //}
                // catch (Exception e)
                // {

                // return message = "操作失败";
                //}

            }
        }

        public string ManualAllocationSaveJson(IEnumerable<PreOrderDetail> PreOrderList, long? ID, string Creator)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Proc_WMS_UpdatePreOrderDetali", conn);//Proc_WMS_ManualAllocation_Total
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pod", PreOrderList.Select(p => new WMSPreOrderDetailInfoToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Creator", Creator);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                cmd.Parameters[3].Size = 1000;
                cmd.CommandTimeout = 300;
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return "";
                //}
                // catch (Exception e)
                // {

                // return message = "操作失败";
                //}

            }
        }

        public IEnumerable<DistributionInformation> WorkersAlloctions(IEnumerable<PreOrderDetail> PreOrderList, long? ID, string Creator, string CustomerId, string Criterion, string SqlProc)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(SqlProc, conn);//Proc_WMS_ManualAllocation_Total
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pod", PreOrderList.Select(p => new WMSPreOrderDetailInfoToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerId);
                cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Creator", Creator);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Criterion", Criterion);
                cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[5].Direction = ParameterDirection.Output;
                cmd.Parameters[5].Size = 1000;
                cmd.CommandTimeout = 600;
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                // return message;
                return ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<DistributionInformation>();
                //}
                // catch (Exception e)
                // {

                // return message = "操作失败";
                //}

            }
        }

        //预出库查询
        private string GetPreOrderWhere(PreOrderSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.PreOrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.PreOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.PreOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.PreOrderNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.PreOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and PreOrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and PreOrderNumber  like '%" + SearchCondition.PreOrderNumber.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ExternOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ExternOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ExternOrderNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ExternOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and ExternOrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and ExternOrderNumber  like '%" + SearchCondition.ExternOrderNumber.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(SearchCondition.CustomerIDs))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.CustomerIDs.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.CustomerIDs.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.CustomerIDs.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.CustomerIDs.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and CustomerID in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("").Append(s).Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and CustomerID  = " + SearchCondition.CustomerIDs.Trim() + " ");
                }
            }

            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (SearchCondition.ID != 0)
            {
                sb.Append(" AND ID=").Append(SearchCondition.ID).Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.CustomerName))
            {
                sb.Append(" AND CustomerName in ('").Append(SearchCondition.CustomerName).Append("')");
            }
            //if (SearchCondition.WarehouseId != null)
            //{
            //    sb.Append(" AND WarehouseId=").Append(SearchCondition.WarehouseId).Append(" ");
            //}
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND Warehouse in ('").Append(SearchCondition.Warehouse).Append("')");
            }
            if (!string.IsNullOrEmpty(SearchCondition.OrderType))
            {
                sb.Append(" AND OrderType='").Append(SearchCondition.OrderType).Append("' ");
            }
            if (SearchCondition.Status != null)
            {
                sb.Append(" AND Status=").Append(SearchCondition.Status).Append(" ");
            }
            if (SearchCondition.OrderTime != null)
            {
                sb.Append(" AND OrderTime >='").Append(SearchCondition.OrderTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndOrderTime != null)
            {
                sb.Append(" AND OrderTime <'").Append(SearchCondition.EndOrderTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Province))
            {
                sb.Append(" AND Province ='").Append(SearchCondition.Province).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.City))
            {
                sb.Append(" AND City ='").Append(SearchCondition.City).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.District))
            {
                sb.Append(" and District like '%" + SearchCondition.District + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Address))
            {
                sb.Append(" and Address like '%" + SearchCondition.Address + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Consignee))
            {
                sb.Append(" and Consignee = '" + SearchCondition.Consignee + "' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExpressCompany))
            {
                sb.Append(" and ExpressCompany  like '%" + SearchCondition.ExpressCompany + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.SKUModel))
            {
                if (SearchCondition.SKUModel == "0")
                {
                    sb.Append(" and  DetailCount=1  ");
                }
                else
                {
                    sb.Append(" and  DetailCount>1 ");
                }
            }
            if (SearchCondition.CreateTime != null)
            {
                sb.Append(" and CreateTime >= '").Append(SearchCondition.CreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" and CreateTime <= '").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            //if (SearchCondition.CreateTime != null)
            //{
            //    sb.Append(" and CreateTime >= '").Append(SearchCondition.CreateTime.DateTimeToString("yyyy-MM-dd HH:mm:ss.00")).Append("' ");
            //}
            //if (SearchCondition.EndCreateTime != null)
            //{
            //    sb.Append(" and CreateTime <= '").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:59.99")).Append("' ");
            //}
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
                sb.Append(" and ( str15 like '%" + SearchCondition.str15 + "%' or str6 like '%" + SearchCondition.str15 + "%') ");
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
            if (SearchCondition.DateTime1 != null)
            {
                sb.Append(" AND DateTime1 >='").Append(SearchCondition.DateTime1.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime1 != null)
            {
                sb.Append(" AND DateTime1 <='").Append(SearchCondition.EndDateTime1.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.DateTime2 != null)
            {
                sb.Append(" AND DateTime2 >='").Append(SearchCondition.DateTime2.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime2 != null)
            {
                sb.Append(" AND DateTime2 <='").Append(SearchCondition.EndDateTime2.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.DateTime3 != null)
            {
                sb.Append(" AND DateTime3 >='").Append(SearchCondition.DateTime3.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime3 != null)
            {
                sb.Append(" AND a.DateTime3 <='").Append(SearchCondition.EndDateTime3.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.DateTime4 != null)
            {
                sb.Append(" AND DateTime4 >='").Append(SearchCondition.DateTime4.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime4 != null)
            {
                sb.Append(" AND DateTime4 <='").Append(SearchCondition.EndDateTime4.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.DateTime5 != null)
            {
                sb.Append(" AND DateTime5 >='").Append(SearchCondition.DateTime5.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndDateTime5 != null)
            {
                sb.Append(" AND DateTime5 <='").Append(SearchCondition.EndDateTime5.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.Int1 != null)
            {
                sb.Append(" AND Int1=").Append(SearchCondition.Int1).Append(" ");
            }
            if (SearchCondition.Int2 != null)
            {
                sb.Append(" AND Int2=").Append(SearchCondition.Int2).Append(" ");
            }
            if (SearchCondition.Int3 != null)
            {
                sb.Append(" AND isnull(Int3,'0')=").Append(SearchCondition.Int3).Append(" ");
            }
            if (SearchCondition.Int4 != null)
            {
                sb.Append(" AND Int4=").Append(SearchCondition.Int4).Append(" ");
            }
            if (SearchCondition.Int5 != null)
            {
                sb.Append(" AND Int5=").Append(SearchCondition.Int5).Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Model) && SearchCondition.Model == "产品")
            {
                sb.Append(" AND OrderType Like '%").Append(SearchCondition.Model).Append("%' ");
            }
            else {
                sb.Append(" AND OrderType Like '%").Append(SearchCondition.Model).Append("%' ");

            }
            return sb.ToString();
        }

        public PreOrderAndPreOrderDetail CheckOutboundOrder(string Id)
        {
            PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Id", DbType.String,Id, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_CheckOutboundOrder", dbParams);
            PreOrderAndDetail.OrderInfo = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            return PreOrderAndDetail;
        }

        public PreOrderAndPreOrderDetail AddPreOrderAndPreOrderDetail(IEnumerable<PreOrder> PreOrderList, IEnumerable<PreOrderDetail> PreDetail, string Creator)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddPreOrderANDPreOrderDetali", conn);//默认
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Po", PreOrderList.Select(p => new WMSPreOrderInfoToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Pod", PreDetail.Select(p => new WMSPreOrderDetailInfoToDb(p)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
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
                    PreOrderAndDetail.PreO = ds.Tables[ds.Tables.Count - 1].ConvertToEntity<PreOrder>();
                    //PreOrderAndDetail.PreOrderList = ds.Tables[ds.Tables.Count - 3].ConvertToEntityCollection<PreOrder>();
                    return PreOrderAndDetail;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public PreOrderAndPreOrderDetail AddPreOrderAndPreOrderDetail(IEnumerable<PreOrder> PreOrderList, IEnumerable<PreOrderDetail> PreDetail, string Creator, string OperationType)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddPreOrderANDPreOrderDetali_new", conn);//编辑
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Po", PreOrderList.Select(p => new WMSPreOrderInfoToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Pod", PreDetail.Select(p => new WMSPreOrderDetailInfoToDb(p)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
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
                    PreOrderAndDetail.PreO = ds.Tables[ds.Tables.Count - 1].ConvertToEntity<PreOrder>();
                    PreOrderAndDetail.PreOrderList = ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<PreOrder>();
                    return PreOrderAndDetail;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// /Nike-cord接口用
        /// </summary>
        /// <param name="PreOrderList"></param>
        /// <param name="PreDetail"></param>
        /// <param name="Creator"></param>
        /// <param name="connectionStr">不同的数据库地址</param>
        /// <returns></returns>
        public PreOrderAndPreOrderDetail AddPreOrderAndPreOrderDetailDynamicConn(IEnumerable<PreOrder> PreOrderList, IEnumerable<PreOrderDetail> PreDetail, string Creator, string connectionStr)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                try
                {
                    PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_AddPreOrderANDPreOrderDetali]", conn);//默认
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Po", PreOrderList.Select(p => new WMSPreOrderInfoToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Pod", PreDetail.Select(p => new WMSPreOrderDetailInfoToDb(p)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
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
                    PreOrderAndDetail.PreO = ds.Tables[ds.Tables.Count - 1].ConvertToEntity<PreOrder>();
                    PreOrderAndDetail.PreOrderList = ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<PreOrder>();
                    return PreOrderAndDetail;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public IEnumerable<PreOrder> GetWMSPreOrdersByExternNumber(IEnumerable<PreOrder> PreOrderList, string connectionStr)
        {
            string custumers = "";
            PreOrderList.Select(m => m.CustomerID).Distinct().ToList().ForEach((item) => { custumers += item + ","; });
            custumers = custumers.Substring(0, custumers.Length - 1);

            string preos = "";
            PreOrderList.Select(m => m.ExternOrderNumber).Distinct().ToList().ForEach((item) => { preos += "'" + item + "',"; });
            preos = preos.Substring(0, preos.Length - 1);


            string sql = "SELECT * FROM dbo.WMS_PreOrder p WHERE p.CustomerID IN (" + custumers + ") AND p.ExternOrderNumber IN(" + preos + ") AND p.Status!=-1";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                try
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand(sql, conn);//默认
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 300;//超时时间
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    conn.Close();
                    return ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }



        public string AddInventoryOfOutbound(IEnumerable<PreOrder> PreOrderList, IEnumerable<PreOrderDetail> PreDetail, string Creator)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_InventoryOfOutbound]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Po", PreOrderList.Select(p => new WMSPreOrderInfoToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Pod", PreDetail.Select(p => new WMSPreOrderDetailInfoToDb(p)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
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
                    ////PreOrderAndDetail.SearchCondition = ds.Tables[ds.Tables.Count - 2].ConvertToEntity<PreOrderSearchCondition>();
                    //PreOrderAndDetail.PreOd = ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<PreOrderDetail>();
                    // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                    return message;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            //rece.asnDetails = ReturnNewDt(rece.asnDetails.ToDataTable()).ConvertToEntityCollection<ASNDetail>();
            //using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            //{
            //    string message = "";
            //    try
            //    {
            //        IList<Receipt> result = new List<Receipt>();
            //        IList<ReceiptDetail> receiptDetail = new List<ReceiptDetail>();
            //        SqlCommand cmd = new SqlCommand("Proc_WMS_AddASNANDASNDetali", conn);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@Asn", rece.asn.Select(ASN => new WMSASNToDb(ASN)));
            //        cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
            //        cmd.Parameters.AddWithValue("@AsnDetali", rece.asnDetails.Select(asnDetali => new WMSASNDetailToDb(asnDetali)));
            //        cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
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
            //        //return message + "(" + ex.Message + ")";
            //        throw ex;
            //    }
            //}
        }

        public string Cancel(List<PreOrderBackStatus> preorderlist, string CustomerID, string reasonCode, string reasonRemark)// string Criterion
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_PreOrderStatusCancel]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UdtOrder", preorderlist.Select(a => new WMSPreOrderToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@reasonCode", reasonCode);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@reasonRemark", reasonRemark);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;//声明当前类型string
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                    return message;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public string OrderFinish(List<PreOrderBackStatus> preorderlist)// string Criterion
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_WMS_OrderFinish_Batch]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UdtOrder", preorderlist.Select(a => new WMSPreOrderToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;//声明当前类型string
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                    return message;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        //public string AutomaticAllocation(string ides, string UserName, string Criterion)
        //{
        //    using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
        //    {

        //        // @PreOrderIds [WMS_Udt_PreOrderIds] READONLY ,
        //        //@UserName VARCHAR(50),
        //        //@Criterion VARCHAR(50),
        //        //@Message VARCHAR(100) OUTPUT
        //        try
        //        {
        //            string message = "";
        //            DataSet ds = new DataSet();
        //            SqlCommand cmd = new SqlCommand("[[Proc_WMS_AutomatedOutbound]]", conn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@ides", ides);
        //            cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
        //            cmd.Parameters.AddWithValue("@message", message);
        //            cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;//声明当前类型string
        //            cmd.Parameters[1].Direction = ParameterDirection.Output;
        //            cmd.Parameters[1].Size = 50;
        //            cmd.CommandTimeout = 300;
        //            conn.Open();

        //            SqlDataAdapter sda = new SqlDataAdapter();
        //            sda.SelectCommand = cmd;
        //            sda.Fill(ds);
        //            message = sda.SelectCommand.Parameters["@message"].Value.ToString();
        //            conn.Close();
        //            return message;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //获取要上传的出库订单和出库订单明细
        public GetOrderAndOrderDetailByConditionResponse GetConfirmOrderAndOrderDetail(string mark)
        {
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            //string sqlWhere = strwhere;

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@mark", DbType.String, mark, ParameterDirection.Input)

            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetAkzoOrderConfirm", dbParams);

            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
        }
        //获取要上传的出库订单和出库订单明细
        public GetOrderAndOrderDetailByConditionResponse GetConfirmOrderAndOrderDetailByNikeCE(string mark)
        {
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            //string sqlWhere = strwhere;

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@mark", DbType.String, mark, ParameterDirection.Input)

            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetNikeCEOrderConfirm", dbParams);

            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();

            response.ShipRequestHeaderCollection = ds.Tables[2].ConvertToEntityCollection<WMS_ShipRequestHeader>();
            response.ShipRequestDetailCollection = ds.Tables[3].ConvertToEntityCollection<WMS_ShipRequestDetail>();
            return response;
        }
        //获取要上传的出库订单和出库订单明细
        public PreOrderAndPreOrderDetail GetPreOrderAndPreOrderDetail(string mark)
        {
            PreOrderAndPreOrderDetail response = new PreOrderAndPreOrderDetail();
            //string sqlWhere = strwhere;

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@mark", DbType.String, mark, ParameterDirection.Input)

            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetAkzoOrderConfirm", dbParams);

            response.PreOrderList = ds.Tables[0].ConvertToEntityCollection<PreOrder>();
            response.PreOrderDetailList = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            return response;
        }

        public IEnumerable<DistributionInformation> AutomaticAllocation(IEnumerable<PreOrderIds> preorderlist, string Creator, string Criterion, string SqlProc)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand(SqlProc, conn);//Proc_WMS_AutomatedOutbound     Proc_WMS_AutomatedOutbound_Total
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PreOrderIds", preorderlist.Select(a => new WMSPreOrderIdsToDb(a)));//这是声明一个参数 并赋值
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;  //声明第一个参数的类型
                    cmd.Parameters.AddWithValue("@Creator", Creator);//声明第二个参数  并赋值
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar; // 声明第二个参数的类型
                    //cmd.Parameters[1].Direction = ParameterDirection.Output; //这句话的意思是 输入  当参数不需要输出的时候不需要  这次声明
                    //cmd.Parameters[1].Size = 50;// 这是输出参数的长度   需要输入时  声明  （参数输入 不等于返回值  是out输入的）
                    cmd.Parameters.AddWithValue("@Criterion", Criterion);//声明第三个参数
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;//声明参数类型
                    cmd.Parameters.AddWithValue("@Message", message);//声明第四个参数
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;//声明参数的类型
                    cmd.Parameters[3].Direction = ParameterDirection.Output;//声明参数是输出类型
                    cmd.Parameters[3].Size = 8000;
                    cmd.CommandTimeout = 600;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);//将得到的数据 填充到DataTable中
                    message = sda.SelectCommand.Parameters["@Message"].Value.ToString();//获得数据库  out出来的参数的值 （并不是由return 而来）
                    conn.Close();
                    return ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<DistributionInformation>();
                    //if (ds.Tables.Count > 1)
                    //{
                    //    return ds.Tables[1].ConvertToEntityCollection<DistributionInformation>();
                    //}
                    //else
                    //{
                    //    return ds.Tables[0].ConvertToEntityCollection<DistributionInformation>();
                    //}

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }




        public IEnumerable<DistributionInformation> AutomaticAllocationBox(IEnumerable<PreOrderIds> preorderlist, string Creator, string Criterion, string SqlProc)
        {
            List<DistributionInformation> distributions = new List<DistributionInformation>();
            foreach (var item in preorderlist)
            {

                SqlTransaction trans = null;
                using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();
                        SqlCommand com = new SqlCommand();
                        com.Connection = conn;
                        com.Transaction = trans;

                        //查询出订单
                        DataSet ds = new DataSet();
                        com.CommandText = (@"select *,(CASE WHEN OrderType='正常出库' THEN 'AB品' WHEN OrderType='残次出库' THEN 'C品' END) cArea from  WMS_PreOrder where  ID=" + item.ID + @"
                        select * from WMS_PreOrderDetail where POID= " + item.ID);
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = com;
                        sda.Fill(ds);
                        PreOrder po = ds.Tables[0].ConvertToEntity<PreOrder>();
                        IEnumerable<PreOrderDetail> pod = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
                        if (po.Status == 9)
                        {
                            break;
                        }
                        //查询符和条件的库存数据

                        //查询出订单
                        //StringBuilder sb = new StringBuilder();
                        //sb.Append(@"");

                        DataTable dt = new DataTable();
                        com.CommandText = (@" select i.*,l.LocationType  from WMS_PreOrderDetail pod left join 
                        WMS_Inventory i
                        on pod.CustomerID=i.CustomerID
                        and pod.SKU=i.SKU
                        and pod.Warehouse=i.Warehouse
                        left join WMS_Warehouse_Location l
                        on i.Location=l.Location 
                        where  pod.POID=" + po.ID + @"
						and i.inventorytype = 1 
                        AND i.qty > 0 									   
						AND i.sku = i.sku 
						AND ( i.UPC = i.UPC OR  ISNULL(i.UPC,'')='' ) 
						AND ( i.batchnumber = i.batchnumber  OR  ISNULL(i.batchnumber,'')='' ) 
                        AND ( i.boxnumber = i.boxnumber OR ISNULL(i.boxnumber,'') ='' ) 
						AND ( i.Unit = i.Unit  OR  ISNULL(i.Unit,'')='' ) 
                        AND ( i.Specifications = i.Specifications OR ISNULL(i.Specifications,'') ='' )  ");
                        SqlDataAdapter sdai = new SqlDataAdapter();
                        sdai.SelectCommand = com;
                        sdai.Fill(dt);
                        IEnumerable<Inventorys> invs = dt.ConvertToEntityCollection<Inventorys>();

                        //比较是否满足分配条件/不满足条件就跳出
                        bool flag = false;
                        foreach (var podSKU in pod)
                        {
                            if (invs.Where(a => a.SKU == podSKU.SKU).Sum(a => a.Qty) < podSKU.OriginalQty)
                            {
                                distributions.Add(new DistributionInformation()
                                {
                                    POID = podSKU.POID,
                                    ExternOrderNumber = podSKU.ExternOrderNumber,
                                    Customer = podSKU.CustomerName,
                                    Note = "3",
                                    Type = "1",
                                    SKU = podSKU.SKU,
                                    QTY = (invs.Where(a => a.SKU == podSKU.SKU).Sum(a => a.Qty) - podSKU.OriginalQty).ToString()
                                });
                                flag = true;
                            }
                        }
                        if (flag)
                        {

                            com.CommandText = (@"   UPDATE wms_preorder
                             SET[status] = (CASE[status] WHEN 5 THEN 5  ELSE 3  END ) 
				             WHERE id =" + po.ID + @" AND Status!= 9 AND Status != -1; ");
                            trans.Commit();//执行提交事务
                            break;
                        }
                        string OrderNumber = "OR" + DateTime.Now.Ticks;


                        //拼接需要执行的Sql
                        StringBuilder sbSql = new StringBuilder();
                        //拼接主表
                        sbSql.Append(@"   DECLARE @OrderInfo TABLE 
                         ( 
                            Id                BIGINT, 
                            ordernumber       VARCHAR(50), 
                            externordernumber VARCHAR(50), 
                            poid              BIGINT, 
                            warehouse         VARCHAR(50) 
                         );  
                         DECLARE @ODId BIGINT; 
                         DECLARE @IIdTemp BIGINT; 
  INSERT INTO [wms_order] ([ordernumber], 
[poid], [preordernumber], [externordernumber], [customerid], [customername], [warehouse], [ordertype], [status], [ordertime], [province], [city], [district], [address], [consignee], 
[contact], [ismerged], [mergenumber], [expresscompany], [expressnumber], [expressstatus], [pickprintcount], [expressprintcount], [podprintcount], [otherprintcount], [detailcount], 
[creator], [createtime], [updator], [updatetime], [remark], [str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], [str11], [str12], [str13], [str14], 
[str15], [str16], [str17], [str18], [str19], [str20], [datetime1], [datetime2], [datetime3], [datetime4], [datetime5], [int1], [int2], [int3], [int4], [int5]) 
						 OUTPUT      inserted.id,     inserted.ordernumber,     inserted.[externordernumber],     inserted.poid,     inserted.warehouse 
							INTO @OrderInfo  SELECT '" + OrderNumber + @"',
p.[id], p.[preordernumber], p.[externordernumber], p.[customerid], p.[customername], p.[warehouse], p.[ordertype], 1, p.[ordertime], p.[province], p.[city], p.[district], p.[address], p.[consignee], 
p.[contact], 0, NULL,p.[expresscompany], NULL, NULL, NULL, NULL, NULL, NULL, p.[detailcount], 
'" + Creator + @"', Getdate(), p.[updator], p.[updatetime], p.[remark], p.[str1],p.[str2],p.[str3], p.[str4], p.[str5], p.[str6], p.[str7], p.[str8], p.[str9], p.[str10], p.[str11], p.[str12], p.[str13], p.[str14], 
p.[str15], p.[str16],p.[str17],p.[str18],p.[str19],p.[str20], p.[datetime1],p.[datetime2],p.[datetime3],p.[datetime4],p.[datetime5],p.[int1],p.[int2],p.[int3],p.[int4],p.[int5] 
							FROM   [wms_preorder] p 
							WHERE  p.id =" + po.ID);
                        //拼接明细
                        //声明剩余数量

                        foreach (var orderDetail in pod)
                        {
                            IEnumerable<Inventorys> scatteredinvs = invs.Where(a => a.LocationType == 2 && a.SKU == orderDetail.SKU);
                            IEnumerable<Inventorys> wholeinvs = invs.Where(a => a.LocationType == 1 && a.SKU == orderDetail.SKU);

                            //比较零拣区数量是否足够(足够  直接分配)
                            if (orderDetail.OriginalQty <= scatteredinvs.Where(a => a.SKU == orderDetail.SKU).Sum(a => a.Qty))
                            {
                                sbSql.Append(DetailSql(orderDetail, scatteredinvs, Creator));
                            }
                            else
                            {
                                //零拣区不足，到整货区取货
                                sbSql.Append(DetailSql(orderDetail, wholeinvs, scatteredinvs, Creator));
                            }

                        }
                        //完善分配数据
                        sbSql.Append(@"UPDATE wms_preorderdetail  
							SET    allocatedqty = (select ISNULL(SUM(Qty),0) from [WMS_OrderDetail] a  where a.POID=b.POID	and a.PODID = b.ID  ) from wms_preorderdetail b
							WHERE  POID =" + po.ID);

                        sbSql.Append(@"UPDATE wms_preorder   
							SET    [status] = ( CASE WHEN ( (SELECT Count(*) FROM wms_preorderdetail WHERE poid = " + po.ID + @" AND originalqty > allocatedqty) = 0 ) THEN 9 ELSE 5 END ) 
							WHERE  id = " + po.ID + @" AND Status!=9 and Status !=-1;");

                        sbSql.Append(@" UPDATE wms_order 
							SET    [detailcount] = (SELECT Count(*) FROM   wms_orderdetail WHERE  oid IN (SELECT id FROM   @OrderInfo)) 
							WHERE  id IN (SELECT id FROM   @OrderInfo) ");




                        com.CommandText = (sbSql.ToString());

                        com.ExecuteNonQuery();//执行方式自己选择

                        trans.Commit();//执行提交事务

                        distributions.Add(new DistributionInformation()
                        {
                            POID = po.ID,
                            ExternOrderNumber = po.ExternOrderNumber,
                            Customer = po.CustomerName,
                            Note = "9",
                            Type = "1"
                            //SKU = podSKU.SKU,
                            //QTY = (invs.Where(a => a.SKU == podSKU.SKU).Sum(a => a.Qty) - podSKU.OriginalQty).ToString()
                        });
                    }
                    catch (Exception ex)
                    {

                        trans.Rollback();//如果前面有异常则事务回滚
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return distributions;
        }

        private StringBuilder DetailSql(PreOrderDetail detail, IEnumerable<Inventorys> wholeinvs, IEnumerable<Inventorys> scatteredinvs, string Creator)
        {
            int NumberRemaining = 0;
            StringBuilder sb = new StringBuilder();
            //寻找最优的出库箱子
            Inventorys inv = wholeinvs.OrderBy(a => Math.Abs(a.Qty - detail.OriginalQty)).First();
            if (detail.OriginalQty > inv.Qty)
            {

                sb.Append(@"INSERT INTO [dbo].[wms_orderdetail] ([oid], [ordernumber], [externordernumber], [poid], [podid], [customerid], [customername], 
	[linenumber], [sku], [UPC], [goodsname], [goodstype], [lot], batchnumber, [warehouse], [area], [location], [qty], [picker], [picktime], 
	[confirmer], [confirmetime], [creator], [createtime], [updator], [updatetime], [remark], [str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], 
	[str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], [datetime1], [datetime2], [datetime3], [datetime4], [datetime5], 
	[int1], [int2], [int3], [int4], [int5],[BoxNumber],[Unit],[Specifications]) 
									(SELECT o.id, o.ordernumber, o.[externordernumber], o.[poid], P.[id], p.[customerid], p.[customername], 
	p.[linenumber], p.[sku], p.[UPC], p.[goodsname], p.[goodstype], 0,'" + inv.BatchNumber + @"',o.warehouse, '" + inv.Area + @"', '" + inv.Location + @"'," + inv.Qty + @", NULL, NULL, 
	NULL, NULL, '" + Creator + @"', Getdate(), p.[updator], p.[updatetime], p.[remark], p.[str1], p.[str2], p.[str3], p.[str4], p.[str5], p.[str6], p.[str7], p.[str8], p.[str9], p.[str10], 
	p.[str11], p.[str12], p.[str13], p.[str14], p.[str15], p.[str16], p.[str17], p.[str18], p.[str19], p.[str20], p.[datetime1], p.[datetime2], p.[datetime3], p.[datetime4], p.[datetime5], 
	p.[int1], p.[int2], p.[int3], p.[int4], p.[int5],'" + inv.BoxNumber + @"','" + inv.Unit + @"','" + inv.Specifications + @"'
									FROM   [dbo].[wms_preorderdetail] p 
									LEFT JOIN @OrderInfo o  ON p.poid = o.poid 
									WHERE  p.id =" + detail.ID + ");");
                sb.Append(@"SET @ODId= @@IDENTITY   UPDATE [WMS_Inventory] 
									SET    qty =0,  inventorytype = 9
									WHERE  id = " + inv.ID);
                sb.Append(@" INSERT INTO [WMS_Inventory] ([rrid], [receiptnumber], [warehouse], [area], [location], [superid], [customerid], [customername], 
	[sku], [UPC], [goodsname], [goodstype], [qty], [inventorytype], [relatedid], batchnumber, [BoxNumber],[creator], [createtime], [updator], [updatetime], 
	[str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], [str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], 
	[datetime1], [datetime2], [datetime3], [datetime4], [datetime5], [int1], [int2], [int3], [int4], [int5],[Unit],[Specifications]) 
									(SELECT [rrid], [receiptnumber], [warehouse], [area], [location], id, [customerid], [customername], 
	[sku], [UPC],[goodsname], [goodstype], " + inv.Qty + @", 2, @ODId, batchnumber, [BoxNumber],'" + Creator + @"', Getdate(), [updator], [updatetime], 
	[str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], [str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], 
	[datetime1], [datetime2], [datetime3], [datetime4], [datetime5], [int1], [int2], [int3], [int4], [int5],[Unit],[Specifications] 
									 FROM   [WMS_Inventory] 
									 WHERE  id = " + inv.ID + ");   SET @IIdTemp= @@IDENTITY  ");
                sb.Append(@" UPDATE wms_orderdetail 
									SET    lot = @IIdTemp 
									WHERE  id = @ODId  ");
                detail.OriginalQty = detail.OriginalQty - inv.Qty;
                inv.Qty = 0;
            }
            else
            {

                sb.Append(@"INSERT INTO [dbo].[wms_orderdetail] ([oid], [ordernumber], [externordernumber], [poid], [podid], [customerid], [customername], 
	[linenumber], [sku], [UPC], [goodsname], [goodstype], [lot], batchnumber, [warehouse], [area], [location], [qty], [picker], [picktime], 
	[confirmer], [confirmetime], [creator], [createtime], [updator], [updatetime], [remark], [str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], 
	[str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], [datetime1], [datetime2], [datetime3], [datetime4], [datetime5], 
	[int1], [int2], [int3], [int4], [int5],[BoxNumber],[Unit],[Specifications]) 
									(SELECT o.id, o.ordernumber, o.[externordernumber], o.[poid], P.[id], p.[customerid], p.[customername], 
	p.[linenumber], p.[sku], p.[UPC], p.[goodsname], p.[goodstype], 0,'" + inv.BatchNumber + @"',o.warehouse, '" + inv.Area + @"', '" + inv.Location + @"'," + detail.OriginalQty + @", NULL, NULL, 
	NULL, NULL, '" + Creator + @"', Getdate(), p.[updator], p.[updatetime], p.[remark], p.[str1], p.[str2], p.[str3], p.[str4], p.[str5], p.[str6], p.[str7], p.[str8], p.[str9], p.[str10], 
	p.[str11], p.[str12], p.[str13], p.[str14], p.[str15], p.[str16], p.[str17], p.[str18], p.[str19], p.[str20], p.[datetime1], p.[datetime2], p.[datetime3], p.[datetime4], p.[datetime5], 
	p.[int1], p.[int2], p.[int3], p.[int4], p.[int5],'" + inv.BoxNumber + @"','" + inv.Unit + @"','" + inv.Specifications + @"'
									FROM   [dbo].[wms_preorderdetail] p 
									LEFT JOIN @OrderInfo o  ON p.poid = o.poid 
									WHERE  p.id =" + detail.ID + ");");
                sb.Append(@"SET @ODId= @@IDENTITY   UPDATE [WMS_Inventory] 
									SET    qty = ( qty -" + detail.OriginalQty + @"),  inventorytype = ( CASE  WHEN qty =" + detail.OriginalQty + @"  THEN 9   ELSE 1   END ) 
									WHERE  id = " + inv.ID);
                sb.Append(@" INSERT INTO [WMS_Inventory] ([rrid], [receiptnumber], [warehouse], [area], [location], [superid], [customerid], [customername], 
	[sku], [UPC], [goodsname], [goodstype], [qty], [inventorytype], [relatedid], batchnumber, [BoxNumber],[creator], [createtime], [updator], [updatetime], 
	[str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], [str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], 
	[datetime1], [datetime2], [datetime3], [datetime4], [datetime5], [int1], [int2], [int3], [int4], [int5],[Unit],[Specifications]) 
									(SELECT [rrid], [receiptnumber], [warehouse], [area], [location], id, [customerid], [customername], 
	[sku], [UPC],[goodsname], [goodstype], " + detail.OriginalQty + @", 2, @ODId, batchnumber, [BoxNumber],'" + Creator + @"', Getdate(), [updator], [updatetime], 
	[str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], [str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], 
	[datetime1], [datetime2], [datetime3], [datetime4], [datetime5], [int1], [int2], [int3], [int4], [int5],[Unit],[Specifications] 
									 FROM   [WMS_Inventory] 
									 WHERE  id = " + inv.ID + ");   SET @IIdTemp= @@IDENTITY  ");
                sb.Append(@" UPDATE wms_orderdetail 
									SET    lot = @IIdTemp 
									WHERE  id = @ODId  ");
                inv.Qty = inv.Qty - detail.OriginalQty;
                detail.OriginalQty = 0;
                return sb;
            }

            if (detail.OriginalQty <= scatteredinvs.Where(a => a.SKU == detail.SKU).Sum(a => a.Qty))
            {
                sb.Append(DetailSql(detail, scatteredinvs, Creator));
            }
            else
            {
                if (detail.OriginalQty == 0)
                {
                    return sb;
                }
                sb.Append(DetailSql(detail, wholeinvs, scatteredinvs, Creator));
            }
            return sb;
        }
        private StringBuilder DetailSql(PreOrderDetail detail, IEnumerable<Inventorys> inventorys, string Creator)
        {
            StringBuilder sb = new StringBuilder();
            int NumberRemaining = 0;
            foreach (var scattered in inventorys)
            {
                if (detail.OriginalQty >= scattered.Qty)
                {

                    sb.Append(@" INSERT INTO [dbo].[wms_orderdetail] ([oid], [ordernumber], [externordernumber], [poid], [podid], [customerid], [customername], 
	[linenumber], [sku], [UPC], [goodsname], [goodstype], [lot], batchnumber, [warehouse], [area], [location], [qty], [picker], [picktime], 
	[confirmer], [confirmetime], [creator], [createtime], [updator], [updatetime], [remark], [str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], 
	[str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], [datetime1], [datetime2], [datetime3], [datetime4], [datetime5], 
	[int1], [int2], [int3], [int4], [int5],[BoxNumber],[Unit],[Specifications]) 
									(SELECT o.id, o.ordernumber, o.[externordernumber], o.[poid], P.[id], p.[customerid], p.[customername], 
	p.[linenumber], p.[sku], p.[UPC], p.[goodsname], p.[goodstype], 0, '" + scattered.BatchNumber + @"',o.warehouse,  '" + scattered.Area + @"', '" + scattered.Location + @"', " + scattered.Qty + @", NULL, NULL, 
	NULL, NULL, '" + Creator + @"', Getdate(), p.[updator], p.[updatetime], p.[remark], p.[str1], p.[str2], p.[str3], p.[str4], p.[str5], p.[str6], p.[str7], p.[str8], p.[str9], p.[str10], 
	p.[str11], p.[str12], p.[str13], p.[str14], p.[str15], p.[str16], p.[str17], p.[str18], p.[str19], p.[str20], p.[datetime1], p.[datetime2], p.[datetime3], p.[datetime4], p.[datetime5], 
	p.[int1], p.[int2], p.[int3], p.[int4], p.[int5], '" + scattered.BoxNumber + @"', '" + scattered.Unit + @"', '" + scattered.Specifications + @"'
									FROM   [dbo].[wms_preorderdetail] p 
									LEFT JOIN @OrderInfo o  ON p.poid = o.poid 
									WHERE  p.id = " + detail.ID + ");");
                    sb.Append(@" SET @ODId= @@IDENTITY     UPDATE [WMS_Inventory] 
									SET    qty = 0,  inventorytype = 9
									WHERE  id =" + scattered.ID);
                    sb.Append(@"INSERT INTO [WMS_Inventory] ([rrid], [receiptnumber], [warehouse], [area], [location], [superid], [customerid], [customername], 
	[sku], [UPC], [goodsname], [goodstype], [qty], [inventorytype], [relatedid], batchnumber, [BoxNumber],[creator], [createtime], [updator], [updatetime], 
	[str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], [str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], 
	[datetime1], [datetime2], [datetime3], [datetime4], [datetime5], [int1], [int2], [int3], [int4], [int5],[Unit],[Specifications]) 
									(SELECT [rrid], [receiptnumber], [warehouse], [area], [location], id, [customerid], [customername], 
	[sku], [UPC],[goodsname], [goodstype], '" + scattered.Qty + @"', 2, @ODId, batchnumber, [BoxNumber],'" + Creator + @"', Getdate(), [updator], [updatetime], 
	[str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], [str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], 
	[datetime1], [datetime2], [datetime3], [datetime4], [datetime5], [int1], [int2], [int3], [int4], [int5],[Unit],[Specifications] 
									 FROM   [WMS_Inventory] 
									 WHERE  id =" + scattered.ID + ");  SET @IIdTemp= @@IDENTITY  ");
                    sb.Append(@"UPDATE wms_orderdetail 
									SET    lot = @IIdTemp 
									WHERE  id = @ODId  ");


                    detail.OriginalQty = detail.OriginalQty - scattered.Qty;
                    scattered.Qty = 0;
                }
                else
                {


                    sb.Append(@" INSERT INTO [dbo].[wms_orderdetail] ([oid], [ordernumber], [externordernumber], [poid], [podid], [customerid], [customername], 
	[linenumber], [sku], [UPC], [goodsname], [goodstype], [lot], batchnumber, [warehouse], [area], [location], [qty], [picker], [picktime], 
	[confirmer], [confirmetime], [creator], [createtime], [updator], [updatetime], [remark], [str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], 
	[str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], [datetime1], [datetime2], [datetime3], [datetime4], [datetime5], 
	[int1], [int2], [int3], [int4], [int5],[BoxNumber],[Unit],[Specifications]) 
									(SELECT o.id, o.ordernumber, o.[externordernumber], o.[poid], P.[id], p.[customerid], p.[customername], 
	p.[linenumber], p.[sku], p.[UPC], p.[goodsname], p.[goodstype], 0, '" + scattered.BatchNumber + @"',o.warehouse,  '" + scattered.Area + @"', '" + scattered.Location + @"', " + detail.OriginalQty + @", NULL, NULL, 
	NULL, NULL, '" + Creator + @"', Getdate(), p.[updator], p.[updatetime], p.[remark], p.[str1], p.[str2], p.[str3], p.[str4], p.[str5], p.[str6], p.[str7], p.[str8], p.[str9], p.[str10], 
	p.[str11], p.[str12], p.[str13], p.[str14], p.[str15], p.[str16], p.[str17], p.[str18], p.[str19], p.[str20], p.[datetime1], p.[datetime2], p.[datetime3], p.[datetime4], p.[datetime5], 
	p.[int1], p.[int2], p.[int3], p.[int4], p.[int5], '" + scattered.BoxNumber + @"', '" + scattered.Unit + @"', '" + scattered.Specifications + @"'
									FROM   [dbo].[wms_preorderdetail] p 
									LEFT JOIN @OrderInfo o  ON p.poid = o.poid 
									WHERE  p.id = " + detail.ID + ");");
                    sb.Append(@" SET @ODId= @@IDENTITY     UPDATE [WMS_Inventory] 
									SET    qty = ( qty -" + detail.OriginalQty + @"),  inventorytype = ( CASE  WHEN qty =" + detail.OriginalQty + @"  THEN 9   ELSE 1   END ) 
									WHERE  id =" + scattered.ID);
                    sb.Append(@"INSERT INTO [WMS_Inventory] ([rrid], [receiptnumber], [warehouse], [area], [location], [superid], [customerid], [customername], 
	[sku], [UPC], [goodsname], [goodstype], [qty], [inventorytype], [relatedid], batchnumber, [BoxNumber],[creator], [createtime], [updator], [updatetime], 
	[str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], [str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], 
	[datetime1], [datetime2], [datetime3], [datetime4], [datetime5], [int1], [int2], [int3], [int4], [int5],[Unit],[Specifications]) 
									(SELECT [rrid], [receiptnumber], [warehouse], [area], [location], id, [customerid], [customername], 
	[sku], [UPC],[goodsname], [goodstype], '" + detail.OriginalQty + @"', 2, @ODId, batchnumber, [BoxNumber],'" + Creator + @"', Getdate(), [updator], [updatetime], 
	[str1], [str2], [str3], [str4], [str5], [str6], [str7], [str8], [str9], [str10], [str11], [str12], [str13], [str14], [str15], [str16], [str17], [str18], [str19], [str20], 
	[datetime1], [datetime2], [datetime3], [datetime4], [datetime5], [int1], [int2], [int3], [int4], [int5],[Unit],[Specifications] 
									 FROM   [WMS_Inventory] 
									 WHERE  id =" + scattered.ID + ");  SET @IIdTemp= @@IDENTITY  ");
                    sb.Append(@"UPDATE wms_orderdetail 
									SET    lot = @IIdTemp 
									WHERE  id = @ODId  ");
                    return sb;
                }
            }
            return sb;
        }



        public string AddShipRequestHeaderAndDetail(IEnumerable<WMS_ShipRequestHeader> header, IEnumerable<WMS_ShipRequestDetail> detail)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    PreOrderAndPreOrderDetail PreOrderAndDetail = new PreOrderAndPreOrderDetail();
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_AddShipRequestHeaderAndDetail]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Header", header.Select(p => new WMS_ShipRequestHeaderToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Detail", detail.Select(p => new WMS_ShipRequestDetailToDB(p)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
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
                    //PreOrderAndDetail.PreO = ds.Tables[ds.Tables.Count - 1].ConvertToEntity<PreOrder>();
                    //PreOrderAndDetail.PreOrderList = ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<PreOrder>();
                    //PreOrderAndDetail.PreOd = ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<PreOrderDetail>();
                    // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                    return message;
                }
                catch (Exception e)
                {

                    throw e;
                }

            }
        }

        public IEnumerable<Inventorys> GetOutboundInventory()
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("ProcGetOutboundInventory", conn);//Proc_WMS_AutomatedOutbound     Proc_WMS_AutomatedOutbound_Total
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);//将得到的数据 填充到DataTable中
                    conn.Close();
                    return ds.Tables[0].ConvertToEntityCollection<Inventorys>();
                    //if (ds.Tables.Count > 1)
                    //{
                    //    return ds.Tables[1].ConvertToEntityCollection<DistributionInformation>();
                    //}
                    //else
                    //{
                    //    return ds.Tables[0].ConvertToEntityCollection<DistributionInformation>();
                    //}

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public string BatchIimportUpdateLoadKey(string CustomerID, IEnumerable<PreOrder> PreOrderList)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    PreOrder PreOrder = new PreOrder();
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateLoadKey", conn);//默认
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Po", PreOrderList.Select(p => new WMSPreOrderInfoToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
                    cmd.CommandTimeout = 300;//超时时间
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
                    throw e;
                }
            }
        }

        public string BatchIimportUpdateGoodsType(string CustomerID, IEnumerable<ASNDetail> ReceiptList)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    PreOrder PreOrder = new PreOrder();
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateGoodsType", conn);//默认
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Receipt", ReceiptList.Select(p => new WMSASNDetailToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
                    cmd.CommandTimeout = 300;//超时时间
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
                    throw e;
                }
            }
        }

        /// <summary>
        /// 查询取消单信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="rowcounts"></param>
        /// <returns></returns>
        public IEnumerable<CancelOrderInfo> GetCancelOrderList(CancelOrderSearchCondition search, string Proc, out int rowcounts)
        {
            rowcounts = 0;
            string where = GenCancelOrderWhere(search);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, where, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, search.PageIndex*search.PageSize, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, search.PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, rowcounts, ParameterDirection.Output)
            };
            DataSet ds = new DataSet();
            ds = this.ExecuteDataSet(Proc, dbParams);
            rowcounts = (int)dbParams[3].Value;
            return ds.Tables[0].ConvertToEntityCollection<CancelOrderInfo>();
        }

        /// <summary>
        /// 取消单查询条件拼接
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public string GenCancelOrderWhere(CancelOrderSearchCondition search)
        {
            StringBuilder sb = new StringBuilder();
            if (search.CustomerID != 0 && search.CustomerID != null)
            {
                sb.Append(" AND p.CustomerID=").Append(search.CustomerID).Append(" ");
            }
            if (!string.IsNullOrEmpty(search.Warehouse))
            {
                sb.Append(" AND p.Warehouse='").Append(search.Warehouse).Append("' ");
            }
            if (!string.IsNullOrEmpty(search.ExternOrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (search.ExternOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = search.ExternOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (search.ExternOrderNumber.IndexOf(',') > 0)
                {
                    numbers = search.ExternOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and p.ExternOrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and p.ExternOrderNumber  like '%" + search.ExternOrderNumber.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(search.PreOrderNumber))
            {
                sb.Append(" AND p.PreOrderNumber='").Append(search.PreOrderNumber.Trim()).Append("' ");
            }

            if (!string.IsNullOrEmpty(search.StartCreateTime))
            {
                sb.Append(" AND p.CreateTime >='").Append(DateTime.Parse(search.StartCreateTime).DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (!string.IsNullOrEmpty(search.EndCreateTime))
            {
                sb.Append(" AND p.CreateTime <='").Append(DateTime.Parse(search.EndCreateTime).DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            return sb.ToString();
        }


        /// <summary>
        /// 验证外部单号是否存在
        /// </summary>
        /// <param name="PreOrderList"></param>       
        /// <returns></returns>
        public IEnumerable<PreOrder> GetWMSPreOrderlistByLoadKey(IEnumerable<PreOrder> PreOrderList)
        {
            string custumers = "";
            PreOrderList.Select(m => m.CustomerID).Distinct().ToList().ForEach((item) => { custumers += item + ","; });
            custumers = custumers.Substring(0, custumers.Length - 1);

            string preos = "";
            PreOrderList.Select(m => m.str9).Distinct().ToList().ForEach((item) => { preos += "'" + item + "',"; });
            preos = preos.Substring(0, preos.Length - 1);


            string sql = "SELECT * FROM dbo.WMS_PreOrder p WHERE p.CustomerID IN (" + custumers + ") AND p.str9 IN(" + preos + ") AND p.Status!=-1";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand(sql, conn);//默认
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 300;//超时时间
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    conn.Close();
                    return ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }

    }
}