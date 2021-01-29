using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Receipt
{
    public class ASNAbnormalSearchCondition : ASNAbnormalTracking
    {
        public string StartCreateTime { get; set; }//开始时间
        public string EndCreateTime { get; set; }//结束时间
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
