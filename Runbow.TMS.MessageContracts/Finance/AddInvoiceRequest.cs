using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddInvoiceRequest
    {
        public IEnumerable<SettledPod> SettledPods { get; set; }

        public Invoice Invoice { get; set; }
    }
}
