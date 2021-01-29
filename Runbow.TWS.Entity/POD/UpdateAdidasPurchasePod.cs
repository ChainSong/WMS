using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD
{
    public class UpdateAdidasPurchasePod
    {
        public long CustomerID
        {
            get;

            set;
        }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("TS", "TS")]
        public double TS { get; set; }

        [EntityPropertyExtension("Volume", "Volume")]
        public double Volume { get; set; }


        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public DateTime ActualDeliveryDate { get; set; }

        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }
    }
}
