using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class CompleteOrCancelInvoiceRequest
    {
        public long ID { get; set; }

        public bool CurrentCompleteState { get; set; }
    }
}
