using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ShipperRelatedInfo
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("RelatedCustomerID", "RelatedCustomerID")]
        public long RelatedCustomerID { get; set; }

        [EntityPropertyExtension("ShipperID", "ShipperID")]
        public long ShipperID { get; set; }

        [EntityPropertyExtension("ShipperName", "ShipperName")]
        public string ShipperName { get; set; }

        [EntityPropertyExtension("EndTime", "EndTime")]
        public DateTime? EndTime { get; set; }

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

        [EntityPropertyExtension("Str11", "Str11")]
        public string Str11 { get; set; }

        [EntityPropertyExtension("Str12", "Str12")]
        public string Str12 { get; set; }

        [EntityPropertyExtension("Bit1", "Bit1")]
        public bool? Bit1 { get; set; }

        [EntityPropertyExtension("Bit2", "Bit2")]
        public bool? Bit2 { get; set; }

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }

        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }
    }
}
