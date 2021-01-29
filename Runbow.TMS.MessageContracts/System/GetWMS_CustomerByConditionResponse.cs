using Runbow.TWS.Entity.WMS.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.System
{
   public class GetWMS_CustomerByConditionResponse
    {
        public IEnumerable<WMS_Customer> Customer { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
