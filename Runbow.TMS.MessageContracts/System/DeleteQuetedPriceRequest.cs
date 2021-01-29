using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class DeleteQuetedPriceRequest
    {
        public IEnumerable<long> QutedPriceIDs { get; set; }
    }
}