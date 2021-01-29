using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodViewModel
    {
        public Pod Pod { get; set; }

        public Module ModuleConfig { get; set; }

        public IEnumerable<SelectListItem> PodStates { get; set; }

        public IEnumerable<SelectListItem> ShipperTypes { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Shippers { get; set; }

        public IEnumerable<SelectListItem> PODTypes { get; set; }

        public IEnumerable<SelectListItem> TtlOrTpls { get; set; }

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

        public bool IsEditModel { get; set; }
    }
}