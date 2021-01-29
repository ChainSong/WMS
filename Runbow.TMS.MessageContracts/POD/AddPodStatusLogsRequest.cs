using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPodStatusLogsRequest
    {
        public IEnumerable<PodStatusLog> PodStatusLogs { get; set; }

        public long CustomerID { get; set; }
    }
}