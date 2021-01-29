using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.Box;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Warehouse
{
    public class CalculateBoxResponse
    {
        public IEnumerable<CalculateBoxModel> calculateBoxModels { get; set; }
        public IEnumerable<BoxTypeModel> boxTypeModels  { get; set; }

    }
}
