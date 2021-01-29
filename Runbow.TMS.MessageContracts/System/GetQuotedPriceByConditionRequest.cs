using System;

namespace Runbow.TWS.MessageContracts
{
    public class GetQuotedPriceByConditionRequest
    {
        public long ProjectID { get; set; }

        public int Target { get; set; }

        public long? CustomerOrShipperID { get; set; }

        public long? TransportationLineID { get; set; }

        public long? StartCityID { get; set; }

        public long? EndCityID { get; set; }

        public long? ShipperTypeID { get; set; }

        public long? PodTypeID { get; set; }

        public long? TtlOrTplID { get; set; }

        public DateTime? EffectiveStartTime { get; set; }

        public DateTime? EffectiveEndTime { get; set; }

        public long? RelatedCustomerID { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}