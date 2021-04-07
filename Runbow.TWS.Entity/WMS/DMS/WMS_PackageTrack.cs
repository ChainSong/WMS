using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 成品订单跟踪表
    /// </summary>
    public class WMS_PackageTrack
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

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
        /// 成品订单流水号
        /// </summary>
        [EntityPropertyExtension("PackageNumber", "PackageNumber")]
        public string PackageNumber { get; set; }

        /// <summary>
        /// 跟踪日期
        /// </summary>
        [EntityPropertyExtension("TrackDatae", "TrackDatae")]
        public DateTime? TrackDatae { get; set; }

        /// <summary>
        /// 所在位置
        /// </summary>
        [EntityPropertyExtension("Position", "Position")]
        public string Position { get; set; }

        /// <summary>
        /// 跟踪明细
        /// </summary>
        [EntityPropertyExtension("Detailed", "Detailed")]
        public string Detailed { get; set; }

        /// <summary>
        /// 跟踪状态
        /// </summary>
        [EntityPropertyExtension("CurrentState", "CurrentState")]
        public string CurrentState { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        [EntityPropertyExtension("ExpressNumber", "ExpressNumber")]
        public string ExpressNumber { get; set; }


        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }
        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }
        [EntityPropertyExtension("Str6", "Str6")]
        public string Str6 { get; set; }
        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7 { get; set; }
        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8 { get; set; }
        [EntityPropertyExtension("Str9", "Str9")]
        public string Str9 { get; set; }
        [EntityPropertyExtension("Str10", "Str10")]
        public string Str10 { get; set; }
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
        [EntityPropertyExtension("DateTime6", "DateTime6")]
        public DateTime? DateTime6 { get; set; }
        [EntityPropertyExtension("DateTime7", "DateTime7")]
        public DateTime? DateTime7 { get; set; }
        [EntityPropertyExtension("DateTime8", "DateTime8")]
        public DateTime? DateTime8 { get; set; }
        [EntityPropertyExtension("DateTime9", "DateTime9")]
        public DateTime? DateTime9 { get; set; }
        [EntityPropertyExtension("DateTime10", "DateTime10")]
        public DateTime? DateTime10 { get; set; }
    }
}
