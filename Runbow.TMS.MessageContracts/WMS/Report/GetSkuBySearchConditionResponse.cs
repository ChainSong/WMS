using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Receipt;

namespace Runbow.TWS.MessageContracts.WMS.Report
{
    public class GetSkuBySearchConditionResponse
    {
       public IEnumerable<ReportSku> SkuCollection { get; set; }

       public int PageCount { get; set; }

       public int PageIndex { get; set; }

       
    }
}
