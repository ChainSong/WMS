using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.Reports
{
    public class WMSOrderStatusRequest
    {

        public int ID { get; set; }

        public string Project { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? StartCreateTime { get; set; }

        public DateTime? EndCreateTime { get; set; }

        
    }
}
