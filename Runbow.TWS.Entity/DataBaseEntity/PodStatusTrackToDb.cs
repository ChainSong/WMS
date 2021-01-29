using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class PodStatusTrackToDb : SqlDataRecord
    {
        public PodStatusTrackToDb(PodStatusTrack podStatusTrack)
            : base(s_metadata)
        {
            SetSqlInt64(0, podStatusTrack.ID);
            SetSqlInt64(1, podStatusTrack.PodID);
            SetSqlString(2, podStatusTrack.SystemNumber);
            SetSqlString(3, podStatusTrack.CustomerOrderNumber);
            SetSqlString(4, podStatusTrack.Creator);
            SetSqlDateTime(5, podStatusTrack.CreateTime);
            SetSqlString(6, podStatusTrack.Str1);
            SetSqlString(7, podStatusTrack.Str2);
            SetSqlString(8, podStatusTrack.Str3);
            SetSqlString(9, podStatusTrack.Str4);
            SetSqlString(10, podStatusTrack.Str5);
            SetSqlString(11, podStatusTrack.Str6);
            SetSqlString(12, podStatusTrack.Str7);
            SetSqlString(13, podStatusTrack.Str8);
            SetSqlString(14, podStatusTrack.Str9);
            SetSqlString(15, podStatusTrack.Str10);
            SetSqlDateTime(16, podStatusTrack.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(17, podStatusTrack.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(18, podStatusTrack.DateTime1 ?? SqlTypes.SqlDateTime.Null);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("PodID", SqlDbType.BigInt),
            new SqlMetaData("SystemNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerOrderNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str6", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str7", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str8", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str9", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str10", SqlDbType.NVarChar, 50),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("DateTime3", SqlDbType.DateTime)
        };
    }
}