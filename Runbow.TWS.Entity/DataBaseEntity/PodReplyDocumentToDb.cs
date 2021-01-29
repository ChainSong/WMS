using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class PodReplyDocumentToDb : SqlDataRecord
    {
        public PodReplyDocumentToDb(PodReplyDocument podReplyDocument)
            : base(s_metadata)
        {
            SetSqlInt64(0, podReplyDocument.ID);
            SetSqlInt64(1, podReplyDocument.PodID);
            SetSqlString(2, podReplyDocument.SystemNumber);
            SetSqlString(3, podReplyDocument.CustomerOrderNumber);
            SetSqlString(4, podReplyDocument.Replier);
            SetSqlDateTime(5, podReplyDocument.ReplyTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(6, podReplyDocument.Remark);
            SetSqlString(7, podReplyDocument.AttachmentGroupID);
            SetSqlString(8, podReplyDocument.Creator);
            SetSqlDateTime(9, podReplyDocument.CreateTime);
            SetSqlString(10, podReplyDocument.Str1);
            SetSqlString(11, podReplyDocument.Str2);
            SetSqlString(12, podReplyDocument.Str3);
            SetSqlString(13, podReplyDocument.Str4);
            SetSqlString(14, podReplyDocument.Str5);
            SetSqlDateTime(15, podReplyDocument.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(16, podReplyDocument.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(17, podReplyDocument.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlBoolean(18, podReplyDocument.IsAudit ?? false);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("PodID", SqlDbType.BigInt),
            new SqlMetaData("SystemNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerOrderNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("Replier", SqlDbType.NVarChar, 50),
            new SqlMetaData("ReplyTime", SqlDbType.DateTime),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 500),
            new SqlMetaData("AttachmentGroupID", SqlDbType.NVarChar, 50),
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
            new SqlMetaData("IsAutid",SqlDbType.Bit)
        };
    }
}