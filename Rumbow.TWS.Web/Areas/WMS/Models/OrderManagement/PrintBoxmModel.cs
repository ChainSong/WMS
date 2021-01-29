using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderManagement
{
    public class PrintBoxmModel
    {
        public Table Config { get; set; }

        public PodSearchCondition SearchCondition { get; set; }

        public IEnumerable<PackageDetailInfo> PackageDetails { get; set; }

        public IEnumerable<SelectListItem> PodStates { get; set; }

        public int OrderID { get; set; }
        public int PackageID { get; set; }
        public string OrderNumber { get; set; }
        public string PackageNumber { get; set; }
        public string ShipFrom { get; set; }
        public string ShipTo { get; set; }
        public string ShipName { get; set; }
        public string RetrunNumber { get; set; }
        public string ShipAddress { get; set; }
        public string UPC { get; set; }
        public string SKU { get; set; }
        public string UOM { get; set; }
        public decimal QTY { get; set; }
        public string ReturnClientMessage { get; set; }
    }
}