using System;
using Runbow.TWS.Common;
namespace Runbow.TWS.Entity
{
   public class UpdateNikePodBG
    {
        public long CustomerID
        {
            get
            {
                return 8;
            }
        }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public double? BoxNumber { get; set; }

        [EntityPropertyExtension("GoodsNumber", "GoodsNumber")]
        public double? GoodsNumber { get; set; }

      
    }
}
