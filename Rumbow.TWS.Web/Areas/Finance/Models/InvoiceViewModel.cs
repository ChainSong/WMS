using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.Finance.Models
{
    public class InvoiceViewModel
    {
        public IEnumerable<SettledPod> SettledPods { get; set; }

        public Invoice Invoice { get; set; }

        public IEnumerable<SelectListItem> InvoiceTypes { get; set; }

        public bool IsViewModel { get; set; }

        public string ServerValues { get; set; }

        public int ReturnType { get; set; }
    }
}