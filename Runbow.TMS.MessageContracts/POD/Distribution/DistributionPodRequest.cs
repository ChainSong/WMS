using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class DistributionPodRequest
    {
        public PodDistribution SearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long ProjectID { get; set; }
    }
}