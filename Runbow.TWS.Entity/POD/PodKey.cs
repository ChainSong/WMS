using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class PodKey
    {
        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("ShipperName", "ShipperName")]
        public string ShipperName { get; set; }

        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public string ActualDeliveryDate { get; set; }

        [EntityPropertyExtension("StartCityName", "StartCityName")]
        public string StartCityName { get; set; }

        [EntityPropertyExtension("PODTypeName", "PODTypeName")]
        public string PODTypeName { get; set; }

        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("PODStateName", "PODStateName")]
        public string PODStateName { get; set; }

        [EntityPropertyExtension("ShipperTypeName", "ShipperTypeName")]
        public string ShipperTypeName { get; set; }

        [EntityPropertyExtension("TtlOrTplName", "TtlOrTplName")]
        public string TtlOrTplName { get; set; }

        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        [EntityPropertyExtension("Weight", "Weight")]
        public string Weight { get; set; }

        [EntityPropertyExtension("GoodsNumber", "GoodsNumber")]
        public string GoodsNumber { get; set; }

        [EntityPropertyExtension("Volume", "Volume")]
        public string Volume { get; set; }
    }
}