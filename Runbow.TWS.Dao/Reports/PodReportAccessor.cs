using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.MobileWeb;
using Runbow.TWS.Entity.Reports;
using Runbow.TWS.MessageContracts.Reports;

namespace Runbow.TWS.Dao
{
    public class PodReportAccessor : BaseAccessor
    {
        public IEnumerable<CustomerPodYearCount> QueryCustomerYearPodCount(DateTime StartTime, DateTime EndTime)
        {

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StartTime", DbType.DateTime, StartTime, ParameterDirection.Input),
                new DbParam("@EndTime", DbType.DateTime, EndTime, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_QueryCustomerPodCountByTimeRange", dbParams).ConvertToEntityCollection<CustomerPodYearCount>();
        }

        public IEnumerable<CustomerPodMonthlyAndDayCount> QueryCustomerMonthlyAndDailyPodCount(DateTime StartTime, DateTime EndTime, long CustomerID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StartTime", DbType.DateTime, StartTime, ParameterDirection.Input),
                new DbParam("@EndTime", DbType.DateTime, EndTime, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.Int64, CustomerID, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_QueryCustomerMonthlyAndDailyPodCount", dbParams).ConvertToEntityCollection<CustomerPodMonthlyAndDayCount>();
        }

        public IEnumerable<PodRegionCount> QueryCustomerPodCountByRegionAndTime(DateTime StartTime, DateTime EndTime, long CustomerID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StartTime", DbType.DateTime, StartTime, ParameterDirection.Input),
                new DbParam("@EndTime", DbType.DateTime, EndTime, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.Int64, CustomerID, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_QueryCustomerPodCountByRegionAndTime", dbParams).ConvertToEntityCollection<PodRegionCount>();
        }

        public IEnumerable<CustomerPodYearMonthCount> QueryCustomerPodYearMonthCountByTimeRange(DateTime StartTime, DateTime EndTime, long ShipperID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StartTime", DbType.DateTime, StartTime, ParameterDirection.Input),
                new DbParam("@EndTime", DbType.DateTime, EndTime, ParameterDirection.Input),
                new DbParam("@ShipperID", DbType.Int64, ShipperID, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_QueryCustomerPodYearMonthCountByTimeRange", dbParams).ConvertToEntityCollection<CustomerPodYearMonthCount>();
        }


        public IEnumerable<BTBMoneySummaryReport> BTBMoneySummaryReport()//, out int RowCount
        {
            //string SqlWhere = GenQueryAttachmentSql(Request);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, " and [CustomerName] ='新宏阳'", ParameterDirection.Input),
               // new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                //new DbParam("@EndTime", DbType.DateTime, EndTime, ParameterDirection.Input),
                //new DbParam("@ShipperID", DbType.Int64, ShipperID, ParameterDirection.Input)
            };
            //RowCount = (int)dbParams[3].Value;
            return base.ExecuteDataTable("Proc_GetBTBMoneySummaryReport", dbParams).ConvertToEntityCollection<BTBMoneySummaryReport>();
        }
        public IEnumerable<TransOrder> QueryTransOrderRange(TransOrderRequest Request)//, out int RowCount
        {
            string SqlWhere = GenQueryAttachmentSql(Request);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, SqlWhere, ParameterDirection.Input),
               // new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                //new DbParam("@EndTime", DbType.DateTime, EndTime, ParameterDirection.Input),
                //new DbParam("@ShipperID", DbType.Int64, ShipperID, ParameterDirection.Input)
            };
            //RowCount = (int)dbParams[3].Value;
            return base.ExecuteDataTable("Proc_OrderToNull", dbParams).ConvertToEntityCollection<TransOrder>();
        }


        public IEnumerable<TransOrder> QueryTransOrderRanges(TransOrderRequest Request, out int RowCount)//, 
        {
            string SqlWhere = GenQueryAttachmentSql(Request);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, SqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, Request.PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, Request.PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_OrderToNullFen", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<TransOrder>();
            //base.ExecuteDataTable("Proc_OrderToNullFen", dbParams).ConvertToEntityCollection<TransOrder>();
        }
        /// <summary>
        /// 查询装箱单号拼接sql
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="Customers"></param>
        /// <returns></returns>
        private string GenQueryAttachmentSql(TransOrderRequest Request)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Request.ID))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (Request.ID.IndexOf("\n") > 0)
                {
                    numbers = Request.ID.Split('\n').Select(s => { return s.Trim(); });
                }
                if (Request.ID.IndexOf(',') > 0)
                {
                    numbers = Request.ID.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and a.CustomerOrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and a.CustomerOrderNumber  like '%" + Request.ID.Trim() + "%' ");
                }
            }
            if (Request.Customers != "0" && !string.IsNullOrEmpty(Request.Customers))//
            {
                string CustomerName = "";
                switch (Request.Customers)
                {
                    case "1":
                        CustomerName = "Adidas";
                        break;
                    case "2":
                        CustomerName = "Nike";
                        break;
                    case "3":
                        CustomerName = "Hilti";
                        break;
                    case "4":
                        CustomerName = "AKZO";
                        break;
                    case "5":
                        CustomerName = "Dowcorning";
                        break;
                    default:
                        CustomerName = Request.Customers + "%' or a.CustomerOrderNumber like'%" + Request.Customers + "%' or EndCityName like '%" + Request.Customers + "%' or ShipperName like '%" + Request.Customers;
                        break;
                }
                sb.Append("and (CustomerName like  '%" + CustomerName + "%')");
            }
            if (!string.IsNullOrEmpty(Request.ShipperName))
            {
                sb.Append(" and ShipperName='" + Request.ShipperName + "'");
            }

            if (!string.IsNullOrEmpty(Request.startCityTreeName))
            {
                sb.Append("and a.StartCityName='" + Request.startCityTreeName + "'");
            }
            if (!string.IsNullOrEmpty(Request.endCityTreeName))
            {
                sb.Append("and a.EndCityName='" + Request.endCityTreeName + "'");
            }
            if (!string.IsNullOrEmpty(Request.StartTime))
            {
                sb.Append("and ActualDeliveryDate>='" + Request.StartTime + "'");
            }
            if (!string.IsNullOrEmpty(Request.StartTime))
            {
                sb.Append("and ActualDeliveryDate<'" + Request.EndTime + " 23:59:59'");

            }
            if (!string.IsNullOrEmpty(Request.Time))
            {
                sb.Append("and CONVERT(varchar(10), ActualDeliveryDate, 120) ='" + Request.Time + "'");

            }
            return sb.ToString();
        }
        public IEnumerable<IncomAndExpenses> QueryIncomAndExpenses(DateTime StartTime, DateTime EndTime, long CustomerID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StartTime", DbType.DateTime, StartTime, ParameterDirection.Input),
                new DbParam("@EndTime", DbType.DateTime, EndTime, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.Int64, CustomerID, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_QueryIncomAndExpenses", dbParams).ConvertToEntityCollection<IncomAndExpenses>();
        }

        public IEnumerable<ShipperCost> QueryShipperCost(DateTime StartTime, DateTime EndTime, long ShipperID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@StartTime", DbType.DateTime, StartTime, ParameterDirection.Input),
                new DbParam("@EndTime", DbType.DateTime, EndTime, ParameterDirection.Input),
                new DbParam("@ShipperID", DbType.Int64, ShipperID, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_QueryShipperCost", dbParams).ConvertToEntityCollection<ShipperCost>();
        }
        public IEnumerable<ShowCaseHotMap> ShowCaseHotMap(ShowCaseHotMapRequest Request)
        {
            string SqlWhere = "";
            if (Request != null)
            {
                SqlWhere = Where(Request);
            }
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, SqlWhere, ParameterDirection.Input),
                 new DbParam("@City", DbType.String, Request.City, ParameterDirection.Input),
             
            };
            if (Request.HotMapType == 0)
            {
                return base.ExecuteDataTable("Proc_ShowCaseHotMap", dbParams).ConvertToEntityCollection<ShowCaseHotMap>();
            }
            else
            {
                return base.ExecuteDataTable("Proc_ShowCaseHotMapType", dbParams).ConvertToEntityCollection<ShowCaseHotMap>();
            }
        }
        private string Where(ShowCaseHotMapRequest Request)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("and CustomerId =" + Request.Customer + "");
            if (!string.IsNullOrEmpty(Request.City))
            {
                sb.Append("and EndCityName ='" + Request.City + "'");
            }
            if (!string.IsNullOrEmpty(Request.StartActualDeliveryDate))
            {
                sb.Append("and ActualDeliveryDate >='" + Request.StartActualDeliveryDate + "'");
            }
            if (!string.IsNullOrEmpty(Request.EndActualDeliveryDate))
            {
                sb.Append("and ActualDeliveryDate <'" + Request.EndActualDeliveryDate + " 23:59:59'");
            }
            return sb.ToString();
        }
    }
}
