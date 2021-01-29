using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class WeiQueryPod
    {
        [EntityPropertyExtension("WID", "WID")]
        public string WID { get; set; }

        [EntityPropertyExtension("ProjectId", "ProjectId")]
        public string ProjectId { get; set; }

        [EntityPropertyExtension("ProjectName", "ProjectName")]
        public string ProjectName { get; set; }

        [EntityPropertyExtension("CustomerCode", "CustomerCode")]
        public string CustomerCode { get; set; }

        [EntityPropertyExtension("UID", "UID")]
        public string UID { get; set; }

        [EntityPropertyExtension("TransOrderNumber", "TransOrderNumber")]
        public string TransOrderNumber { get; set; }

        [EntityPropertyExtension("OriginLoadCity", "OriginLoadCity")]
        public string OriginLoadCity { get; set; }

        [EntityPropertyExtension("DestinationCity", "DestinationCity")]
        public string DestinationCity { get; set; }

        [EntityPropertyExtension("ArrTime", "ArrTime")]
        public string ArrTime { get; set; }

        [EntityPropertyExtension("PlanTtlPiecQuantity", "PlanTtlPiecQuantity")]
        public string PlanTtlPiecQuantity { get; set; }

        [EntityPropertyExtension("CarrierCode", "CarrierCode")]
        public string CarrierCode { get; set; }
    }
}
