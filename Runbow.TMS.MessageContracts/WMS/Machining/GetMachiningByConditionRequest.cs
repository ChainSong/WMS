using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.Machining
{
    public class GetMachiningByConditionRequest
    {
        public MachiningSearchCondition SearchCondition { get; set; }

        public IEnumerable<WMS_MachiningHeaderAndDetail> MachiningCollection { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
