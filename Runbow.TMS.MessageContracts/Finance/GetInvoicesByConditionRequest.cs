using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetInvoicesByConditionRequest
    {
        public InvoiceSearchCondition SearchCondition { get; set; }
    }
}