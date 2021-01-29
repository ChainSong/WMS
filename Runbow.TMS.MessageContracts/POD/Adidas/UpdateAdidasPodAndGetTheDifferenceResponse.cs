using Runbow.TWS.Entity.POD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.POD.Adidas
{
    public class UpdateAdidasPodAndGetTheDifferenceResponse
    {
        public IEnumerable<UpdateAdidasPod> UpdatedPodAD { get; set; }

        public IEnumerable<UpdateAdidasPod> NotUpdatedPodAD { get; set; }

        public IEnumerable<UpdateAdidasPod> CityNotMatchPodAD { get; set; }
    }
}
