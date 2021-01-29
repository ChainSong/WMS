using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class UpdateAkzoModelsPodAndGetTheDifferenceRequest
    {
        public IEnumerable<UpdateAKZOModelsPod> Pods { get; set; }
    }
}