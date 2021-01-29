using System.Collections.Generic;
using System.Web.Mvc;
using ProjectRoleEntity = Runbow.TWS.Entity.ProjectRole;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class ProjectUserAllocateViewModel
    {
        public long UserID { get; set; }

        public long ProjectRoleID { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

        public IEnumerable<ProjectRoleEntity> ProjectRoles { get; set; }
    }
}