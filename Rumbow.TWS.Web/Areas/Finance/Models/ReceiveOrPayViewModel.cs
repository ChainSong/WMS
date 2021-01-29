using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.Finance.Models
{
    public class ReceiveOrPayViewModel
    {
        public Invoice Invoice { get; set; }

        public IEnumerable<ReceiveOrPayOrders> ReceiveOrPayOrders { get; set; }
    }
}