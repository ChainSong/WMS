using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Runbow.TWS.MessageContracts.POD.Nike
{
    public  class GetNikeReportExportRequest
    {
        public DataTable NikeReport { get; set; }
        public string  SqlWhere { get; set; }
        public string ReportName { get; set; }


        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }
    }
}
