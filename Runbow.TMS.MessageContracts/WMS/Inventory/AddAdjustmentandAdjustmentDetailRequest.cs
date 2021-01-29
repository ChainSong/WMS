using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
   public class AddAdjustmentandAdjustmentDetailRequest
    {
       public string AdID { get; set; }
       public IEnumerable<Adjustment> adjustment { get; set; }
       public IEnumerable<AdjustmentDetail> adjustmentDetails { get; set; }
    }
}
