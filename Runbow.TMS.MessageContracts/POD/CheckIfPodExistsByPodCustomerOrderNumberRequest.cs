using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class CheckIfPodExistsByPodCustomerOrderNumberRequest
    {
        public IEnumerable<string> CustomerOrderNumberCollection { get; set; }

        public long CustomerID { get; set; }

        public long ProjectID { get; set; }
    }
}