using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Dao.RFWeb
{
    public class BestLocationManagementAccessor : BaseAccessor
    {
        /// <summary>
        /// 获取推荐库位
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="WarehouseName"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public string GetBestLocation(string CustomerId, string WarehouseName, string BoxNumber)
        {

            string SKU = "";
            //判断有没有推荐过

            string Str = "select * from  WMS_Inventory_103_Temp where  str2='" + BoxNumber + "'";

            IEnumerable<Inventorys> IsCheck = this.ScanDataTable(Str.ToString()).ConvertToEntityCollection<Inventorys>();
            if (IsCheck.Count() > 0)
            {
                return IsCheck.First().Location;
            }
            //获取SKU
            string GetSKU = "select * from WMS_ReceiptDetail where str2='" + BoxNumber + "'";
            IEnumerable<ReceiptDetail> ReceiptDetail = this.ScanDataTable(GetSKU.ToString()).ConvertToEntityCollection<ReceiptDetail>();
            if (ReceiptDetail.Count() > 0)
            {
                SKU = ReceiptDetail.First().SKU;
            }
            //获取该SKU所在的库位
            string SqlLocation = @" select  distinct  i. Location from WMS_Inventory_103 i
            where InventoryType = 1  and SKU = '" + SKU + "'";

            string SqlLocationTemp = @"select distinct Location   from[dbo].[WMS_Inventory_103_Temp] where Status = 1
            and SKU = '" + SKU + "'";


            StringBuilder sb = new StringBuilder();

            sb.Append(@"select Area,l.Location,SUM(l.Str2) Str2,SUM(Qty) Qty from ( select Area,Location,COUNT(Str2) Str2,SUM(Qty) Qty from WMS_Inventory_103 where InventoryType=1
            and Location in (" + SqlLocation + @")
            group by Location,Area
            union (select Area, Location,COUNT(Str2) Str2,SUM(Qty) Qty from WMS_Inventory_103_Temp
            where Location in (" + SqlLocationTemp + @")
            group by Location,Area)) as l 
			left join WMS_Warehouse_Area wa
            on l.Area=wa.AreaName
            left join  WMS_Warehouse_Location wl
            on l.Location=wl.Location and wa.ID=wl.AreaID
            GROUP BY l.Location,wl.MaxNumber,Area
            HAVING COUNT(l.Str2)< isnull(wl.MaxNumber,12)
            ");

            IEnumerable<Inventorys> inventorys = this.ScanDataTable(sb.ToString()).ConvertToEntityCollection<Inventorys>();
            LocationInfo info = new LocationInfo();
            if (inventorys == null)
            {
                info = SqlGetNullLocationfun();
            }
            else
            {
                foreach (var item in inventorys)
                {

                    info.Location = item.Location;
                    info.AreaName = item.Area;
                    break;
                }
                if (string.IsNullOrEmpty(info.Location))
                {
                    info = SqlGetNullLocationfun();
                }
            }
            if (!string.IsNullOrEmpty(info.Location))
            {
                string InvEntoryTemp = (@" insert into WMS_Inventory_103_Temp
	            ([RID]
                ,[RDID]
                ,[ReceiptNumber]
                ,[ExternReceiptNumber]
                ,[CustomerID]
                ,[CustomerName]
                ,[SKU]
                ,[UPC]
                ,[BoxNumber]
                ,[BatchNumber]
                ,[GoodsName]
                ,[Qty]
	            ,[Creator]
	            ,[CreateTime]
                ,[Area]
                ,[Location]
                ,[Status]
                ,[Str2])
	            select top 1000 [RID] 
                ,[ID]
                ,[ReceiptNumber]
                ,[ExternReceiptNumber]
                ,[CustomerID]
                ,[CustomerName]
                ,[SKU]
                ,[UPC]
                ,[BoxNumber]
                ,[BatchNumber]
                ,[GoodsName]
                ,[QtyExpected]
                ,[Creator]
                ,[CreateTime]
	            ,'" + info.AreaName + @"'
	            , '" + info.Location + @"'
                , 1
                ,[str2] from WMS_ReceiptDetail where CustomerID = " + CustomerId + @"
                and str2 = '" + BoxNumber + "' ");

                this.ScanDataTable(InvEntoryTemp).ConvertToEntityCollection<Inventorys>();

                return info.Location;
            }
            return "";



            //            SqlTransaction trans = null;
            //            SqlConnection con = new SqlConnection(BaseAccessor._dataBase.ConnectionString);
            //            try
            //            {
            //                con.Open();
            //                trans = con.BeginTransaction();

            //                SqlCommand com = new SqlCommand();

            //                com.Connection = con;
            //                com.Transaction = trans;
            //                com.CommandText = @"select distinct  Location from (select  top 1 i. Location from WMS_Inventory_103 i
            //left join WMS_Warehouse_Area wa
            //on i.Area = wa.AreaName
            //left join  WMS_Warehouse_Location wl
            //on i.Location = wl.Location and wa.ID = wl.AreaID
            //where InventoryType = 1 and wl.LocationType = 2 and SKU = '00885177858643'
            //union(select Location   from[dbo].[WMS_Inventory_103_Temp] where Status = 1
            //and SKU = '00885177858643') ) aa";
            //                //SqlParameter para = new SqlParameter("变量", "值");
            //                //com.Parameters.Add(para);
            //                //com.CommandText = com.CommandText;
            //                int i = com.ExecuteNonQuery();//执行方式自己选择
            //                com.CommandText = (str);
            //                com.ExecuteNonQuery();//执行方式自己选择
            //                com.CommandText = (str);

            //                com.ExecuteNonQuery();//执行方式自己选择

            //                com.CommandText = (str);

            //                com.ExecuteNonQuery();//执行方式自己选择



            //                trans.Commit();//执行提交事务

            //            }
            //            catch (Exception ex)
            //            {
            //                trans.Rollback();//如果前面有异常则事务回滚
            //            }
            //            finally
            //            {
            //                con.Close();
            //            }
        }


        private LocationInfo SqlGetNullLocationfun()
        {
            string SqlGetNullLocation = @"select top 1  * from WMS_Warehouse_Location wl
             left join WMS_Warehouse_Area wa
             on wl.AreaID=wa.ID where
             not exists (select * from WMS_Inventory_103 i where i.Location=wa.AreaName and i.Location=wl.Location) 
             order by Location  ";

            return this.ScanDataTable(SqlGetNullLocation).ConvertToEntity<LocationInfo>();
        }
        public string RefreshLocation(string CustomerId, string WarehouseName, string BoxNumber,string Location)
        {
            string SqlGetNullLocation = @"select top 1  * from WMS_Warehouse_Location wl
             left join WMS_Warehouse_Area wa
             on wl.AreaID=wa.ID where
             not exists (select * from WMS_Inventory_103 i where i.Location=wa.AreaName and i.Location=wl.Location) 
             order by Location  ";

            return "";
        }

        
    }
}
