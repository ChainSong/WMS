using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.ASNs
{
   public class GetASNDetailByConditionResponse
    {
        public IEnumerable<ASNDetail> AsnDetailCollection { get; set; }
        public ASN asn { get; set; }
        public IEnumerable<ASN> AsnCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
