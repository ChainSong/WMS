using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class ShipperRegionCoveredToDb : SqlDataRecord
    {
        public ShipperRegionCoveredToDb(ShipperRegionCovered shipperRegionCovered)
            : base(s_metadata)
        {
            SetSqlInt64(0, shipperRegionCovered.ID);
            SetSqlInt64(1, shipperRegionCovered.ProjectID);
            SetSqlInt64(2, shipperRegionCovered.RelatedCustomerID);
            SetSqlInt64(3, shipperRegionCovered.ShipperID);
            SetSqlString(4, shipperRegionCovered.ShipperName);
            SetSqlInt64(5, shipperRegionCovered.StartCityID);
            SetSqlString(6, shipperRegionCovered.StartCityName);
            SetSqlInt64(7, shipperRegionCovered.EndCityID);
            SetSqlString(8, shipperRegionCovered.EndCityName);
            SetSqlDateTime(9, shipperRegionCovered.StartTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(10, shipperRegionCovered.EndTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(11, shipperRegionCovered.Str1);
            SetSqlString(12, shipperRegionCovered.Str2);
            SetSqlString(13, shipperRegionCovered.Str3);
            SetSqlBoolean(14, shipperRegionCovered.Bit1 ?? SqlTypes.SqlBoolean.Null);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("RelatedCustomerID", SqlDbType.BigInt),
            new SqlMetaData("ShipperID", SqlDbType.BigInt),
            new SqlMetaData("ShipperName", SqlDbType.NVarChar, 50),
            new SqlMetaData("StartCityID", SqlDbType.BigInt),
            new SqlMetaData("StartCityName", SqlDbType.NVarChar, 50),
            new SqlMetaData("EndCityID", SqlDbType.BigInt),
            new SqlMetaData("EndCityName", SqlDbType.NVarChar, 50),
            new SqlMetaData("StartTime", SqlDbType.DateTime),
            new SqlMetaData("EndTime", SqlDbType.DateTime),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 500),
            new SqlMetaData("Bit1", SqlDbType.Bit)
        };
    }
}
