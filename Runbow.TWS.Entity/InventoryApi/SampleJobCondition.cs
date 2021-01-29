using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class SampleJobCondition
    {
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
