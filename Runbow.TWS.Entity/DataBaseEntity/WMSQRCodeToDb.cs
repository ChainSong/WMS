using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSQRCodeToDb : SqlDataRecord
    {
        public WMSQRCodeToDb(QRCodeInfo wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlInt64(1, wmsInfo.ProjectID);
            SetSqlInt64(2, wmsInfo.CustomerID);
            SetSqlInt64(3, wmsInfo.WareHouseID);
            SetSqlString(4, wmsInfo.QRCode);
            SetSqlString(5, wmsInfo.Remark);
            SetSqlString(6, wmsInfo.Str1 == "" ? null : wmsInfo.Str1);
            SetSqlString(7, wmsInfo.Str2 == "" ? null : wmsInfo.Str2);
            SetSqlString(8, wmsInfo.Str3 == "" ? null : wmsInfo.Str3);
            SetSqlString(9, wmsInfo.Str4 == "" ? null : wmsInfo.Str4);
            SetSqlString(10, wmsInfo.Str5 == "" ? null : wmsInfo.Str5);
            SetSqlString(11, wmsInfo.GoodsShelfID);
            SetSqlString(12, wmsInfo.X == "" ? null : wmsInfo.X);
            SetSqlString(13, wmsInfo.Y == "" ? null : wmsInfo.Y);
            SetSqlString(14, wmsInfo.MapType == "" ? null : wmsInfo.MapType);
            SetSqlInt64(15, wmsInfo.MapID);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("WareHouseID", SqlDbType.BigInt),
            new SqlMetaData("QRCode", SqlDbType.NVarChar, 100),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 100),
            new SqlMetaData("GoodsShelfID", SqlDbType.NVarChar, 100),
            new SqlMetaData("X", SqlDbType.NVarChar, 100),
            new SqlMetaData("Y", SqlDbType.NVarChar, 100),
            new SqlMetaData("MapType", SqlDbType.NVarChar, 100),
            new SqlMetaData("MapID", SqlDbType.BigInt)
        };
    }
}
