using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPodExceptionsRequest
    {
        public IEnumerable<PodException> PodExceptions { get; set; }

        public long CustomerID { get; set; }
    }
}