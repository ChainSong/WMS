using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class UpdateNikePodAndGetTheDifferenceRequest
    {
        public IEnumerable<UpdateNikePod> Pods { get; set; }
    }
}