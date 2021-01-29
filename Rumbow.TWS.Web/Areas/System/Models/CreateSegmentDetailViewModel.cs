using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class CreateSegmentDetailViewModel
    {
        public Segment Segment { get; set; }

        public IEnumerable<SegmentDetail> SegmentDetailCollection { get; set; }
    }
}