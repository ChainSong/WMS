using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS;

namespace Runbow.TWS.MessageContracts.WMS.BarCode
{
    public class GetBarCodeByConditionResponse
    {
        public IEnumerable<BarCodeInfo> BarCodeCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
