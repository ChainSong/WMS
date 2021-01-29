using Runbow.TWS.Entity.WMS.NikeOSRBJPrint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.NikeOSRBJPrint
{
    public class ConsignmentManagementResponse
    {
        public PrintPodInfo BoxListinfo { get; set; }

        public IEnumerable<PrintPodInfo> EnumerableBoxListinfo { get; set; }

        public IEnumerable<PrintPodInfo> PrintPodDetails { get; set; }
    }
}
