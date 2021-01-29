using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class PodStatusLogToDb : SqlDataRecord
    {
        public PodStatusLogToDb(PodStatusLog podStatusLog)
            : base(s_metadata)
        {
            SetSqlInt64(0, podStatusLog.ID);
            SetSqlInt64(1, podStatusLog.PodID);
            SetSqlString(2, podStatusLog.SystemNumber);
            SetSqlString(3, podStatusLog.CustomerOrderNumber);
            SetSqlString(4, podStatusLog.Creator);
            SetSqlDateTime(5, podStatusLog.CreateTime);
            SetSqlString(6, podStatusLog.Str1);
            SetSqlString(7, podStatusLog.Str2);
            SetSqlString(8, podStatusLog.Str3);
            SetSqlString(9, podStatusLog.Str4);
            SetSqlString(10, podStatusLog.Str5);
            SetSqlString(11, podStatusLog.Str6);
            SetSqlString(12, podStatusLog.Str7);
            SetSqlString(13, podStatusLog.Str8);
            SetSqlString(14, podStatusLog.Str9);
            SetSqlString(15, podStatusLog.Str10);
            SetSqlDateTime(16, podStatusLog.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(17, podStatusLog.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(18, podStatusLog.DateTime3 ?? SqlTypes.SqlDateTime.Null);
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