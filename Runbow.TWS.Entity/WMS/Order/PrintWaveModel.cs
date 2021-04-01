using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    /// <summary>
    /// 波次打印类
    /// </summary>
    public class PrintWaveModel
    {

        public IEnumerable<WMS_Wave> WaveHeaderLists { get; set; }
        public IEnumerable<WMS_WaveDetail> WaveDetailLists { get; set; }

        public IEnumerable<OrderInfo> OrderLists { get; set; }
        public IEnumerable<OrderDetailInfo> OrderDetailLists { get; set; }

    }
}
