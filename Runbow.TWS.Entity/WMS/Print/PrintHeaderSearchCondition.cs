using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Print
{
    public class PrintHeaderSearchCondition:PrintHeader
    {
        public string OrderKey { get; set; }

        public DateTime? StartUpdateTime { get; set; }
        public DateTime? EndUpdateTime { get; set; }
        public string CustomerIDs;
    }
}
