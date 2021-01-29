using System.Collections.Generic;
using Runbow.TWS.Entity;


namespace Runbow.TWS.MessageContracts
{
    public class UpdateLog56PhoneStatusRequest
    {
        public IEnumerable<Log56PhoneStatus> Log56PhoneStatus { get; set; }
    }
}
