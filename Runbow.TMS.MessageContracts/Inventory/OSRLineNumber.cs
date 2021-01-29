using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.Inventory
{
   public class OSRLineNumber
    {
        //EPL交接PackListNumber(唛箱号)
       public string PackListNumber { get; set; }
       public string POLineNumber { get; set; }
    }
}
