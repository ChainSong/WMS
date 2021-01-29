using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.System
{
    public class ProjectRoleMenu
    {
        [EntityPropertyExtension("ProjectRoleId", "ProjectRoleId")]
        public long ProjectRoleId { get; set; }

        [EntityPropertyExtension("MenuID", "MenuID")]
        public long MenuID { get; set; }

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }
    }
}
