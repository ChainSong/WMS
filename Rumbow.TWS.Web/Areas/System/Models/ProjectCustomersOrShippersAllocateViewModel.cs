using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class ProjectCustomersOrShippersAllocateViewModel
    {
        public int Target { get; set; }

        public long ProjectID { get; set; }

        public IEnumerable<ProjectCustomersOrShippers> CustomerOrShippersCollection { get; set; }

        public string SelectedConfig { get; set; }
    }
}