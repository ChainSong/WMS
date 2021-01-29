using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class SetAttachmentRemarkRequest
    {
        public long ID { get; set; }

        public string Remark { get; set; }

        public string AuditUser { get; set; }
    }
}
