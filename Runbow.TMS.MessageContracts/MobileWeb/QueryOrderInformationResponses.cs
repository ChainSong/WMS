using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.MobilePOD;

namespace Runbow.TWS.MessageContracts.MobilePOD
{
    public class QueryOrderInformationResponses
    {
        public string conditions { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public string permissions { get; set; }

        public IEnumerable<OrderManagementInfo> orderManagement { get; set; }

 
    }
}
