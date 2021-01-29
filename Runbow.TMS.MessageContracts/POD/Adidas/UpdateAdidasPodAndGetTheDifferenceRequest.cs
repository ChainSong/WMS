using Runbow.TWS.Entity.POD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.POD.Adidas
{
    public class UpdateAdidasPodAndGetTheDifferenceRequest
    {
        public IEnumerable<UpdateAdidasPod> PodAD { get; set; }
    }
}
