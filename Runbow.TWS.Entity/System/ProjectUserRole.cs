using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ProjectUserRole
    {
        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("ProjectName", "ProjectName")]
        public string ProjectName { get; set; }

        [EntityPropertyExtension("UserID", "UserID")]
        public long UserID { get; set; }

        [EntityPropertyExtension("UserName", "UserName")]
        public string UserName { get; set; }

        [EntityPropertyExtension("RoleID", "RoleID")]
        public long RoleID { get; set; }

        [EntityPropertyExtension("RoleName", "RoleName")]
        public string RoleName { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

        [EntityPropertyExtension("StoreType", "StoreType")]
        public int StoreType { get; set; }

    }
}