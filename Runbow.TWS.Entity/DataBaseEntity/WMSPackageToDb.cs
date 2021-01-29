using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSPackageToDb : SqlDataRecord
    {
        public WMSPackageToDb(PackageInfo wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(1, wmsInfo.PackageNumber);
            SetSqlInt64(2, wmsInfo.OID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(3, wmsInfo.OrderNumber);
            SetSqlString(4, wmsInfo.ExternOrderNumber);
            SetSqlInt64(5, wmsInfo.CustomerID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(6, wmsInfo.CustomerName);
            SetSqlString(7, wmsInfo.Warehouse);
            SetSqlString(8, wmsInfo.PackageType);
            SetSqlString(9, wmsInfo.Length);
            SetSqlString(10, wmsInfo.Width);
            SetSqlString(11, wmsInfo.Height);
            SetSqlString(12, wmsInfo.NetWeight);
            SetSqlString(13, wmsInfo.GrossWeight);
            SetSqlString(14, wmsInfo.ExpressCompany);
            SetSqlString(15, wmsInfo.ExpressNumber);
            SetSqlInt32(16, wmsInfo.IsComposited ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(17, wmsInfo.IsHandovered ?? SqlTypes.SqlInt32.Null);
            SetSqlString(18, wmsInfo.Handoveror);
            SetSqlDateTime(19, wmsInfo.HandoverTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt32(20, wmsInfo.Status ?? SqlTypes.SqlInt32.Null);
            SetSqlDateTime(21, wmsInfo.PackageTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt32(22, wmsInfo.DetailCount ?? SqlTypes.SqlInt32.Null);
            SetSqlString(23, wmsInfo.Creator);
            SetSqlDateTime(24, wmsInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(25, wmsInfo.Updator);
            SetSqlDateTime(26, wmsInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(27, wmsInfo.Remark);

            #region 备用字段
            SetSqlString(28, wmsInfo.str1 ?? SqlTypes.SqlString.Null);
            SetSqlString(29, wmsInfo.str2 ?? SqlTypes.SqlString.Null);
            SetSqlString(30, wmsInfo.str3 ?? SqlTypes.SqlString.Null);
            SetSqlString(31, wmsInfo.str4 ?? SqlTypes.SqlString.Null);
            SetSqlString(32, wmsInfo.str5 ?? SqlTypes.SqlString.Null);
            SetSqlString(33, wmsInfo.str6 ?? SqlTypes.SqlString.Null);
            SetSqlString(34, wmsInfo.str7 ?? SqlTypes.SqlString.Null);
            SetSqlString(35, wmsInfo.str8 ?? SqlTypes.SqlString.Null);
            SetSqlString(36, wmsInfo.str9 ?? SqlTypes.SqlString.Null);
            SetSqlString(37, wmsInfo.str10 ?? SqlTypes.SqlString.Null);
            SetSqlString(38, wmsInfo.str11 ?? SqlTypes.SqlString.Null);
            SetSqlString(39, wmsInfo.str12 ?? SqlTypes.SqlString.Null);
            SetSqlString(40, wmsInfo.str13 ?? SqlTypes.SqlString.Null);
            SetSqlString(41, wmsInfo.str14 ?? SqlTypes.SqlString.Null);
            SetSqlString(42, wmsInfo.str15 ?? SqlTypes.SqlString.Null);
            SetSqlString(43, wmsInfo.str16 ?? SqlTypes.SqlString.Null);
            SetSqlString(44, wmsInfo.str17 ?? SqlTypes.SqlString.Null);
            SetSqlString(45, wmsInfo.str18 ?? SqlTypes.SqlString.Null);
            SetSqlString(46, wmsInfo.str19 ?? SqlTypes.SqlString.Null);
            SetSqlString(47, wmsInfo.str20 ?? SqlTypes.SqlString.Null);           

            SetSqlDateTime(48, wmsInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(49, wmsInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(50, wmsInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(51, wmsInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(52, wmsInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);

            SetSqlInt32(53, wmsInfo.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(54, wmsInfo.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(55, wmsInfo.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(56, wmsInfo.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(57, wmsInfo.Int5 ?? SqlTypes.SqlInt32.Null);
            #endregion
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),          
            new SqlMetaData("PackageNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("OID", SqlDbType.BigInt),
            new SqlMetaData("OrderNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExternOrderNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("Warehouse", SqlDbType.NVarChar, 50),
            new SqlMetaData("PackageType", SqlDbType.NVarChar, 50),
            new SqlMetaData("Length", SqlDbType.NVarChar, 50),
            new SqlMetaData("Width", SqlDbType.NVarChar, 50),
            new SqlMetaData("Height", SqlDbType.NVarChar, 50),
            new SqlMetaData("NetWeight", SqlDbType.NVarChar, 50),
            new SqlMetaData("GrossWeight", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExpressCompany", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExpressNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("IsComposited", SqlDbType.Int),
            new SqlMetaData("IsHandovered", SqlDbType.Int),
            new SqlMetaData("Handoveror", SqlDbType.NVarChar, 50),
            new SqlMetaData("HandoverTime", SqlDbType.DateTime),
            new SqlMetaData("Status", SqlDbType.Int),
            new SqlMetaData("PackageTime", SqlDbType.DateTime),
            new SqlMetaData("DetailCount", SqlDbType.Int),
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
            new SqlMetaData("Int5", SqlDbType.Int)
        };
    }
}
