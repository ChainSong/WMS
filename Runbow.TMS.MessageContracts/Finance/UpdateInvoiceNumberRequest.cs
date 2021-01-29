namespace Runbow.TWS.MessageContracts
{
    public class UpdateInvoiceNumberRequest
    {
        public long ID { get; set; }

        public string InvoiceNumber { get; set; }
    }
}