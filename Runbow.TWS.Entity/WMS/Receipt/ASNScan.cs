using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 预入库主订单扫描
    /// </summary>
    [Serializable]
    public class ASNScan
    {
        #region Model
        /// <summary>
        /// 期望总箱数
        /// </summary>
        [EntityPropertyExtension("ExpectTotalBox", "ExpectTotalBox")]
        public int ExpectTotalBox { get; set; }
        /// <summary>
        /// 实际总箱数
        /// </summary>
        [EntityPropertyExtension("ReceiveTotalBox", "ReceiveTotalBox")]
        public int ReceiveTotalBox { get; set; }
        /// <summary>
        /// 期望总件数
        /// </summary>
        [EntityPropertyExtension("ExpectTotalSKU", "ExpectTotalSKU")]
        public decimal ExpectTotalSKU { get; set; }
        /// <summary>
        /// 实际总件数
        /// </summary>
        [EntityPropertyExtension("ReceiveTotalSKU", "ReceiveTotalSKU")]
        public decimal ReceiveTotalSKU { get; set; }
        /// <summary>
        /// 期望箱件数
        /// </summary>
        [EntityPropertyExtension("ExpectBoxSKU", "ExpectBoxSKU")]
        public decimal ExpectBoxSKU { get; set; }
        /// <summary>
        /// 实际箱件数
        /// </summary>
        [EntityPropertyExtension("ReceiveBoxSKU", "ReceiveBoxSKU")]
        public decimal ReceiveBoxSKU { get; set; }
        /// <summary>
        ///期望箱明细
        /// </summary>
        [EntityPropertyExtension("ExpectBoxDetailSKU", "ExpectBoxDetailSKU")]
        public decimal ExpectBoxDetailSKU { get; set; }
        /// <summary>
        /// 实际箱明细
        /// </summary>
        [EntityPropertyExtension("BoxDetailSKU", "BoxDetailSKU")]
        public decimal BoxDetailSKU { get; set; }
        /// <summary>
        /// 箱号 
        /// </summary>
        [EntityPropertyExtension("str2", "str2")]
        public string str2 { get; set; }
        /// <summary>
        /// SKU
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        #endregion Model
    }
}
