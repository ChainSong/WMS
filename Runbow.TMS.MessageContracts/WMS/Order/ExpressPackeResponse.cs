using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class ExpressPackageResponse
   {
       public PackageInfo PackageCollection { get; set; }
       public IEnumerable<OrderDetailInfo> OrderDetailCollection { get; set; }
       public int AssociationStatus { get; set; }
    }
}
