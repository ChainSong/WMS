using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class GroupedPods
    {
        [EntityPropertyExtension("Target", "Target")]
        public int Target { get; set; }

        [EntityPropertyExtension("TargetID", "TargetID")]
        public long TargetID { get; set; }

        [EntityPropertyExtension("TargetName", "TargetName")]
        public string TargetName { get; set; }

        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public string ActualDeliveryDate { get; set; }

        [EntityPropertyExtension("ShipperTypeID", "ShipperTypeID")]
        public long ShipperTypeID { get; set; }

        [EntityPropertyExtension("ShipperTypeName", "ShipperTypeName")]
        public string ShipperTypeName { get; set; }

        [EntityPropertyExtension("PODTypeID", "PODTypeID")]
        public long PODTypeID { get; set; }

        [EntityPropertyExtension("PODTypeName", "PODTypeName")]
        public string PODTypeName { get; set; }

        [EntityPropertyExtension("TtlOrTplID", "TtlOrTplID")]
        public long TtlOrTplID { get; set; }

        [EntityPropertyExtension("TtlOrTplName", "TtlOrTplName")]
        public string TtlOrTplName { get; set; }

        [EntityPropertyExtension("StartCityID", "StartCityID")]
        public long StartCityID { get; set; }

        [EntityPropertyExtension("StartCityName", "StartCityName")]
        public string StartCityName { get; set; }

        [EntityPropertyExtension("EndCityID", "EndCityID")]
        public long EndCityID { get; set; }

        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public double BoxNumber { get; set; }

        [EntityPropertyExtension("Weight", "Weight")]
        public double Weight { get; set; }

        [EntityPropertyExtension("GoodsNumber", "GoodsNumber")]
        public double GoodsNumber { get; set; }

        [EntityPropertyExtension("Volume", "Volume")]
        public double Volume { get; set; }

        [EntityPropertyExtension("Volume", "Volume")]
        public int PodNumbers { get; set; }

        [EntityPropertyExtension("PodIDs", "PodIDs")]
        public string PodIDs { get; set; }
    }
}