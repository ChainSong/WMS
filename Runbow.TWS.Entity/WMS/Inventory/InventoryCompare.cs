using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 库存对比信息
    /// </summary>
    [Serializable]
    public class InventoryCompare
    {
        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }

        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        #region Model
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

        [EntityPropertyExtension("ReceiptNumber", "ReceiptNumber")]
        public string ReceiptNumber { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }

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
        public long CustomerID { get; set; }

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
        /// 产品编码
        /// </summary>
        [EntityPropertyExtension("Article", "Article")]
        public string Article { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }

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
        public double Qty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("NIKEQty", "NIKEQty")]
        public double NIKEQty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("WMSQty", "WMSQty")]
        public double WMSQty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("DifferenceQty", "DifferenceQty")]
        public double DifferenceQty { get; set; }

        //可用数量
        [EntityPropertyExtension("InventoryQty", "InventoryQty")]
        public double InventoryQty { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        [EntityPropertyExtension("InventoryType", "InventoryType")]
        public string InventoryType { get; set; }


        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }


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
        /// 创建日期
        /// </summary>
        [EntityPropertyExtension("EndCreateTime", "EndCreateTime")]
        public DateTime? EndCreateTime { get; set; }

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

        /// <summary>
        /// 备用
        /// </summary>
        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }

        [EntityPropertyExtension("str2", "str2")]
        public string str2 { get; set; }

        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }

        [EntityPropertyExtension("str4", "str4")]
        public string str4 { get; set; }

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
        #endregion Model
        [EntityPropertyExtension("Total", "Total")]
        public double Total { get; set; }

        [EntityPropertyExtension("Price", "Price")]
        public double Price { get; set; }


    }


}
