using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.Inventory
{
   public class SampleImport
    {
       public string SKU { get; set; }
       public string FactoryCode { get; set; }
       public string ModelName { get; set; }
       public string FOB { get; set; }
       public string RetailPrice { get; set; }
       public string KeepInTighet { get; set; }
    }
}
