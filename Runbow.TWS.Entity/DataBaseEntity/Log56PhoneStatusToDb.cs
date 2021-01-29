using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class Log56PhoneStatusToDb : SqlDataRecord
    {
        public Log56PhoneStatusToDb(Log56PhoneStatus phoneStatus)
            : base(s_metadata)
        {
            SetSqlInt64(0, phoneStatus.ID);
            SetSqlString(1, phoneStatus.Phone);
            SetSqlString(2, phoneStatus.Status);
            SetSqlString(3, phoneStatus.Str1);
            SetSqlString(4, phoneStatus.Str2);
            SetSqlString(5, phoneStatus.Str3);
            SetSqlString(6, phoneStatus.Str4);
            SetSqlString(7, phoneStatus.Str5);
            SetSqlString(8, phoneStatus.Str6);
            SetSqlString(9, phoneStatus.Str7);
            SetSqlString(10, phoneStatus.Str8);
            SetSqlString(11, phoneStatus.Str9);
            SetSqlString(12, phoneStatus.Str10);
            SetSqlDateTime(13, phoneStatus.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(14, phoneStatus.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(15, phoneStatus.DateTime3 ?? SqlTypes.SqlDateTime.Null);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("Phone", SqlDbType.NVarChar, 50),
            new SqlMetaData("Status", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str1",SqlDbType.NVarChar,50),
            new SqlMetaData("Str2",SqlDbType.NVarChar,50),
            new SqlMetaData("Str3",SqlDbType.NVarChar,50),
            new SqlMetaData("Str4",SqlDbType.NVarChar,50),
            new SqlMetaData("Str5",SqlDbType.NVarChar,50),
            new SqlMetaData("Str6",SqlDbType.NVarChar,100),
            new SqlMetaData("Str7",SqlDbType.NVarChar,100),
            new SqlMetaData("Str8",SqlDbType.NVarChar,100),
            new SqlMetaData("Str9",SqlDbType.NVarChar,500),
            new SqlMetaData("Str10",SqlDbType.NVarChar,500),
            new SqlMetaData("DateTime1",SqlDbType.DateTime),
            new SqlMetaData("DateTime2",SqlDbType.DateTime),
            new SqlMetaData("DateTime3",SqlDbType.DateTime)
        };
    }
}
