using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.System
{
    public class UserWarehouseInfo
    {
        [EntityPropertyExtension("Id", "Id")]
        public long Id { get; set; }

        [EntityPropertyExtension("ProjectId", "ProjectId")]
        public long ProjectId { get; set; }

        [EntityPropertyExtension("UserId", "UserId")]
        public long UserId { get; set; }

        [EntityPropertyExtension("WarehouseId", "WarehouseId")]
        public long WarehouseId { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

    }
}
