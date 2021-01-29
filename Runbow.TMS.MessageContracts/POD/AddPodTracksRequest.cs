using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPodTracksRequest
    {
        public IEnumerable<PodTrack> PodTracks { get; set; }

        public long CustomerID { get; set; }
    }
}