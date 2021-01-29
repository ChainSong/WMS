using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.Report;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.Entity.WMS.Receipt;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Entity.WMS.Report;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Entity.WMS.NikeNFSPrint;

namespace Runbow.TWS.Dao.WMS
{
    public class ReportManagementAccessor : BaseAccessor
    {
        public IEnumerable<ReportSku> GetSkuBySearchCondition(ReportSkuSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetSku(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetSkuBySearchCondition_Report", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReportSku>();
        }


        //库存报表
        public IEnumerable<ReportInventory> GetInventoryBySearchCondition(ReportInventorySearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetInventory(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerId", DbType.String, SearchCondition.CustomerID, ParameterDirection.Input),
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventoryBySearchCondition_Report", dbParams);
            RowCount = (int)dbParams[4].Value;
            return dt.ConvertToEntityCollection<ReportInventory>();
        }
        /// <summary>
        /// 查询门店
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<WMS_Customer> GetWMS_CustomerByCustomerID(ReportInventorySearchCondition SearchCondition)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerId", DbType.String, SearchCondition.CustomerID, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetWMS_CustomerByCustomerID", dbParams);
            return dt.ConvertToEntityCollection<WMS_Customer>();
        }
        public GetInventoryBySearchConditionResponse GetNikeReport(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StoreID", DbType.String, StoreID, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.String, CustomerID, ParameterDirection.Input),
                new DbParam("@start_CompleteDate", DbType.String, start_CompleteDate, ParameterDirection.Input),
                new DbParam("@end_CompleteDate", DbType.String, end_CompleteDate, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetNikeDayReportSendToStorer", dbParams);
            //response.NikePackageReportCollcetion= ds.Tables[0].ConvertToEntityCollection<NFSPrintBoxInfo>();
            response.OrderReportCollection = ds.Tables[0].ConvertToEntityCollection<ReprotTableIn>();
            response.InventoryCollection = ds.Tables[1].ConvertToEntityCollection<ReportInventory>();
            response.ReceiptBackReportCollection = ds.Tables[2].ConvertToEntityCollection<ReportReceiptReport>();
            response.ReceiptReportCollection = ds.Tables[3].ConvertToEntityCollection<ReportReceiptReport>();
            response.WMS_CustomerCollection = ds.Tables[4].ConvertToEntityCollection<WMS_Customer>();
            response.EmailConfig = ds.Tables[5].ConvertToEntity<WMS_Customer>();
            return response;
        }
        /// <summary>
        /// 获取NIke每日报表邮件表格中的数据
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public GetInventoryBySearchConditionResponse GetNikeReportEmail(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StoreID", DbType.String, StoreID, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.String, CustomerID, ParameterDirection.Input),
                 new DbParam("@start_CompleteDate", DbType.String, start_CompleteDate, ParameterDirection.Input),
                  new DbParam("@end_CompleteDate", DbType.String, end_CompleteDate, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetNikeDayReportEmailSendToStorer", dbParams);

            response.InventoryTongjiEmailCollection = ds.Tables[0].ConvertToEntityCollection<ReportInventory>();
            response.ReceiptEmailCollection = ds.Tables[1].ConvertToEntityCollection<ReportInventory>();
            response.ReceiptBackEmailCollection = ds.Tables[2].ConvertToEntityCollection<ReportInventory>();
            response.BuHuoEmailCollection = ds.Tables[3].ConvertToEntityCollection<ReportInventory>();
            response.DiaoRuEmailCollection = ds.Tables[4].ConvertToEntityCollection<ReportInventory>();
            response.DiaoChuEmailCollection = ds.Tables[5].ConvertToEntityCollection<ReportInventory>();
            response.AdjustmentEmailCollection = ds.Tables[6].ConvertToEntityCollection<ReportInventory>();
            response.WMS_CustomerCollection = ds.Tables[7].ConvertToEntityCollection<WMS_Customer>();
            response.AdjustmentAddEmailCollection = ds.Tables[8].ConvertToEntityCollection<ReportInventory>();
            response.AdjustmentFrezzEmailCollection = ds.Tables[9].ConvertToEntityCollection<ReportInventory>();
            response.MenDianZhiFaEmailCollection = ds.Tables[10].ConvertToEntityCollection<ReportInventory>();
            response.o2oEmailCollection = ds.Tables[11].ConvertToEntityCollection<ReportInventory>();
            return response;
        }
        /// <summary>
        /// 获取每日邮件中的内容-EpackList
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public GetInventoryBySearchConditionResponse GetEpackListEmail(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StoreID", DbType.String, StoreID, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.String, CustomerID, ParameterDirection.Input),
                new DbParam("@start_CompleteDate", DbType.String, start_CompleteDate, ParameterDirection.Input),
                new DbParam("@end_CompleteDate", DbType.String, end_CompleteDate, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetEpackListEmailSendToStorer", dbParams);

            response.InventoryCollection = ds.Tables[0].ConvertToEntityCollection<ReportInventory>();
            response.WMS_CustomerCollection = ds.Tables[1].ConvertToEntityCollection<WMS_Customer>();
            return response;
        }
        /// <summary>
        /// 获取NikeEpackList
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public GetInventoryBySearchConditionResponse GetNikeEpackListReport(string StoreID, string CustomerID, string DriverName, string DriverTel, string CarNo, string ExpectTime, string start_CompleteDate, string end_CompleteDate)
        {
            GetInventoryBySearchConditionResponse response = new GetInventoryBySearchConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StoreID", DbType.String, StoreID, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.String, CustomerID, ParameterDirection.Input),
                 new DbParam("@DriverName", DbType.String, DriverName, ParameterDirection.Input),
                  new DbParam("@DriverTel", DbType.String, DriverTel, ParameterDirection.Input),
                   new DbParam("@CarNo", DbType.String, CarNo, ParameterDirection.Input),
                    new DbParam("@ExpectTime", DbType.String, ExpectTime, ParameterDirection.Input),
                    new DbParam("@start_CompleteDate", DbType.String, start_CompleteDate, ParameterDirection.Input),
                    new DbParam("@end_CompleteDate", DbType.String, end_CompleteDate, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetNikeEpackListSendToStorerAndInsertStoreReceive", dbParams);
            response.NikePackageReportCollcetion = ds.Tables[0].ConvertToEntityCollection<NFSPrintBoxInfo>();
            response.WMS_CustomerCollection = ds.Tables[1].ConvertToEntityCollection<WMS_Customer>();
            response.EmailConfig = ds.Tables[2].ConvertToEntity<WMS_Customer>();
            return response;
        }
        /// <summary>
        /// 库存汇总查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<ReportInventorySummary> GetInventorySummaryBySearchCondition(ReportInventorySummarySearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetInventorySummary(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@Type", DbType.Int32, (int)SearchCondition.IsLocationBy, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventorySummaryBySearchCondition_Report", dbParams);
            RowCount = (int)dbParams[4].Value;
            return dt.ConvertToEntityCollection<ReportInventorySummary>();
        }

        /// <summary>
        /// 导出库存汇总报表
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public IEnumerable<ReportInventorySummary> ExportInventorySummaryBySearchCondition(ReportInventorySummarySearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetInventorySummary(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@Type", DbType.Int32, (int)SearchCondition.IsLocationBy, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventorySummaryBySearchCondition_ExportReport", dbParams);
            return dt.ConvertToEntityCollection<ReportInventorySummary>();
        }


        /// <summary>
        /// 库存汇总查询条件合并
        /// </summary>
        /// <returns></returns>
        public string GenGetInventorySummary(ReportInventorySummarySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND i.CustomerID='").Append(SearchCondition.CustomerID).Append("' ");
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
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND i.Warehouse in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.Warehouse.Trim()).Append(")) ");
            }

            return sb.ToString();

        }

        //sku进出库日志报表查询
        public IEnumerable<ReportSkuInAndOut> GetSkuInAndOutBySearchCondition(ReportSkuInAndOutSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetSkuInAndOut(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetSkuInAndOutBySearchCondition_Report", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReportSkuInAndOut>();
        }

        /// <summary>
        /// SKU变动表查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<ReportSKUChange> GetSKUChangeBySearchCondition(ReportSKUChangeSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetSKUChange(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetSKUChangeBySearchCondition_Report", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReportSKUChange>();
        }
        /// <summary>
        /// SKU变更报表导出
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public IEnumerable<ReportSKUChange> ExportSKUChangeBySearchCondition(ReportSKUChangeSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetSKUChange(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetSKUChangeBySearchCondition_ExportReport", dbParams);
            return dt.ConvertToEntityCollection<ReportSKUChange>();
        }

        public IEnumerable<ReportInventoryChange> GetInventoryChangeBySearchCondition(ReportInventoryChangeSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetInventoryChange(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventoryChangeBySearchCondition_Report", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReportInventoryChange>();
        }

        //出库报表查询。。。就是他（出库单差异报表也会转到这里）
        public string GenGetReportTableIn(ReportTableSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND Warehouse in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Area))
            {
                sb.Append(" AND Area in ('").Append(SearchCondition.Area.Trim()).Append("') ");
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND  CustomerID = ").Append(SearchCondition.CustomerID);
            }

            if (SearchCondition.StartCompleteDate != null)
            {
                sb.Append(" AND  CompleteDate >='").Append(SearchCondition.StartCompleteDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCompleteDate != null)
            {
                sb.Append(" AND  CompleteDate <='").Append(SearchCondition.EndCompleteDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }


            //根据出库单状态进行查询
            if (!string.IsNullOrEmpty(SearchCondition.Status))
            {
                sb.Append(" AND   Status =").Append(SearchCondition.Status);
            }


            //根据出库类型进行查询
            if (!string.IsNullOrEmpty(SearchCondition.OrderType))
            {
                sb.Append(" AND   OrderType in ('").Append(SearchCondition.OrderType).Append("') ");
            }


            if (SearchCondition.OrderNumber != null)
            {
                sb.Append(" AND  OrderNumber =  '").Append(SearchCondition.OrderNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
            {
                sb.Append(" AND  ExternOrderNumber like '%" + SearchCondition.ExternOrderNumber.Trim() + "%' ");
            }

            //根据库位进行出库单查询
            if (!string.IsNullOrEmpty(SearchCondition.Location))
            {
                sb.Append("AND Location like '%" + SearchCondition.Location.Trim() + "%'");
            }

            //根据 SKU 进行出库单查询
            if (!string.IsNullOrEmpty(SearchCondition.SKU))
            {
                sb.Append("AND SKU like '%" + SearchCondition.SKU.Trim() + "%'");
            }

            //根据 Article 进行出库单查询
            if (!string.IsNullOrEmpty(SearchCondition.Article))
            {
                sb.Append("AND Article like '%" + SearchCondition.Article.Trim() + "%'");
            }

            //根据门店代码进行查询
            if (!string.IsNullOrEmpty(SearchCondition.str4))
            {
                sb.Append("AND str4 like '%" + SearchCondition.str4.Trim() + "%'");
            }


            return sb.ToString();

        }

        //出库报表
        public IEnumerable<ReprotTableIn> ExportReprotTableBySearchCondition(ReportTableSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetReportTableIn(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                    new DbParam("@CustomerID", DbType.String,SearchCondition.CustomerID , ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_ExportReportTableBySearchCondition_Report", dbParams);
            return dt.ConvertToEntityCollection<ReprotTableIn>();
        }

        /// <summary>
        /// 导出出库单报表
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="Proc">存储过程</param>
        /// <returns></returns>
        public IEnumerable<ReprotTableIn> ExportReprotTableBySearchCondition(ReportTableSearchCondition SearchCondition, string Proc)
        {
            string sqlWhere = this.GenGetReportTableIn(SearchCondition);
            if (SearchCondition.CustomerID == 83 || SearchCondition.CustomerID == 79)
            {
                sqlWhere = this.GenGetReportTableInYXDR(SearchCondition);
            }
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                    new DbParam("@CustomerID", DbType.String,SearchCondition.CustomerID , ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable(Proc, dbParams);
            return dt.ConvertToEntityCollection<ReprotTableIn>();
        }

        public IEnumerable<ReprotTableIn> GetReprotTableBySearchCondition(ReportTableSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetReportTableIn(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output),
                new DbParam("@CustomerID", DbType.Int32, SearchCondition.CustomerID, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetReportTableBySearchCondition_Report", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReprotTableIn>();
        }
        /// <summary>
        /// 查询出库单报表
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <param name="proc">存储过程变成可配置</param>
        /// <returns></returns>
        public IEnumerable<ReprotTableIn> GetReprotTableBySearchCondition(ReportTableSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount, string Proc)
        {
            string sqlWhere = this.GenGetReportTableIn(SearchCondition);
            if (SearchCondition.CustomerID == 83 || SearchCondition.CustomerID == 79)
            {
                sqlWhere = this.GenGetReportTableInYXDR(SearchCondition);
            }
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output),
                new DbParam("@CustomerID", DbType.Int32, SearchCondition.CustomerID, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable(Proc, dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReprotTableIn>();
        }

        public string GenGetReportTableInYXDR(ReportTableSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND a.Warehouse in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND  a.CustomerID = ").Append(SearchCondition.CustomerID);
            }
            if (SearchCondition.StartReportDate != null)
            {
                sb.Append(" AND  a.OrderTime >='").Append(SearchCondition.StartReportDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndReportDate != null)
            {
                sb.Append(" AND  a.OrderTime <='").Append(SearchCondition.EndReportDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.OrderNumber != null)
            {
                sb.Append(" AND  a.OrderNumber =  '").Append(SearchCondition.OrderNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
            {
                sb.Append(" AND  a.ExternOrderNumber =  '").Append(SearchCondition.ExternOrderNumber).Append("' ");
            }
            return sb.ToString();

        }
        /// <summary>
        /// 快递单报表导出
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public string GenGetReportExpressInfo(ReportExpressInfoSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND a.Warehouse in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND a.CustomerID = ").Append(SearchCondition.CustomerID);
            }
            //if (SearchCondition.StartReportDate != null)
            //{
            //    sb.Append(" AND  completedate >='").Append(SearchCondition.StartReportDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            //}
            //if (SearchCondition.EndReportDate != null)
            //{
            //    sb.Append(" AND  completedate <='").Append(SearchCondition.EndReportDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            //}
            if (SearchCondition.StartReportDate != null)
            {
                sb.Append(" AND  a.CreateTime >='").Append(SearchCondition.StartReportDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndReportDate != null)
            {
                sb.Append(" AND  a.CreateTime <='").Append(SearchCondition.EndReportDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.OrderNumber != null)
            {
                sb.Append(" AND  a.OrderNumber =  '").Append(SearchCondition.OrderNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
            {
                sb.Append(" AND  a.ExternOrderNumber =  '").Append(SearchCondition.ExternOrderNumber).Append("' ");
            }
            return sb.ToString();

        }
        /// <summary>
        /// 快递单报表导出
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public IEnumerable<ReportExpressInfo> ExportReprotExpressBySearchCondition(ReportExpressInfoSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetReportExpressInfo(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                    new DbParam("@CustomerID", DbType.String,SearchCondition.CustomerID , ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_ExportReportExpressSearchCondition_Report", dbParams);
            return dt.ConvertToEntityCollection<ReportExpressInfo>();
        }
        /// <summary>
        /// 查询快递单报表
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<ReportExpressInfo> GetReprotExpressBySearchCondition(ReportExpressInfoSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetReportExpressInfo(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output),
                new DbParam("@CustomerID", DbType.Int32, SearchCondition.CustomerID, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetReportExpressBySearchCondition_Report", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReportExpressInfo>();
        }

        public IEnumerable<ReportWarehouseStorageDensity> GetWarehouseStorageDensityBySearchCondition(ReportWarehouseStorageDensitySearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetWarehouseStorageDensity(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseStorageDensityBySearchCondition_Report", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReportWarehouseStorageDensity>();
        }

        public IEnumerable<ReportSku> ExportSkuBySearchCondition(ReportSkuSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetSku(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),

            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_ExportSkuBySearchCondition_Report", dbParams);
            return dt.ConvertToEntityCollection<ReportSku>();
        }

        public IEnumerable<ReportInventory> ExportInventoryBySearchCondition(ReportInventorySearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetInventory(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerId", DbType.String, SearchCondition.CustomerID, ParameterDirection.Input),
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),

            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventoryBySearchCondition_ExportReport", dbParams);
            return dt.ConvertToEntityCollection<ReportInventory>();
        }

        public DataTable ExportInventoryBySearchCondition2(ReportInventorySearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetInventory(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),

            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventoryBySearchCondition_ExportReport", dbParams);
            return dt;
        }

        public IEnumerable<ReportSkuInAndOut> ExportSkuInAndOutBySearchCondition(ReportSkuInAndOutSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetSkuInAndOut(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetSkuInAndOutBySearchCondition_ExportReport", dbParams);
            return dt.ConvertToEntityCollection<ReportSkuInAndOut>();
        }

        public IEnumerable<ReportInventoryChange> ExportInventoryChangeBySearchCondition(ReportInventoryChangeSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetInventoryChange(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventoryChangeBySearchCondition_ExportReport", dbParams);
            return dt.ConvertToEntityCollection<ReportInventoryChange>();
        }

        public IEnumerable<ReportWarehouseStorageDensity> ExportWarehouseStorageDensityBySearchCondition(ReportWarehouseStorageDensitySearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetWarehouseStorageDensity(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseStorageDensityBySearchCondition_ExportReport", dbParams);
            return dt.ConvertToEntityCollection<ReportWarehouseStorageDensity>();
        }

        public string GenGetSku(ReportSkuSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND WarehouseName in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND CustomerID = ").Append(SearchCondition.CustomerID);
            }
            if (!string.IsNullOrEmpty(SearchCondition.SkuReportType))
            {
                sb.Append(" AND Types = '").Append(SearchCondition.SkuReportType).Append("' ");
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
                    sb.Append(" AND SKU in (");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND SKU LIKE '%" + SearchCondition.SKU.Trim() + "%' ");
                }
            }
            if (SearchCondition.StartExpectDate != null)
            {
                sb.Append(" AND Dates >='").Append(SearchCondition.StartExpectDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndExpectDate != null)
            {
                sb.Append(" AND Dates <='").Append(SearchCondition.EndExpectDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            return sb.ToString();
        }

        //库存
        public string GenGetInventory(ReportInventorySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.GoodsType))
            {
                sb.Append(" AND i.GoodsType='" + SearchCondition.GoodsType + "'");
            }

            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND  i.Warehouse in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.Warehouse.Trim()).Append(")) ");
                //sb.Append(" AND i.Warehouse in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Area))
            {
                sb.Append(" AND i.Area = '").Append(SearchCondition.Area.Trim()).Append("' ");
            }
            if (SearchCondition.InventoryType != 0)
            {
                sb.Append(" AND i.InventoryType = ").Append(SearchCondition.InventoryType);
            }
            else
            {
                sb.Append(" AND i.InventoryType !=9 ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND i.CustomerID = ").Append(SearchCondition.CustomerID);
            }

            //根据时间查询
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND  i.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND  i.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
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
                    sb.Append(" AND i.SKU in (");
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
                    sb.Append(" AND i.Location in (");
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

            //根据 Article 进行查询库存报表
            if (!string.IsNullOrEmpty(SearchCondition.Article))
            {

                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.Article.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.Article.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.Article.IndexOf(",") > 0)
                {
                    numbers = SearchCondition.Article.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" AND p.str10 in (");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND p.str10 like '%" + SearchCondition.Article.Trim() + "%'");
                }
            }

            //根据 门店代码 进行查询库存报表
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {

                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.str3.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.str3.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.str3.IndexOf(",") > 0)
                {
                    numbers = SearchCondition.str3.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" AND i.str3 in (");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND i.str3 like '%" + SearchCondition.str3.Trim() + "%'");
                }


            }

            return sb.ToString();
        }

        public string GenGetSkuInAndOut(ReportSkuInAndOutSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            //if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            //{
            //    sb.Append(" AND Warehouse in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            //}
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND CustomerID = ").Append(SearchCondition.CustomerID);
            }
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND Warehouse in (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.Warehouse.Trim()).Append(")) ");

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
                    sb.Append(" AND SKU in (");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND SKU = '" + SearchCondition.SKU.Trim() + "' ");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.Article))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.Article.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.Article.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.Article.IndexOf(",") > 0)
                {
                    numbers = SearchCondition.Article.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" AND Article in (");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND Article LIKE '%" + SearchCondition.Article.Trim() + "%' ");
                }
            }

            //根据门店代码进行查询SKU进出库日志报表
            if (!string.IsNullOrEmpty(SearchCondition.storerkey))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.storerkey.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.storerkey.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.storerkey.IndexOf(",") > 0)
                {
                    numbers = SearchCondition.storerkey.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" AND s.storerkey in (");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND s.storerkey LIKE '%" + SearchCondition.storerkey.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(SearchCondition.Size))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.Size.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.Size.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.Size.IndexOf(",") > 0)
                {
                    numbers = SearchCondition.Size.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" AND Size in (");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND Size LIKE '%" + SearchCondition.Size.Trim() + "%' ");
                }
            }



            //
            if (!string.IsNullOrEmpty(SearchCondition.BatchNumber))
            {
                sb.Append(" AND BatchNumber LIKE '%" + SearchCondition.BatchNumber.Trim() + "%'");
            }

            if (SearchCondition.BeginCreateTime != null)
            {
                sb.Append(" AND CreateTime >='").Append(SearchCondition.BeginCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public string GenGetSKUChange(ReportSKUChangeSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND  s.Warehouse in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.Warehouse.Trim()).Append(")) ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND s.CustomerID = ").Append(SearchCondition.CustomerID);
            }
            if (!string.IsNullOrEmpty(SearchCondition.SKU))
            {
                sb.Append(" AND s.SKU = '" + SearchCondition.SKU.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.BatchNumber))
            {
                sb.Append(" AND s.BatchNumber=  '" + SearchCondition.BatchNumber.Trim() + "'");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ToLocation))
            {
                sb.Append(" AND s.ToLocation='" + SearchCondition.ToLocation.Trim() + "'");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" and s.CreateTime >= '").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:ss.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" and s.CreateTime <= '").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:59.99")).Append("' ");
            }
            return sb.ToString();
        }

        public string GenGetInventoryChange(ReportInventoryChangeSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                //sb.Append(" AND Warehouse in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
                sb.Append(" AND  Warehouse in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.Warehouse.Trim()).Append(")) ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.InventoryChangeTypes))
            {
                //if (SearchCondition.InventoryChangeTypes == "1")//adjustment
                //{
                //    sb.Append(" AND AdjustmentType ='库存调整单' ");
                //}
                //else if (SearchCondition.InventoryChangeTypes == "2")//hold
                //{
                //    sb.Append(" AND AdjustmentType ='库存冻结单' ");
                //}
                //else//move
                //{
                //    sb.Append(" AND AdjustmentType ='库存移动单' ");
                //}
                sb.Append(" AND AdjustmentType ='" + SearchCondition.InventoryChangeTypes + "' ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND CustomerID = ").Append(SearchCondition.CustomerID);
            }

            //根据门店代码进行查询
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.str3.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.str3.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.str3.IndexOf(",") > 0)
                {
                    numbers = SearchCondition.str3.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" AND s.str3 in (");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND s.str3 LIKE '%" + SearchCondition.str3.Trim() + "%' ");
                }
            }


            //if (!string.IsNullOrEmpty(SearchCondition.Location))
            //{
            //    IEnumerable<string> numbers = Enumerable.Empty<string>();
            //    if (SearchCondition.Location.IndexOf("\n") > 0)
            //    {
            //        numbers = SearchCondition.Location.Split('\n').Select(s => { return s.Trim(); });
            //    }
            //    if (SearchCondition.Location.IndexOf(",") > 0)
            //    {
            //        numbers = SearchCondition.Location.Split(',').Select(s => { return s.Trim(); });
            //    }
            //    if (numbers != null && numbers.Any())
            //    {
            //        numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            //    }
            //    if (numbers != null && numbers.Any())
            //    {
            //        sb.Append(" AND Location in (");
            //        foreach (string s in numbers)
            //        {
            //            sb.Append("'").Append(s).Append("',");
            //        }
            //        sb.Remove(sb.Length - 1, 1);
            //        sb.Append(" ) ");
            //    }
            //    else
            //    {
            //        sb.Append(" AND Location LIKE '%" + SearchCondition.Location.Trim() + "%' ");
            //    }
            //}
            return sb.ToString();
        }

        public string GenGetWarehouseStorageDensity(ReportWarehouseStorageDensitySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND Warehouse in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND CustomerID = ").Append(SearchCondition.CustomerID);
            }
            //if (!string.IsNullOrEmpty(SearchCondition.Location))
            //{
            //    IEnumerable<string> numbers = Enumerable.Empty<string>();
            //    if (SearchCondition.Location.IndexOf("\n") > 0)
            //    {
            //        numbers = SearchCondition.Location.Split('\n').Select(s => { return s.Trim(); });
            //    }
            //    if (SearchCondition.Location.IndexOf(",") > 0)
            //    {
            //        numbers = SearchCondition.Location.Split(',').Select(s => { return s.Trim(); });
            //    }
            //    if (numbers != null && numbers.Any())
            //    {
            //        numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            //    }
            //    if (numbers != null && numbers.Any())
            //    {
            //        sb.Append(" AND Location in (");
            //        foreach (string s in numbers)
            //        {
            //            sb.Append("'").Append(s).Append("',");
            //        }
            //        sb.Remove(sb.Length - 1, 1);
            //        sb.Append(" ) ");
            //    }
            //    else
            //    {
            //        sb.Append(" AND Location LIKE '%" + SearchCondition.Location.Trim() + "%' ");
            //    }
            //}
            return sb.ToString();
        }

        public IEnumerable<ReceiptPrint> PrintUpdateReceipt(long ID, IList<ReceiptDetail> request)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateReceiptDetailByReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@ReceiptDetail", request.Select(receipt => new WMSReceiptDetailToDb(receipt)));
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                //if (message == "更新成功")
                return dt.ConvertToEntityCollection<ReceiptPrint>();
                //return message;

            }
        }

        public IEnumerable<ReceiptPrint> GetPrintLabel(IList<ReceiptDetail> request)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_WMS_GetReceiptDetailLabelByReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReceiptDetail", request.Select(receipt => new WMSReceiptDetailToDb(receipt)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                //if (message == "更新成功")
                return dt.ConvertToEntityCollection<ReceiptPrint>();
                //return message;

            }
        }

        public IEnumerable<ReceiptPrint> GetPrintLabelInventorySearch(IList<ReceiptDetail> request)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_WMS_GetInventoryLabelByReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReceiptDetail", request.Select(receipt => new WMSReceiptDetailToDb(receipt)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                //if (message == "更新成功")
                return dt.ConvertToEntityCollection<ReceiptPrint>();
                //return message;

            }
        }

        public string GetLocationMax(string Location, long warehouseid)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_WMS_GetLocationMax", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Location", Location);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[0].Size = 100;
                cmd.Parameters.AddWithValue("@warehouseid", warehouseid);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return message;
            }
        }
        /// <summary>
        /// NIke每日报表发送成功之后插入表
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string InsertReportSendEmail(string StoreID, string CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_InsertWMS_NikeReportSendEmail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StoreID", StoreID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 100;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 100;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                return message;

            }
        }
        /// <summary>
        ///  NIke EpackList发送成功之后插入表
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string InsertEpackListSendEmail(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_InsertWMS_NikeEpackListSendEmailSendEmail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StoreID", StoreID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 100;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 100;
                    cmd.Parameters.AddWithValue("@start_CompleteDate", start_CompleteDate);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters.AddWithValue("@end_CompleteDate", end_CompleteDate);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Size = 100;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                return message;

            }
        }
        public IEnumerable<ReceiptPrint> PrintUpdateNumber(long ID, string BoxNumber, long EverySum)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                //try
                //{
                SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateReceiptDetailNumberByReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@BoxNumber", BoxNumber);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Size = 100;
                cmd.Parameters.AddWithValue("@EverySum", EverySum);
                cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                cmd.Parameters[3].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return dt.ConvertToEntityCollection<ReceiptPrint>();
                //}
                //catch (Exception ex)
                //{
                //    return message + "(" + ex.Message + ")";
                //}
            }
        }
        /// <summary>
        /// 出库单差异报表查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public IEnumerable<ReprotTableIn> GetReprotTableDifferenceBySearch(ReportTableSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount, string Proc)
        {
            string sqlWhere = this.GenGetReportTableDifference(SearchCondition);

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output),
                new DbParam("@CustomerID", DbType.Int32, SearchCondition.CustomerID, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable(Proc, dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReprotTableIn>();
        }
        /// <summary>
        /// 导出出库单差异报表
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="Proc">存储过程</param>
        /// <returns></returns>
        public IEnumerable<ReprotTableIn> ExportReprotTableDifferenceBySearch(ReportTableSearchCondition SearchCondition, string Proc)
        {
            string sqlWhere = this.GenGetReportTableDifference(SearchCondition);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                    new DbParam("@CustomerID", DbType.String,SearchCondition.CustomerID , ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable(Proc, dbParams);
            return dt.ConvertToEntityCollection<ReprotTableIn>();
        }

        public string GenGetReportTableDifference(ReportTableSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND a.Warehouse in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND  a.CustomerID = ").Append(SearchCondition.CustomerID);
            }
            if (SearchCondition.StartReportDate != null)
            {
                sb.Append(" AND  a.CompleteDate >='").Append(SearchCondition.StartReportDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndReportDate != null)
            {
                sb.Append(" AND  a.CompleteDate <='").Append(SearchCondition.EndReportDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.OrderNumber != null)
            {
                sb.Append(" AND  a.OrderNumber =  '").Append(SearchCondition.OrderNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
            {
                sb.Append(" AND  a.ExternOrderNumber =  '").Append(SearchCondition.ExternOrderNumber).Append("' ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 导出入库单差异报表
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="Proc">存储过程</param>
        /// <returns></returns>
        public IEnumerable<ReportReceiptReport> ExportReportReceiptDifferBySearch(ReceiptSearchCondition SearchCondition, string Proc)
        {
            string sqlWhere = this.GenGetReportReceiptDifference(SearchCondition);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                    new DbParam("@CustomerID", DbType.String,SearchCondition.CustomerID , ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable(Proc, dbParams);
            return dt.ConvertToEntityCollection<ReportReceiptReport>();
        }
        /// <summary>
        /// 入库单差异报表查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public IEnumerable<ReportReceiptReport> GetReportReceiptDiffBySearch(ReceiptSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount, string Proc)
        {
            string sqlWhere = this.GenGetReportReceiptDifference(SearchCondition);

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output),
                new DbParam("@CustomerID", DbType.Int32, SearchCondition.CustomerID, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable(Proc, dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ReportReceiptReport>();
        }

        public string GenGetReportReceiptDifference(ReceiptSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND a.WarehouseName in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND  a.CustomerID = ").Append(SearchCondition.CustomerID);
            }
            if (SearchCondition.StartCompleteDate != null)
            {
                sb.Append(" AND  a.CompleteDate >='").Append(SearchCondition.StartCompleteDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCompleteDate != null)
            {
                sb.Append(" AND  a.CompleteDate <='").Append(SearchCondition.EndCompleteDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.ReceiptNumber != null)
            {
                sb.Append(" AND  a.ReceiptNumber =  '").Append(SearchCondition.ReceiptNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternReceiptNumber))
            {
                sb.Append(" AND  a.ExternReceiptNumber =  '").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }



            return sb.ToString();
        }

        /// <summary>
        /// 门店库存查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<ReportInventory> GetInventoryStorerBySearchCondition(ReportInventorySearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetInventoryStorer(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.String,SearchCondition.CustomerID.ToString(), ParameterDirection.Input),
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetInventoryStorerBySearchCondition", dbParams);
            RowCount = (int)dbParams[4].Value;
            return dt.ConvertToEntityCollection<ReportInventory>();
        }

        /// <summary>
        /// 门店库存查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public string GenGetInventoryStorer(ReportInventorySearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND  i.Warehouse in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.Warehouse.Trim()).Append(")) ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Area))//如果后面这个用户想要看到多个库区的货，那么要写成仓库那样的拼接格式
            {
                sb.Append(" AND i.Area = '").Append(SearchCondition.Area.Trim()).Append("' ");
            }

            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND i.CustomerID = ").Append(SearchCondition.CustomerID);
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
                    sb.Append(" AND i.SKU in (");
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
            if (!string.IsNullOrEmpty(SearchCondition.Article))
            {
                sb.Append(" and p.str10 like'%" + SearchCondition.Article.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Size))
            {
                sb.Append(" and p.str9 like '%" + SearchCondition.Size.Trim() + "%'");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 门店库存导出查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public IEnumerable<ReportInventory> ExportInventoryStorerBySearchCondition(ReportInventorySearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetInventoryStorer(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                 new DbParam("@CustomerID", DbType.String, SearchCondition.CustomerID.ToString(), ParameterDirection.Input),
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),

            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_ExportInventoryStorerBySearchCondition_ExportReport", dbParams);
            return dt.ConvertToEntityCollection<ReportInventory>();
        }


        /// <summary>
        /// 查询工时统计
        /// </summary>
        /// <param name="request"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<WMS_WorkingStatis> GetWorkingStatisBySearchCondition(WorkingSearchCondition request, int PageIndex, int PageSize, out int RowCount)
        {
            string SqlWhere = GenWorkingStatis(request);
            int count = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, SqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, count, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetWorkingStatisBySearchCondition", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<WMS_WorkingStatis>();

        }

        public string GenWorkingStatis(WorkingSearchCondition request)
        {
            StringBuilder sb = new StringBuilder();
            if (request.CustomerID != 0)
            {
                sb.Append(" AND w.CustomerID = ").Append(request.CustomerID);
            }
            if (!string.IsNullOrEmpty(request.WarehouseName))
            {
                sb.Append(" AND  w.WarehouseName in  (select WarehouseName from wms_warehouse where id in ( ").Append(request.WarehouseName.Trim()).Append(")) ");
            }
            if (request.BeginCreateTime != null)
            {
                sb.Append(" AND w.CreateTime >='").Append(request.BeginCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:ss.00")).Append("' ");
            }
            if (request.EndCreateTime != null)
            {
                sb.Append(" AND w.CreateTime <='").Append(request.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }


            return sb.ToString();
        }


        /// <summary>
        /// 新增工时统计
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool CreateWorkingStatis(WorkingSearchCondition searchCondition)
        {
            //先判断今天是否已经新增了工时    
            string sql = "";
            sql = " SELECT ID,CustomerID,CustomerName,CustomerName,WarehouseID,StatisDate FROM dbo.WMS_WorkingStatis WHERE CustomerID=" + searchCondition.CustomerID + "" +
                  " AND WarehouseID=" + searchCondition.WarehouseID + " and  StatisDate='" + searchCondition.StatisDate + "'";
            var response = this.ExecuteDataTableBySqlString(sql).ConvertToEntityCollection<WMS_WorkingStatis>();
            if (response != null && response.Any())
            {
                //存在则更新
                sql = " UPDATE dbo.WMS_WorkingStatis  SET PersonNumber='" + searchCondition.PersonNumber + "',WorkHour='" + searchCondition.WorkHour + "'," +
                      " Updator='" + searchCondition.Creator + "',UpdateTime=getdate() where  ID= " + response.FirstOrDefault().ID;
            }
            else
            {

                sql = " INSERT INTO dbo.WMS_WorkingStatis" +
                      " (WorkingNumber, CustomerID, CustomerName, WarehouseID, WarehouseName, PersonNumber, WorkHour, Creator, CreateTime, Updator, UpdateTime," +
                       " StatisDate)VALUES('" + searchCondition.WorkingNumber + "'," + searchCondition.CustomerID + ",'" + searchCondition.CustomerName + "'," + searchCondition.WarehouseID + "," +
                       " '" + searchCondition.WarehouseName + "','" + searchCondition.PersonNumber + "','" + searchCondition.WorkHour + "','" + searchCondition.Creator + "',getdate(),'" + searchCondition.Creator + "'," +
                       " getdate(),'" + searchCondition.StatisDate + "' ) ";
            }
            return this.ScanExecuteNonQuery(sql) > 0;

        }

        /// <summary>
        /// 更新工时统计
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool UpdateWorkingStatis(WorkingSearchCondition searchCondition)
        {
            string sql = "";

            sql = "  UPDATE dbo.WMS_WorkingStatis SET PersonNumber='" + searchCondition.PersonNumber + "',WorkHour='" + searchCondition.WorkHour + "'," +
                  "  Updator='" + searchCondition.Creator + "',UpdateTime=getdate() WHERE ID=" + searchCondition.ID + " ";
            return this.ScanExecuteNonQuery(sql) > 0;

        }

        /// <summary>
        /// 获取进程列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<WMS_ProcessTracking> GetProcessTrackingList(ProcessTrackingSearchCondition request, out int rowCount)
        {
            int tempRowCount = 0;
            string Where = this.GenGetProcessTrackingWhere(request);
            IEnumerable<WMS_ProcessTracking> response;
            DbParam[] dbParams = new DbParam[] {
            new DbParam("@Where",DbType.String,Where,ParameterDirection.Input),
            new DbParam("@PageIndex",DbType.Int32,request.PageIndex,ParameterDirection.Input),
            new DbParam("@PageSize",DbType.Int32,request.PageSize,ParameterDirection.Input),
            new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetProcessTrackingList", dbParams);
            rowCount = (int)dbParams[3].Value;
            response = ds.Tables[0].ConvertToEntityCollection<WMS_ProcessTracking>();
            return response;
        }

        /// <summary>
        /// 导出进程列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<WMS_ProcessTracking> ExportProcessTracking(ProcessTrackingSearchCondition request)
        {
            string Where = this.GenGetProcessTrackingWhere(request);
            IEnumerable<WMS_ProcessTracking> response;
            DbParam[] dbParams = new DbParam[] {
            new DbParam("@Where",DbType.String,Where,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_ExportProcessTracking", dbParams);
            response = ds.Tables[0].ConvertToEntityCollection<WMS_ProcessTracking>();
            return response;
        }

        private string GenGetProcessTrackingWhere(ProcessTrackingSearchCondition request)
        {
            StringBuilder sb = new StringBuilder();
            if (request.CustomerID != 0)
            {
                sb.Append(" AND p.CustomerID=").Append(request.CustomerID).Append(" ");
            }
            if (!string.IsNullOrEmpty(request.StorerKey))
            {
                sb.Append(" AND p.StorerKey='").Append(request.StorerKey).Append("' ");
            }
            if (request.Type != null)
            {
                sb.Append(" AND p.Type=").Append(request.Type).Append(" ");
            }
            if (request.StartCreateTime != null)
            {
                sb.Append(" AND p.CreateTime >='").Append(DateTime.Parse(request.StartCreateTime).DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (request.EndCreateTime != null)
            {
                sb.Append(" AND p.CreateTime <='").Append(DateTime.Parse(request.EndCreateTime).DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
                //sb.Append(" AND s.CreateTime <='").Append(request.EndCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:59.99")).Append("' ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// RSO质检报告查询
        /// </summary>
        /// <param name="Searchcondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<InspectionReport> GetInspectionReportBySearchCondition(InspectionReportSearchCondition Searchcondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = GetInspectionReportSqlWhere(Searchcondition);
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@SqlWhere",DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output)
            };
            DataTable dt = base.ExecuteDataTable("Proc_GetInspectionReportBySearchCondition", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<InspectionReport>();
        }

        public IEnumerable<InspectionReport> ExportInspectionReportBySearchCondition(InspectionReportSearchCondition SearchCondition)
        {
            string sqlWhere = GetInspectionReportSqlWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@SqlWhere",DbType.String,sqlWhere,ParameterDirection.Input)
            };
            DataTable dt = base.ExecuteDataTable("Proc_ExportInspectionReportBySearchCondition", dbParams);
            return dt.ConvertToEntityCollection<InspectionReport>();
        }

        public string GetInspectionReportSqlWhere(InspectionReportSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.ASNNumber))
            {
                IEnumerable<string> asns = Enumerable.Empty<string>();
                if (SearchCondition.ASNNumber.IndexOf("\n") > 0)
                {
                    asns = SearchCondition.ASNNumber.Split('\n').Select(i => { return i.Trim(); });
                }
                if (SearchCondition.ASNNumber.IndexOf(",") > 0)
                {
                    asns = SearchCondition.ASNNumber.Split(',').Select(i => { return i.Trim(); });
                }
                if (asns != null && asns.Any())
                {
                    sb.Append(" AND A.ASNNumber in ( ");
                    foreach (var a in asns)
                    {
                        sb.Append("'").Append(a).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" )");
                }
                else
                    sb.Append(" AND A.ASNNumber LIKE '%" + SearchCondition.ASNNumber.Trim() + "%'");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternReceiptNumber))
            {
                IEnumerable<string> erns = Enumerable.Empty<string>();
                if (SearchCondition.ExternReceiptNumber.IndexOf("\n") > 0)
                {
                    erns = SearchCondition.ExternReceiptNumber.Split('\n').Select(i => { return i.Trim(); });
                }
                if (SearchCondition.ExternReceiptNumber.IndexOf(',') > 0)
                {
                    erns = SearchCondition.ExternReceiptNumber.Split(',').Select(i => { return i.Trim(); });
                }
                if (erns != null && erns.Any())
                {
                    sb.Append(" AND A.ExternReceiptNumber in ( ");
                    foreach (var e in erns)
                    {
                        sb.Append("'").Append(e).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                    sb.Append(" AND A.ExternReceiptNumber LIKE '%" + SearchCondition.ExternReceiptNumber.Trim() + "%'");
            }
            if (!string.IsNullOrEmpty(SearchCondition.SKU))
            {
                IEnumerable<string> skus = Enumerable.Empty<string>();
                if (SearchCondition.SKU.IndexOf("\n") > 0)
                {
                    skus = SearchCondition.SKU.Split('\n').Select(i => { return i.Trim(); });
                }
                if (SearchCondition.SKU.IndexOf(",") > 0)
                {
                    skus = SearchCondition.SKU.Split(',').Select(i => { return i.Trim(); });
                }
                if (skus != null && skus.Any())
                {
                    sb.Append(" AND A.SKU in ( ");
                    foreach (var s in skus)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                    sb.Append(" AND A.SKU LIKE '%" + SearchCondition.SKU.Trim() + "%'");
            }
            if (SearchCondition.CustomerID.HasValue)
            {
                sb.Append(" AND A.CustomerID=" + SearchCondition.CustomerID.Value);
            }
            if (SearchCondition.WareHouseID.HasValue)
            {
                sb.Append(" AND A.WareHouseID=" + SearchCondition.WareHouseID.Value);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 查询出库耗材报表
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <param name="proc">存储过程变成可配置</param>
        /// <returns></returns>
        public IEnumerable<ReprotTableIn> GetOrderConsumable(ReportTableSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount, string Proc)
        {
            try
            {
                string sqlWhere = this.GenGetOrderConsumable(SearchCondition);
                if (SearchCondition.CustomerID == 83 || SearchCondition.CustomerID == 79)
                {
                    sqlWhere = this.GenGetReportTableInYXDR(SearchCondition);
                }
                int tempRowCount = 0;
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output),
                new DbParam("@CustomerID", DbType.Int32, SearchCondition.CustomerID, ParameterDirection.Input)
            };
                DataTable dt = this.ExecuteDataTable(Proc, dbParams);
                RowCount = (int)dbParams[3].Value;
                return dt.ConvertToEntityCollection<ReprotTableIn>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GenGetOrderConsumable(ReportTableSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND o.Warehouse in ('").Append(SearchCondition.Warehouse.Trim()).Append("') ");
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND  o.CustomerID = ").Append(SearchCondition.CustomerID);
            }
            if (SearchCondition.StartCompleteDate != null)
            {
                sb.Append(" AND  o.CompleteDate >='").Append(SearchCondition.StartCompleteDate.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCompleteDate != null)
            {
                sb.Append(" AND  o.CompleteDate <='").Append(SearchCondition.EndCompleteDate.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }

            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND  o.OrderTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND  o.OrderTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }

            if (SearchCondition.OrderNumber != null)
            {
                sb.Append(" AND  p.OrderNumber =  '").Append(SearchCondition.OrderNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
            {
                sb.Append(" AND  p.ExternOrderNumber like '%" + SearchCondition.ExternOrderNumber.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.Status))
            {
                sb.Append(" AND  o.Status =  '").Append(SearchCondition.Status).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.OrderType))
            {
                sb.Append(" AND  o.OrderType =  '").Append(SearchCondition.OrderType).Append("' ");
            }
            return sb.ToString();

        }

        /// <summary>
        /// 导出出库耗材报表
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="Proc">存储过程</param>
        /// <returns></returns>
        public IEnumerable<ReprotTableIn> ExportGetOrderConsumable(ReportTableSearchCondition SearchCondition, string Proc)
        {
            string sqlWhere = this.GenGetOrderConsumable(SearchCondition);
            if (SearchCondition.CustomerID == 83 || SearchCondition.CustomerID == 79)
            {
                sqlWhere = this.GenGetReportTableInYXDR(SearchCondition);
            }
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                    new DbParam("@CustomerID", DbType.String,SearchCondition.CustomerID , ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable(Proc, dbParams);
            return dt.ConvertToEntityCollection<ReprotTableIn>();
        }


    }
}
