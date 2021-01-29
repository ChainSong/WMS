using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    [Serializable]
    public class User
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }

        [EntityPropertyExtension("DisplayName", "DisplayName")]
        public string DisplayName { get; set; }

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }

        [EntityPropertyExtension("Password", "Password")]
        public string Password { get; set; }

        [EntityPropertyExtension("ProjectName", "ProjectName")]
        public string ProjectName { get; set; }

        [EntityPropertyExtension("ProjectRoleID", "ProjectRoleID")]
        public long ProjectRoleID { get; set; }

        [EntityPropertyExtension("Sex", "Sex")]
        public char Sex { get; set; }

        [EntityPropertyExtension("Tel", "Tel")]
        public string Tel { get; set; }

        [EntityPropertyExtension("Mobile", "Mobile")]
        public string Mobile { get; set; }

        [EntityPropertyExtension("Email", "Email")]
        public string Email { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

        [EntityPropertyExtension("CreateDate", "CreateDate")]
        public DateTime CreateDate { get; set; }

        [EntityPropertyExtension("UserType", "UserType")]
        public int UserType { get; set; }

        [EntityPropertyExtension("CustomerOrShipperID", "CustomerOrShipperID")]
        public long CustomerOrShipperID { get; set; }

        [EntityPropertyExtension("CustomerOrShipperName", "CustomerOrShipperName")]
        public string CustomerOrShipperName { get; set; }

        [EntityPropertyExtension("RuleArea", "RuleArea")]
        public string RuleArea { get; set; }

        [EntityPropertyExtension("RuleAreaName", "RuleAreaName")]
        public string RuleAreaName { get; set; }

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

        [EntityPropertyExtension("Description", "Description")]
        public string Description { get; set; }

        
    }
}