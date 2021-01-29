using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class ShipperRelatedInfoViewModel
    {
        public long ProjectID { get; set; }

        public long RelatedCustomerID { get; set; }

        public long ShipperID { get; set; }

        public string ShipperName { get; set; }

        public long StartCityID { get; set; }

        public string StartCityName { get; set; }

        public long EndCityID { get; set; }

        public string EndCityName { get; set; }

        public ShipperRelatedInfo ShipperRelatedInfo { get; set; }

        public IEnumerable<ShipperRegionCovered> ShipperRegionCoveredCollection { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Shippers { get; set; }
    }
}