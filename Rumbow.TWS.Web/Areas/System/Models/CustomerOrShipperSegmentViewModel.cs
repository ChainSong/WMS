using System.Collections.Generic;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class CustomerOrShipperSegmentViewModel
    {
        public int Target { get; set; }

        public long ProjectID { get; set; }

        public string SelectedCustomerOrShipperID { get; set; }

        public string ShipperName { get; set; }

        public long SelectedCustomerOrShipperSegment { get; set; }

        public long SelectedCustomerID { get; set; }

        public IEnumerable<Runbow.TWS.Entity.ProjectCustomerOrShipperSegment> ProjectCustomerOrShipperSegments { get; set; }

        public IEnumerable<SelectListItem> CustomerOrShippersCollection { get; set; }

        public IEnumerable<SelectListItem> Segments { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }
    }
}