using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddReceiptAndReceiptDetailRequest
    {
        public IEnumerable<Receipt> Receipts { get; set; }
        public IEnumerable<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
