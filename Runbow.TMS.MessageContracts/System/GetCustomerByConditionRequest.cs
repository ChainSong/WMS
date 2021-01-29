using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
   public  class GetCustomerByConditionRequest
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public long UserId { get; set; }

        public long ProjectId { get; set; }

        public bool State { get; set; }

        public int StoreType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

    
    }
}
