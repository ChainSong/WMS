namespace Runbow.TWS.MessageContracts
{
    public class SetProjectCustomerOrShipperSegmentRequest
    {
        public int Target { get; set; }

        public long CustomerOrShipperID { get; set; }

        public string SegmentIDs { get; set; }

        public long SegmentID { get; set; }

        public long ProjectID { get; set; }

        public long RelatedCustomerID { get; set; }

        public string RelatedCustomerIDs { get; set; }
    }
}