using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryScanDataResponses
    {
        public IEnumerable<ScanInfo> ScanDataCollection { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }
    }
}
