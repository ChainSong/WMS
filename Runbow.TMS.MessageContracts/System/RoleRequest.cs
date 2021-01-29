using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
   public class RoleRequest
    {
        public IEnumerable<Role> IEnumerableRole { get; set; }
        public Role Role { get; set; }
        public long ID { get; set; }
        public long ProjectRoleID { get; set; }
        public long ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Satate { get; set; }

        public DateTime CreateDate { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
}
