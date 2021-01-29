using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class UpdateAkzoPodAndGetTheDifferenceResponse
    {
        public IEnumerable<UpdateAKZOPod> UpdatedPods { get; set; }

        public IEnumerable<UpdateAKZOPod> NotUpdatedPods { get; set; }

        public IEnumerable<UpdateAKZOPod> CityNotMatchPods { get; set; }
    }
}