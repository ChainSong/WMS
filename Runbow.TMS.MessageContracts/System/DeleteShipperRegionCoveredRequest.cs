namespace Runbow.TWS.MessageContracts
{
    public class DeleteShipperRegionCoveredRequest
    {
        public long ProjectID { get; set; }

        public long RelatedCustomerID { get; set; }

        public long ShipperID { get; set; }

        public long StartCityID { get; set; }

        public long EndCityID { get; set; }
    }
}