using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.POD
{
    public class SuperPodAll
    {
        public IEnumerable<PodWithAttachment> PodCollections { get; set; }

        public IEnumerable<PodTrack> PodTracks { get; set; }
    }
}
