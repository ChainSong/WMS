using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSASNToDb : SqlDataRecord
    {
        public WMSASNToDb(ASN wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlString(1, wmsInfo.ASNNumber == "" ? SqlTypes.SqlString.Null : wmsInfo.ASNNumber);
            SetSqlString(2, wmsInfo.ExternReceiptNumber == "" ? SqlTypes.SqlString.Null : wmsInfo.ExternReceiptNumber);
            SetSqlInt64(3, wmsInfo.CustomerID??SqlTypes.SqlInt64.Null);
            SetSqlString(4, wmsInfo.CustomerName == "" ? SqlTypes.SqlString.Null : wmsInfo.CustomerName);
            SetSqlInt64(5, wmsInfo.WarehouseID);
            SetSqlString(6, wmsInfo.WarehouseName == "" ? SqlTypes.SqlString.Null : wmsInfo.WarehouseName);
            SetSqlDateTime(7, wmsInfo.ExpectDate ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt32(8, wmsInfo.Status);
            SetSqlString(9, wmsInfo.ASNType == "" ? SqlTypes.SqlString.Null : wmsInfo.ASNType);
            SetSqlString(10, wmsInfo.Creator == "" ? SqlTypes.SqlString.Null : wmsInfo.Creator);
            SetSqlDateTime(11, wmsInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(12, wmsInfo.Updator == "" ? SqlTypes.SqlString.Null : wmsInfo.Updator);
            SetSqlDateTime(13, wmsInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(14, wmsInfo.CompleteDate ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(15, wmsInfo.Remark == "" ? SqlTypes.SqlString.Null : wmsInfo.Remark);

            #region 备用字段
            SetSqlString(16, wmsInfo.str1 == "" ? SqlTypes.SqlString.Null : wmsInfo.str1);
            SetSqlString(17, wmsInfo.str2 == "" ? SqlTypes.SqlString.Null : wmsInfo.str2);
            SetSqlString(18, wmsInfo.str3 == "" ? SqlTypes.SqlString.Null : wmsInfo.str3);
            SetSqlString(19, wmsInfo.str4 == "" ? SqlTypes.SqlString.Null : wmsInfo.str4);
            SetSqlString(20, wmsInfo.str5 == "" ? SqlTypes.SqlString.Null : wmsInfo.str5);
            SetSqlString(21, wmsInfo.str6 == "" ? SqlTypes.SqlString.Null : wmsInfo.str6);
            SetSqlString(22, wmsInfo.str7 == "" ? SqlTypes.SqlString.Null : wmsInfo.str7);
            SetSqlString(23, wmsInfo.str8 == "" ? SqlTypes.SqlString.Null : wmsInfo.str8);
            SetSqlString(24, wmsInfo.str9 == "" ? SqlTypes.SqlString.Null : wmsInfo.str9);
            SetSqlString(25, wmsInfo.str10 == "" ? SqlTypes.SqlString.Null : wmsInfo.str10);
            SetSqlString(26, wmsInfo.str11 == "" ? SqlTypes.SqlString.Null : wmsInfo.str11);
            SetSqlString(27, wmsInfo.str12 == "" ? SqlTypes.SqlString.Null : wmsInfo.str12);
            SetSqlString(28, wmsInfo.str13 == "" ? SqlTypes.SqlString.Null : wmsInfo.str13);
            SetSqlString(29, wmsInfo.str14 == "" ? SqlTypes.SqlString.Null : wmsInfo.str14);
            SetSqlString(30, wmsInfo.str15 == "" ? SqlTypes.SqlString.Null : wmsInfo.str15);
            SetSqlString(31, wmsInfo.str16 == "" ? SqlTypes.SqlString.Null : wmsInfo.str16);
            SetSqlString(32, wmsInfo.str17 == "" ? SqlTypes.SqlString.Null : wmsInfo.str17);
            SetSqlString(33, wmsInfo.str18 == "" ? SqlTypes.SqlString.Null : wmsInfo.str18);
            SetSqlString(34, wmsInfo.str19 == "" ? SqlTypes.SqlString.Null : wmsInfo.str19);
            SetSqlString(35, wmsInfo.str20 == "" ? SqlTypes.SqlString.Null : wmsInfo.str20);           

            SetSqlDateTime(36, wmsInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(37, wmsInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(38, wmsInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(39, wmsInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(40, wmsInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);

            SetSqlInt32(41, wmsInfo.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(42, wmsInfo.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(43, wmsInfo.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(44, wmsInfo.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(45, wmsInfo.Int5 ?? SqlTypes.SqlInt32.Null); 
            #endregion
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ASNNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExternReceiptNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("WarehouseID", SqlDbType.BigInt),
            new SqlMetaData("WarehouseName", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExpectDate", SqlDbType.DateTime),
            new SqlMetaData("Status", SqlDbType.Int),
            new SqlMetaData("ASNType", SqlDbType.NVarChar, 50),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Updator",SqlDbType.NVarChar, 50),
            new SqlMetaData("UpdateTime",  SqlDbType.DateTime),
            new SqlMetaData("CompleteDate", SqlDbType.DateTime),
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
        };
    }
}
