using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.MobilePOD;

namespace Runbow.TWS.MessageContracts.MobilePOD
{
    public class QueryOrderInformationRequests
    {
       // public OrderManagementInfo SearchCondition { get; set; }
        public int UserType { get; set; }

        public string ShipperId { get; set; }

        public string conditions { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string permissions { get; set; }

        //public IEnumerable<OrderManagementInfo> orderManagement { get; set; }
    }
}
