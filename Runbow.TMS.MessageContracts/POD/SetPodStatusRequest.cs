using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class SetPodStatusRequest
    {
        public IEnumerable<long> IDs { get; set; }

        public long PodStatusID { get; set; }

        public string PodStatusName { get; set; }

        public string IsSendMessage { get; set; }
    }
}