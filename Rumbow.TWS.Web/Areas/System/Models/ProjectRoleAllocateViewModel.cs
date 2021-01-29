using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class ProjectRoleAllocateViewModel
    {
        public IEnumerable<ProjectRole> ProjectRoles { get; set; }

        public string SelectedRoleIDs { get; set; }
    }
}