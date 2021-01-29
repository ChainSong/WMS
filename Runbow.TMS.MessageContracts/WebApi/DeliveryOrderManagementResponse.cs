using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.OnlineOrder;

namespace Runbow.TWS.MessageContracts.WebApi
{
    public class DeliveryOrderManagementRequest
    {
        public IEnumerable<DeliveryOrder> DeliveryOrderinfo { get; set; }

        public DeliveryOrderSearchCondition SearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        
        //根据用户名来查询客户自己所操作的单子
        public string UserName { get; set; }

        //1是保存，2是删除
        public char Operation { get; set; }
    }
}
