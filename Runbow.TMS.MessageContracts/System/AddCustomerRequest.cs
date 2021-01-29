using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddCustomerRequest
    {

        public Customer Customer { get; set; }

        public IEnumerable<Customer> customers { get; set; }

        public bool IsCoverOld { get; set; }
   
    }
}
