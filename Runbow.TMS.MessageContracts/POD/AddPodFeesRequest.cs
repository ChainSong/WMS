using System.Collections.Generic;
using Runbow.TWS.Entity;


namespace Runbow.TWS.MessageContracts
{
    public class AddPodFeesRequest
    {
        public IEnumerable<PodFee> PodFees { get; set; }

        public long CustomerID { get; set; }
    }
}
