using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class WXCustomerSearchCondition
    {
        public long ID { get; set; }

        public string WeixinName { get; set; }

        public string RealName { get; set; }

        public string Phone { get; set; }

        public string UnitName { get; set; }

        public int? Status { get; set; }

        public DateTime? StatCreateTime { get; set; }

        public DateTime? EndCreateTime { get; set; }
    }
}
