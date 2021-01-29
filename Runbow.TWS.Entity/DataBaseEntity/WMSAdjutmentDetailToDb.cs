using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;


namespace Runbow.TWS.Entity
{
    public class WMSAdjutmentDetailToDb : SqlDataRecord
    {
        public WMSAdjutmentDetailToDb(AdjustmentDetail wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlInt64(1, wmsInfo.AID);
            SetSqlString(2, wmsInfo.AdjustmentNumber);
            SetSqlInt64(3, wmsInfo.CustomerID);
            SetSqlString(4, wmsInfo.CustomerName);
            SetSqlString(5, wmsInfo.FromLot);
            SetSqlString(6, wmsInfo.ToLot);
            SetSqlString(7, wmsInfo.SKU);
            SetSqlString(8, wmsInfo.UPC);
            SetSqlString(9, wmsInfo.GoodsName);
            SetSqlString(10, wmsInfo.FromWarehouse);
            SetSqlString(11, wmsInfo.ToWarehouse);
            SetSqlString(12, wmsInfo.FromArea);
            SetSqlString(13, wmsInfo.ToArea);
            SetSqlString(14, wmsInfo.FromLocation);
            SetSqlString(15, wmsInfo.ToLocation);
            SetSqlDouble(16, wmsInfo.FromQty);
            SetSqlDouble(17, wmsInfo.ToQty);
            SetSqlString(18, wmsInfo.FromGoodsType);
            SetSqlString(19, wmsInfo.ToGoodsType);
            SetSqlInt32(20, wmsInfo.IsHold ?? SqlTypes.SqlInt32.Null);
            SetSqlString(21, wmsInfo.AdjustmentReason);
            SetSqlString(22, wmsInfo.Creator ?? SqlTypes.SqlString.Null);
            SetSqlDateTime(23, wmsInfo.CreateTime);
            SetSqlString(24, wmsInfo.Updator ?? SqlTypes.SqlString.Null);
            SetSqlDateTime(25, wmsInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(26, wmsInfo.Remark ?? SqlTypes.SqlString.Null);

            #region 备用字段
            SetSqlString(27, wmsInfo.str1 ?? SqlTypes.SqlString.Null);
            SetSqlString(28, wmsInfo.str2 ?? SqlTypes.SqlString.Null);
            SetSqlString(29, wmsInfo.str3 ?? SqlTypes.SqlString.Null);
            SetSqlString(30, wmsInfo.str4 ?? SqlTypes.SqlString.Null);
            SetSqlString(31, wmsInfo.str5 ?? SqlTypes.SqlString.Null);
            SetSqlString(32, wmsInfo.str6 ?? SqlTypes.SqlString.Null);
            SetSqlString(33, wmsInfo.str7 ?? SqlTypes.SqlString.Null);
            SetSqlString(34, wmsInfo.str8 ?? SqlTypes.SqlString.Null);
            SetSqlString(35, wmsInfo.str9 ?? SqlTypes.SqlString.Null);
            SetSqlString(36, wmsInfo.str10 ?? SqlTypes.SqlString.Null);
            SetSqlString(37, wmsInfo.str11 ?? SqlTypes.SqlString.Null);
            SetSqlString(38, wmsInfo.str12 ?? SqlTypes.SqlString.Null);
            SetSqlString(39, wmsInfo.str13 ?? SqlTypes.SqlString.Null);
            SetSqlString(40, wmsInfo.str14 ?? SqlTypes.SqlString.Null);
            SetSqlString(41, wmsInfo.str15 ?? SqlTypes.SqlString.Null);
            SetSqlString(42, wmsInfo.str16 ?? SqlTypes.SqlString.Null);
            SetSqlString(43, wmsInfo.str17 ?? SqlTypes.SqlString.Null);
            SetSqlString(44, wmsInfo.str18 ?? SqlTypes.SqlString.Null);
            SetSqlString(45, wmsInfo.str19 ?? SqlTypes.SqlString.Null);
            SetSqlString(46, wmsInfo.str20 ?? SqlTypes.SqlString.Null);
            SetSqlDateTime(47, wmsInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(48, wmsInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(49, wmsInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(50, wmsInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(51, wmsInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt32(52, wmsInfo.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(53, wmsInfo.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(54, wmsInfo.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(55, wmsInfo.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(56, wmsInfo.Int5 ?? SqlTypes.SqlInt32.Null);
            SetSqlString(57, wmsInfo.BatchNumber == "" ? null : wmsInfo.BatchNumber);
            SetSqlString(58, wmsInfo.BoxNumber == "" ? null : wmsInfo.BoxNumber);
            SetSqlString(59, wmsInfo.Unit == "" ? null : wmsInfo.Unit);
            SetSqlString(60, wmsInfo.Specifications == "" ? null : wmsInfo.Specifications);
            #endregion
        }
        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),         
            new SqlMetaData("AID", SqlDbType.BigInt),     
            new SqlMetaData("AdjustmentNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("FromLot", SqlDbType.NVarChar, 50),
            new SqlMetaData("ToLot", SqlDbType.NVarChar, 50),
            new SqlMetaData("SKU", SqlDbType.NVarChar, 50),
            new SqlMetaData("UPC", SqlDbType.NVarChar, 50),
            new SqlMetaData("GoodsName", SqlDbType.NVarChar, 50),
            new SqlMetaData("FromWarehouse", SqlDbType.NVarChar, 50),
            new SqlMetaData("ToWarehouse", SqlDbType.NVarChar, 50),
            new SqlMetaData("FromArea", SqlDbType.NVarChar, 50),
            new SqlMetaData("ToArea", SqlDbType.NVarChar, 50),
            new SqlMetaData("FromLocation", SqlDbType.NVarChar, 50),
            new SqlMetaData("ToLocation", SqlDbType.NVarChar, 50),
            new SqlMetaData("FromQty", SqlDbType.Float),     
            new SqlMetaData("ToQty", SqlDbType.Float),     
            new SqlMetaData("FromGoodsType", SqlDbType.NVarChar, 50),
            new SqlMetaData("ToGoodsType", SqlDbType.NVarChar, 50),
            new SqlMetaData("IsHold", SqlDbType.Int),
            new SqlMetaData("AdjustmentReason", SqlDbType.NVarChar, 50),
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
            new SqlMetaData("BatchNumber", SqlDbType.NVarChar, 200),
            new SqlMetaData("BoxNumber", SqlDbType.NVarChar, 200),
            new SqlMetaData("Unit", SqlDbType.NVarChar, 100),
            new SqlMetaData("Specifications", SqlDbType.NVarChar, 100)
        };
    }
}
