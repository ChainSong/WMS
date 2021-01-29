using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.ASNs
{
   public class GetASNByConditionResponse
    {
        public IEnumerable<ASNScan> ASNScanBoxSKUCollection { get; set; }
        public IEnumerable<ASNScan> ASNScanBoxDetailSKUCollection { get; set; }
        public ASNScan ExpectTotalBox { get; set; }
        public ASNScan ReceiveTotalBox { get; set; }
        public ASNScan ExpectTotalSKU { get; set; }
        public ASNScan ReceiveTotalSKU { get; set; }
       
        public IEnumerable<ASN> ASNCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
