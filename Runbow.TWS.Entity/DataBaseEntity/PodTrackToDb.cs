using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class PodTrackToDb : SqlDataRecord
    {
        public PodTrackToDb(PodTrack podTrack)
            : base(s_metadata)
        {
            SetSqlInt64(0, podTrack.ID);
            SetSqlInt64(1, podTrack.PodID);
            SetSqlString(2, podTrack.SystemNumber);
            SetSqlString(3, podTrack.CustomerOrderNumber);
            SetSqlString(4, podTrack.Creator);
            SetSqlDateTime(5, podTrack.CreateTime);
            SetSqlString(6, podTrack.Str1);
            SetSqlString(7, podTrack.Str2);
            SetSqlString(8, podTrack.Str3);
            SetSqlString(9, podTrack.Str4);
            SetSqlString(10, podTrack.Str5);
            SetSqlString(11, podTrack.Str6);
            SetSqlString(12, podTrack.Str7);
            SetSqlString(13, podTrack.Str8);
            SetSqlString(14, podTrack.Str9);
            SetSqlString(15, podTrack.Str10);
            SetSqlDateTime(16, podTrack.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(17, podTrack.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(18, podTrack.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(19, podTrack.Str11);
            SetSqlString(20, podTrack.Str12);
            SetSqlString(21, podTrack.Str13);
            SetSqlString(22, podTrack.Str14);
            SetSqlString(23, podTrack.Str15);
            SetSqlDateTime(24, podTrack.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(25, podTrack.DateTime5 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(26, podTrack.DateTime6 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(27, podTrack.DateTime7 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(28, podTrack.DateTime8 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(29, podTrack.DateTime9 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(30, podTrack.DateTime10 ?? SqlTypes.SqlDateTime.Null);
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
            new SqlMetaData("Str3", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str6", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str7", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str8", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str9", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str10", SqlDbType.NVarChar, 500),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("DateTime3", SqlDbType.DateTime),
            new SqlMetaData("Str11", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str12", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str13", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str14", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str15", SqlDbType.NVarChar, 50),
            new SqlMetaData("DateTime4", SqlDbType.DateTime),
            new SqlMetaData("DateTime5", SqlDbType.DateTime),
            new SqlMetaData("DateTime6", SqlDbType.DateTime),
            new SqlMetaData("DateTime7", SqlDbType.DateTime),
            new SqlMetaData("DateTime8", SqlDbType.DateTime),
            new SqlMetaData("DateTime9", SqlDbType.DateTime),
            new SqlMetaData("DateTime10", SqlDbType.DateTime)
        };
    }
}