using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodAllViewModel
    {
        public PodAll PodAll { get; set; }

        public Module ModuleConfig { get; set; }

        public bool? ShowEditRelated { get; set; }

        public long ProjectRole { get; set; }

        public int ReturnStep { get; set; }
    }
}