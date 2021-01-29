using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class PodExceptionToDb : SqlDataRecord
    {
        public PodExceptionToDb(PodException podException)
            : base(s_metadata)
        {
            SetSqlInt64(0, podException.ID);
            SetSqlInt64(1, podException.PodID);
            SetSqlString(2, podException.SystemNumber);
            SetSqlString(3, podException.CustomerOrderNumber);
            SetSqlString(4, podException.Creator);
            SetSqlDateTime(5, podException.CreateTime);
            SetSqlString(6, podException.Str1);
            SetSqlString(7, podException.Str2);
            SetSqlString(8, podException.Str3);
            SetSqlString(9, podException.Str4);
            SetSqlString(10, podException.Str5);
            SetSqlDateTime(11, podException.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(12, podException.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(13, podException.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(14, podException.Str6);
            SetSqlString(15, podException.Str7);
            SetSqlString(16, podException.Str8);
            SetSqlString(17, podException.Str9);
            SetSqlString(18, podException.Str10);
            SetSqlString(19, podException.Str11);
            SetSqlString(20, podException.Str12);
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
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("DateTime3", SqlDbType.DateTime),
            new SqlMetaData("Str6", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str7", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str8", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str9", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str10", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str11", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str12", SqlDbType.NVarChar, 50)
        };
    }
}