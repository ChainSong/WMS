using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.ASNs
{
  public  class GetASNByConditionRequest
    {
        public ASNSearchCondition SearchCondition { get; set; }

        public Int32 ID { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
