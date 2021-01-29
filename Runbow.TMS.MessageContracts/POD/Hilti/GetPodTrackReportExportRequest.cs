using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.POD.Hilti
{
   public class GetPodTrackReportExportRequest
    {
       public DataTable XLDTrackReport { get; set; }
       public string SqlWhere { get; set; }
       public int PageIndex { get; set; }
       public int PageSize { get; set; }
       public int PageCount { get; set; }
       public double SumPoll { get; set; }
       public double SumGrossWeight { get; set; }
       public double SumNetWeight { get; set; }
       public string ReportName { get; set; }
    }
}
