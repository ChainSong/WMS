using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
   public  class GetCustomerByConditionResponse
    {
       public IEnumerable<Customer> Customer { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
