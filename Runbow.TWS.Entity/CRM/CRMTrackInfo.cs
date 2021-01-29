using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class CRMTrackInfo
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("CRMID", "CRMID")]
        public long CRMID { get; set; }

        [EntityPropertyExtension("VisitPeople", "VisitPeople")]
        public string VisitPeople { get; set; }

        [EntityPropertyExtension("VisitTime", "VisitTime")]
        public string VisitTime { get; set; }

        [EntityPropertyExtension("VisitPlace", "VisitPlace")]
        public string VisitPlace { get; set; }

        [EntityPropertyExtension("VisitForm", "VisitForm")]
        public string VisitForm { get; set; }

        [EntityPropertyExtension("GiftsArticles", "GiftsArticles")]
        public string GiftsArticles { get; set; }

        [EntityPropertyExtension("VisitToCustomerEvaluation", "VisitToCustomerEvaluation")]
        public string VisitToCustomerEvaluation { get; set; }

        [EntityPropertyExtension("VisitingPersonnelFeedbackVisit", "VisitingPersonnelFeedbackVisit")]
        public string VisitingPersonnelFeedbackVisit { get; set; }

        [EntityPropertyExtension("ProjectCustomerCommunication", "ProjectCustomerCommunication")]
        public string ProjectCustomerCommunication { get; set; }

        [EntityPropertyExtension("CustomerSupportAndAssistance", "CustomerSupportAndAssistance")]
        public string CustomerSupportAndAssistance { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public string CreateTime { get; set; }

        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public string UpdateTime { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }

        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }

        [EntityPropertyExtension("Str6", "Str6")]
        public string Str6 { get; set; }

        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7 { get; set; }

        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8 { get; set; }
    }
}
