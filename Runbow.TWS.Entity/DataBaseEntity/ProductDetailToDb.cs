using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlTypes = global::System.Data.SqlTypes;
namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class ProductDetailToDb : SqlDataRecord
    {
        public ProductDetailToDb(ProductDetail info)
            : base(s_metadata)
        {
            SetSqlInt64(0, info.ID?? SqlTypes.SqlInt64.Null);
            SetSqlInt64(1, info.PID ?? SqlTypes.SqlInt64.Null);
            SetSqlInt64(2, info.StorerID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(3, info.SKU.Trim());
            SetSqlInt32(4, info.Status);
            SetSqlString(5, info.GoodsName);
            SetSqlInt32(6, info.GoodsType);
            SetSqlString(7, info.UPC);
            SetSqlString(8, info.UPCName);
            SetSqlString(9, info.UPCType);
            SetSqlInt32(10, info.UPCNumber ?? SqlTypes.SqlInt32.Null);
            SetSqlString(11, info.SKUClassification);
            SetSqlString(12, info.SKUGroup);
            SetSqlString(13, info.ManufacturerSKU);
            SetSqlString(14, info.RetailSKU);
            SetSqlString(15, info.ReplaceSKU);
            SetSqlString(16, info.ReplaceSKU);
            SetSqlString(17, info.Packing);
            SetSqlString(18, info.Grade);
            SetSqlString(19, info.Country);
            SetSqlString(20, info.Manufacturer);
            SetSqlString(21, info.DangerCode);
            SetSqlString(22, info.Volume);
            SetSqlString(23, info.StandardVolume);
            SetSqlString(24, info.Weight);
            SetSqlString(25, info.StandardWeight);
            SetSqlString(26, info.NetWeight);
            SetSqlString(27, info.StandardNetWeight);
            SetSqlDecimal(28, info.Price ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(29, info.ActualPrice ?? SqlTypes.SqlDecimal.Null);
            SetSqlString(30, info.Cost);
            SetSqlString(31, info.ActualCost);
            SetSqlString(32, info.StandardOrderingCost);
            SetSqlString(33, info.ShipmentCost);
            SetSqlString(34, info.QcInSpectionLoc);
            SetSqlString(35, info.QCPercentage);
            SetSqlString(36, info.ReceiptQcUom);
            SetInt32(37, info.IsQcEligible);
            SetSqlString(38, info.PutArea);
            SetSqlString(39, info.PutCode);
            SetSqlString(40, info.PutRule);
            SetSqlString(41, info.PutStrategy);
            SetSqlString(42, info.AllocateRule);
            SetSqlString(43, info.PickedCode);
            SetSqlString(44, info.SKUType);
            SetSqlString(45, info.Color);
            SetSqlString(46, info.Size);
            SetSqlString(47, info.Remark);
            SetSqlInt32(48, info.Int1);
            SetSqlInt32(49, info.Int2);
        }
        private static readonly SqlMetaData[] s_metadata =
        {
             new SqlMetaData("ID", SqlDbType.BigInt),
             new SqlMetaData("PID", SqlDbType.BigInt),
             new SqlMetaData("StorerID", SqlDbType.BigInt),
             new SqlMetaData("SKU", SqlDbType.NVarChar,50),
             new SqlMetaData("Status", SqlDbType.Int),
             new SqlMetaData("GoodsName", SqlDbType.NVarChar,200),
             new SqlMetaData("GoodsType", SqlDbType.Int),
             new SqlMetaData("UPC", SqlDbType.NVarChar,50),
             new SqlMetaData("UPCName", SqlDbType.NVarChar,50),
             new SqlMetaData("UPCType", SqlDbType.NVarChar,50),
             new SqlMetaData("UPCNumber", SqlDbType.Int),   
             new SqlMetaData("SKUClassification", SqlDbType.NVarChar,50),
             new SqlMetaData("SKUGroup", SqlDbType.NVarChar,50),
             new SqlMetaData("ManufacturerSKU", SqlDbType.NVarChar,50),
             new SqlMetaData("RetailSKU", SqlDbType.NVarChar,50),
             new SqlMetaData("ReplaceSKU", SqlDbType.NVarChar,50),
             new SqlMetaData("BoxGroup", SqlDbType.NVarChar,50),
             new SqlMetaData("Packing", SqlDbType.NVarChar,50),
             new SqlMetaData("Grade", SqlDbType.NVarChar,50),
             new SqlMetaData("Country", SqlDbType.NVarChar,50),
             new SqlMetaData("Manufacturer", SqlDbType.NVarChar,50),
             new SqlMetaData("DangerCode", SqlDbType.NVarChar,50),
             new SqlMetaData("Volume", SqlDbType.NVarChar,50),
             new SqlMetaData("StandardVolume", SqlDbType.NVarChar,50),
             new SqlMetaData("Weight", SqlDbType.NVarChar,50),
             new SqlMetaData("StandardWeight", SqlDbType.NVarChar,50),
             new SqlMetaData("NetWeight", SqlDbType.NVarChar,50),
             new SqlMetaData("StandardNetWeight", SqlDbType.NVarChar,50),
             new SqlMetaData("Price", SqlDbType.Decimal),
             new SqlMetaData("ActualPrice", SqlDbType.Decimal),
             new SqlMetaData("Cost", SqlDbType.NVarChar,50),
             new SqlMetaData("ActualCost", SqlDbType.NVarChar,50),
             new SqlMetaData("StandardOrderingCost", SqlDbType.NVarChar,50),
             new SqlMetaData("ShipmentCost", SqlDbType.NVarChar,50),
             new SqlMetaData("QcInSpectionLoc", SqlDbType.NVarChar,50),
             new SqlMetaData("QCPercentage", SqlDbType.NVarChar,50),
             new SqlMetaData("ReceiptQcUom", SqlDbType.NVarChar,50),
             new SqlMetaData("IsQcEligible", SqlDbType.Int),
             new SqlMetaData("PutArea", SqlDbType.NVarChar,50),
             new SqlMetaData("PutCode", SqlDbType.NVarChar,50),
             new SqlMetaData("PutRule", SqlDbType.NVarChar,50),
             new SqlMetaData("PutStrategy", SqlDbType.NVarChar,50),
             new SqlMetaData("AllocateRule", SqlDbType.NVarChar,50),
             new SqlMetaData("PickedCode", SqlDbType.NVarChar,50),
             new SqlMetaData("SKUType", SqlDbType.NVarChar,50),
             new SqlMetaData("Color", SqlDbType.NVarChar,50),
             new SqlMetaData("Size", SqlDbType.NVarChar,50),
             new SqlMetaData("Remark", SqlDbType.NVarChar,500),
             new SqlMetaData("Int1", SqlDbType.Int),
             new SqlMetaData("Int2", SqlDbType.Int),
        };

    }
}
