using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 库存信息
    /// </summary>
    [Serializable]
    public class ReportInventory
    {
        #region Model
        /// <summary>
        /// UPC
        /// </summary>
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }
        /// <summary>
        /// Specifications
        /// </summary>
        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }
        /// <summary>
        /// 自增ID
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public int ID { get; set; }
        /// <summary>
        /// 自增ID多列
        /// </summary>
        [EntityPropertyExtension("IDS", "IDS")]
        public string IDS { get; set; }
        /// <summary>
        /// 上架表ID,关联字段
        /// </summary>
        [EntityPropertyExtension("RRID", "RRID")]
        public long RRID { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        [EntityPropertyExtension("ReceiptNumber", "ReceiptNumber")]
        public string ReceiptNumber { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }
        /// <summary>
        /// 库位
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        [EntityPropertyExtension("SuperID", "SuperID")]
        public long SuperID { get; set; }
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
        /// 产品编码
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        /// <summary>
        /// Article
        /// </summary>
        [EntityPropertyExtension("Article", "Article")]
        public string Article { get; set; }
        /// <summary>
        /// Size
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
        /// 商品类型
        /// </summary>
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }
        /// <summary>
        /// 当前数量
        /// </summary>
        [EntityPropertyExtension("Qty", "Qty")]
        public int Qty { get; set; }
        /// <summary>
        ///可用数量
        /// </summary>
        [EntityPropertyExtension("InventoryQty", "InventoryQty")]
        public int InventoryQty { get; set; }
        /// <summary>
        ///操作中数量
        /// </summary>
        [EntityPropertyExtension("UsingQty", "UsingQty")]
        public int UsingQty { get; set; }
        /// <summary>
        ///冻结数量
        /// </summary>
        [EntityPropertyExtension("LockQty", "LockQty")]
        public int LockQty { get; set; }
        /// <summary>
        ///已出库数量
        /// </summary>
        [EntityPropertyExtension("OutQty", "OutQty")]
        public int OutQty { get; set; }
        /// <summary>
        ///库存数量
        /// </summary>
        [EntityPropertyExtension("LocalQty", "LocalQty")]
        public int LocalQty { get; set; }
        /// <summary>
        ///库存类型
        /// </summary>
        [EntityPropertyExtension("InventoryType", "InventoryType")]
        public int InventoryType { get; set; }
        /// <summary>
        ///箱号
        /// </summary>
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }
        /// <summary>
        ///关联ID
        /// </summary>
        [EntityPropertyExtension("RelatedID", "RelatedID")]
        public int RelatedID { get; set; }
        ///// <summary>
        ///// 分配数量
        ///// </summary>
        //[EntityPropertyExtension("QtyAllocated", "QtyAllocated")]
        //public int QtyAllocated { get; set; }
        ///// <summary>
        ///// 拣货数量
        ///// </summary>
        //[EntityPropertyExtension("QtyPicked", "QtyPicked")]
        //public int QtyPicked { get; set; }
        ///// <summary>
        ///// 预计数量
        ///// </summary>
        //[EntityPropertyExtension("QtyExpected", "QtyExpected")]
        //public int QtyExpected { get; set; }
        // <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改操作人
        /// </summary>
        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime? UpdateTime { get; set; }
        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }
        [EntityPropertyExtension("str2", "str2")]
        public string str2 { get; set; }
        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }
        /// <summary>
        /// Category
        /// </summary>
        [EntityPropertyExtension("str4", "str4")]
        public string str4 { get; set; }
        /// <summary>
        /// LongMaterial
        /// </summary>
        [EntityPropertyExtension("str5", "str5")]
        public string str5 { get; set; }
        [EntityPropertyExtension("str6", "str6")]
        public string str6 { get; set; }
        [EntityPropertyExtension("str7", "str7")]
        public string str7 { get; set; }
        [EntityPropertyExtension("str8", "str8")]
        public string str8 { get; set; }
        [EntityPropertyExtension("str9", "str9")]
        public string str9 { get; set; }
        [EntityPropertyExtension("str10", "str10")]
        public string str10 { get; set; }
        [EntityPropertyExtension("str11", "str11")]
        public string str11 { get; set; }
        [EntityPropertyExtension("str12", "str12")]
        public string str12 { get; set; }
        [EntityPropertyExtension("str13", "str13")]
        public string str13 { get; set; }
        [EntityPropertyExtension("str14", "str14")]
        public string str14 { get; set; }
        [EntityPropertyExtension("str15", "str15")]
        public string str15 { get; set; }
        [EntityPropertyExtension("str16", "str16")]
        public string str16 { get; set; }
        [EntityPropertyExtension("str17", "str17")]
        public string str17 { get; set; }
        [EntityPropertyExtension("str18", "str18")]
        public string str18 { get; set; }
        [EntityPropertyExtension("str19", "str19")]
        public string str19 { get; set; }
        [EntityPropertyExtension("str20", "str20")]
        public string str20 { get; set; }
        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }
        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }
        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime? DateTime3 { get; set; }
        [EntityPropertyExtension("DateTime4", "DateTime4")]
        public DateTime? DateTime4 { get; set; }
        [EntityPropertyExtension("DateTime5", "DateTime5")]
        public DateTime? DateTime5 { get; set; }
        /// <summary>
        /// 在途数量
        /// </summary>
        [EntityPropertyExtension("Int1", "Int1")]
        public int? Int1 { get; set; }
        [EntityPropertyExtension("Int2", "Int2")]
        public int? Int2 { get; set; }
        [EntityPropertyExtension("Int3", "Int3")]
        public int? Int3 { get; set; }
        [EntityPropertyExtension("Int4", "Int4")]
        public int? Int4 { get; set; }
        [EntityPropertyExtension("Int5", "Int5")]
        public int? Int5 { get; set; }
        [EntityPropertyExtension("Volume", "Volume")]
        public string Volume { get; set; }
        [EntityPropertyExtension("Weight", "Weight")]
        public string Weight { get; set; }
        [EntityPropertyExtension("NetWeight", "NetWeight")]
        public string NetWeight { get; set; }
        [EntityPropertyExtension("Category", "Category")]
        public string Category { get; set; }
        #endregion Model
    }
}
