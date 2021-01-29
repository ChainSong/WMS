using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.SettlementManagement
{
    public class HiltiCount
    {
        [EntityPropertyExtension("count1", "count1")]
        public int count1 { get; set; }
        [EntityPropertyExtension("count2", "count2")]
        public int count2 { get; set; }
    }

    public class HabalClass
    {
        [EntityPropertyExtension("vo1", "vo1")]
        public decimal? vo1 { get; set; }  //收入订单体积之和
        [EntityPropertyExtension("sum1", "sum1")]
        public int? sum1 { get; set; }     //当天电商出库总单数
        [EntityPropertyExtension("sum2", "sum2")]
        public int? sum2 { get; set; }     //当天经销商出库总单数
        [EntityPropertyExtension("sum3", "sum3")]
        public decimal? sum3 { get; set; } //当天经销商出库订单体积之和
    }
    
    public class IEnumerablerResult
    {
        public IEnumerable<HiltiCount> A { get; set; }
        public IEnumerable<HiltiCount> B { get; set; }
        public IEnumerable<HabalClass> C { get; set; }
        public IEnumerable<HabalClass> D { get; set; }
    }

}
