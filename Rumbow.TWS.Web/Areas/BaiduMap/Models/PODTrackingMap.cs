using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts.BaiduMap;

namespace Runbow.TWS.Web.Areas.BaiduMap.Models
{
    public class PODTrackingMap
    {
        public PODTrackingMapResponse Response { get; set; }

        public PODTrackingMapRequest Request { get; set; }

        public string Type { get; set; }
    }
}