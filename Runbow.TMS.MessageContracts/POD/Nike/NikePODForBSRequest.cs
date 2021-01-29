using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.POD.Nike;

namespace Runbow.TWS.MessageContracts.POD.Nike
{
    public class NikePODForBSRequest
    {
        public NikePodForBSCondition Condition { get; set; }

        public IEnumerable<NikeforBSPOD> PodCollection { get; set; }

        public string UserName { get; set; }

        public string ShipperName { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

    }
}
