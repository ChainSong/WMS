using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class QueryPodRequest
    {
        public PodSearchCondition SearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long ProjectID { get; set; }

        public string PhotoUrl { get; set; }

        public string GroupID { get; set; }
    }
}