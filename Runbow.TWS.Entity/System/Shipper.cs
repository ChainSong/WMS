using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class Shipper
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("Code", "Code")]
        public string Code { get; set; }

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }

        [EntityPropertyExtension("EnglishName", "EnglishName")]
        public string EnglishName { get; set; }

        [EntityPropertyExtension("IsDangerous", "IsDangerous")]
        public bool? IsDangerous { get; set; }

        [EntityPropertyExtension("IsCustoms", "IsCustoms")]
        public bool? IsCustoms { get; set; }

        [EntityPropertyExtension("Email", "Email")]
        public string Email { get; set; }

        [EntityPropertyExtension("LawPerson", "LawPerson")]
        public string LawPerson { get; set; }

        [EntityPropertyExtension("IsSupplier", "IsSupplier")]
        public bool? IsSupplier { get; set; }

        [EntityPropertyExtension("IsBalance", "IsBalance")]
        public bool? IsBalance { get; set; }

        [EntityPropertyExtension("PostCode", "PostCode")]
        public string PostCode { get; set; }

        [EntityPropertyExtension("Address1", "Address1")]
        public string Address1 { get; set; }

        [EntityPropertyExtension("Address2", "Address2")]
        public string Address2 { get; set; }

        [EntityPropertyExtension("Bank", "Bank")]
        public string Bank { get; set; }

        [EntityPropertyExtension("Account", "Account")]
        public string Account { get; set; }

        [EntityPropertyExtension("TaxID", "TaxID")]
        public string TaxID { get; set; }

        [EntityPropertyExtension("InvoiceTitle", "InvoiceTitle")]
        public string InvoiceTitle { get; set; }

        [EntityPropertyExtension("Contactor1", "Contactor1")]
        public string Contactor1 { get; set; }

        [EntityPropertyExtension("Title1", "Title1")]
        public string Title1 { get; set; }

        [EntityPropertyExtension("Phone1", "Phone1")]
        public string Phone1 { get; set; }

        [EntityPropertyExtension("Fax1", "Fax1")]
        public string Fax1 { get; set; }

        [EntityPropertyExtension("Contactor2", "Contactor2")]
        public string Contactor2 { get; set; }

        [EntityPropertyExtension("Title2", "Title2")]
        public string Title2 { get; set; }

        [EntityPropertyExtension("Phone2", "Phone2")]
        public string Phone2 { get; set; }

        [EntityPropertyExtension("Fax2", "Fax2")]
        public string Fax2 { get; set; }

        [EntityPropertyExtension("WebSite", "WebSite")]
        public string WebSite { get; set; }

        [EntityPropertyExtension("RegistAdd", "RegistAdd")]
        public string RegistAdd { get; set; }

        [EntityPropertyExtension("Comment", "Comment")]
        public string Comment { get; set; }

        [EntityPropertyExtension("Creater", "Creater")]
        public string Creater { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("Updater", "Updater")]
        public string Updater { get; set; }

        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        [EntityPropertyExtension("InsuranceCompany", "InsuranceCompany")]
        public string InsuranceCompany { get; set; }

        [EntityPropertyExtension("InsuranceType", "InsuranceType")]
        public string InsuranceType { get; set; }

        [EntityPropertyExtension("InsuranceOrderNo", "InsuranceOrderNo")]
        public string InsuranceOrderNo { get; set; }

        [EntityPropertyExtension("InsuranceCost", "InsuranceCost")]
        public decimal? InsuranceCost { get; set; }

        [EntityPropertyExtension("InsuranceStartTime", "InsuranceStartTime")]
        public DateTime? InsuranceStartTime { get; set; }

        [EntityPropertyExtension("InsuranceEndTime", "InsuranceEndTime")]
        public DateTime? InsuranceEndTime { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("SegmentID", "SegmentID")]
        public long? SegmentID { get; set; }

        [EntityPropertyExtension("SegmentName", "SegmentName")]
        public string SegmentName { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
    }
}