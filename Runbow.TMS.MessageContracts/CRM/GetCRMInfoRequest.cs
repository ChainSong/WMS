using Runbow.TWS.Entity;
using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
   public class GetCRMInfoRequest
    {
       public IEnumerable<CRMInfo> IEnumerableCRMInfo { get; set; }
       public CRMInfo CRMInfo { get; set; }
       public int PageIndex { get; set; }
       public int PageSize { get; set; }
       public int PageCount { get; set; }
       public long? ID { get; set; } 
    }
}
