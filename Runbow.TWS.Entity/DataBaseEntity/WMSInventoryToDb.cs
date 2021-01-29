using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSInventoryToDb : SqlDataRecord
    {
        public WMSInventoryToDb(Inventorys wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlInt64(1, wmsInfo.RRID);
            SetSqlString(2, wmsInfo.ReceiptNumber);
            SetSqlString(3, wmsInfo.Warehouse);
            SetSqlString(4, wmsInfo.Area);
            SetSqlString(5, wmsInfo.Location);
            SetSqlInt64(6, wmsInfo.SuperID);
            SetSqlInt64(7, wmsInfo.CustomerID); 
            SetSqlString(8, wmsInfo.CustomerName);
            SetSqlString(9, wmsInfo.SKU);
            SetSqlString(10, wmsInfo.GoodsName);
            SetSqlString(11, wmsInfo.GoodsType);
            SetSqlDouble(12, wmsInfo.Qty);
            SetInt32(13, wmsInfo.InventoryType);
            SetInt32(14, wmsInfo.RelatedID);
            SetSqlString(15, wmsInfo.Creator);
            SetDateTime(16, wmsInfo.CreateTime);
            SetSqlString(17, wmsInfo.Updator);
            SetSqlDateTime(18, wmsInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
 

            #region 备用字段
            SetSqlString(19, wmsInfo.str1);
            SetSqlString(20, wmsInfo.str2);
            SetSqlString(21, wmsInfo.str3);
            SetSqlString(22, wmsInfo.str4);
            SetSqlString(23, wmsInfo.str5);
            SetSqlString(24, wmsInfo.str6);
            SetSqlString(25, wmsInfo.str7);
            SetSqlString(26, wmsInfo.str8);
            SetSqlString(27, wmsInfo.str9);
            SetSqlString(28, wmsInfo.str10);
            SetSqlString(29, wmsInfo.str11);
            SetSqlString(30, wmsInfo.str12);
            SetSqlString(31, wmsInfo.str13);
            SetSqlString(32, wmsInfo.str14);
            SetSqlString(33, wmsInfo.str15);
            SetSqlString(34, wmsInfo.str16);
            SetSqlString(35, wmsInfo.str17);
            SetSqlString(36, wmsInfo.str18);
            SetSqlString(37, wmsInfo.str19);
            SetSqlString(38, wmsInfo.str20);

            SetSqlDateTime(39, wmsInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(40, wmsInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(41, wmsInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(42, wmsInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(43, wmsInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);

            SetSqlInt32(44, wmsInfo.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(45, wmsInfo.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(46, wmsInfo.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(47, wmsInfo.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(48, wmsInfo.Int5 ?? SqlTypes.SqlInt32.Null);
            #endregion
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("RRID", SqlDbType.BigInt),
            new SqlMetaData("ReceiptNumber",SqlDbType.NVarChar, 50),
            new SqlMetaData("Warehouse", SqlDbType.NVarChar, 50),
            new SqlMetaData("Area", SqlDbType.NVarChar, 50),
            new SqlMetaData("Location", SqlDbType.NVarChar, 50),
            new SqlMetaData("SuperID", SqlDbType.BigInt),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("SKU", SqlDbType.NVarChar, 50),
            new SqlMetaData("GoodsName", SqlDbType.NVarChar, 50),
            new SqlMetaData("GoodsType", SqlDbType.NVarChar, 50),
            new SqlMetaData("Qty", SqlDbType.Float),
            new SqlMetaData("InventoryType", SqlDbType.Int),
            new SqlMetaData("RelatedID", SqlDbType.Int),
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
        };
    }
}
