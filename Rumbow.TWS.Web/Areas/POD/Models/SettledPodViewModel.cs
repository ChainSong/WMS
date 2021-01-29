using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class SettledPodViewModel
    {
        public IEnumerable<GroupedPods> GroupedPods { get; set; }

        public IEnumerable<long> PodIDs { get; set; }

        public int SettledType { get; set; }
    }
}