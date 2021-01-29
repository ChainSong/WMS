using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class ManageShipperEmailInfoRequest
    {
        public long ProjectID { get; set; }

        public long RelatedCustomerID { get; set; }

        public long ShipperID { get; set; }

        public string ShipperName { get; set; }

        public string EmailAddress { get; set; }

        public string EmailContent { get; set; }

        public int Type { get; set; }
    }
}
