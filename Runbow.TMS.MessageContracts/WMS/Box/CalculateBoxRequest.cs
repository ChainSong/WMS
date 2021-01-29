using Runbow.TWS.Entity.WMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Warehouse
{
    public class CalculateBoxRequest
    {
        public IEnumerable<CalculateBoxModel>  calculateBoxModels { get; set; }
    }
}
