using Runbow.TWS.Entity.WMS.PreOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Runbow.TWS.Web.Interface
{
    internal interface IAllocate
    {
        IEnumerable<DistributionInformation> Allocate();
    }
  
}