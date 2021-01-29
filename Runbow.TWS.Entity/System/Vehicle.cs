using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class Vehicle
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ShipperID", "ShipperID")]
        public long ShipperID { get; set; }

        [EntityPropertyExtension("PlateNumber", "PlateNumber")]
        public string PlateNumber { get; set; }

        [EntityPropertyExtension("Pilot", "Pilot")]
        public string Pilot { get; set; }

        [EntityPropertyExtension("JobNumber", "JobNumber")]
        public string JobNumber { get; set; }

        [EntityPropertyExtension("Contract", "Contract")]
        public string Contract { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

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

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }

        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }

        [EntityPropertyExtension("Decimal1", "Decimal1")]
        public decimal? Decimal1 { get; set; }
    }
}