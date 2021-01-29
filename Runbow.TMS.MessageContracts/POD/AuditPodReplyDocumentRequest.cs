using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class AuditPodReplyDocumentRequest
    {
        public IEnumerable<string> SystemNumbers { get; set; }

        public string AuditUser { get; set; }
    }
}
