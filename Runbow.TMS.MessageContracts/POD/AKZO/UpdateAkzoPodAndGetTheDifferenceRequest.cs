using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class UpdateAkzoPodAndGetTheDifferenceRequest
    {
        public IEnumerable<UpdateAKZOPod> Pods { get; set; }
    }
}