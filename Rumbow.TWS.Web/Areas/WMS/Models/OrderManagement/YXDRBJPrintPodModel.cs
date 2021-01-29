using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity.WMS.YXDRBJPrint;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderManagement
{
    public class YXDRBJPrintPodModel
    {

        public YXDRBJPrintPodInfo YXDRPodInfo { get; set; }
        public IEnumerable<YXDRBJPrintPodInfo> EnumerableYXDRPodInfo { get; set; }
    }
}