using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.YXDRBJPrint;

namespace Runbow.TWS.MessageContracts.YXDBJRPrint
{
    public class YXDRBJPODManagementResponse
    {

        public YXDRBJPrintPodInfo YXDRPodInfo { get; set; }
        public IEnumerable<YXDRBJPrintPodInfo> EnumerableYXDRPodInfo { get; set; }
    }
}
