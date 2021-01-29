using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class PodRegionCount
    {
        [EntityPropertyExtension("RegionID", "RegionID")]
        public long RegionID { get; set; }

        [EntityPropertyExtension("RegionName", "RegionName")]
        public string RegionName { get; set; }

        [EntityPropertyExtension("PodCount", "PodCount")]
        public int PodCount { get; set; }
    }
}