using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPodReplyDocumentsRequest
    {
        public IEnumerable<PodReplyDocument> PodReplyDocuments { get; set; }

        public long CustomerID { get; set; }
    }
}