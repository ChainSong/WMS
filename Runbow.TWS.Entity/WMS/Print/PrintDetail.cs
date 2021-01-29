using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Print
{
    /// <summary>
    /// 打印关联明细表
    /// </summary>
    public class PrintDetail
    {
        #region Model
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }


        [EntityPropertyExtension("PrintID", "PrintID")]
        public long PrintID { get; set; }


        [EntityPropertyExtension("PrintKey", "PrintKey")]
        public string PrintKey { get; set; }

        [EntityPropertyExtension("PrintLineNumber", "PrintLineNumber")]
        public long PrintLineNumber { get; set; }

        [EntityPropertyExtension("PrintLineKey", "PrintLineKey")]
        public string PrintLineKey { get; set; }

        [EntityPropertyExtension("OrderID", "OrderID")]
        public long OrderID { get; set; }

        [EntityPropertyExtension("OrderKey", "OrderKey")]
        public string OrderKey { get; set; }


        [EntityPropertyExtension("ExpressCompany", "ExpressCompany")]
        public string ExpressCompany { get; set; }


        [EntityPropertyExtension("ExpressKey", "ExpressKey")]
        public string ExpressKey { get; set; }


        [EntityPropertyExtension("PrintCount", "PrintCount")]
        public long PrintCount { get; set; }


        [EntityPropertyExtension("Status", "Status")]
        public string Status { get; set; }


        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }


        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }


        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }


        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }


        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }


        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }


        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }


        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        //[EntityPropertyExtension("PrintStatus", "PrintStatus")]
        //public string PrintStatus { get; set; }

        //[EntityPropertyExtension("PrintCount", "PrintCount")]
        //public string PrintCount { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
        [EntityPropertyExtension("Int1", "Int1")]
        public string Int1 { get; set; }
        #endregion
    }
}
