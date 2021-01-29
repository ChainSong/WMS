using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ProjectRole : Role
    {
        [EntityPropertyExtension("ProjectRoleID", "ProjectRoleID")]
        public long ProjectRoleID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        public bool Checked { get; set; }
    }
}