using System;
using Runbow.TWS.Common;


namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 成品订单信息表
    /// </summary>
    public class WMS_Package
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        /// <summary>
        /// 成品订单流水号
        /// </summary>
        [EntityPropertyExtension("PackageNumber", "PackageNumber")]
        public string PackageNumber { get; set; }

        /// <summary>
        /// 出库单ID
        /// </summary>
        [EntityPropertyExtension("OID", "OID")]
        public long? OID { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// 外部单号
        /// </summary>
        [EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        public string ExternOrderNumber { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 仓库名称 
        /// </summary>
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }

        /// <summary>
        /// 出库单类型 
        /// </summary>
        [EntityPropertyExtension("PackageType", "PackageType")]
        public string PackageType { get; set; }


        /// <summary>
        /// 出库日期
        /// </summary>
        [EntityPropertyExtension("PackageTime", "PackageTime")]
        public DateTime? PackageTime { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        [EntityPropertyExtension("Length", "Length")]
        public string Length { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        [EntityPropertyExtension("Width", "Width")]
        public string Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        [EntityPropertyExtension("Height", "Height")]
        public string Height { get; set; }

        /// <summary>
        /// NetWeight
        /// </summary>
        [EntityPropertyExtension("NetWeight", "NetWeight")]
        public string NetWeight { get; set; }

        /// <summary>
        /// GrossWeight
        /// </summary>
        [EntityPropertyExtension("GrossWeight", "GrossWeight")]
        public string GrossWeight { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        [EntityPropertyExtension("ExpressCompany", "ExpressCompany")]
        public string ExpressCompany { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        [EntityPropertyExtension("ExpressNumber", "ExpressNumber")]
        public string ExpressNumber { get; set; }

        /// <summary>
        /// IsComposited
        /// </summary>
        [EntityPropertyExtension("IsComposited", "IsComposited")]
        public int? IsComposited { get; set; }

        /// <summary>
        /// IsHandovered
        /// </summary>
        [EntityPropertyExtension("IsHandovered", "IsHandovered")]
        public int? IsHandovered { get; set; }

        /// <summary>
        /// Handoveror 
        /// </summary>
        [EntityPropertyExtension("Handoveror", "Handoveror")]
        public string Handoveror { get; set; }

        /// <summary>
        /// HandoverTime
        /// </summary>
        [EntityPropertyExtension("HandoverTime", "HandoverTime")]
        public DateTime? HandoverTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EntityPropertyExtension("Status", "Status")]
        public int? Status { get; set; }

        /// <summary>
        /// DetailCount
        /// </summary>
        [EntityPropertyExtension("DetailCount", "DetailCount")]
        public int? DetailCount { get; set; }


        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }
        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime? UpdateTime { get; set; }
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }
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
    }
}
