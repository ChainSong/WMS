using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity.WMS.NikeOSRBJPrint;


namespace Runbow.TWS.Web.Areas.WMS.Models.OrderManagement
{
    public class PrintBoxModel
    {
        public PrintBoxInfo CustomerInfoinfo { get; set; }
        public IEnumerable<PrintBoxInfo> EnumerableCustomerInfo { get; set; }
        public IEnumerable<PrintExpressJite> EnumerableExpressInfo { get; set; }
        
    }
}