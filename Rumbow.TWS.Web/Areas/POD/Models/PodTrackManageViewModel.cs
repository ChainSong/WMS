using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodTrackManageViewModel : PodReleatedInforBaseViewModel
    {
        public IEnumerable<PodTrack> PodTracks { get; set; }

        public PodTrack PodTrack { get; set; }
    }
}