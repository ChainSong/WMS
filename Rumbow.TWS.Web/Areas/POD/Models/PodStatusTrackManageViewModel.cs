using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodStatusTrackManageViewModel : PodReleatedInforBaseViewModel
    {
        public IEnumerable<PodStatusTrack> PodStatusTracks { get; set; }

        public PodStatusTrack PodStatusTrack { get; set; }
    }
}