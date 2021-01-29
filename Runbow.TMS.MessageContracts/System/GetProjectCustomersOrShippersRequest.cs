namespace Runbow.TWS.MessageContracts
{
    public class GetProjectCustomersOrShippersRequest
    {
        public int Target { get; set; }

        public long ProjectID { get; set; }
    }
}