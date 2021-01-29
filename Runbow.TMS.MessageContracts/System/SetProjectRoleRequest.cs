using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class SetProjectRoleRequest
    {
        public long ProjectID { get; set; }

        public IEnumerable<long> RoleIDs { get; set; }
    }
}