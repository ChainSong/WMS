using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class Invoice
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }

        [EntityPropertyExtension("InvoiceNumber", "InvoiceNumber")]
        public string InvoiceNumber { get; set; }

        [EntityPropertyExtension("InvoiceType", "InvoiceType")]
        public long? InvoiceType { get; set; }

        [EntityPropertyExtension("InvoiceTypeName", "InvoiceTypeName")]
        public string InvoiceTypeName { get; set; }

        [EntityPropertyExtension("Target", "Target")]
        public int Target { get; set; }

        [EntityPropertyExtension("Sum", "Sum")]
        public decimal Sum { get; set; }

        [EntityPropertyExtension("Remain", "Remain")]
        public decimal Remain { get; set; }

        [EntityPropertyExtension("EstimateDate", "EstimateDate")]
        public DateTime? EstimateDate { get; set; }

        [EntityPropertyExtension("CustomerOrShipperID", "CustomerOrShipperID")]
        public long? CustomerOrShipperID { get; set; }

        [EntityPropertyExtension("CustomerOrShipperName", "CustomerOrShipperName")]
        public string CustomerOrShipperName { get; set; }

        [EntityPropertyExtension("TaxID", "TaxID")]
        public string TaxID { get; set; }

        [EntityPropertyExtension("Address", "Address")]
        public string Address { get; set; }

        [EntityPropertyExtension("Tel", "Tel")]
        public string Tel { get; set; }

        [EntityPropertyExtension("Bank", "Bank")]
        public string Bank { get; set; }

        [EntityPropertyExtension("BankAccout", "BankAccout")]
        public string BankAccount { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("IsComplete", "IsComplete")]
        public bool IsComplete { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

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

        [EntityPropertyExtension("Decimal1", "Decimal1")]
        public decimal? Decimal1 { get; set; }

        [EntityPropertyExtension("Decimal2", "Decimal2")]
        public decimal? Decimal2 { get; set; }

        [EntityPropertyExtension("Decimal3", "Decimal3")]
        public decimal? Decimal3 { get; set; }

        [EntityPropertyExtension("RelatedCustomerID", "RelatedCustomerID")]
        public long? RelatedCustomerID { get; set; }
    }
}
