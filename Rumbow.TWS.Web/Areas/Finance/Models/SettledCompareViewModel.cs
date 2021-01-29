using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.Finance.Models
{
    public class SettledCompareViewModel
    {
        public string DisplyMessage { get; set; }

        public int SettledType { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public long CustomerID { get; set; }

        public long? ShipperID { get; set; }

        public string ShipperName { get; set; }

        public IEnumerable<SettledPodCompare> SettledPodCompareCollection { get; set; }

        public string ErrorMessage { get; set; }
    }
}