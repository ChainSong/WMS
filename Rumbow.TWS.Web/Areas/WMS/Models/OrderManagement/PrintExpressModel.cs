using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WebApi.Express;
using Runbow.TWS.Entity.WMS.NikeOSRBJPrint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderManagement
{
    public class PrintExpressModel
    {
        public IEnumerable<OrderInfo> OrderInfos { get; set; }//订单头信息

        public IEnumerable<PrintExpressJite> EnumerableExpressInfo { get; set; }

        public IEnumerable<PrintExpressYd> YdExpressInfo { get; set; }


    }
}