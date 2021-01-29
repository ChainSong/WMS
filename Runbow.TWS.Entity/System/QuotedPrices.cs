using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.System
{
    public class QuotedPrices
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("ProjectName", "ProjectName")]
        public string ProjectName { get; set; }

        [EntityPropertyExtension("Target", "Target")]
        public int Target { get; set; }

        [EntityPropertyExtension("TargetName", "TargetName")]
        public string TargetName { get; set; }

        [EntityPropertyExtension("StartCityName", "StartCityName")]
        public string StartCityName { get; set; }

        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("PodTypeName", "PodTypeName")]
        public string PodTypeName { get; set; }

        [EntityPropertyExtension("ShipperTypeName", "ShipperTypeName")]
        public string ShipperTypeName { get; set; }

        [EntityPropertyExtension("P200", "P200")]
        public decimal? P200 { get; set; }
        [EntityPropertyExtension("P500", "P500")]
        public decimal? P500 { get; set; }
        [EntityPropertyExtension("P1000", "P1000")]
        public decimal? P1000 { get; set; }
        [EntityPropertyExtension("P2000", "P2000")]
        public decimal? P2000 { get; set; }
        [EntityPropertyExtension("P5000", "P5000")]
        public decimal? P5000 { get; set; }
        [EntityPropertyExtension("P10000", "P10000")]
        public decimal? P10000 { get; set; }
        [EntityPropertyExtension("P20000", "P20000")]
        public decimal? P20000 { get; set; }
        [EntityPropertyExtension("P30000", "P30000")]
        public decimal? P30000 { get; set; }
        [EntityPropertyExtension("P99999", "P99999")]
        public decimal? P99999 { get; set; }

        [EntityPropertyExtension("RelatedCustomerName", "RelatedCustomerName")]
        public string RelatedCustomerName { get; set; }

    }
}
