using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class SettledPod
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("SettledNumber", "SettledNumber")]
        public string SettledNumber { get; set; }

        [EntityPropertyExtension("PodID", "PodID")]
        public long PodID { get; set; }

        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("SettledType", "SettledType")]
        public int? SettledType { get; set; }

        [EntityPropertyExtension("CustomerOrShipperID", "CustomerOrShipperID")]
        public long? CustomerOrShipperID { get; set; }

        [EntityPropertyExtension("CustomerOrShipperName", "CustomerOrShipperName")]
        public string CustomerOrShipperName { get; set; }

        [EntityPropertyExtension("StartCityID", "StartCityID")]
        public long? StartCityID { get; set; }

        [EntityPropertyExtension("StartCityName", "StartCityName")]
        public string StartCityName { get; set; }

        [EntityPropertyExtension("EndCityID", "EndCityID")]
        public long? EndCityID { get; set; }

        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("ShipperTypeID", "ShipperTypeID")]
        public long? ShipperTypeID { get; set; }

        [EntityPropertyExtension("ShipperTypeName", "ShipperTypeName")]
        public string ShipperTypeName { get; set; }

        [EntityPropertyExtension("PodTypeID", "PodTypeID")]
        public long? PODTypeID { get; set; }

        [EntityPropertyExtension("PodTypeName", "PodTypeName")]
        public string PODTypeName { get; set; }

        [EntityPropertyExtension("TtlOrTplID", "TtlOrTplID")]
        public long? TtlOrTplID { get; set; }

        [EntityPropertyExtension("TtlOrTplName", "TtlOrTplName")]
        public string TtlOrTplName { get; set; }

        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public DateTime? ActualDeliveryDate { get; set; }

        [EntityPropertyExtension("Boxnumber", "Boxnumber")]
        public double? BoxNumber { get; set; }

        [EntityPropertyExtension("Weight", "Weight")]
        public double? Weight { get; set; }

        [EntityPropertyExtension("GoodsNumber", "GoodsNumber")]
        public double? GoodsNumber { get; set; }

        [EntityPropertyExtension("Volume", "Volume")]
        public double? Volume { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("ShipAmt", "ShipAmt")]
        public decimal? ShipAmt { get; set; }

        [EntityPropertyExtension("BAFAmt", "BAFAmt")]
        public decimal? BAFAmt { get; set; }

        [EntityPropertyExtension("PointAmt", "PointAmt")]
        public decimal? PointAmt { get; set; }

        [EntityPropertyExtension("OtherAmt", "OtherAmt")]
        public decimal? OtherAmt { get; set; }

        [EntityPropertyExtension("Amt1", "Amt1")]
        public decimal? Amt1 { get; set; }

        [EntityPropertyExtension("Amt2", "Amt2")]
        public decimal? Amt2 { get; set; }

        [EntityPropertyExtension("Amt3", "Amt3")]
        public decimal? Amt3 { get; set; }

        [EntityPropertyExtension("Amt4", "Amt4")]
        public decimal? Amt4 { get; set; }

        [EntityPropertyExtension("Amt5", "Amt5")]
        public decimal? Amt5 { get; set; }

        [EntityPropertyExtension("TotalAmt", "TotalAmt")]
        public decimal? TotalAmt { get; set; }

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

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }

        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("InvoiceID", "InvoiceID")]
        public long? InvoiceID { get; set; }

        [EntityPropertyExtension("RelatedCustomerID","RelatedCustomerID")]
        public long? RelatedCustomerID { get; set; }

        [EntityPropertyExtension("IsAudit", "IsAudit")]
        public bool? IsAudit { get; set; }
    }
}
