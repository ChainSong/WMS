using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class AuditSettledPodRequest
    {
        public IEnumerable<long> SettledPodIDs { get; set; }

        public string Auditor { get; set; }

        public DateTime AuditTime { get; set; }

        public string AuditRemark { get; set; }

        public string AuditTypeMessage { get; set; }

        public int AuditType { get; set; }
    }
}
