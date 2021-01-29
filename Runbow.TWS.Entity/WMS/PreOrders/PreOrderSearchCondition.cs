using Runbow.TWS.Common;
using Runbow.TWS.Entity.WMS.PreOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class PreOrderSearchCondition : PreOrder
    {
        public List<PreOrderIds> Ids { get; set; }
        public DateTime? EndOrderTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public DateTime? EndDateTime1 { get; set; }
        public DateTime? EndDateTime2 { get; set; }
        public DateTime? EndDateTime3 { get; set; }
        public DateTime? EndDateTime4 { get; set; }
        public DateTime? EndDateTime5 { get; set; }
        public int UserType { get; set; }
        public string CustomerIDs { get; set; }
        public string SKUModel{ get; set; }

        //public string PageIndex { get; set; }
        //public string PictureStr { get; set; }
        //public long ID { get; set; }
        //public string PreOrderNumber { get; set; }
        //public string ExternOrderNumber { get; set; }
        //public long? CustomerID { get; set; }
        //public string CustomerName { get; set; }
        //public string Warehouse { get; set; }
        //public long WarehouseId { get; set; }
        //public string OrderType { get; set; }
        //public int? Status { get; set; }
        //public DateTime? OrderTime { get; set; }
        //public string Province { get; set; }
        //public string City { get; set; }
        //public string District { get; set; }
        //public string Address { get; set; }
        //public string Consignee { get; set; }
        //public string Contact { get; set; }
        //public string ExpressCompany { get; set; }
        //public int? DetailCount { get; set; }
        //public string Creator { get; set; }
        //public DateTime? CreateTime { get; set; }
        //public string Updator { get; set; }
        //public DateTime? UpdateTime { get; set; }
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
        //public DateTime? DateTime1 { get; set; }
        //public DateTime? DateTime2 { get; set; }
        //public DateTime? DateTime3 { get; set; }
        //public DateTime? DateTime4 { get; set; }
        //public DateTime? DateTime5 { get; set; }
        //public int? Int1 { get; set; }
        //public int? Int2 { get; set; }
        //public int? Int3 { get; set; }
        //public int? Int4 { get; set; }
        //public int? Int5 { get; set; }

        //public string cArea { get; set; }

        //[EntityPropertyExtension("ID", "ID")]
        //public long ID { get; set; }
        //[EntityPropertyExtension("POID", "POID")]
        //public long POID { get; set; }
        //[EntityPropertyExtension("PreOrderNumber", "PreOrderNumber")]
        //public string PreOrderNumber { get; set; }
        //[EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        //public string ExternOrderNumber { get; set; }
        //[EntityPropertyExtension("CustomerID", "CustomerID")]
        //public long CustomerID { get; set; }
        //[EntityPropertyExtension("CustomerName", "CustomerName")]
        //public string CustomerName { get; set; }
        //[EntityPropertyExtension("LineNumber", "LineNumber")]
        //public string LineNumber { get; set; }
        //[EntityPropertyExtension("WarehouseId", "WarehouseId")]
        //public long WarehouseId { get; set; }
        //[EntityPropertyExtension("Warehouse", "Warehouse")]
        //public string Warehouse { get; set; }
        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }
        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }
        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }
        [EntityPropertyExtension("ManufacturerSKU", "ManufacturerSKU")]
        public string ManufacturerSKU { get; set; }
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }
        [EntityPropertyExtension("InventoryQty", "InventoryQty")]
        public double InventoryQty { get; set; }
        [EntityPropertyExtension("OriginalQty", "OriginalQty")]
        public double OriginalQty { get; set; }
        [EntityPropertyExtension("AllocatedQty", "AllocatedQty")]
        public double AllocatedQty { get; set; }
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }
        //[EntityPropertyExtension("Creator", "Creator")]
        //public string Creator { get; set; }
        //[EntityPropertyExtension("CreateTime", "CreateTime")]
        //public DateTime? CreateTime { get; set; }
        //[EntityPropertyExtension("Updator", "Updator")]
        //public string Updator { get; set; }
        //[EntityPropertyExtension("UpdateTime", "UpdateTime")]
        //public DateTime? UpdateTime { get; set; }
        //[EntityPropertyExtension("Remark", "Remark")]
        //public string Remark { get; set; }
        //[EntityPropertyExtension("str1", "str1")]
        //public string str1 { get; set; }
        //[EntityPropertyExtension("str2", "str2")]
        //public string str2 { get; set; }
        //[EntityPropertyExtension("str3", "str3")]
        //public string str3 { get; set; }
        //[EntityPropertyExtension("str4", "str4")]
        //public string str4 { get; set; }
        //[EntityPropertyExtension("str5", "str5")]
        //public string str5 { get; set; }
        //[EntityPropertyExtension("str6", "str6")]
        //public string str6 { get; set; }
        //[EntityPropertyExtension("str7", "str7")]
        //public string str7 { get; set; }
        //[EntityPropertyExtension("str8", "str8")]
        //public string str8 { get; set; }
        //[EntityPropertyExtension("str9", "str9")]
        //public string str9 { get; set; }
        //[EntityPropertyExtension("str10", "str10")]
        //public string str10 { get; set; }
        //[EntityPropertyExtension("str11", "str11")]
        //public string str11 { get; set; }
        //[EntityPropertyExtension("str12", "str12")]
        //public string str12 { get; set; }
        //[EntityPropertyExtension("str13", "str13")]
        //public string str13 { get; set; }
        //[EntityPropertyExtension("str14", "str14")]
        //public string str14 { get; set; }
        //[EntityPropertyExtension("str15", "str15")]
        //public string str15 { get; set; }
        //[EntityPropertyExtension("str16", "str16")]
        //public string str16 { get; set; }
        //[EntityPropertyExtension("str17", "str17")]
        //public string str17 { get; set; }
        //[EntityPropertyExtension("str18", "str18")]
        //public string str18 { get; set; }
        //[EntityPropertyExtension("str19", "str19")]
        //public string str19 { get; set; }
        //[EntityPropertyExtension("str20", "str20")]
        //public string str20 { get; set; }
        //[EntityPropertyExtension("DateTime1", "DateTime1")]
        //public DateTime? DateTime1 { get; set; }
        //[EntityPropertyExtension("DateTime2", "DateTime2")]
        //public DateTime? DateTime2 { get; set; }
        //[EntityPropertyExtension("DateTime3", "DateTime3")]
        //public DateTime? DateTime3 { get; set; }
        //[EntityPropertyExtension("DateTime4", "DateTime4")]
        //public DateTime? DateTime4 { get; set; }
        //[EntityPropertyExtension("DateTime5", "DateTime5")]
        //public DateTime? DateTime5 { get; set; }
        //[EntityPropertyExtension("Int1", "Int1")]
        //public int? Int1 { get; set; }
        //[EntityPropertyExtension("Int2", "Int2")]
        //public int? Int2 { get; set; }
        //[EntityPropertyExtension("Int3", "Int3")]
        //public int? Int3 { get; set; }
        //[EntityPropertyExtension("Int4", "Int4")]
        //public int? Int4 { get; set; }
        //[EntityPropertyExtension("Int5", "Int5")]
        //public int? Int5 { get; set; }
        //sku相关信息
        [EntityPropertyExtension("PStr11", "PStr11")]
        public string PStr11 { get; set; }//包装规格
        [EntityPropertyExtension("PStr12", "PStr12")]
        public string PStr12 { get; set; }//料箱规格
        [EntityPropertyExtension("PInt2", "PInt2")]
        public int? PInt2 { get; set; }//是否预警
        [EntityPropertyExtension("PGrade", "PGrade")]
        public string PGrade { get; set; }//安全库存
        //分配需要Inventory ID
        [EntityPropertyExtension("IID", "IID")]
        public long IID { get; set; }
        [EntityPropertyExtension("ID2", "ID2")]
        public string ID2 { get; set; }
        [EntityPropertyExtension("IID2", "IID2")]
        public string IID2 { get; set; }

    }
}
