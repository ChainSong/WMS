using System;
using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class SetProjectUserCustomersRequest
    {
        public long UserID { get; set; }

        public long ProjectID { get; set; }

        public IEnumerable<long> CustomerIDs { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }
    }
}