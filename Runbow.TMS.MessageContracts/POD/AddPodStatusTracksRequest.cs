using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPodStatusTracksRequest
    {
        public IEnumerable<PodStatusTrack> PodStatusTracks { get; set; }

        public long CustomerID { get; set; }
    }
}