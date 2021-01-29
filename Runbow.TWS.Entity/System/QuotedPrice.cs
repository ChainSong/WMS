using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class QuotedPrice
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("ProjectName", "ProjectName")]
        public string ProjectName { get; set; }

        [EntityPropertyExtension("Target", "Target")]
        public int Target { get; set; }

        [EntityPropertyExtension("TargetID", "TargetID")]
        public long TargetID { get; set; }

        [EntityPropertyExtension("TargetName", "TargetName")]
        public string TargetName { get; set; }

        [EntityPropertyExtension("SegmentDetailID", "SegmentDetailID")]
        public long SegmentDetailID { get; set; }

        [EntityPropertyExtension("StartVal", "StartVal")]
        public float StartVal { get; set; }

        [EntityPropertyExtension("EndVal", "EndVal")]
        public float EndVal { get; set; }

        [EntityPropertyExtension("TransportationLineID", "TransportationLineID")]
        public long? TransportationLineID { get; set; }

        [EntityPropertyExtension("StartCityID", "StartCityID")]
        public long StartCityID { get; set; }

        [EntityPropertyExtension("StartCityName", "StartCityName")]
        public string StartCityName { get; set; }

        [EntityPropertyExtension("EndCityID", "EndCityID")]
        public long EndCityID { get; set; }

        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("ShipperTypeID", "ShipperTypeID")]
        public long ShipperTypeID { get; set; }

        [EntityPropertyExtension("ShipperTypeName", "ShipperTypeName")]
        public string ShipperTypeName { get; set; }

        [EntityPropertyExtension("PodTypeID", "PodTypeID")]
        public long PodTypeID { get; set; }

        [EntityPropertyExtension("PodTypeName", "PodTypeName")]
        public string PodTypeName { get; set; }

        [EntityPropertyExtension("TplOrTtlID", "TplOrTtlID")]
        public long TplOrTtlID { get; set; }

        [EntityPropertyExtension("TplOrTtlName", "TplOrTtlName")]
        public string TplOrTtlName { get; set; }

        [EntityPropertyExtension("Price", "Price")]
        public decimal Price { get; set; }

        [EntityPropertyExtension("Point", "Point")]
        public decimal? Point { get; set; }

        [EntityPropertyExtension("MinPrice", "MinPrice")]
        public decimal? MinPrice { get; set; }

        [EntityPropertyExtension("EffectiveStartTime", "EffectiveStartTime")]
        public DateTime EffectiveStartTime { get; set; }

        [EntityPropertyExtension("EffectiveEndTime", "EffectiveEndTime")]
        public DateTime? EffectiveEndTime { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("RelatedCustomerID", "RelatedCustomerID")]
        public long? RelatedCustomerID { get; set; }

        [EntityPropertyExtension("RelatedCustomerName", "RelatedCustomerName")]
        public string RelatedCustomerName { get; set; }

        [EntityPropertyExtension("EmptyCarryPrice", "EmptyCarryPrice")]
        public decimal? EmptyCarryPrice { get; set; }

        [EntityPropertyExtension("Decimal1", "Decimal1")]
        public decimal? Decimal1 { get; set; }

        [EntityPropertyExtension("Decimal2", "Decimal2")]
        public decimal? Decimal2 { get; set; }
    }
}