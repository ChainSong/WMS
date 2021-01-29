using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class QueryTianjinPodViewModel
    {
        public long? CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string CustomerOrderNumbers { get; set; }

        public long? ShipperID { get; set; }

        public string ShipperName { get; set; }

        public string InOrOut { get; set; }

        public DateTime? ActualDeliverlyDate { get; set; }

        public DateTime? EndActualDeliverlyDate { get; set; }

        public DateTime? ExpertArrivalDate { get; set; }

        public DateTime? EndExpertArrivalDate { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<Pod> PodCollection { get; set; }

        public IEnumerable<SelectListItem> InOrOuts
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "进HUB" },
                    new SelectListItem() { Value = "1", Text = "出HUB" }
                };
            }
        }

    }
}