using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class SettledPodAuditHistoryToDb : SqlDataRecord
    {
        public SettledPodAuditHistoryToDb(SettledPodAuditHistory settledPodAuditHistory)
            : base(s_metadata)
        {
            SetSqlInt64(0, settledPodAuditHistory.ID);
            SetSqlInt64(1, settledPodAuditHistory.SettledPodID);
            SetSqlString(2, settledPodAuditHistory.Auditor);
            SetSqlDateTime(3, settledPodAuditHistory.AuditTime);
            SetSqlString(4, settledPodAuditHistory.AuditRemark);
            SetSqlString(5, settledPodAuditHistory.Str1);
            SetSqlString(6, settledPodAuditHistory.Str2);
            SetSqlString(7, settledPodAuditHistory.Str3);
            SetSqlDateTime(8, settledPodAuditHistory.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlBoolean(9, settledPodAuditHistory.Bit ?? false);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("SettledPodID", SqlDbType.BigInt),
            new SqlMetaData("Auditor",SqlDbType.NVarChar,50),
            new SqlMetaData("AuditTime",SqlDbType.DateTime),
            new SqlMetaData("AuditRemark", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2",SqlDbType.NVarChar,100),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 500),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("Bit", SqlDbType.Bit)
        };
    }
}
