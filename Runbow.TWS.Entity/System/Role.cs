using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class Role
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectRoleID", "ProjectRoleID")]
        public long ProjectRoleID { get; set; }

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }

        [EntityPropertyExtension("Description", "Description")]
        public string Description { get; set; }

        [EntityPropertyExtension("Satate", "Satate")]
        public bool State { get; set; }

        [EntityPropertyExtension("CreateDate", "CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}