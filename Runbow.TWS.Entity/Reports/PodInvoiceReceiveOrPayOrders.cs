using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class PodInvoiceReceiveOrPayOrders
    {
        public IEnumerable<Pod> PodCollection { get; set; }

        public IEnumerable<SettledPod> SettledPodCollection { get; set; }

        public IEnumerable<Invoice> InvoiceCollection { get; set; }

        public IEnumerable<ReceiveOrPayOrders> ReceiveOrPayOrderCollection { get; set; }
    }
}
