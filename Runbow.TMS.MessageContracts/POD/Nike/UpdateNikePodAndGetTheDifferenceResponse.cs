using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class UpdateNikePodAndGetTheDifferenceResponse
    {
        public IEnumerable<UpdateNikePod> UpdatedPods { get; set; }

        public IEnumerable<UpdateNikePod> NotUpdatedPods { get; set; }

        public IEnumerable<UpdateNikePod> CityNotMatchPods { get; set; }

        public IEnumerable<UpdateNikePod> StartCityNotMatchPods { get; set; }
    }
}