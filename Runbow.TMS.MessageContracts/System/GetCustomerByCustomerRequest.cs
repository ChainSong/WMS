namespace Runbow.TWS.MessageContracts
{
    public class GetCustomerByCustomerIdRequest
    {
        public long CustomerID { get; set; }
    }

    public class SaveCustomer
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}