using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.NikeOSRBJPrint;

namespace Runbow.TWS.MessageContracts.NikeOSRBJPrint
{
   public class BoxListManagementResponse
    {
        public PrintBoxInfo BoxListinfo { get; set; }

        public IEnumerable<PrintBoxInfo> EnumerableBoxListinfo { get; set; }

        public IEnumerable<PrintExpressJite> EnumerableExpressListInfo { get; set; }

    }
}

