using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class SetProjectRoleMenuRequest
    {
        public long ProjectRoleID { get; set; }

        public IEnumerable<int> MenuIDs { get; set; }
    }
}