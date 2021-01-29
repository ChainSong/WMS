using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class WXPODBarCode
    {
        [EntityPropertyExtension("TicketID", "TicketID")]
        public long TicketID { get; set; }

        [EntityPropertyExtension("PODID", "PODID")]
        public long PODID { get; set; }

        [EntityPropertyExtension("ticketkey", "ticketkey")]
        public string ticketkey { get; set; }

        [EntityPropertyExtension("UseStatus", "UseStatus")]
        public int UseStatus { get; set; }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }
    }
}
