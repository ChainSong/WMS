using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class AddInvoiceAndPayOrdersRequest
    {
        public long SettledPodID { get; set; }

        public string InvoiceSystemNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public decimal TotalAmt { get; set; }

        public string Remark { get; set; }

        public DateTime Date { get; set; }

        public string ReceiveOrPayOrderNumber { get; set; }

        public string Creator { get; set; }
    }
}
