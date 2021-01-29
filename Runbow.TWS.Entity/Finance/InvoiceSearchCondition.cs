using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class InvoiceSearchCondition : Invoice
    {
        public DateTime? EndCreateTime { get; set; }

        public DateTime? EndEstimateDate { get; set; }

        public IEnumerable<long> ProjectUserCustomerIDs { get; set; }
    }
}
