using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Receipt;
using Runbow.TWS.Entity.WMS.Report;
namespace Runbow.TWS.MessageContracts.WMS.Report
{
    public class GetReportExpressBySearchConditionResponse
    {
        public IEnumerable<ReportExpressInfo> ReprotExpressInfoCollection { get; set; }
        public long? newID { get; set; }
        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
