using System;

namespace Runbow.TWS.Entity
{
    [Serializable]
    public class ASNSearchCondition:ASN
    {
        #region Model
        public int UserType { get; set; }
        public string CustomerIDs { get; set; }
        //public long ID { get; set; }
        //public string ASNNumber { get; set; }
        //public string ExternReceiptNumber { get; set; }
        //public long CustomerID { get; set; }
        //public string CustomerName { get; set; }
        //public long WarehouseId { get; set; }
        //public string WarehouseName { get; set; }
        public DateTime?  StartExpectDate { get; set; }
        public DateTime? EndExpectDate { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        //public int Status { get; set; }
        //public string ASNType { get; set; }
        //public string Creator { get; set; }
        //public DateTime CreateTime { get; set; }
        //public string Updator { get; set; }
        //public DateTime? UpdateTime { get; set; }
        //public DateTime? CompleteDate { get; set; }
        //public string Remark { get; set; }
        //public string str1 { get; set; }
        //public string str2 { get; set; }
        //public string str3 { get; set; }
        //public string str4 { get; set; }
        //public string str5 { get; set; }
        //public string str6 { get; set; }
        //public string str7 { get; set; }
        //public string str8 { get; set; }
        //public string str9 { get; set; }
        //public string str10 { get; set; }
        //public string str11 { get; set; }
        //public string str12 { get; set; }
        //public string str13 { get; set; }
        //public string str14 { get; set; }
        //public string str15 { get; set; }
        //public string str16 { get; set; }
        //public string str17 { get; set; }
        //public string str18 { get; set; }
        //public string str19 { get; set; }
        //public string str20 { get; set; }
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
        //public int? Int1 { get; set; }
        //public int? Int2 { get; set; }
        //public int? Int3 { get; set; }
        //public int? Int4 { get; set; }
        //public int? Int5 { get; set; } 
        #endregion
    }
}
