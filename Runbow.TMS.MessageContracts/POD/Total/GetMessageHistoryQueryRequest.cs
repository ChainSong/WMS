using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Runbow.TWS.MessageContracts.POD.Total
{
    public class GetMessageHistoryQueryRequest
    {
        public string SqlWhere { get; set; }
        public DataTable MessageHistoryTable { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
}
