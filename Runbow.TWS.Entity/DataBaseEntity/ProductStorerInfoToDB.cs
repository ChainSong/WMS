using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Product;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class ProductStorerInfoToDB : SqlDataRecord
    {
        public ProductStorerInfoToDB(ProductStorerInfo productStorerInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, productStorerInfo.ID);
            SetSqlInt64(1, productStorerInfo.StorerID);
            SetSqlString(2, productStorerInfo.SKU.Trim());
            SetSqlInt32(3, productStorerInfo.Status);
            SetSqlString(4, productStorerInfo.GoodsName.Trim());
            SetSqlInt32(5, productStorerInfo.GoodsType);
            SetSqlString(6, productStorerInfo.SKUClassification);
            SetSqlString(7, productStorerInfo.SKUGroup);
            SetSqlString(8, productStorerInfo.ManufacturerSKU);
            SetSqlString(9, productStorerInfo.RetailSKU);
            SetSqlString(10, productStorerInfo.ReplaceSKU);
            SetSqlString(11, productStorerInfo.ReplaceSKU);
            SetSqlString(12, productStorerInfo.Packing);
            SetSqlString(13, productStorerInfo.Grade);
            SetSqlString(14, productStorerInfo.Country);
            SetSqlString(15, productStorerInfo.Manufacturer);
            SetSqlString(16, productStorerInfo.DangerCode);
            SetSqlString(17, productStorerInfo.Volume);
            SetSqlString(18, productStorerInfo.StandardVolume);
            SetSqlString(19, productStorerInfo.Weight);
            SetSqlString(20, productStorerInfo.StandardWeight);
            SetSqlString(21, productStorerInfo.NetWeight);
            SetSqlString(22, productStorerInfo.StandardNetWeight);
            SetSqlDecimal(23, productStorerInfo.Price ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(24, productStorerInfo.ActualPrice ?? SqlTypes.SqlDecimal.Null);
            SetSqlString(25, productStorerInfo.Cost);
            SetSqlString(26, productStorerInfo.ActualCost);
            SetSqlString(27, productStorerInfo.StandardOrderingCost);
            SetSqlString(28, productStorerInfo.ShipmentCost);
            SetSqlString(29, productStorerInfo.QcInSpectionLoc);
            SetSqlString(30, productStorerInfo.QCPercentage);
            SetSqlString(31, productStorerInfo.ReceiptQcUom);
            SetInt32(32, productStorerInfo.IsQcEligible);
            SetSqlString(33, productStorerInfo.PutArea);
            SetSqlString(34, productStorerInfo.PutCode);
            SetSqlString(35, productStorerInfo.PutRule);
            SetSqlString(36, productStorerInfo.PutStrategy);
            SetSqlString(37, productStorerInfo.AllocateRule);
            SetSqlString(38, productStorerInfo.PickedCode);
            SetSqlString(39, productStorerInfo.SKUType);
            SetSqlString(40, productStorerInfo.Color);
            SetSqlString(41, productStorerInfo.Size);
            SetSqlString(42, productStorerInfo.Remark);
            SetSqlInt32(43, productStorerInfo.Int1);
            SetSqlInt32(44, productStorerInfo.Int2 == true ? 1 : 0);
            SetSqlString(45, productStorerInfo.Str11);
            SetSqlString(46, productStorerInfo.Str12);

            SetSqlString(47, productStorerInfo.Str1);
            SetSqlString(48, productStorerInfo.Str2);
            SetSqlString(49, productStorerInfo.Str3);
            SetSqlString(50, productStorerInfo.Str4);
            SetSqlString(51, productStorerInfo.Str5);
            SetSqlString(52, productStorerInfo.Str6);
            SetSqlString(53, productStorerInfo.Str7);
            SetSqlString(54, productStorerInfo.Str8);

            SetSqlString(55, productStorerInfo.Str9);
            SetSqlString(56, productStorerInfo.Str10);

            SetSqlString(57, productStorerInfo.Str13);
            SetSqlString(58, productStorerInfo.Str14);
            SetSqlString(59, productStorerInfo.Str15);
            SetSqlString(60, productStorerInfo.Str16);
            SetSqlString(61, productStorerInfo.Str17);
            SetSqlString(62, productStorerInfo.Str18);
            SetSqlString(63, productStorerInfo.Str19);

        }
        private static readonly SqlMetaData[] s_metadata =
        {
             new SqlMetaData("ID", SqlDbType.BigInt),
             new SqlMetaData("StorerID", SqlDbType.BigInt),
             new SqlMetaData("SKU", SqlDbType.NVarChar,50),
             new SqlMetaData("Status", SqlDbType.Int),
             new SqlMetaData("GoodsName", SqlDbType.NVarChar,200),
             new SqlMetaData("GoodsType", SqlDbType.Int),
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
             new SqlMetaData("Str11", SqlDbType.NVarChar,100),
             new SqlMetaData("Str12", SqlDbType.NVarChar,100),

             new SqlMetaData("Str1",SqlDbType.NVarChar,50),
             new SqlMetaData("Str2",SqlDbType.NVarChar,50),
             new SqlMetaData("Str3",SqlDbType.NVarChar,50),
             new SqlMetaData("Str4",SqlDbType.NVarChar,50),
             new SqlMetaData("Str5",SqlDbType.NVarChar,50),
             new SqlMetaData("Str6",SqlDbType.NVarChar,50),
             new SqlMetaData("Str7",SqlDbType.NVarChar,50),
             new SqlMetaData("Str8",SqlDbType.NVarChar,50),
              new SqlMetaData("Str9",SqlDbType.NVarChar,50),
             new SqlMetaData("Str10",SqlDbType.NVarChar,50),

             new SqlMetaData("Str13",SqlDbType.NVarChar,50),
             new SqlMetaData("Str14",SqlDbType.NVarChar,50),
             new SqlMetaData("Str15",SqlDbType.NVarChar,50),
             new SqlMetaData("Str16",SqlDbType.NVarChar,50),
             new SqlMetaData("Str17",SqlDbType.NVarChar,50),
             new SqlMetaData("Str18",SqlDbType.NVarChar,50),
             new SqlMetaData("Str19",SqlDbType.NVarChar,50)

        };

    }
}
