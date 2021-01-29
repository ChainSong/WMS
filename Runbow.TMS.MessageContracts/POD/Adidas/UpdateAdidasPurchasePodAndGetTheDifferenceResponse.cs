using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.POD;

namespace Runbow.TWS.MessageContracts.POD.Adidas
{
    public class UpdateAdidasPurchasePodAndGetTheDifferenceResponse
    {
        public IEnumerable<UpdateAdidasPurchasePod> UpdatedPodAD { get; set; }

        public IEnumerable<UpdateAdidasPurchasePod> NotUpdatedPodAD { get; set; }

        public IEnumerable<UpdateAdidasPurchasePod> CityNotMatchPodAD { get; set; }
    }
}
