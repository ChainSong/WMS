using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSGoodsShelfToDb : SqlDataRecord
    {
        public WMSGoodsShelfToDb(GoodsShelfInfo wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlInt64(1, wmsInfo.ProjectID);
            SetSqlInt64(2, wmsInfo.CustomerID);
            SetSqlInt64(3, wmsInfo.WareHouseID);
            SetSqlString(4, wmsInfo.GoodsShelvesName);
            SetSqlString(5, wmsInfo.Levels);
            SetSqlString(6, wmsInfo.Grids);
            SetSqlString(7, wmsInfo.Weights);
            SetSqlString(8, wmsInfo.Lengths);
            SetSqlString(9, wmsInfo.Widths);
            SetSqlString(10, wmsInfo.Heights);
            SetSqlString(11, wmsInfo.Remark);
            SetSqlString(12, wmsInfo.Creator);
            SetSqlString(13, wmsInfo.Updator);
            SetSqlDateTime(14, wmsInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(15, wmsInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(16, wmsInfo.Str1 == "" ? null : wmsInfo.Str1);
            SetSqlString(17, wmsInfo.Str2 == "" ? null : wmsInfo.Str2);
            SetSqlString(18, wmsInfo.Str3 == "" ? null : wmsInfo.Str3);
            SetSqlString(19, wmsInfo.Str4 == "" ? null : wmsInfo.Str4);
            SetSqlString(20, wmsInfo.Str5 == "" ? null : wmsInfo.Str5);
            SetSqlInt32(21, wmsInfo.Int1);
            SetSqlInt32(22, wmsInfo.Int2);
            SetSqlInt32(23, wmsInfo.Int3);
            SetSqlInt32(24, wmsInfo.Int4);
            SetSqlInt32(25, wmsInfo.Int5);
            SetSqlDateTime(26, wmsInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(27, wmsInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(28, wmsInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(29, wmsInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(30, wmsInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(31, wmsInfo.Location == "" ? null : wmsInfo.Location);
            SetSqlInt64(32, wmsInfo.LevelsNumber);
            SetSqlInt64(33, wmsInfo.SerialNumber);
            SetSqlString(34, wmsInfo.AreaName);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("WareHouseID", SqlDbType.BigInt),
            new SqlMetaData("GoodsShelvesName", SqlDbType.NVarChar, 100),
            new SqlMetaData("Levels", SqlDbType.NVarChar, 100),
            new SqlMetaData("Grids", SqlDbType.NVarChar, 100),
            new SqlMetaData("Weights", SqlDbType.NVarChar, 100),
            new SqlMetaData("Lengths", SqlDbType.NVarChar, 100),
            new SqlMetaData("Widths", SqlDbType.NVarChar, 100),
            new SqlMetaData("Heights", SqlDbType.NVarChar, 100),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 200),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 100),
            new SqlMetaData("Updator", SqlDbType.NVarChar, 100),
            new SqlMetaData("CreateTime",SqlDbType.DateTime),
            new SqlMetaData("UpdateTime", SqlDbType.DateTime),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 100),
            new SqlMetaData("Int1", SqlDbType.Int),
            new SqlMetaData("Int2", SqlDbType.Int),
            new SqlMetaData("Int3", SqlDbType.Int),
            new SqlMetaData("Int4", SqlDbType.Int),
            new SqlMetaData("Int5", SqlDbType.Int),
            new SqlMetaData("DateTime1",SqlDbType.DateTime),
            new SqlMetaData("DateTime2",SqlDbType.DateTime),
            new SqlMetaData("DateTime3",SqlDbType.DateTime),
            new SqlMetaData("DateTime4",SqlDbType.DateTime),
            new SqlMetaData("DateTime5",SqlDbType.DateTime),
            new SqlMetaData("Location", SqlDbType.NVarChar, 100),
            new SqlMetaData("LevelsNumber", SqlDbType.BigInt),
            new SqlMetaData("SerialNumber", SqlDbType.BigInt),
            new SqlMetaData("AreaName", SqlDbType.NVarChar, 50),
        };
    }
}
