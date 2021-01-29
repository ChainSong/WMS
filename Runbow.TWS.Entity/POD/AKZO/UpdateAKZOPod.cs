using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class UpdateAKZOPod
    {
        public long CustomerID
        {
            get
            {
                return 7;
            }
        }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("Weight", "Weight")]
        public double Weight { get; set; }

        [EntityPropertyExtension("TS", "TS")]
        public double TS { get; set; }

        [EntityPropertyExtension("SS", "SS")]
        public double SS { get; set; }

        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public DateTime ActualDeliveryDate { get; set; }

        [EntityPropertyExtension("CustomerAccount", "CustomerAccount")]
        public string CustomerAccount { get; set; }
    }
}