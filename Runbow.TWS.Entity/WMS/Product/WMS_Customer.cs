using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Product
{
    public class WMS_Customer
    {
        [EntityPropertyExtension("StorerKey", "StorerKey")]
        public string StorerKey
        {
            get;
            set;
        }
        [EntityPropertyExtension("Active", "Active")]
        public bool Active
        {
            get;
            set;
        }
        [EntityPropertyExtension("Status", "Status")]
        public string Status
        {
            get;
            set;
        }
        [EntityPropertyExtension("Company", "Company")]
        public string Company
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReceiptPrefix", "ReceiptPrefix")]
        public string ReceiptPrefix
        {
            get;
            set;
        }
        [EntityPropertyExtension("OrderPrefix", "OrderPrefix")]
        public string OrderPrefix
        {
            get;
            set;
        }
        [EntityPropertyExtension("CompanyCode", "CompanyCode")]
        public string CompanyCode
        {
            get;
            set;
        }
        [EntityPropertyExtension("Type", "Type")]
        public string Type
        {
            get;
            set;
        }
        [EntityPropertyExtension("AddressLine1", "AddressLine1")]
        public string AddressLine1
        {
            get;
            set;
        }
        [EntityPropertyExtension("AddressLine2", "AddressLine2")]
        public string AddressLine2
        {
            get;
            set;
        }
        [EntityPropertyExtension("AddressLine3", "AddressLine3")]
        public string AddressLine3
        {
            get;
            set;
        }
        [EntityPropertyExtension("AddressLine4", "AddressLine4")]
        public string AddressLine4
        {
            get;
            set;
        }
        [EntityPropertyExtension("City", "City")]
        public string City
        {
            get;
            set;
        }
        [EntityPropertyExtension("State", "State")]
        public string State
        {
            get;
            set;
        }
        [EntityPropertyExtension("PostCode", "PostCode")]
        public string PostCode
        {
            get;
            set;
        }
        [EntityPropertyExtension("Country", "Country")]
        public string Country
        {
            get;
            set;
        }
        [EntityPropertyExtension("CountryCode", "CountryCode")]
        public string CountryCode
        {
            get;
            set;
        }
        [EntityPropertyExtension("Contact1", "Contact1")]
        public string Contact1
        {
            get;
            set;
        }
        [EntityPropertyExtension("Contact2", "Contact2")]
        public string Contact2
        {
            get;
            set;
        }
        [EntityPropertyExtension("PhoneNum1", "PhoneNum1")]
        public string PhoneNum1
        {
            get;
            set;
        }
        [EntityPropertyExtension("PhoneNum2", "PhoneNum2")]
        public string PhoneNum2
        {
            get;
            set;
        }
        [EntityPropertyExtension("FaxNum1", "FaxNum1")]
        public string FaxNum1
        {
            get;
            set;
        }
        [EntityPropertyExtension("FaxNum2", "FaxNum2")]
        public string FaxNum2
        {
            get;
            set;
        }
        [EntityPropertyExtension("Email1", "Email1")]
        public string Email1
        {
            get;
            set;
        }
        [EntityPropertyExtension("Email2", "Email2")]
        public string Email2
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToCompany", "BillToCompany")]
        public string BillToCompany
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToContact1", "BillToContact1")]
        public string BillToContact1
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToContact2", "BillToContact2")]
        public string BillToContact2
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToAddressLine1", "BillToAddressLine1")]
        public string BillToAddressLine1
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToAddressLine2", "BillToAddressLine2")]
        public string BillToAddressLine2
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToAddressLine3", "BillToAddressLine3")]
        public string BillToAddressLine3
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToAddressLine4", "BillToAddressLine4")]
        public string BillToAddressLine4
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToCity", "BillToCity")]
        public string BillToCity
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToRegion", "BillToRegion")]
        public string BillToRegion
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToPostCode", "BillToPostCode")]
        public string BillToPostCode
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToCountry", "BillToCountry")]
        public string BillToCountry
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToISOCntryCode", "BillToISOCntryCode")]
        public string BillToISOCntryCode
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToPhoneNum1", "BillToPhoneNum1")]
        public string BillToPhoneNum1
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToPhoneNum2", "BillToPhoneNum2")]
        public string BillToPhoneNum2
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToFaxNum1", "BillToFaxNum1")]
        public string BillToFaxNum1
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToFaxNum2", "BillToFaxNum2")]
        public string BillToFaxNum2
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToEmail1", "BillToEmail1")]
        public string BillToEmail1
        {
            get;
            set;
        }
        [EntityPropertyExtension("BillToEmail2", "BillToEmail2")]
        public string BillToEmail2
        {
            get;
            set;
        }
        [EntityPropertyExtension("Memo", "Memo")]
        public string Memo
        {
            get;
            set;
        }
        [EntityPropertyExtension("CreditLimit", "CreditLimit")]
        public string CreditLimit
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToCompany", "ShipToCompany")]
        public string ShipToCompany
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToContact1", "ShipToContact1")]
        public string ShipToContact1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToContact2", "ShipToContact2")]
        public string ShipToContact2
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToAddressLine1", "ShipToAddressLine1")]
        public string ShipToAddressLine1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToAddressLine2", "ShipToAddressLine2")]
        public string ShipToAddressLine2
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToAddressLine3", "ShipToAddressLine3")]
        public string ShipToAddressLine3
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToAddressLine4", "ShipToAddressLine4")]
        public string ShipToAddressLine4
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToCity", "ShipToCity")]
        public string ShipToCity
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToRegion", "ShipToRegion")]
        public string ShipToRegion
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToPostCode", "ShipToPostCode")]
        public string ShipToPostCode
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToCountry", "ShipToCountry")]
        public string ShipToCountry
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToPhone1", "ShipToPhone1")]
        public string ShipToPhone1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToPhone2", "ShipToPhone2")]
        public string ShipToPhone2
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToFax1", "ShipToFax1")]
        public string ShipToFax1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToFax2", "ShipToFax2")]
        public string ShipToFax2
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToEmail1", "ShipToEmail1")]
        public string ShipToEmail1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ShipToEmail2", "ShipToEmail2")]
        public string ShipToEmail2
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToCompany", "ReturnToCompany")]
        public string ReturnToCompany
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnAttentionTo", "ReturnAttentionTo")]
        public string ReturnAttentionTo
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToContact1", "ReturnToContact1")]
        public string ReturnToContact1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToContact2", "ReturnToContact2")]
        public string ReturnToContact2
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToAddressLine1", "ReturnToAddressLine1")]
        public string ReturnToAddressLine1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToAddressLine2", "ReturnToAddressLine2")]
        public string ReturnToAddressLine2
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToAddressLine3", "ReturnToAddressLine3")]
        public string ReturnToAddressLine3
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToAddressLine4", "ReturnToAddressLine4")]
        public string ReturnToAddressLine4
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToCity", "ReturnToCity")]
        public string ReturnToCity
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToRegion", "ReturnToRegion")]
        public string ReturnToRegion
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToPostCode", "ReturnToPostCode")]
        public string ReturnToPostCode
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToCountry", "ReturnToCountry")]
        public string ReturnToCountry
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToPhone1", "ReturnToPhone1")]
        public string ReturnToPhone1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToPhone2", "ReturnToPhone2")]
        public string ReturnToPhone2
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToFax1", "ReturnToFax1")]
        public string ReturnToFax1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToFax2", "ReturnToFax2")]
        public string ReturnToFax2
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToEmail1", "ReturnToEmail1")]
        public string ReturnToEmail1
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReturnToEmail2", "ReturnToEmail2")]
        public string ReturnToEmail2
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef1", "UserDef1")]
        public string UserDef1
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef2", "UserDef2")]
        public string UserDef2
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef3", "UserDef3")]
        public string UserDef3
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef4", "UserDef4")]
        public string UserDef4
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef5", "UserDef5")]
        public string UserDef5
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef6", "UserDef6")]
        public string UserDef6
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef7", "UserDef7")]
        public string UserDef7
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef8", "UserDef8")]
        public string UserDef8
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef9", "UserDef9")]
        public string UserDef9
        {
            get;
            set;
        }
        [EntityPropertyExtension("UserDef10", "UserDef10")]
        public string UserDef10
        {
            get;
            set;
        }
        [EntityPropertyExtension("TriggerDog", "TriggerDog")]
        public bool TriggerDog
        {
            get;
            set;
        }
        [EntityPropertyExtension("CreateUser", "CreateUser")]
        public string CreateUser
        {
            get;
            set;
        }
        [EntityPropertyExtension("CreateDate", "CreateDate")]
        public DateTime CreateDate
        {
            get;
            set;
        }
        [EntityPropertyExtension("UpdateUser", "UpdateUser")]
        public string UpdateUser
        {
            get;
            set;
        }
        [EntityPropertyExtension("UpdateDate", "UpdateDate")]
        public DateTime UpdateDate
        {
            get;
            set;
        }
        [EntityPropertyExtension("stamp", "stamp")]
        public string stamp
        {
            get;
            set;
        }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID
        {
            get;
            set;
        }

        [EntityPropertyExtension("ReportSendStatus", "ReportSendStatus")]
        public int? ReportSendStatus
        {
            get;
            set;
        }
        [EntityPropertyExtension("EpackListSendCount", "EpackListSendCount")]
        public int? EpackListSendCount
        {
            get;
            set;
        }
        [EntityPropertyExtension("ReportSendTime", "ReportSendTime")]
        public DateTime? ReportSendTime
        {
            get;
            set;
        }
        [EntityPropertyExtension("EpackListLastSendTime", "EpackListLastSendTime")]
        public DateTime? EpackListLastSendTime
        {
            get;
            set;
        }
        [EntityPropertyExtension("MailToConfig", "MailToConfig")]
        public string MailToConfig
        {
            get;
            set;
        }
        [EntityPropertyExtension("CCEmailConfig", "CCEmailConfig")]
        public string CCEmailConfig
        {
            get;
            set;
        }

    }
}
