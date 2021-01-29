namespace Runbow.TWS.MessageContracts
{
    public class QueryCustomerPodCountByRegionAndTimeRequest
    {
        public long CustomerID { get; set; }

        public string Year { get; set; }

        public int Month { get; set; }
    }
}