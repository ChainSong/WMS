using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 库存信息
    /// </summary>
    [Serializable]
    public class ReportReceiptReport
    {
        #region Model
        [EntityPropertyExtension("ReceiptDate", "ReceiptDate")]
        public DateTime? ReceiptDate { get; set; }
        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }
        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }
        [EntityPropertyExtension("StringDateTime1", "StringDateTime1")]
        public string StringDateTime1 { get; set; }
        [EntityPropertyExtension("StringDateTime2", "StringDateTime2")]
        public string StringDateTime2 { get; set; }
        [EntityPropertyExtension("ASNQtyExpected", "ASNQtyExpected")]
        public double ASNQtyExpected { get; set; }
        /// <summary>
        /// 自增ID
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("DID", "DID")]
        public long DID { get; set; }
        /// <summary>
        /// 收货主表ID
        /// </summary>
        [EntityPropertyExtension("RID", "RID")]
        public long RID { get; set; }
        /// <summary>
        /// 收货单号，系统自动生成
        /// </summary>
        [EntityPropertyExtension("ReceiptNumber", "ReceiptNumber")]
        public string ReceiptNumber { get; set; }
        /// <summary>
        /// 库位
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }
        /// <summary>
        /// 外部单号，由外部指定
        /// </summary>
        [EntityPropertyExtension("ExternReceiptNumber", "ExternReceiptNumber")]
        public string ExternReceiptNumber { get; set; }
        [EntityPropertyExtension("ASNID", "ASNID")]
        public long ASNID { get; set; }
        [EntityPropertyExtension("ASNNumber", "ASNNumber")]
        public string ASNNumber { get; set; }
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
        /// 行号
        /// </summary>
        [EntityPropertyExtension("LineNumber", "LineNumber")]
        public string LineNumber { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        //Article
        [EntityPropertyExtension("Article", "Article")]
        public string Article { get; set; }

        //尺码
        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }
        /// <summary>
        /// BU
        /// </summary>
        [EntityPropertyExtension("BU", "BU")]
        public string BU { get; set; }
        //Price
        [EntityPropertyExtension("Price", "Price")]
        public string Price { get; set; }
        /// <summary>
        /// 安全扣
        /// </summary>
        [EntityPropertyExtension("SafeLock", "SafeLock")]
        public string SafeLock { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        [EntityPropertyExtension("Gender", "Gender")]
        public string Gender { get; set; }
        /// <summary>
        /// Hanger
        /// </summary>
        [EntityPropertyExtension("Hanger", "Hanger")]
        public string Hanger { get; set; }
        //描述
        [EntityPropertyExtension("Description", "Description")]
        public string Description { get; set; }
        //箱单号
        [EntityPropertyExtension("UserDef7", "UserDef7")]
        public string UserDef7 { get; set; }
        //From Location
        [EntityPropertyExtension("PCDNumber", "PCDNumber")]
        public string PCDNumber { get; set; }
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public string WarehouseID { get; set; }
        /// <summary>
        /// 期望收货数量
        /// </summary>
        [EntityPropertyExtension("QtyExpected", "QtyExpected")]
        public double QtyExpected { get; set; }

        /// <summary>
        /// 实际收货数量
        /// </summary>
        [EntityPropertyExtension("QtyReceived", "QtyReceived")]
        public double QtyReceived { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        [EntityPropertyExtension("CompleteDate", "CompleteDate")]
        public string CompleteDate { get; set; }

        /// <summary>
        /// 差异数量
        /// </summary>
        [EntityPropertyExtension("Qty", "Qty")]
        public double Qty { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

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

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }
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
    }
}
