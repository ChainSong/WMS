using System;

namespace Runbow.TWS.Entity
{
    [Serializable]
    public class ReceiptReceivingSearchCondition
    {


        #region Model
        public long ID { get; set; }
        public long RID { get; set; }
        public long RDID { get; set; }
        public string ReceiptNumber { get; set; }
        public string ExternReceiptNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string LineNumber { get; set; }
        public string SkuLineNumber { get; set; }
        public string SKU { get; set; }
        public string UPC { get; set; }
        public decimal QtyReceived { get; set; }
        public string Warehouse { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        public string GoodsType { get; set; }
        public string BatchNumber { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Updator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string str1 { get; set; }
        public string str2 { get; set; }
        public string str3 { get; set; }
        public string str4 { get; set; }
        public string str5 { get; set; }
        public string str6 { get; set; }
        public string str7 { get; set; }
        public string str8 { get; set; }
        public string str9 { get; set; }
        public string str10 { get; set; }
        public string str11 { get; set; }
        public string str12 { get; set; }
        public string str13 { get; set; }
        public string str14 { get; set; }
        public string str15 { get; set; }
        public string str16 { get; set; }
        public string str17 { get; set; }
        public string str18 { get; set; }
        public string str19 { get; set; }
        public string str20 { get; set; }
        public DateTime? DateTime1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public DateTime? DateTime3 { get; set; }
        public DateTime? DateTime4 { get; set; }
        public DateTime? DateTime5 { get; set; }
        public int? Int1 { get; set; }
        public int? Int2 { get; set; }
        public int? Int3 { get; set; }
        public int? Int4 { get; set; }
        public int? Int5 { get; set; }



        public string StartTime { get; set; }

        public string Endtime { get; set; }

        //public string StartCreateTime { get; set; }
        //public string EndCreateTime { get; set; }
        #endregion Model
    }
}
