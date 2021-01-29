using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.System
{
    public class GetWMS_CustomerByConditionRequest
    {
        public string StorerKey { get; set; }
        public string Company { get; set; }
        public string PhoneNum { get; set; }
        public string AddressLine1 { get; set; }
        public string Contact1 { get; set; }
        public int PageIndex { get; set; }
        public string CustomerID { get; set; }
        public int PageSize { get; set; }
    }
}
