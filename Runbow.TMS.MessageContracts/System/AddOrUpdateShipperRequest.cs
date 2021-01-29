using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddOrUpdateShipperRequest
    {
        public Shipper Shipper { get; set; }

        public long  ProjectId { get; set; }
    }
}