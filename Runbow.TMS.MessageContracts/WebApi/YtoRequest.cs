
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WebApi.Express;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WebApi
{
    public class YtoRequest
    {
        public YtoExpressrNumberInfo numberInfo { get; set; }
        public Entity.OrderInfo orderInfo { get; set; }
        public YtoSender ytoSender { get; set; }
        public Entity.WarehouseInfo warehouseInfo { get; set; }
        public List<Entity.OrderDetailInfo> orderDetailInfos { get; set; }
        public ExpressDelivery  expressDelivery { get; set; }
        public IEnumerable<ExpressDelivery>  expressDeliverys { get; set; }
        public IEnumerable<PackageDetailInfo> packageDetailInfos { get; set; }

    }
}
