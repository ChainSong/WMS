using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSReceiptDetailToDb : SqlDataRecord
    {
        public WMSReceiptDetailToDb(ReceiptDetail wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlString(1, wmsInfo.ExternReceiptNumber);
            SetSqlInt64(2, wmsInfo.ASNID);
            SetSqlString(3, wmsInfo.ASNNumber);
            SetSqlInt64(4, wmsInfo.CustomerID);
            SetSqlString(5, wmsInfo.CustomerName);
            SetSqlString(6, wmsInfo.LineNumber);
            SetSqlString(7, wmsInfo.SKU);
            SetSqlDouble(8, wmsInfo.QtyExpected);
            SetSqlDouble(9, wmsInfo.QtyReceived);
            SetSqlString(10, wmsInfo.Creator ?? SqlTypes.SqlString.Null);
            SetSqlDateTime(11, wmsInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(12, wmsInfo.Updator ?? SqlTypes.SqlString.Null);
            SetSqlDateTime(13, wmsInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);

            #region 备用字段
            SetSqlString(14, wmsInfo.str1 ?? SqlTypes.SqlString.Null);
            SetSqlString(15, wmsInfo.str2 ?? SqlTypes.SqlString.Null);
            SetSqlString(16, wmsInfo.str3 ?? SqlTypes.SqlString.Null);
            SetSqlString(17, wmsInfo.str4 ?? SqlTypes.SqlString.Null);
            SetSqlString(18, wmsInfo.str5 ?? SqlTypes.SqlString.Null);
            SetSqlString(19, wmsInfo.str6 ?? SqlTypes.SqlString.Null);
            SetSqlString(20, wmsInfo.str7 ?? SqlTypes.SqlString.Null);
            SetSqlString(21, wmsInfo.str8 ?? SqlTypes.SqlString.Null);
            SetSqlString(22, wmsInfo.str9 ?? SqlTypes.SqlString.Null);
            SetSqlString(23, wmsInfo.str10 ?? SqlTypes.SqlString.Null);
            SetSqlString(24, wmsInfo.str11 ?? SqlTypes.SqlString.Null);
            SetSqlString(25, wmsInfo.str12 ?? SqlTypes.SqlString.Null);
            SetSqlString(26, wmsInfo.str13 ?? SqlTypes.SqlString.Null);
            SetSqlString(27, wmsInfo.str14 ?? SqlTypes.SqlString.Null);
            SetSqlString(28, wmsInfo.str15 ?? SqlTypes.SqlString.Null);
            SetSqlString(29, wmsInfo.str16 ?? SqlTypes.SqlString.Null);
            SetSqlString(30, wmsInfo.str17 ?? SqlTypes.SqlString.Null);
            SetSqlString(31, wmsInfo.str18 ?? SqlTypes.SqlString.Null);
            SetSqlString(32, wmsInfo.str19 ?? SqlTypes.SqlString.Null);
            SetSqlString(33, wmsInfo.str20 ?? SqlTypes.SqlString.Null);           

            SetSqlDateTime(34, wmsInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(35, wmsInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(36, wmsInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(37, wmsInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(38, wmsInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);

            SetSqlInt32(39, wmsInfo.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(40, wmsInfo.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(41, wmsInfo.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(42, wmsInfo.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(43, wmsInfo.Int5 ?? SqlTypes.SqlInt32.Null);
            SetSqlString(44, wmsInfo.ReceiptNumber);
            SetSqlInt64(45, wmsInfo.RID);
            SetSqlString(46, wmsInfo.GoodsName ?? SqlTypes.SqlString.Null);
            SetSqlString(47, wmsInfo.GoodsType ?? SqlTypes.SqlString.Null);
            SetSqlString(48, wmsInfo.BatchNumber ?? SqlTypes.SqlString.Null);
            SetSqlString(49, wmsInfo.BoxNumber ?? SqlTypes.SqlString.Null);
            SetSqlString(50, wmsInfo.Unit ?? SqlTypes.SqlString.Null);
            SetSqlString(51, wmsInfo.Specifications ?? SqlTypes.SqlString.Null);
            SetSqlString(52, wmsInfo.UPC ?? SqlTypes.SqlString.Null);
            #endregion
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ExternReceiptNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("ASNID", SqlDbType.BigInt),
            new SqlMetaData("ASNNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("LineNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("SKU", SqlDbType.VarChar, 50),
            new SqlMetaData("QtyExpected", SqlDbType.Float),
            new SqlMetaData("QtyReceived", SqlDbType.Float),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Updator",SqlDbType.NVarChar, 50),
            new SqlMetaData("UpdateTime",  SqlDbType.DateTime),

            new SqlMetaData("str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("str2", SqlDbType.NVarChar, 50),
            new SqlMetaData("str3", SqlDbType.NVarChar, 50),
            new SqlMetaData("str4", SqlDbType.NVarChar, 50),
            new SqlMetaData("str5", SqlDbType.NVarChar, 50),
            new SqlMetaData("str6", SqlDbType.NVarChar, 50),
            new SqlMetaData("str7", SqlDbType.NVarChar, 50),
            new SqlMetaData("str8", SqlDbType.NVarChar, 50),
            new SqlMetaData("str9", SqlDbType.NVarChar, 50),
            new SqlMetaData("str10", SqlDbType.NVarChar, 50),
            new SqlMetaData("str11", SqlDbType.NVarChar, 50),
            new SqlMetaData("str12", SqlDbType.NVarChar, 50),
            new SqlMetaData("str13", SqlDbType.NVarChar, 50),
            new SqlMetaData("str14", SqlDbType.NVarChar, 50),
            new SqlMetaData("str15", SqlDbType.NVarChar, 50),
            new SqlMetaData("str16", SqlDbType.NVarChar, 200),
            new SqlMetaData("str17", SqlDbType.NVarChar, 200),
            new SqlMetaData("str18", SqlDbType.NVarChar, 200),
            new SqlMetaData("str19", SqlDbType.NVarChar, 500),
            new SqlMetaData("str20", SqlDbType.NVarChar, 500),
            new SqlMetaData("DateTime1",  SqlDbType.DateTime),
            new SqlMetaData("DateTime2",  SqlDbType.DateTime),
            new SqlMetaData("DateTime3",  SqlDbType.DateTime),
            new SqlMetaData("DateTime4",  SqlDbType.DateTime),
            new SqlMetaData("DateTime5",  SqlDbType.DateTime),
            new SqlMetaData("Int1", SqlDbType.Int),
            new SqlMetaData("Int2", SqlDbType.Int),
            new SqlMetaData("Int3", SqlDbType.Int),
            new SqlMetaData("Int4", SqlDbType.Int),
            new SqlMetaData("Int5", SqlDbType.Int),
            new SqlMetaData("ReceiptNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("RID", SqlDbType.BigInt),
            new SqlMetaData("GoodsName", SqlDbType.NVarChar, 500),
            new SqlMetaData("GoodsType", SqlDbType.NVarChar, 200),
            new SqlMetaData("BatchNumber", SqlDbType.NVarChar, 200),
            new SqlMetaData("BoxNumber", SqlDbType.NVarChar, 200),
            new SqlMetaData("Unit", SqlDbType.NVarChar, 100),
            new SqlMetaData("Specifications", SqlDbType.NVarChar, 100),
            new SqlMetaData("UPC", SqlDbType.NVarChar, 100)
        };
    }
}
