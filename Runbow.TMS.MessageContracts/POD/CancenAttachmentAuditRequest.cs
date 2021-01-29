using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class CancenAttachmentAuditRequest
    {
        public long AttachmentID { get; set; }

        public long PodID { get; set; }
    }
}
