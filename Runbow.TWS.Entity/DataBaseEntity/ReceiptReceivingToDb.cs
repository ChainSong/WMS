using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;
namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class ReceiptReceivingToDb : SqlDataRecord
    {
        public ReceiptReceivingToDb(ReceiptReceiving receipt)
            : base(s_metadata)
        {
            SetSqlInt64(0, receipt.RID);
            SetSqlInt64(1, receipt.RDID);
            SetSqlString(2, receipt.ReceiptNumber);
            SetSqlString(3, receipt.ExternReceiptNumber);
            SetSqlInt64(4, receipt.CustomerID);
            SetSqlString(5, receipt.CustomerName);
            SetSqlString(6, receipt.LineNumber);
            SetSqlString(7, receipt.SkuLineNumber);
            SetSqlString(8, receipt.SKU);
            SetSqlString(9, receipt.UPC);
            SetSqlDouble(10, receipt.QtyReceived);
            SetSqlString(11, receipt.Warehouse);
            SetSqlString(12, receipt.Area);
            SetSqlString(13, receipt.Location);
            SetSqlString(14, receipt.GoodsName);
            SetSqlString(15, receipt.GoodsType);

            SetSqlString(16, receipt.BatchNumber == "" ? null : receipt.BatchNumber);
            SetSqlString(17, receipt.Creator);
            //  SetSqlDateTime(16, receipt.CreateTime);
            SetSqlString(18, receipt.Updator);
            SetSqlDateTime(19, receipt.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(20, receipt.Remark);
            SetSqlString(21, receipt.BoxNumber == "" ? null : receipt.BoxNumber);
            SetSqlDateTime(22, receipt.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(23, receipt.Unit == "" ? null : receipt.Unit);
            SetSqlString(24, receipt.Specifications == "" ? null : receipt.Specifications);
            //SetSqlString(25, receipt.str1);
            //SetSqlString(26, receipt.str2);
            //SetSqlString(27, receipt.str3);
            //SetSqlString(28, receipt.str4);
            //SetSqlString(29, receipt.str5);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("RID", SqlDbType.BigInt),
            new SqlMetaData("RDID", SqlDbType.BigInt),
            new SqlMetaData("ReceiptNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExternReceiptNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar,50),
            new SqlMetaData("LineNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("SkuLineNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("SKU", SqlDbType.NVarChar,50),
            new SqlMetaData("UPC",SqlDbType.NVarChar,50),
            new SqlMetaData("QtyReceived",SqlDbType.Float),
            new SqlMetaData("Warehouse",SqlDbType.NVarChar,50),
            new SqlMetaData("Area",SqlDbType.NVarChar,50),
            new SqlMetaData("Location",SqlDbType.NVarChar,50),
            new SqlMetaData("GoodsName",SqlDbType.NVarChar,50),
            new SqlMetaData("GoodsType",SqlDbType.NVarChar,50),
        
            new SqlMetaData("BatchNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("Creator", SqlDbType.NVarChar,50),
         //   new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Updator", SqlDbType.NVarChar,50),
            new SqlMetaData("UpdateTime", SqlDbType.DateTime),
            new SqlMetaData("Remark", SqlDbType.VarChar,500),
            new SqlMetaData("BoxNumber", SqlDbType.NVarChar,200),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("Unit", SqlDbType.NVarChar,100),
            new SqlMetaData("Specifications", SqlDbType.NVarChar,100)
            //new SqlMetaData("str1", SqlDbType.NVarChar,50),
            //new SqlMetaData("str2", SqlDbType.NVarChar,50),
            //new SqlMetaData("str3", SqlDbType.NVarChar,50),
            //new SqlMetaData("str4", SqlDbType.NVarChar,50),
            //new SqlMetaData("str5", SqlDbType.NVarChar,50)
            //new SqlMetaData("str6", SqlDbType.NVarChar,50),
            //new SqlMetaData("str7", SqlDbType.NVarChar,50),
            //new SqlMetaData("str8", SqlDbType.NVarChar,50),
            //new SqlMetaData("str9", SqlDbType.NVarChar,50),
            //new SqlMetaData("str10", SqlDbType.NVarChar,50),
            //new SqlMetaData("str11", SqlDbType.NVarChar,50),
            //new SqlMetaData("str12", SqlDbType.NVarChar,50),
            //new SqlMetaData("str13", SqlDbType.NVarChar,50),
            //new SqlMetaData("str14", SqlDbType.NVarChar,50),
            //new SqlMetaData("str15", SqlDbType.NVarChar,50),
            //new SqlMetaData("str16", SqlDbType.NVarChar,50),
            //new SqlMetaData("str17", SqlDbType.NVarChar,50),
            //new SqlMetaData("str18", SqlDbType.NVarChar,50),
            //new SqlMetaData("str19", SqlDbType.NVarChar,50),
            //new SqlMetaData("str20", SqlDbType.NVarChar,50),
            //new SqlMetaData("DateTime1", SqlDbType.DateTime),
            //new SqlMetaData("DateTime2", SqlDbType.DateTime),
            //new SqlMetaData("DateTime3", SqlDbType.DateTime),
            //new SqlMetaData("DateTime4", SqlDbType.DateTime),
            //new SqlMetaData("DateTime5", SqlDbType.DateTime),
            //new SqlMetaData("Int1", SqlDbType.Int),
            //new SqlMetaData("Int2", SqlDbType.Int),
            //new SqlMetaData("Int3", SqlDbType.Int),
            //new SqlMetaData("Int4", SqlDbType.Int),
            //new SqlMetaData("Int5", SqlDbType.Int),
        };
    }
}