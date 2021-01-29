using System;

namespace Runbow.TWS.Entity
{
    public class ReportSkuSearchCondition : ReportSku
    {
        public long? CustomerID { get; set; }

        public string Warehouse { get; set; }

        public string SkuReportType { get; set; }
        public DateTime? StartExpectDate { get; set; }
        public DateTime? EndExpectDate { get; set; }
        public string DaTuoType { get; set; }
        
    }
}
