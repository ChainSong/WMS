using System;

namespace Runbow.TWS.Entity
{
    public class ReportTableSearchCondition : ReprotTableIn
    {
        public DateTime? StartReportDate { get; set; }
        public DateTime? EndReportDate { get; set; }
        public string CustomerIDs { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public DateTime? StartCompleteDate { get; set; }
        public DateTime? EndCompleteDate { get; set; }
        public DateTime? StartReceiptDate { get; set; }
        public DateTime? EndReceiptDate { get; set; }
        public DateTime? StartDateTime1 { get; set; }
        public DateTime? EndDateTime1 { get; set; }
        public DateTime? StartDateTime2 { get; set; }
        public DateTime? EndDateTime2 { get; set; }
        public DateTime? StartDateTime3 { get; set; }
        public DateTime? EndDateTime3 { get; set; }
        public DateTime? StartDateTime4 { get; set; }
        public DateTime? EndDateTime4 { get; set; }
        public DateTime? StartDateTime5 { get; set; }
        public DateTime? EndDateTime5 { get; set; }
    }
}
