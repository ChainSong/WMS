using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class QueryPodResponse
    {
        public IEnumerable<PodWithAttachment> PodCollections { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}