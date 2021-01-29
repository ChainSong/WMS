using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class Customer
    {
        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("UserID", "UserID")]
        public long UserID { get; set; }

        [EntityPropertyExtension("Code", "Code")]
        public string Code { get; set; }

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }

        [EntityPropertyExtension("Description", "Description")]
        public string Description { get; set; }

        [EntityPropertyExtension("StoreType", "StoreType")]
        public int StoreType { get; set; }

        [EntityPropertyExtension("Types", "Types")]
        public int Types { get; set; }

        [EntityPropertyExtension("StoreStatus", "StoreStatus")]
        public int StoreStatus { get; set; }

        [EntityPropertyExtension("CreditLine", "CreditLine")]
        public string CreditLine { get; set; }

       [EntityPropertyExtension("ProvinceCity", "ProvinceCity")]
        public string ProvinceCity { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

        [EntityPropertyExtension("CreateDate", "CreateDate")]
        public DateTime CreateDate { get; set; }

        [EntityPropertyExtension("Email", "Email")]
        public string Email { get; set; }

        [EntityPropertyExtension("LawPerson", "LawPerson")]
        public string LawPerson { get; set; }

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

        [EntityPropertyExtension("Str20", "Str20")]
        public string Str20 { get; set; }

    }
}