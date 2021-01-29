using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.SettlementManagement
{
    public class WMS_HiltibjSettled
    {
        [EntityPropertyExtension("ID", "ID")]
        public int ID { get; set; }
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public int WarehouseID { get; set; }
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        [EntityPropertyExtension("SettlementNumber", "SettlementNumber")]
        public string SettlementNumber { get; set; }
        //public int SettlementType { get; set; }
        //public string SettlementMonth { get; set; }
        [EntityPropertyExtension("OutsourcingTotalSum", "OutsourcingTotalSum")]
        public decimal? OutsourcingTotalSum { get; set; } //包干总费用
        [EntityPropertyExtension("OutsourcingAveragecost", "OutsourcingAveragecost")]
        public decimal? OutsourcingAveragecost { get; set; } //包干平均费用
        //public decimal? Price1 { get; set; }
        //public decimal? Price2 { get; set; }
        //public decimal? Price3 { get; set; }
        //public decimal? Price4 { get; set; }
        [EntityPropertyExtension("Number1", "Number1")]
        public int? Number1 { get; set; }
        [EntityPropertyExtension("Number2", "Number2")]
        public int? Number2 { get; set; }
        [EntityPropertyExtension("Number3", "Number3")]
        public int? Number3 { get; set; }
        [EntityPropertyExtension("Number4", "Number4")]
        public int? Number4 { get; set; }
        [EntityPropertyExtension("OperationCost1", "OperationCost1")]
        public decimal? OperationCost1 { get; set; }
        [EntityPropertyExtension("OperationCost2", "OperationCost2")]
        public decimal? OperationCost2 { get; set; }
        [EntityPropertyExtension("OperationCost3", "OperationCost3")]
        public decimal? OperationCost3 { get; set; }
        [EntityPropertyExtension("OperationCost4", "OperationCost4")]
        public decimal? OperationCost4 { get; set; }
        [EntityPropertyExtension("OperationTotal", "OperationTotal")]
        public decimal? OperationTotal { get; set; }

        //public decimal? Rate1 { get; set; }
        //public decimal? Rate2 { get; set; }
        //public decimal? Rate3 { get; set; }
        //public decimal? Rate4 { get; set; }
        //public decimal? Rate5 { get; set; }
        //public decimal? Rate6 { get; set; }
        //public decimal? Rate7 { get; set; }
        [EntityPropertyExtension("Num1", "Num1")]
        public int? Num1 { get; set; }
        [EntityPropertyExtension("Num2", "Num2")]
        public int? Num2 { get; set; }
        [EntityPropertyExtension("Num3", "Num3")]
        public int? Num3 { get; set; }
        [EntityPropertyExtension("Num4", "Num4")]
        public int? Num4 { get; set; }
        [EntityPropertyExtension("Num5", "Num5")]
        public int? Num5 { get; set; }
        [EntityPropertyExtension("Num6", "Num6")]
        public int? Num6 { get; set; }
        [EntityPropertyExtension("Num7", "Num7")]
        public int? Num7 { get; set; }
        [EntityPropertyExtension("Cost1", "Cost1")]
        public decimal? Cost1 { get; set; }
        [EntityPropertyExtension("Cost2", "Cost2")]
        public decimal? Cost2 { get; set; }
        [EntityPropertyExtension("Cost3", "Cost3")]
        public decimal? Cost3 { get; set; }
        [EntityPropertyExtension("Cost4", "Cost4")]
        public decimal? Cost4 { get; set; }
        [EntityPropertyExtension("Cost5", "Cost5")]
        public decimal? Cost5 { get; set; }
        [EntityPropertyExtension("Cost6", "Cost6")]
        public decimal? Cost6 { get; set; }
        [EntityPropertyExtension("Cost7", "Cost7")]
        public decimal? Cost7 { get; set; }
        [EntityPropertyExtension("TotalCost", "TotalCost")]
        public decimal? TotalCost { get; set; }
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }
        //public string str2 { get; set; }
        //public string str3 { get; set; }
        //public string str4 { get; set; }
        //public string str5 { get; set; }
        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }
        //public DateTime? DateTime2 { get; set; }
        //public DateTime? DateTime3 { get; set; }
        //public DateTime? DateTime4 { get; set; }
        //public DateTime? DateTime5 { get; set; }
        [EntityPropertyExtension("PageIndex", "PageIndex")]
        public int PageIndex { get; set; }
        [EntityPropertyExtension("PageSize", "PageSize")]
        public int PageSize { get; set; }
    }
}
