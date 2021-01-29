using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPodsRequest
    {
        public IEnumerable<Pod> Pods { get; set; }
    }
}