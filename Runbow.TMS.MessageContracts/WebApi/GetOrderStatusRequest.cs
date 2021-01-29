using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WebApi;

namespace Runbow.TWS.MessageContracts.WebApi
{
    public  class GetOrderStatusRequest
    {
        public OrderCancel OrderCancel { get; set; }
    }
}
