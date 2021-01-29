using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddASNAndASNDetailRequest
    {
        public IEnumerable<ASN> asn { get; set; }
        public IEnumerable<ASNDetail> asnDetail { get; set; }
    }
}
