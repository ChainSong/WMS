using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.WMS.Inventory;
using System.Data.SqlClient;
using Runbow.TWS.Entity.DataBaseEntity;
namespace Runbow.TWS.Dao.WMS
{
    public class InventoryManagementAccessor : BaseAccessor
    {
        public IEnumerable<Inventorys> GetInventoryByIDS(string IDS)
        {
            string Where = "and i.id in (SELECT col FROM f_splitSTR(" + IDS + ",','))";
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, Where, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventoryByIDS", dbParams);
            return dt.ConvertToEntityCollection<Inventorys>();
        }

        public IEnumerable<Inventorys> GetInventoryBySearchCondition(InventorySearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            //string sqlWhere = this.GenGetInventory(SearchCondition);
            //string sqlOrderByType = this.OrderByType(SearchCondition);
            //int tempRowCount = 0;
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            //    new DbParam("@OrderByType", DbType.String, sqlOrderByType, ParameterDirection.Input),
            //    new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
            //    new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
            //    new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            //};
            //DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventoryBySearchCondition", dbParams);
            //RowCount = (int)dbParams[4].Value;
            //return dt.ConvertToEntityCollection<Inventorys>();

            string sqlWhere = this.GenGetInventory(SearchCondition);
            string sqlOrderByType = this.OrderByType(SearchCondition);
            RowCount = 0;
            try
            {


                return this.ExecuteDataTableBySqlString(@"   SELECT *,COUNT(*) OVER( order  by CustomerID) AS TotalCount FROM (SELECT ROW_NUMBER() OVER (" + sqlOrderByType + @") RowID, CustomerID,(SELECT ID FROM dbo.WMS_Warehouse WHERE WarehouseName=i.warehouse) AS WarehouseID,p.Int1,CustomerName,max(RelatedID) as RelatedID , i.Warehouse,i.Area,i.Location,i.SKU,i.GoodsName,i.GoodsType 
, InventoryType, sum(Qty) Qty,ISNULL(i.BatchNumber,'') BatchNumber, ISNULL(i.BoxNumber,'') BoxNumber, ISNULL(i.Unit,'') Unit, ISNULL(i.Specifications,'') Specifications, ISNULL(i.UPC,'') UPC, i.DateTime1,i.str3, (SELECT convert(varchar, ii.ID) + ',' FROM WMS_Inventory_" + SearchCondition.CustomerID + @"(NOLOCK) ii
  WHERE ii.InventoryType =i.InventoryType and ii.Warehouse = i.Warehouse and ii.Area = i.Area and ii.Location = i.Location and ii.SKU = i.SKU and ii.GoodsType = i.GoodsType  and isnull(ii.BatchNumber, '') = isnull(i.BatchNumber, '') and isnull(ii.BoxNumber, '') = isnull(i.BoxNumber, '')
  and isnull(ii.Unit, '') = isnull(i.Unit, '') and isnull(ii.Specifications, '') = isnull(i.Specifications, '')
   and isnull(ii.UPC, '') = isnull(i.UPC, '')  and isnull(ii.DateTime1, '') = isnull(i.DateTime1, '')
  FOR XML PATH('')) AS IDS
 from dbo.WMS_Inventory_" + SearchCondition.CustomerID + @"(NOLOCK)i left join WMS_Product(NOLOCK)  p ON i.CustomerID = p.StorerID AND i.SKU = p.SKU
 where 1 = 1 and Qty> 0  and isnull(p.Str8,'') <> '02' " + sqlWhere + @"
 group by CustomerID,CustomerName,i.GoodsName,i.Warehouse,i.Area,i.Location,i.SKU,i.GoodsType,InventoryType,ISNULL(i.BatchNumber,''),ISNULL(i.BoxNumber,''),ISNULL(i.Unit,''),ISNULL(i.Specifications,''),p.Int1,ISNULL(i.UPC,''),i.DateTime1,i.str3
  ) R
ORDER BY R.CustomerID
                 OFFSET " + PageIndex * PageSize + @" ROWS
                 FETCH NEXT " + PageSize + " ROWS ONLY").ConvertToEntityCollection<Inventorys>(out RowCount);
                //response.PageIndex = PageIndex;
                //response.PageCount = TotalCount;
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {

            }
            return null;



        }

        public GetInventoryBySearchConditionResponse GetInventoryBySearchConditionGroup(InventorySearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();
            string sqlWhere = this.GenGetInventory(SearchCondition);
            string sqlOrderByType = this.OrderByType(SearchCondition);
            string sqlOrderByType2 = this.OrderByType2(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@OrderByType", DbType.String, sqlOrderByType, ParameterDirection.Input),
                new DbParam("@OrderByType2", DbType.String, sqlOrderByType2, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetInventoryBySearchConditionGroup", dbParams);
            RowCount = (int)dbParams[5].Value;
            //return ds.Tables[0].ConvertToEntityCollection<Inventorys>();
            response.InventoryCollection = ds.Tables[0].ConvertToEntityCollection<Inventorys>();
            response.InventoryCollection2 = ds.Tables[1].ConvertToEntityCollection<Inventorys>();
            return response;
        }
        /// <summary>
        /// 库存快照查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<InventorySnapshoot> GetInventorySnapshootBySearchCondition(InventorySearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetInventorySnapshoot(SearchCondition);
            //string sqlOrderByType = this.OrderByType(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input), 
                //new DbParam("@OrderByType", DbType.String, sqlOrderByType, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventorySnapshootBySearchCondition", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<InventorySnapshoot>();
        }
        /// <summary>
        /// 库存快照导出
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public IEnumerable<InventorySnapshoot> ExportInventorySnapshootBySearchCondition(InventorySearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetInventorySnapshoot(SearchCondition);
            //string sqlOrderByType = this.OrderByType(SearchCondition);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input), 
               //new DbParam("@OrderByType", DbType.String, sqlOrderByType, ParameterDirection.Input),
                
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_ExportInventorySnapshootBySearchCondition", dbParams);

            return dt.ConvertToEntityCollection<InventorySnapshoot>();
        }

        public IEnumerable<DirectAddInventory> InventoryRemaining(InventorySearchCondition Condition)
        {
            string sqlWhere = this.GetInventoryRemainingWhere(Condition);
            // string sqlOrderByType = this.OrderByType(Condition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
                //new DbParam("@OrderByType", DbType.String, sqlOrderByType, ParameterDirection.Input),
                //new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                //new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                //new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("[Proc_InventoryRemaining]", dbParams);
            //RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<DirectAddInventory>();
        }

        public GetInventoryBySearchConditionResponse TotalReport(InventorySearchCondition Condition)
        {
            GetInventoryBySearchConditionResponse Response = new GetInventoryBySearchConditionResponse();
            string sqlWhere = this.GetInventoryRemainingWhere(Condition);
            // string sqlOrderByType = this.OrderByType(Condition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input) ,
                 new DbParam("@StateDate", DbType.String,"AND CONVERT(VARCHAR(100), CreateTime, 23) ='" + Condition.StateDate.Value.ToString("yyyy-MM-dd")+ "'", ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("[Proc_SCInventorySummaryReport]", dbParams);
            Response.directAddInventory = ds.Tables[0].ConvertToEntityCollection<DirectAddInventory>();
            Response.Total = ds.Tables[1].ConvertToEntityCollection<DirectAddInventory>();
            Response.daily = ds.Tables[2].ConvertToEntityCollection<DirectAddInventory>();
            //RowCount = (int)dbParams[3].Value;
            return Response;
        }
        /// <summary>
        /// 查询 要上传的库存信息 
        /// </summary>
        /// <returns></returns>
        public GetInventoryBySearchConditionResponse GetConfirmInventory()
        {
            GetInventoryBySearchConditionResponse Response = new GetInventoryBySearchConditionResponse();
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetAkzoInventoryConfirm");
            Response.InventoryCollection = ds.Tables[0].ConvertToEntityCollection<Inventorys>();
            return Response;
        }

        public GetInventoryBySearchConditionResponse dailyReport(InventorySearchCondition Condition)
        {
            GetInventoryBySearchConditionResponse Response = new GetInventoryBySearchConditionResponse();
            string sqlWhere = this.GetInventoryRemainingWhere(Condition);
            string SqlCustromer = this.GetCutomerWhere(Condition);
            // string sqlOrderByType = this.OrderByType(Condition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input) ,
                new DbParam("@StateDate", DbType.String,"AND CONVERT(VARCHAR(100), CreateTime, 23) ='" + Condition.StateDate.Value.ToString("yyyy-MM-dd")+ "'", ParameterDirection.Input) ,
                new DbParam("@Customer", DbType.String,SqlCustromer, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("[Proc_DCInventorySummaryReport]", dbParams);
            Response.directAddInventory = ds.Tables[0].ConvertToEntityCollection<DirectAddInventory>();
            Response.Total = ds.Tables[1].ConvertToEntityCollection<DirectAddInventory>();
            Response.daily = ds.Tables[2].ConvertToEntityCollection<DirectAddInventory>();
            //RowCount = (int)dbParams[3].Value;
            return Response;
            // return ds.ConvertToEntityCollection<DirectAddInventory>();
        }

        private string GetCutomerWhere(InventorySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND StorerID=" + SearchCondition.CustomerID + "");
            }
            if (!string.IsNullOrEmpty(SearchCondition.CustomerIDs))
            {
                sb.Append(" AND StorerID in (" + SearchCondition.CustomerIDs + ")");
            }
            return sb.ToString();
        }

        public GetInventoryBySearchConditionResponse detailReport(InventorySearchCondition Condition)
        {
            GetInventoryBySearchConditionResponse Response = new GetInventoryBySearchConditionResponse();
            string sqlWhere = this.GetInventoryRemainingWhere(Condition);
            string SqlCustromer = this.GetCutomerWhere(Condition);
            // string sqlOrderByType = this.OrderByType(Condition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input) ,
                 new DbParam("@StateDate", DbType.String,"AND CONVERT(VARCHAR(100), CreateTime, 23) ='" + Condition.StateDate.Value.ToString("yyyy-MM-dd")+ "'", ParameterDirection.Input) ,
           new DbParam("@Customer", DbType.String,SqlCustromer, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("[Proc_ECInventorySummaryReport]", dbParams);
            Response.directAddInventory = ds.Tables[0].ConvertToEntityCollection<DirectAddInventory>();
            Response.Total = ds.Tables[1].ConvertToEntityCollection<DirectAddInventory>();

            Response.detail = ds.Tables[2].ConvertToEntityCollection<DirectAddInventory>();
            //RowCount = (int)dbParams[3].Value;
            return Response;
            // return ds.ConvertToEntityCollection<DirectAddInventory>();
        }

        private string GetInventoryRemainingWhere(InventorySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (SearchCondition.CreateTime != null)
            {
                sb.Append(" AND CreateTime>='" + SearchCondition.CreateTime + "'");
                sb.Append(" AND CreateTime<='" + SearchCondition.CreateTime.Value.ToShortDateString() + " 23:59:59'");
            }
            if (SearchCondition.InventoryType != 0)
            {
                sb.Append(" AND InventoryType=" + SearchCondition.InventoryType + "");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND CustomerID=" + SearchCondition.CustomerID + "");
            }
            if (!string.IsNullOrEmpty(SearchCondition.CustomerIDs))
            {
                sb.Append(" AND CustomerID in (" + SearchCondition.CustomerIDs + ")");
            }
            //if (SearchCondition.StateDate != null)
            //{
            //    sb.Append(" AND CreateTime>'" + SearchCondition.StateDate + "'");
            //    sb.Append(" AND CreateTime<='" + SearchCondition.CreateTime.Value.ToShortDateString() + " 23:59:59'");
            //}
            return sb.ToString();
        }

        public IEnumerable<DirectAddInventory> InventorydDtails(string CustomerId, string ProduceType, DateTime? Date)
        {
            string sqlWhere = " and CustomerID=" + CustomerId + "and InventoryType=" + ProduceType + " and CONVERT(varchar(100), CreateTime, 23)='" + Date.Value.ToString("yyyy-MM-dd") + "'";
            //string sqlOrderByType = this.OrderByType(SearchCondition);
            //int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String,sqlWhere, ParameterDirection.Input) 
                //new DbParam("@OrderByType", DbType.String, sqlOrderByType, ParameterDirection.Input),
                //new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                //new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                //new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("[Proc_InventorydDtails]", dbParams);
            //RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<DirectAddInventory>();
        }

        public string Unboxing_akzo(string IDS, float UnboxingQty, string Creator, IList<PreOrderDetail> ToSKUJson)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_Unboxing_akzo", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", IDS);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 500;
                    cmd.Parameters.AddWithValue("@UnboxingQty", UnboxingQty);
                    cmd.Parameters[1].SqlDbType = SqlDbType.Float;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 50;
                    cmd.Parameters.AddWithValue("@ToSKUJson", ToSKUJson.Select(p => new WMSPreOrderDetailInfoToDb(p)));
                    cmd.Parameters[3].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 500;
                    cmd.CommandTimeout = 500;
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
                    throw ex;
                }
            }
        }

        public string DirectAddInventoryImports(IList<DirectAddInventory> SearchCondition)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Receipt> result = new List<Receipt>();
                    IList<ReceiptDetail> receiptDetail = new List<ReceiptDetail>();
                    SqlCommand cmd = new SqlCommand("Proc_DirectAddInventory", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DirectAddInventory", SearchCondition.Select(p => new DirectAddInventoryToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 500;
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

        public IEnumerable<DirectAddInventory> SupplyChainReport()
        {
            string sqlWhere = "";
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input) };
            DataTable dt = this.ExecuteDataTable("[Proc_GetDirectAddInventoryReport]", dbParams);
            return dt.ConvertToEntityCollection<DirectAddInventory>();
        }

        public bool DelDirectAddInventory(string Id)
        {
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, Id, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)};

            DataTable dt = this.ExecuteDataTable("[Proc_DelDirectAddInventory]", dbParams);
            int RowCount = (int)dbParams[1].Value;
            if (RowCount > 0)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<Inventorys> GetInventoryRecord(InventorySearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetInventory(SearchCondition);
            //string sqlOrderByType = this.OrderByType(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                //new DbParam("@OrderByType", DbType.String, sqlOrderByType, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetInventoryRecord]", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<Inventorys>();
        }

        public string GenGetInventory(InventorySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.GoodsType))
            {
                sb.Append(" AND i.GoodsType='" + SearchCondition.GoodsType + "'");
            }

            if (SearchCondition.Int1 == 1)
            {
                sb.Append(" AND DATEDIFF(" + "dd" + ",i.DateTime1,GETDATE())<=90");
            }
            if (!string.IsNullOrEmpty(SearchCondition.CustomerName))
            {
                sb.Append(" AND i.CustomerName ='").Append(SearchCondition.CustomerName.Trim()).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND i.Warehouse in (").Append(SearchCondition.Warehouse.Trim()).Append(") ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Area))
            {
                sb.Append(" AND i.Area = '").Append(SearchCondition.Area.Trim()).Append("' ");
            }
            if (SearchCondition.InventoryType != 0)
            {
                sb.Append(" AND i.InventoryType = ").Append(SearchCondition.InventoryType);
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND i.CustomerID = ").Append(SearchCondition.CustomerID);
            }
            if (SearchCondition.CustomerID == 0 && SearchCondition.CustomerIDs != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                numbers = SearchCondition.CustomerIDs.Split(',').Select(s => { return s.Trim(); });
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                sb.Append(" and i.CustomerID in ( ");
                foreach (string s in numbers)
                {
                    sb.Append("'").Append(s).Append("',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ) ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.BatchNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.BatchNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.BatchNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.BatchNumber.IndexOf(",") > 0)
                {
                    numbers = SearchCondition.BatchNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" AND i.BatchNumber in (");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND i.BatchNumber LIKE '%" + SearchCondition.BatchNumber.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.Location))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.Location.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.Location.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.Location.IndexOf(",") > 0)
                {
                    numbers = SearchCondition.Location.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" AND i.Location in （");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND i.Location LIKE '%" + SearchCondition.Location.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.SKU))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.SKU.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.SKU.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.SKU.IndexOf(",") > 0)
                {
                    numbers = SearchCondition.SKU.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" AND i.SKU in （");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND i.SKU LIKE '%" + SearchCondition.SKU.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.str3))//门店代码
            {
                sb.Append(" AND i.str3='").Append("" + SearchCondition.str3 + "'");
            }
            return sb.ToString();
        }

        public string GenGetInventorySnapshoot(InventorySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" AND i.InventoryType !=9 ");
            if (!string.IsNullOrEmpty(SearchCondition.CustomerName))
            {
                sb.Append(" AND i.CustomerName ='").Append(SearchCondition.CustomerName.Trim()).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND i.Warehouse in (").Append(SearchCondition.Warehouse.Trim()).Append(") ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND i.CustomerID = ").Append(SearchCondition.CustomerID);
            }
            if (SearchCondition.CustomerID == 0 && SearchCondition.CustomerIDs != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                numbers = SearchCondition.CustomerIDs.Split(',').Select(s => { return s.Trim(); });
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                sb.Append(" and i.CustomerID in ( ");
                foreach (string s in numbers)
                {
                    sb.Append("'").Append(s).Append("',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ) ");
            }
            if (SearchCondition.InventoryDate != null)
            {
                sb.Append(" AND i.InventoryDate='").Append(SearchCondition.InventoryDate.Value.ToString("yyyy/MM/dd")).Append("'");
            }
            else
            {
                sb.Append(" AND i.InventoryDate=''");
            }
            return sb.ToString();
        }

        public string OrderByType(InventorySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (SearchCondition.OrderByType != null)
            {
                if (SearchCondition.OrderByType == "1")
                {
                    sb.Append("order by i.sku desc");
                }
                if (SearchCondition.OrderByType == "2")
                {
                    sb.Append("order by i.sku ");
                }
            }
            else
            {
                sb.Append("order by i.sku desc");
            }
            return sb.ToString();
        }

        public string OrderByType2(InventorySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (SearchCondition.OrderByType != null)
            {
                if (SearchCondition.OrderByType == "1")
                {
                    sb.Append("order by t1.sku desc");
                }
                if (SearchCondition.OrderByType == "2")
                {
                    sb.Append("order by t1.sku ");
                }
            }
            else
            {
                sb.Append("order by t1.sku desc");
            }
            return sb.ToString();
        }

        public GetInventoryBySearchConditionResponse GetInventoryByLocation(string Warehouse, string CustomerID, string location)
        {
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Location",DbType.String,location,ParameterDirection.Input),
                 new DbParam("@CustomerID",DbType.String,CustomerID,ParameterDirection.Input),
                 new DbParam("@Warehouse",DbType.String,Warehouse,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetInventoryByLocation", dbParams);

            response.InventoryCollection = ds.Tables[0].ConvertToEntityCollection<Inventorys>();
            response.ReceiptCollection = ds.Tables[1].ConvertToEntityCollection<Receipt>();
            response.OrderCollection = ds.Tables[2].ConvertToEntityCollection<OrderInfo>();
            response.AdjustCollection = ds.Tables[3].ConvertToEntityCollection<Adjustment>();

            return response;
        }

        public GetInventoryBySearchConditionResponse GetInventoryBySKU(string Warehouse, string CustomerID, string SKU)
        {
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SKU",DbType.String,SKU,ParameterDirection.Input),
                new DbParam("@CustomerID",DbType.String,CustomerID,ParameterDirection.Input),
                new DbParam("@Warehouse",DbType.String,Warehouse,ParameterDirection.Input)

            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetInventoryBySKU", dbParams);

            response.InventoryCollection = ds.Tables[0].ConvertToEntityCollection<Inventorys>();
            response.ReceiptCollection = ds.Tables[1].ConvertToEntityCollection<Receipt>();
            response.OrderCollection = ds.Tables[2].ConvertToEntityCollection<OrderInfo>();
            response.AdjustCollection = ds.Tables[3].ConvertToEntityCollection<Adjustment>();

            return response;
        }

        public GetInventoryBySearchConditionResponse GetPrintByAdjust(string AdjustNumber)
        {
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@adjustnumber",DbType.String,AdjustNumber,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintByAdjust", dbParams);
            response.AdjustDetailCollection = ds.Tables[0].ConvertToEntityCollection<AdjustmentDetail>();
            return response;
        }
        /// <summary>
        /// 库存移动上传数据查询
        /// </summary>
        /// <returns></returns>
        public GetInventoryBySearchConditionResponse GetAkzoInventoryMoveConfirm()
        {
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@adjustnumber",DbType.String,AdjustNumber,ParameterDirection.Input)
            //};
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetAkzoInventoryMoveConfirm");
            response.AdjustCollection = ds.Tables[0].ConvertToEntityCollection<Adjustment>();
            response.AdjustDetailCollection = ds.Tables[1].ConvertToEntityCollection<AdjustmentDetail>();
            return response;
        }

        public GetInventoryBySearchConditionResponse InventoryCompare(DataSet ds, User p, out string message)
        {
            message = "";
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@adjustnumber",DbType.String,AdjustNumber,ParameterDirection.Input)
            //};
            //DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintByAdjust", dbParams);
            DataTable dt = ds.Tables[0];
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Pro_wms_GetInventoryCompareResult", conn);//对比库存存过
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsertData", dt);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@userid", p.ID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@projectid", p.ProjectID);
                cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@customerid", p.CustomerID);
                cmd.Parameters[3].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@warehouseid", p.WarehouseID);
                cmd.Parameters[4].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.Add("@message", SqlDbType.NVarChar, 50);
                cmd.Parameters[5].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataSet dss = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dss);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();

                response.InventoryCompareCollection = dss.Tables[0].ConvertToEntityCollection<InventoryCompare>();
            }
            return response;
        }

        public DataTable GetNikeCEInventorySnapshot()
        {
            DataTable dt = null;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetNikeCEInventorySnapshot", conn);
                cmd.CommandTimeout = 300;
                conn.Open();

                DataSet dss = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dss);
                conn.Close();
                dt = dss.Tables[0];
            }
            return dt;
        }

        /// <summary>
        /// 修改批次
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string UpdateInventoryBatch(AddInventroyRequest request)
        {
            #region 注释
            //using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            //{
            //    string message = "";
            //    try
            //    {
            //        SqlCommand cmd = new SqlCommand("Proc_WSM_UpdateInventoryBactch", conn);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@inventrys", request.inventorys.Select(p => new WMSInventoryToDb(p)));
            //        cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
            //        cmd.Parameters.AddWithValue("@CustomerID", request.inventorys.FirstOrDefault().CustomerID);
            //        cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
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
            #endregion 


            string msg = "";
            int RowCount = 0;
            string SqlStr = "";
            foreach (var item in request.inventorys)
            {
                SqlStr += "  UPDATE dbo.WMS_Inventory_88  SET BatchNumber='" + item.str3 + "',DateTime1='" + item.str4 + "' " +
                          "  WHERE CustomerID=" + item.CustomerID + " AND CustomerName='" + item.CustomerName + "' AND Warehouse='" + item.Warehouse + "' " +
                          "  AND Area='" + item.Area + "' AND Location='" + item.Location + "' AND SKU='" + item.SKU + "' AND GoodsType='" + item.GoodsType + "' AND InventoryType=" + item.InventoryType + " " +
                          "  AND isnull(BatchNumber,'')='" + item.str1 + "' AND isnull(Unit,'')='" + item.str2 + "' ";
            }

            SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString);
            conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand(SqlStr, conn);
            cmd.Transaction = transaction;
            try
            {
                RowCount = cmd.ExecuteNonQuery();
                transaction.Commit();
                if (RowCount <= 0)
                {
                    msg = "没有找到需要修改的库存，请刷新重试！";
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                msg = "数据库更新错误：" + ex.Message.ToString();
            }
            finally
            {
                conn.Close();
                transaction.Dispose();
                conn.Dispose();
            }
            return msg;
        }
    }
}
