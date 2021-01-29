using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodReleatedInforBaseViewModel
    {
        public Table Config { get; set; }

        public bool IsEditModel { get; set; }

        public long CustomerID { get; set; }

        public bool IsOuterUser { get; set; }

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
    }
}