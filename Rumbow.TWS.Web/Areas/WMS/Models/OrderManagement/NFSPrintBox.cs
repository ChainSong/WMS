using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity.WMS.NikeNFSPrint;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderManagement
{
    public class NFSPrintBox
    {
        public NFSPrintBoxInfo CustomerInfoinfo { get; set; }
        public IEnumerable<NFSPrintBoxInfo> EnumerableCustomerInfo { get; set; }
    }
}