using System.Collections.Generic;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;

namespace Runbow.TWS.MessageContracts
{
    public class AddQuotedPriceRequest
    {
        public IEnumerable<QuotedPrice> QuotedPrices { get; set; }

        public IEnumerable<QuotedPrices> QuotedPrices_New { get; set; }
    }
}