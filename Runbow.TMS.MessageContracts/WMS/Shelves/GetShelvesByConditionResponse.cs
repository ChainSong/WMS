using System.Collections.Generic;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Shelves;
namespace Runbow.TWS.MessageContracts.WMS.Shelves
{
    public class GetShelvesByConditionResponse
    {
        public IEnumerable<Runbow.TWS.Entity.WMS.Shelves.Shelves> Shelves { get; set; }

        public IEnumerable<ReceiptDetail> ReceiptDetailCollection { get; set; }
        public StoresByGetReceipt storesByGetReceipt { get; set; }
    }
}
