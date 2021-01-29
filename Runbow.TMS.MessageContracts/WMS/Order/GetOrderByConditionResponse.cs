using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
   public class GetOrderByConditionResponse
   {
       public IEnumerable<OrderInfo> OrderCollection { get; set; }

       public IEnumerable<OrderDetailInfo> OrderDetailCollection { get; set; }

       public IEnumerable<PackageInfo> packages { get; set; }
       public IEnumerable<PackageDetailInfo> packageDetails { get; set; }

       public int PageCount { get; set; }

       public int PageIndex { get; set; }
    }
}
