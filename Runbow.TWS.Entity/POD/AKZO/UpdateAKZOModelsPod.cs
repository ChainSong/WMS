using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class UpdateAKZOModelsPod
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


        [EntityPropertyExtension("Models", "Models")]
        public string Models { get; set; }
       
    }
}