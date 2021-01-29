using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Replenishment
{
    /// <summary>
    /// 拣货单明细
    /// </summary>
    [Serializable]
    public class ReplenishmentDetail
    {
        #region Model
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        [EntityPropertyExtension("RSID", "RSID")]
        public long RSID { get; set; }
        [EntityPropertyExtension("ReplenishmentNumber", "ReplenishmentNumber")]
        public string ReplenishmentNumber { get; set; }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        [EntityPropertyExtension("LineNumber", "LineNumber")]
        public string LineNumber { get; set; }
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }
        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }
        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }

        [EntityPropertyExtension("FromWarehouse", "FromWarehouse")]
        public string FromWarehouse { get; set; }
        [EntityPropertyExtension("FromArea", "FromArea")]
        public string FromArea { get; set; }

        [EntityPropertyExtension("FromLocation", "FromLocation")]
        public string FromLocation { get; set; }

        [EntityPropertyExtension("ToWarehouse", "ToWarehouse")]
        public string ToWarehouse { get; set; }
        [EntityPropertyExtension("ToArea", "ToArea")]
        public string ToArea { get; set; }

        [EntityPropertyExtension("ToLocation", "ToLocation")]
        public string ToLocation { get; set; }
        [EntityPropertyExtension("Qty", "Qty")]
        public double Qty { get; set; }
        [EntityPropertyExtension("InventoryID", "InventoryID")]
        public long InventoryID { get; set; }
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }
        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }
        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 备用字段
        /// </summary>

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
        public DateTime DateTime1 { get; set; }
        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime DateTime2 { get; set; }
        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime DateTime3 { get; set; }
        [EntityPropertyExtension("DateTime4", "DateTime4")]
        public DateTime DateTime4 { get; set; }
        [EntityPropertyExtension("DateTime5", "DateTime5")]
        public DateTime DateTime5 { get; set; }

        [EntityPropertyExtension("Int1", "Int1")]
        public int Int1 { get; set; }
        [EntityPropertyExtension("Int2", "Int2")]
        public int Int2 { get; set; }
        [EntityPropertyExtension("Int3", "Int3")]
        public int Int3 { get; set; }
        [EntityPropertyExtension("Int4", "Int4")]
        public int Int4 { get; set; }
        [EntityPropertyExtension("Int5", "Int5")]
        public int Int5 { get; set; }
        #endregion
    }
}
