using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.Finance.Models
{
    public class InvoiceManageViewModel
    {
        public InvoiceSearchCondition SearchCondition { get; set; }

        public IEnumerable<Invoice> Invoices { get; set; }

        public IEnumerable<SelectListItem> IsCompletes = new List<SelectListItem>() { new SelectListItem() { Value = "false", Text = "N" }, new SelectListItem() { Value = "true", Text = "Y" } };

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Shippers { get; set; }

        public IEnumerable<SelectListItem> InvoiceTypes { get; set; }

        public string Message { get; set; }

        public bool ForReceiveOrPay { get; set; }
    }
}