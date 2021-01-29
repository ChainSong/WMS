using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Data.SqlClient;
using System.Data;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.Warehouse;
using Runbow.TWS.Logger.LogHelper;

namespace Runbow.TWS.Dao.RFWeb
{

    public class InventoryManagementAccessor : BaseAccessor
    {
        public IEnumerable<Inventorys> GetInventoryForRFBySKU(long CustomerID, string WarehouseName, string SKU)
        {
            DataTable dt = new DataTable();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.Int64, CustomerID, ParameterDirection.Input),

                new DbParam("@WarehouseName", DbType.String, WarehouseName, ParameterDirection.Input),

                new DbParam("@SKU", DbType.String, SKU, ParameterDirection.Input)
                };
            dt = this.ExecuteDataTable("Proc_WMS_GetRFInventory", dbParams);
            return dt.ConvertToEntityCollection<Inventorys>();
        }
        /// <summary>
        /// 验证原库位能否移库
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WarehouseName"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public int CheckBoxNumber(long CustomerID, string WarehouseName, string BoxNumber)
        {
            try
            {

                string sqlCheck = (@"select id,SKU,InventoryType,Qty from WMS_Inventory_103 where  Warehouse='" + WarehouseName + @"' and Location='" + BoxNumber + @"'
                and InventoryType!=9");
                IEnumerable<Inventorys> inventorys = this.ScanDataTable(sqlCheck).ConvertToEntityCollection<Inventorys>();
                if (inventorys != null)
                {
                    if (inventorys.Count() == 0)
                    {
                        return 3;
                    }
                    if (inventorys.Where(a => a.InventoryType == 2 || a.InventoryType == 3).Count() < 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return 0;
                }
                //return this.ScanExecuteNonQuery(sqlCheck);
                //return dt.ConvertToEntityCollection<Inventorys>();

            }
            catch (Exception ex)
            {
                SysLogWriter.Error("CheckBoxNumber（Dao层）" + BoxNumber + ex.ToString());
            }
            return 0;
        }
        /// <summary>
        /// 移库
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WarehouseName"></param>
        /// <param name="BoxNumber"></param>
        /// <param name="Location"></param>
        /// <param name="Creator"></param>
        /// <returns></returns>
        public int MoveLocation(long CustomerID, string WarehouseName, string BoxNumber, string Location, string Creator)
        {
            try
            {


                string SqlCheckLocation = @"select wl.* from WMS_Warehouse w left join WMS_Warehouse_Location wl
               on w.ID = wl.WarehouseID where wl.Location = '" + Location + @"'  and w.WarehouseName='" + WarehouseName + "' ";
                if (this.ScanDataTable(SqlCheckLocation).Rows.Count < 1)
                {
                    return 2;
                }
                string sqlCheck = (@"select * from WMS_Inventory_103 where  Warehouse='" + WarehouseName + @"' and Location='" + BoxNumber + @"'
                and InventoryType!=9");
                IEnumerable<Inventorys> inventorys = this.ScanDataTable(sqlCheck).ConvertToEntityCollection<Inventorys>();
                //提交前查看是否还有数据Check
                if (inventorys == null || inventorys.Count() == 0)
                {
                    return 0;
                }
                string AdjustmentNumber = "ADJ" + DateTime.Now.Ticks;
                //新建库存移动单
                string SqlAdjustment = (@"
              DECLARE @AID bigint; 
             insert into [WMS_Adjustment] ([AdjustmentNumber]
            ,[CustomerID]
            ,[CustomerName]
            ,[Warehouse]
            ,[Status]
            ,[AdjustmentType]
            ,[AdjustmentReason]
            ,[AdjustmentTime]
            ,[IsHold]
            ,[Creator]
            ,[CreateTime] 
            ,[Remark] ,Str3) values('" + AdjustmentNumber + @"'
            ,'" + inventorys.First().CustomerID + @"'
            ,'" + inventorys.First().CustomerName + @"'
            ,'" + inventorys.First().Warehouse + @"'
            ,1
            ,'库存移动单'
            ,'RF主动移库'
            ,getdate()
            ,0
            ,'" + Creator + @"'
            ,getdate()
            ,'RF主动移库','QQQQQQ' ) 
            set @AID=@@IDENTITY
            insert into  WMS_AdjustmentDetail ([AID]
            ,[AdjustmentNumber]
            ,[CustomerID]
            ,[CustomerName]
            ,[FromLot]
            ,[ToLot]
            ,[SKU]
            ,[UPC]
            ,[BatchNumber]
            ,[BoxNumber]
            ,[GoodsName]
            ,[FromWarehouse]
            ,[ToWarehouse]
            ,[FromArea]
            ,[ToArea]
            ,[FromLocation]
            ,[ToLocation]
            ,[FromQty]
            ,[ToQty]
            ,[FromGoodsType]
            ,[ToGoodsType]
            ,[Unit]
            ,[Specifications]
            ,[IsHold]
            ,[AdjustmentReason]
            ,[Creator]
            ,[CreateTime],Str3 ) (  select @AID,'" + AdjustmentNumber + @"', CustomerID,CustomerName,null,null,SKU,UPC,BatchNumber,BoxNumber,GoodsName,Warehouse,Warehouse,Area,Area,
             Location,'" + Location + @"',Qty,Qty,GoodsType,GoodsType,Unit,Specifications,0,'','" + Creator + @"',getdate(),Str3 from WMS_Inventory_103 where    InventoryType=1 and Warehouse='" + WarehouseName + "' and   Location='" + BoxNumber + @"' )
             select * from WMS_Adjustment where [CustomerID]=" + CustomerID + " and AdjustmentNumber='" + AdjustmentNumber + @"'");
                //SqlAdjustment += (@"");
                Adjustment adjustment = this.ScanDataTable(SqlAdjustment).ConvertToEntity<Adjustment>();

                AdjustmentManagementAccessor adjustmentDetail = new AdjustmentManagementAccessor();
                string str = adjustmentDetail.Complets(adjustment.ID, "库存移动单");

                if (str.Contains("提交成功"))
                {
                    return 1;
                }
                else
                {
                    string UpdateadjustmentSql = (@" update WMS_Adjustment set  Status=-1  where AdjustmentNumber='" + AdjustmentNumber + @"' and CustomerID=" + CustomerID);
                    this.ScanDataTable(UpdateadjustmentSql);
                    return 0;
                }
                //return this.ScanExecuteNonQuery(sqlCheck);
                //return dt.ConvertToEntityCollection<Inventorys>();

            }
            catch (Exception ex)
            {
                SysLogWriter.Error("MoveLocation（Dao层）" + Creator + ex.ToString());
            }
            return 0;
        }


        //
        public IEnumerable<WarehouseCheck> GetWarehouseCheck(long CustomerID, string WarehouseName)
        {
            #region 查询条件
            StringBuilder sb = new StringBuilder(); //普通盘点
            sb.Append(" and customerid=" + CustomerID + " and Warehouse='" + WarehouseName + "'");
            #endregion
            string sqlWhere = sb.ToString();
            DataTable dt = new DataTable();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
                };
            dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByConditionForRF", dbParams);
            return dt.ConvertToEntityCollection<WarehouseCheck>();

        }
        public bool InsertcheckDetailList(IEnumerable<WarehouseCheckDetail> Request, string UserName, string CheckNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AddCheckDetailByCheckNumber]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CheckNumber", CheckNumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@CheckList", Request.Select(p => new WarehouseCheckDetailToDb(p)));
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
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
    }
}
