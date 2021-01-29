using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.Reports;
using Runbow.TWS.MessageContracts.Reports;

namespace Runbow.TWS.Web.Areas.Reports.Models
{
    public class TransOrderModel
    {
        public TransOrderRequest transOrderRequest { get; set; }

        public PodSearchCondition SearchCondition { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public IEnumerable<TransOrder> transOrder { get; set; }

        public IEnumerable<SelectListItem> Customers
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "==请选择==" }, 
                    new SelectListItem() { Value = "1", Text = "Adidas" }, 
                    new SelectListItem() { Value = "2", Text = "Nike" } ,
                    new SelectListItem() { Value = "3", Text = "Hilti"},
                    new SelectListItem() { Value = "4", Text = "Akzo" },
                    new SelectListItem() { Value = "5", Text = "Dowcorning" },
                };

            }
        }


    }
}