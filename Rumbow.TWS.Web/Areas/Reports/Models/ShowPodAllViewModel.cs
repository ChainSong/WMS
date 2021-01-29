using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.Reports.Models
{
    public class ShowPodAllViewModel
    {
        public Table Config { get; set; }

        public PodSearchCondition SearchCondition { get; set; }

        public PodInvoiceReceiveOrPayOrders PodInvoiceReceiveOrPayOrders { get; set; }

        public IEnumerable<SelectListItem> PodStates { get; set; }

        public IEnumerable<SelectListItem> ShipperTypes { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Shippers { get; set; }

        public IEnumerable<SelectListItem> PODTypes { get; set; }

        public IEnumerable<SelectListItem> TtlOrTpls { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public bool ShowEditRelated { get; set; }

        public long ProjectRoleID { get; set; }

        public bool IsInnerUser { get; set; }

        public IEnumerable<SelectListItem> TrueOrFalse
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "N" }, 
                    new SelectListItem() { Value = "1", Text = "Y" } 
                };
            }
        }
        public string ReturnClientMessage { get; set; }
    }
}