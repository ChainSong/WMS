using System.Collections.Generic;

namespace Runbow.TWS.Entity
{
    public class ShipperAllInfo
    {
        public Shipper Shipper { get; set; }

        public ShipperRelatedInfo ShipperRelatedInfo { get; set; }

        public IEnumerable<ShipperRegionCovered> ShipperRegionCoveredCollection { get; set; }
    }
}