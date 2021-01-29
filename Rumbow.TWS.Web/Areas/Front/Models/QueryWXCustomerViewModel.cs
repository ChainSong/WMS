using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.Front
{
    public class QueryWXCustomerViewModel
    {
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

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

        public WXCustomerSearchCondition SearchCondition { get; set; }

        public IEnumerable<WXCustomer> WXCustomerCollection { get; set; } 
    }
}