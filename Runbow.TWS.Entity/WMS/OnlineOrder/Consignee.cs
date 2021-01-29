using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.OnlineOrder
{
    public class Consignee
    {
        [EntityPropertyExtension("UserUID", "UserUID")]
        public string UserUID { get; set; }
        [EntityPropertyExtension("UserAccount", "UserAccount")]
        public string UserAccount { get; set; }
        [EntityPropertyExtension("UserPWD", "UserPWD")]
        public string UserPWD { get; set; }
        [EntityPropertyExtension("UserPhone", "UserPhone")]
        public string UserPhone { get; set; }
        [EntityPropertyExtension("UserAddress", "UserAddress")]
        public string UserAddress { get; set; }
        [EntityPropertyExtension("UserName", "UserName")]
        public string UserName { get; set; }
        [EntityPropertyExtension("UserEmail", "UserEmail")]
        public string UserEmail { get; set; }
        [EntityPropertyExtension("UserType", "UserType")]
        public int UserType { get; set; }
        [EntityPropertyExtension("UserFettle", "UserFettle")]
        public int UserFettle { get; set; }
        [EntityPropertyExtension("UserCode", "UserCode")]
        public string UserCode { get; set; }
        [EntityPropertyExtension("UserModification", "UserModification")]
        public int UserModification { get; set; }
        [EntityPropertyExtension("UserNotes", "UserNotes")]
        public string UserNotes { get; set; }
        [EntityPropertyExtension("SessionID", "SessionID")]
        public string SessionID { get; set; }
        [EntityPropertyExtension("LoginCount", "LoginCount")]
        public int LoginCount { get; set; }
        [EntityPropertyExtension("LastLoginTime", "LastLoginTime")]
        public string LastLoginTime { get; set; }
        [EntityPropertyExtension("LastLoginIP", "LastLoginIP")]
        public string LastLoginIP { get; set; }
        [EntityPropertyExtension("Customercode", "Customercode")]
        public string Customercode { get; set; }
     
    }
}
