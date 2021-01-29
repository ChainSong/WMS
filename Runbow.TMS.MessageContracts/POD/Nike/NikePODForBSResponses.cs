using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.POD.Nike;

namespace Runbow.TWS.MessageContracts.POD.Nike
{
    public class NikePODForBSResponses
    {
        public IEnumerable<NikeforBSPOD> PodCollection { get; set; }

        public string str { get; set; }

        public int RowCount { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
