using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity.WMS.NikeOSRBJPrint;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderManagement
{
    public class PrintPodModel
    {
      
        public PrintPodInfo CustomerInfoinfo { get; set; }
        /// <summary>
        /// 主
        /// </summary>
        public IEnumerable<PrintPodInfo> EnumerableCustomerInfo { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public IEnumerable<PrintPodInfo> PrintPodInfos { get; set; }
    }
}