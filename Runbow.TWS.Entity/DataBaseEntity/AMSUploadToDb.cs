using System.Data;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity
{
    public class AMSUploadToDb : SqlDataRecord
    {//ID,FileName,FileType,ServerName,FilePath,ProjectID,ProjectName,OrderNo,Creator,CreateTime,Updator,UpdateTime,Status
        public AMSUploadToDb(AMSUpload amsUpload)
            : base(s_metadata)
        {
            SetSqlString(0, amsUpload.FileName);
            SetSqlString(1, amsUpload.FileType);
            SetSqlString(2, amsUpload.ServerName);
            SetSqlString(3, amsUpload.FilePath);
            SetSqlInt64(4, amsUpload.ProjectID);
            SetSqlString(5, amsUpload.ProjectName);
            SetSqlString(6, amsUpload.OrderNo);
            SetSqlString(7, amsUpload.Creator);
            SetSqlDateTime(8, amsUpload.CreateTime);
            SetSqlString(9, amsUpload.Updator);
            SetSqlDateTime(10, amsUpload.UpdateTime);
            SetSqlBoolean(11, amsUpload.Status ?? false);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("FileName", SqlDbType.NVarChar, 100),
            new SqlMetaData("FileType", SqlDbType.NVarChar, 20),
            new SqlMetaData("ServerName", SqlDbType.NVarChar, 100),
            new SqlMetaData("FilePath", SqlDbType.NVarChar, 200),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("ProjectName", SqlDbType.NVarChar, 50),
            new SqlMetaData("OrderNo", SqlDbType.NVarChar, 50),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Updator", SqlDbType.NVarChar, 50),
            new SqlMetaData("UpdateTime", SqlDbType.DateTime),
            new SqlMetaData("Status", SqlDbType.Bit),
        };
    }
}
