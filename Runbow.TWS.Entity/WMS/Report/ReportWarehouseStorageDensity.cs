using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// Bob 仓储密度
    /// </summary>
    [Serializable]
    public class ReportWarehouseStorageDensity
    {
        #region Model
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Date", "Date")]
        public DateTime Date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("SKUGroup", "SKUGroup")]
        public string SKUGroup { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("NoOfUnits", "NoOfUnits")]
        public decimal NoOfUnits { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("NoOfOccupiedLocation", "NoOfOccupiedLocation")]
        public string NoOfOccupiedLocation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("TotalNoOfLocationsInDC", "TotalNoOfLocationsInDC")]
        public decimal TotalNoOfLocationsInDC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("BulkLocationUtilization", "BulkLocationUtilization")]
        public decimal BulkLocationUtilization { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("WarehouseTotalCapacity", "WarehouseTotalCapacity")]
        public decimal WarehouseTotalCapacity { get; set; }
       

        /// <summary>
        /// 客户(货主)编号
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }
        /// <summary>
        /// 客户(货主)名称
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }

        #endregion Model
    }
}
