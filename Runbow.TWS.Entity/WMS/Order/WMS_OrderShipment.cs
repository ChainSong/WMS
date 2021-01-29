using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    /// <summary>
    /// 系统收到的发运单头信息
    /// </summary>
    public class WMS_OrderShipment
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ShipmentNumber", "ShipmentNumber")]
        public string ShipmentNumber { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long? WarehouseID { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }


        [EntityPropertyExtension("Status", "Status")]
        public int? Status { get; set; }//1 新增，2.已发送打印，3已发送goodsissue

        [EntityPropertyExtension("Type", "Type")]
        public int? Type { get; set; }//场景

        [EntityPropertyExtension("DetailCount", "DetailCount")]
        public int? DetailCount { get; set; }//DN单数  

        [EntityPropertyExtension("PrintCreator", "PrintCreator")]
        public string PrintCreator { get; set; }
        [EntityPropertyExtension("PrintTime", "PrintTime")]
        public DateTime? PrintTime { get; set; }
        [EntityPropertyExtension("GoodsissueCreator", "GoodsissueCreator")]
        public string GoodsissueCreator { get; set; }
        [EntityPropertyExtension("GoodsissueTime", "GoodsissueTime")]
        public DateTime? GoodsissueTime { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }
        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime? UpdateTime { get; set; }
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }
        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }
        [EntityPropertyExtension("str2", "str2")]
        public string str2 { get; set; }
        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }
        [EntityPropertyExtension("str4", "str4")]
        public string str4 { get; set; }
        [EntityPropertyExtension("str5", "str5")]
        public string str5 { get; set; }
        [EntityPropertyExtension("str6", "str6")]
        public string str6 { get; set; }
        [EntityPropertyExtension("str7", "str7")]
        public string str7 { get; set; }
        [EntityPropertyExtension("str8", "str8")]
        public string str8 { get; set; }
        [EntityPropertyExtension("str9", "str9")]
        public string str9 { get; set; }
        [EntityPropertyExtension("str10", "str10")]
        public string str10 { get; set; }
        [EntityPropertyExtension("str11", "str11")]
        public string str11 { get; set; }
        [EntityPropertyExtension("str12", "str12")]
        public string str12 { get; set; }
        [EntityPropertyExtension("str13", "str13")]
        public string str13 { get; set; }
        [EntityPropertyExtension("str14", "str14")]
        public string str14 { get; set; }
        [EntityPropertyExtension("str15", "str15")]
        public string str15 { get; set; }
        [EntityPropertyExtension("str16", "str16")]
        public string str16 { get; set; }
        [EntityPropertyExtension("str17", "str17")]
        public string str17 { get; set; }
        [EntityPropertyExtension("str18", "str18")]
        public string str18 { get; set; }
        [EntityPropertyExtension("str19", "str19")]
        public string str19 { get; set; }
        [EntityPropertyExtension("str20", "str20")]
        public string str20 { get; set; }
        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }
        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }
        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime? DateTime3 { get; set; }
        [EntityPropertyExtension("DateTime4", "DateTime4")]
        public DateTime? DateTime4 { get; set; }
        [EntityPropertyExtension("DateTime5", "DateTime5")]
        public DateTime? DateTime5 { get; set; }
        [EntityPropertyExtension("Int1", "Int1")]
        public int? Int1 { get; set; }
        [EntityPropertyExtension("Int2", "Int2")]
        public int? Int2 { get; set; }
        [EntityPropertyExtension("Int3", "Int3")]
        public int? Int3 { get; set; }
        [EntityPropertyExtension("Int4", "Int4")]
        public int? Int4 { get; set; }
        [EntityPropertyExtension("Int5", "Int5")]
        public int? Int5 { get; set; }

        //public IEnumerable<WMS_OrderShipmentDetail> OrderList { get; set; }
    }
}
