using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.NikeNFSPrint;

namespace Runbow.TWS.MessageContracts.NikeNFSPrint
{
   public class NFSBoxListManagementResponse
    {
       public NFSPrintBoxInfo BoxListinfo { get; set; }

       public IEnumerable<NFSPrintBoxInfo> EnumerableBoxListinfo { get; set; }
    }
}
