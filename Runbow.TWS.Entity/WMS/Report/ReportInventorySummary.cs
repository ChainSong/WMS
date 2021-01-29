using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 库存信息
    /// </summary>
    [Serializable]
    public class ReportInventorySummary
    {

        /// <summary>
        /// 客户(货主)编号
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long? WarehouseID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }

        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }

        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        /// <summary>
        /// 基本信息-制造商  这里用于品牌
        /// </summary>
        [EntityPropertyExtension("Manufacturer", "Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 制造商sku
        /// </summary>
        [EntityPropertyExtension("ManufacturerSKU", "ManufacturerSKU")]
        public string ManufacturerSKU { get; set; }
        
        /// <summary>
        /// 商品条码
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

             
        /// <summary>
        /// 次品（不知道是数量还是什么鬼）
        /// </summary>
        [EntityPropertyExtension("Qty4", "Qty4")]
        public decimal Qty4 { get; set; }


        /// <summary>
        /// 当前数量
        /// </summary>
        [EntityPropertyExtension("Qty", "Qty")]
        public int Qty { get; set; }


        /// <summary>
        /// 在库良品库存
        /// </summary>
        //可用数量
        [EntityPropertyExtension("InventoryQty", "InventoryQty")]
        public int InventoryQty { get; set; }

        /// <summary>
        /// 订单占用良品库存
        /// </summary>
        //操作中数量
        [EntityPropertyExtension("UsingQty", "UsingQty")]
        public int UsingQty { get; set; }

        //冻结数量
        [EntityPropertyExtension("LockQty", "LockQty")]
        public int LockQty { get; set; }

        //已出库数量
        [EntityPropertyExtension("OutQty", "OutQty")]
        public int OutQty { get; set; }

       
        /// <summary>
        /// 品名
        /// </summary>
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [EntityPropertyExtension("Color", "Color")]
        public string Color { get; set; }
        
      
        /// <summary>
        /// 单价
        /// </summary>
        [EntityPropertyExtension("Price", "Price")]
        public decimal Price { get; set; }


        /// <summary>
        /// Str1用于年份
        /// </summary>
        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }

        /// <summary>
        /// Str2用于系列
        /// </summary>
        [EntityPropertyExtension("str2", "str2")]
        public string str2 { get; set; }

        /// <summary>
        /// Str3用于季节
        /// </summary>
        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }

        /// <summary>
        /// Str4用于折扣类别
        /// </summary>
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

        /// <summary>
        /// 尺码
        /// </summary>
        [EntityPropertyExtension("str9", "str9")]
        public string str9 { get; set; }

        /// <summary>
        /// 款号 Article
        /// </summary>
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

        /// <summary>
        /// 首次入库时间
        /// </summary>
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
        //在途数量
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

 

     
    }
}
