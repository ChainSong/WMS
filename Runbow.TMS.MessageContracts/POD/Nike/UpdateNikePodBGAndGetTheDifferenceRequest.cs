using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class UpdateNikePodBGAndGetTheDifferenceRequest
    {
        public IEnumerable<UpdateNikePodBG> Pods { get; set; }
    }
}