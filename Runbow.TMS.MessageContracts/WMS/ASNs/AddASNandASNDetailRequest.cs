using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.ASNs
{
   public class AddASNandASNDetailRequest
    {
        public IEnumerable<ASN> asn { get; set; }
        public IEnumerable<ASNDetail> asnDetails { get; set; }
    }
}
