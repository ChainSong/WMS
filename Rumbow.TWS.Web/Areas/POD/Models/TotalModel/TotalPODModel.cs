using Runbow.TWS.Entity;
using Runbow.TWS.Entity.POD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.POD.Models.TotalModel
{
    public class TotalPODModel
    {
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public IEnumerable<SelectListItem> States
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "否" }, 
                    new SelectListItem() { Value = "1", Text = "是" } 
                };

            }
        }
        public TotalPODEntity SearchCondition { get; set; }

        public IEnumerable<Pod> TotalPODCollection { get; set; }
    }
}