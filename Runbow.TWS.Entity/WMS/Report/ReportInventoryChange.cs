using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// Bob 库存变更
    /// </summary>
    [Serializable]
    public class ReportInventoryChange
    {
        #region Model
        /// <summary>
        /// 调整单号
        /// </summary>
        [EntityPropertyExtension("AdjustmentKey", "AdjustmentKey")]
        public string AdjustmentKey { get; set; }
        /// <summary>
        /// 调整行号
        /// </summary>
        [EntityPropertyExtension("AdjustmentLineNumber", "AdjustmentLineNumber")]
        public string AdjustmentLineNumber { get; set; }
        /// <summary>
        /// 原类型
        /// </summary>
        [EntityPropertyExtension("OriginalInvType", "OriginalInvType")]
        public string OriginalInvType { get; set; }
        /// <summary>
        /// 调整类型
        /// </summary>
        [EntityPropertyExtension("AdjustedInvType", "AdjustedInvType")]
        public string AdjustedInvType { get; set; }
        /// <summary>
        /// 调整数量
        /// </summary>
        [EntityPropertyExtension("AdjustedQty", "AdjustedQty")]
        public decimal AdjustedQty { get; set; }
        /// <summary>
        /// 冻结单号
        /// </summary>
        [EntityPropertyExtension("HoldKey", "HoldKey")]
        public string HoldKey { get; set; }
        /// <summary>
        /// 冻结状态
        /// </summary>
        [EntityPropertyExtension("HoldStatus", "HoldStatus")]
        public int HoldStatus { get; set; }
        /// <summary>
        /// 冻结原因
        /// </summary>
        [EntityPropertyExtension("HoldReason", "HoldReason")]
        public string HoldReason { get; set; }
        /// <summary>
        /// 移库单号
        /// </summary>
        [EntityPropertyExtension("MoveKey", "MoveKey")]
        public string MoveKey { get; set; }
        /// <summary>
        /// 新库位
        /// </summary>
        [EntityPropertyExtension("ToLocation", "ToLocation")]
        public string ToLocation { get; set; }
        /// <summary>
        /// 新ContainerKey
        /// </summary>
        [EntityPropertyExtension("ToContainerKey", "ToContainerKey")]
        public string ToContainerKey { get; set; }
        /// <summary>
        /// 报表类型
        /// </summary>
        [EntityPropertyExtension("InventoryChangeTypes", "InventoryChangeTypes")]
        public string InventoryChangeTypes { get; set; }
        /// <summary>
        /// 货主
        /// </summary>
        [EntityPropertyExtension("Storer", "Storer")]
        public string Storer { get; set; }
        /// <summary>
        /// SKU
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        [EntityPropertyExtension("Lot", "Lot")]
        public string Lot { get; set; }
        /// <summary>
        /// 原库位
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }
        /// <summary>
        /// 原数量
        /// </summary>
        [EntityPropertyExtension("Qty", "Qty")]
        public decimal Qty { get; set; }
        /// <summary>
        /// 原ContainerKey
        /// </summary>
        [EntityPropertyExtension("ContainerKey", "ContainerKey")]
        public string ContainerKey { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [EntityPropertyExtension("Status", "Status")]
        public string Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("CreateUser", "CreateUser")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [EntityPropertyExtension("CreateDate", "CreateDate")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        [EntityPropertyExtension("UpdateUser", "UpdateUser")]
        public string UpdateUser { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        [EntityPropertyExtension("UpdateDate", "UpdateDate")]
        public DateTime UpdateDate { get; set; }


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
        /// 调整类型
        /// </summary>
        [EntityPropertyExtension("AdjustmentType", "AdjustmentType")]
        public string AdjustmentType { get; set; }

        /// <summary>
        /// 门店代码
        /// </summary>
        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }


        #endregion Model
    }
}
