using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSPackageDetailToDb : SqlDataRecord
    {
        public WMSPackageDetailToDb(PackageDetailInfo wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID ?? SqlTypes.SqlInt64.Null);
            SetSqlInt64(1, wmsInfo.PID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(2, wmsInfo.PackageNumber);
            SetSqlInt64(3, wmsInfo.CustomerID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(4, wmsInfo.CustomerName);
            SetSqlString(5, wmsInfo.SKU);
            SetSqlString(6, wmsInfo.GoodsName);
            SetSqlString(7, wmsInfo.GoodsType);
            SetSqlDecimal(8, wmsInfo.Qty ?? SqlTypes.SqlDecimal.Null);
            SetSqlString(9, wmsInfo.Creator);
            SetSqlDateTime(10, wmsInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(11, wmsInfo.Updator);
            SetSqlDateTime(12, wmsInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(13, wmsInfo.Remark);

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
            SetSqlString(44, wmsInfo.UPC == null ? "" : wmsInfo.UPC);
            #endregion
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("PID", SqlDbType.BigInt),
            new SqlMetaData("PackageNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("SKU", SqlDbType.NVarChar, 50),
            new SqlMetaData("GoodsName", SqlDbType.NVarChar, 50),
            new SqlMetaData("GoodsType", SqlDbType.NVarChar, 50),
            new SqlMetaData("Qty", SqlDbType.Decimal),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Updator",SqlDbType.NVarChar, 50),
            new SqlMetaData("UpdateTime",  SqlDbType.DateTime),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 500),

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
            new SqlMetaData("UPC", SqlDbType.NVarChar, 50)
        };
    }
}
