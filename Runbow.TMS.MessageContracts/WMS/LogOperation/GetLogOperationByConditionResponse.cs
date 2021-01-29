using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Log;

namespace Runbow.TWS.MessageContracts.WMS.LogOperation
{
    public class GetLogOperationByConditionResponse
    {
        public IEnumerable<WMS_Log_Operation> LogOperationCollection { get; set; }

        public IEnumerable<WMS_Log_OperationRF> LogOperationRFCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
