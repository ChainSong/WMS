using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class SetProjectCustomerOrShippersRequest
    {
        public IEnumerable<ProjectCustomersOrShippers> ProjectCustomersOrShippers { get; set; }

        public long ProjectID { get; set; }

        public int Target { get; set; }
    }
}