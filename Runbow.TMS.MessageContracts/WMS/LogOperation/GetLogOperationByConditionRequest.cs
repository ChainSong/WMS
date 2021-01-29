using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Log;

namespace Runbow.TWS.MessageContracts.WMS.LogOperation
{
    public class GetLogOperationByConditionRequest
    {
        public LogOperationSearchCondition SearchCondition { get; set; }
        public LogOperationRFSearchCondition RFSearchCondition { get; set; }

        public Int32 ID { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
