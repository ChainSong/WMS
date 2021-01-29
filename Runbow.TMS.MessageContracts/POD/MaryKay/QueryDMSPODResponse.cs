using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class QueryDMSPODResponse
    {
        public IEnumerable<DMS_POD> DMS_PODCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
