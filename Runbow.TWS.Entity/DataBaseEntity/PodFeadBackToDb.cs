using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class PodFeadBackToDb : SqlDataRecord
    {
        public PodFeadBackToDb(PodFeadBack podFeadBack)
            : base(s_metadata)
        {
            SetSqlInt64(0, podFeadBack.ID);
            SetSqlInt64(1, podFeadBack.PodID);
            SetSqlString(2, podFeadBack.SystemNumber);
            SetSqlString(3, podFeadBack.CustomerOrderNumber);
            SetSqlString(4, podFeadBack.Creator);
            SetSqlDateTime(5, podFeadBack.CreateTime);
            SetSqlString(6, podFeadBack.Str1);
            SetSqlString(7, podFeadBack.Str2);
            SetSqlString(8, podFeadBack.Str3);
            SetSqlString(9, podFeadBack.Str4);
            SetSqlString(10, podFeadBack.Str5);
            SetSqlString(11, podFeadBack.Str6);
            SetSqlString(12, podFeadBack.Str7);
            SetSqlString(13, podFeadBack.Str8);
            SetSqlString(14, podFeadBack.Str9);
            SetSqlString(15, podFeadBack.Str10);
            SetSqlDateTime(16, podFeadBack.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(17, podFeadBack.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(18, podFeadBack.DateTime1 ?? SqlTypes.SqlDateTime.Null);
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
            new SqlMetaData("Str6", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str7", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str8", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str9", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str10", SqlDbType.NVarChar, 100),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("DateTime3", SqlDbType.DateTime)
        };
    }
}