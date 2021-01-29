using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class OrderDetailSearchCondition : OrderDetailInfo
    {
        public int UserType { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public DateTime? StartOrderTime { get; set; }
        public DateTime? EndOrderTime { get; set; }
        public DateTime? StartUpdateTime { get; set; }
        public DateTime? EndUpdateTime { get; set; }
        public DateTime? StartDateTime1 { get; set; }
        public DateTime? EndDateTime1 { get; set; }
        public DateTime? StartDateTime2 { get; set; }
        public DateTime? EndDateTime2 { get; set; }
        public DateTime? StartDateTime3 { get; set; }
        public DateTime? EndDateTime3 { get; set; }
        public DateTime? StartDateTime4 { get; set; }
        public DateTime? EndDateTime4 { get; set; }
        public DateTime? StartDateTime5 { get; set; }
        public DateTime? EndDateTime5 { get; set; }
    }
}
