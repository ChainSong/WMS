using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
   public class AdjustmentAndAdjustmentDetail
    {
       public Adjustment adjustment { get; set; }
       public IEnumerable<AdjustmentDetail> adjustmentDetails { get; set; }
    }
}
