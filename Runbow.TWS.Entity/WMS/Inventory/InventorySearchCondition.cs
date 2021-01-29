using System;

namespace Runbow.TWS.Entity
{
    public class InventorySearchCondition
    {
        #region Model
        public string UPC { set; get; }
        public string Unit { set; get; }
        public string Specifications { set; get; }
        public string CustomerIDs { set; get; }
        public string OrderByType { get; set; }
        public int ID { get; set; }
        public long RRID { get; set; }
        public string Warehouse { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        public long SuperID { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string SKU { get; set; }
        public string BoxNumber { get; set; }
        public string BatchNumber { get; set; }
        public string GoodsType { get; set; }
        public int Qty { get; set; }
        public int InventoryType { get; set; }
        //public int QtyAllocated { get; set; }
        //public int QtyPicked { get; set; }
        //public int QtyExpected { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateTime { get; set; }
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

        public DateTime? InventoryDate { get; set; }
        public DateTime? StateDate  { get; set; }

        #endregion Model
    }
}
