using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.ASNs
{
    [Serializable]
   public class ASNAndASNDetail
    {
        public ASN asn { get; set; }
        public  IEnumerable <ASNDetail> asnDetails { get; set; }
    }
}
