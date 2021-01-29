using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetSegmentAndDetailResponse
    {
        public Segment Segment { get; set; }

        public IEnumerable<SegmentDetail> SegmentDetailCollection { get; set; }
    }
}