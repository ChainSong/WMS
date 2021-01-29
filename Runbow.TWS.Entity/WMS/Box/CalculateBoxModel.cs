using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS
{
    public class CalculateBoxModel
    {

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        [EntityPropertyExtension("POID", "POID")]
        public long POID { get; set; }
        [EntityPropertyExtension("PreOrderNumber", "PreOrderNumber")]
        public string PreOrderNumber { get; set; }
        [EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        public string ExternOrderNumber { get; set; }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        [EntityPropertyExtension("LineNumber", "LineNumber")]
        public string LineNumber { get; set; }
        [EntityPropertyExtension("WarehouseId", "WarehouseId")]
        public long WarehouseId { get; set; }
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }

        [EntityPropertyExtension("BoxCode", "BoxCode")]
        public string BoxCode { get; set; }

        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }


        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
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

        //[EntityPropertyExtension("OriginalQty", "OriginalQty")]
        //public double OriginalQty { get; set; }
        [EntityPropertyExtension("Qty", "Qty")]
        public double Qty { get; set; }
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }
         
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }
        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }
        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }


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

        //sku相关信息
        [EntityPropertyExtension("SKUlength", "SKUlength")]
        public double SKUlength { get; set; }//SKU长度
        [EntityPropertyExtension("SKUwidth", "SKUwidth")]
        public double SKUwidth { get; set; }//SKU宽度
        [EntityPropertyExtension("SKUhigh", "SKUhigh")]
        public double SKUhigh { get; set; }//SKU高度
        [EntityPropertyExtension("SKUvolume", "SKUvolume")]
        public double SKUvolume { get; set; }//SKU体积

        [EntityPropertyExtension("SKUBoxspecifications", "SKUBoxspecifications")]
        public double SKUBoxspecifications { get; set; } = 3;//SKU箱规格


        //箱子信息
        [EntityPropertyExtension("BoxType", "BoxType")]
        public string BoxType { get; set; }//SKU体积


        [EntityPropertyExtension("PInt2", "PInt2")]
        public int? PInt2 { get; set; }//是否预警
        [EntityPropertyExtension("PGrade", "PGrade")]
        public string PGrade { get; set; }//安全库存
        //------------------------------------------分配需要Inventory ID     --------------------------------
        [EntityPropertyExtension("IID", "IID")]
        public long IID { get; set; }

        [EntityPropertyExtension("ID2", "ID2")]
        public string ID2 { get; set; }

        [EntityPropertyExtension("IID2", "IID2")]
        public string IID2 { get; set; }

        public CalculateBoxModel()
        {
        }
        public CalculateBoxModel(CalculateBoxModel model)
        {

            this.ID = model.ID;
            //this.OID = model.OID;
            //this.OrderNumber = model.OrderNumber;
            this.ExternOrderNumber = model.ExternOrderNumber;
            this.POID = model.POID;
            //this.PODID = model.PODID;
            this.CustomerID = model.CustomerID;
            this.CustomerName = model.CustomerName;
            this.LineNumber = model.LineNumber;
            this.SKU = model.SKU;
            this.UPC = model.UPC;
            this.GoodsName = model.GoodsName;
            this.GoodsType = model.GoodsType;
            //this.Lot = model.Lot;
            this.BoxNumber = model.BoxNumber;
            this.BatchNumber = model.BatchNumber;
            this.Unit = model.Unit;
            this.Specifications = model.Specifications;
            this.Warehouse = model.Warehouse;
            this.Area = model.Area;
            this.Location = model.Location;
            this.Qty = model.Qty;
            //this.Picker = model.Picker;
            //this.PickTime = model.PickTime;
            //this.Confirmer = model.Confirmer;
            //this.ConfirmeTime = model.ConfirmeTime;
            this.Creator = model.Creator;
            this.CreateTime = model.CreateTime;
            this.Updator = model.Updator;
            this.UpdateTime = model.UpdateTime;
            this.Remark = model.Remark;
            this.str1 = model.str1;
            this.str2 = model.str2;
            this.str3 = model.str3;
            this.str4 = model.str4;
            this.str5 = model.str5;
            this.str6 = model.str6;
            this.str7 = model.str7;
            this.str8 = model.str8;
            this.str9 = model.str9;
            this.str10 = model.str10;
            this.str11 = model.str11;
            this.str12 = model.str12;
            this.str13 = model.str13;
            this.str14 = model.str14;
            this.str15 = model.str15;
            this.str16 = model.str16;
            this.str17 = model.str17;
            this.str18 = model.str18;
            this.str19 = model.str19;
            this.str20 = model.str20;
            this.DateTime1 = model.DateTime1;
            this.DateTime2 = model.DateTime2;
            this.DateTime3 = model.DateTime3;
            this.DateTime4 = model.DateTime4;
            this.DateTime5 = model.DateTime5;
            this.Int1 = model.Int1;
            this.Int2 = model.Int2;
            this.Int3 = model.Int3;
            this.Int4 = model.Int4;
            this.Int5 = model.Int5;


        }
    }
}
