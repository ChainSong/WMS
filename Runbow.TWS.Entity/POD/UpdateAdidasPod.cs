using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.POD
{
    public  class UpdateAdidasPod
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


        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public DateTime ActualDeliveryDate { get; set; }
    }
}
