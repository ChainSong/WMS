using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class UpdateAkzoModelsPodAndGetTheDifferenceResponse
    {
        public IEnumerable<UpdateAKZOModelsPod> UpdatedPods { get; set; }

        public IEnumerable<UpdateAKZOModelsPod> NotUpdatedPods { get; set; }

        public IEnumerable<UpdateAKZOModelsPod> CityNotMatchPods { get; set; }
    }
}