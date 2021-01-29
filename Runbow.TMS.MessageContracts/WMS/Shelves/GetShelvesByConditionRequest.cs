using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;



namespace Runbow.TWS.MessageContracts.WMS.Shelves
{
    public class GetShelvesByConditionRequest
    {
        public IEnumerable<ReceiptReceiving> receiptReceiving { get; set; }

        public ReceiptReceivingSearchCondition SearchCondition { get; set; }

        public string User { get; set; }
 
       
       
    }
}
