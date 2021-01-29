using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// Bob SKU进出库日志
    /// </summary>
    [Serializable]
    public class ReportSkuInAndOut
    {
        //select a.CreateDate , a.FromSKU , b.SKUCategory10 as Article , b.SKUCategory09 as Size , a.Qty ,a.FromLocation , a.Type from ##aa a left join tbl_wms_SKU b on a.FromSKU = b.SKU
        //         DateTime      varchar25       varchar50                  varchar50               decimal varchar25 varchar25    

        #region Model

        /// <summary>
        /// 外部单号
        /// </summary>
        [EntityPropertyExtension("ExternReceiptNumber", "ExternReceiptNumber")]
        public string ExternReceiptNumber { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// SKU
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        /// <summary>
        /// 物品代码
        /// </summary>
        [EntityPropertyExtension("Article", "Article")]
        public string Article { get; set; }
        /// <summary>
        /// 物品尺寸
        /// </summary>
        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }
        /// <summary>
        /// BU
        /// </summary>
        [EntityPropertyExtension("BU", "BU")]
        public string BU { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [EntityPropertyExtension("Gender", "Gender")]
        public string Gender { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [EntityPropertyExtension("Price", "Price")]
        public string Price { get; set; }
        /// <summary>
        /// SafeLock
        /// </summary>
        [EntityPropertyExtension("SafeLock", "SafeLock")]
        public string SafeLock { get; set; }
        /// <summary>
        /// Hanger
        /// </summary>
        [EntityPropertyExtension("Hanger", "Hanger")]
        public string Hanger { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [EntityPropertyExtension("Qty", "Qty")]
        public decimal Qty { get; set; }
        /// <summary>
        /// 库位
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [EntityPropertyExtension("Type", "Type")]
        public string Type { get; set; }

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

        /// <summary>
        /// 批次号
        /// </summary>
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }

        /// <summary>
        ///   门店代码
        /// </summary>
        [EntityPropertyExtension("storerkey", "storerkey")]
        public string storerkey { get; set; }


        ///// <summary>
        ///// 开始日期
        ///// </summary>
        //[EntityPropertyExtension("BeginTime", "BeginTime")]
        //public string BeginTime { get; set; }
        ///// <summary>
        ///// 结束日期
        ///// </summary>
        //[EntityPropertyExtension("EndTime", "EndTime")]
        //public string EndTime { get; set; }

        #endregion Model
    }
}
