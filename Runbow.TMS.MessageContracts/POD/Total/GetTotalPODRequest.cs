using Runbow.TWS.Entity.POD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.POD.Total
{
    public class GetTotalPODRequest
    {
        public TotalPODEntity SearchCondition { get; set; } 

        public DataTable PODChainTable { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int? StateID { get; set; }
    }
}
