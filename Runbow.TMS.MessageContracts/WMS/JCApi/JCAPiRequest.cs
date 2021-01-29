using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.JCApi
{
    public class JCAPiRequest
    {
        public List<JCRequestLists> jCRequestLists { get; set; }
    }
    public class JCRequestLists
    {
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string WarehouseID { get; set; }

        public string RelateNumber { get; set; } 
    }
}
