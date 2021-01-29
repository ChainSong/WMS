using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetUsersByConditionResponse
    {
        public IEnumerable<User> Users { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}