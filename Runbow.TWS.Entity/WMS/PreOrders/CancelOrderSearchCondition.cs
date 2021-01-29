using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.PreOrders
{
    /// <summary>
    /// 出库单取消查询条件model
    /// </summary>
    public class CancelOrderSearchCondition : CancelOrderInfo
    {
        public string StartCreateTime { get; set; }//开始时间
        public string EndCreateTime { get; set; }//结束时间
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
