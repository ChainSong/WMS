using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;


namespace Runbow.TWS.Entity
{
    public class ReceiveOrPayOrders
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ReceiveOrPayNumber", "ReceiveOrPayNumber")]
        public string ReceiveOrPayNumber { get; set; }

        [EntityPropertyExtension("InvoiceID", "InvoiceID")]
        public long InvoiceID { get; set; }

        [EntityPropertyExtension("InvoiceNumber", "InvoiceNumber")]
        public string InvoiceNumber { get; set; }

        [EntityPropertyExtension("Target", "Target")]
        public int Target { get; set; }

        [EntityPropertyExtension("CustomerOrShipperID", "CustomerOrShipperID")]
        public long? CustomerOrShipperID { get; set; }

        [EntityPropertyExtension("CustomerOrShipperName", "CustomerOrShipperName")]
        public string CustomerOrShipperName { get; set; }

        [EntityPropertyExtension("AMT", "AMT")]
        public decimal AMT { get; set; }

        [EntityPropertyExtension("Date", "Date")]
        public DateTime Date { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

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

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }

        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }

        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime? DateTime3 { get; set; }

        [EntityPropertyExtension("Decimal1", "Decimal1")]
        public decimal? Decimal1 { get; set; }

        [EntityPropertyExtension("Decimal2", "Decimal2")]
        public decimal? Decimal2 { get; set; }

        [EntityPropertyExtension("RelatedCustomerID", "RelatedCustomerID")]
        public long? RelatedCustomerID { get; set; }
    }
}
