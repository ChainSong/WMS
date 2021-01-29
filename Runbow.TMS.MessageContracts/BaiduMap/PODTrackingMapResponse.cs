using System.Collections.Generic;
using Runbow.TWS.Entity.BaiduMap;

namespace Runbow.TWS.MessageContracts.BaiduMap
{
    public class PODTrackingMapResponse
    {
        public IEnumerable<PODTrackingMap> Response { get; set; }

        public IEnumerable<PODTrackingMap> ResponseHub { get; set; }

        public IEnumerable<PodStatusLogMap> PODTrackingMap { get; set; }
    }
}
