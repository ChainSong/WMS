namespace Runbow.TWS.MessageContracts
{
    public class GetSegmentDetailByCustomerOrShipperRequest
    {
        public long ProjectID { get; set; }

        public int Target { get; set; }

        public long CustomerOrShipperID { get; set; }

        public long RelatedCustomerID { get; set; }
    }
}