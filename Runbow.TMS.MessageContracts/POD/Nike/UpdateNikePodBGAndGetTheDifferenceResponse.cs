using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class UpdateNikePodBGAndGetTheDifferenceResponse
    {
        public IEnumerable<UpdateNikePodBG> UpdatedPods { get; set; }

        public IEnumerable<UpdateNikePodBG> NotUpdatedPods { get; set; }

        public IEnumerable<UpdateNikePodBG> StateNotMatchPods { get; set; }

        public IEnumerable<UpdateNikePodBG> RepeatPods { get; set; }


    }
}