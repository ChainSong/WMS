using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Print
{
    /// <summary>
    /// 打印关联主表
    /// </summary>
    public class PrintHeader
    {
        #region Model
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("PrintKey", "PrintKey")]
        public string PrintKey { get; set; }

        [EntityPropertyExtension("PrintStatus", "PrintStatus")]
        public string PrintStatus { get; set; }

        [EntityPropertyExtension("RelateCount", "RelateCount")]
        public long RelateCount { get; set; }

        [EntityPropertyExtension("PrintCount", "PrintCount")]
        public int PrintCount { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public string Status { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }

        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime UpdateTime { get; set; }


        #endregion
    }
}
