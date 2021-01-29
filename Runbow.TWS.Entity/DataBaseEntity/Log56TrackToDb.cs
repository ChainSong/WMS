using System.Data;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity
{
    public class Log56TrackToDb : SqlDataRecord
    {
        public Log56TrackToDb(Log56Track log56Track)
            : base(s_metadata)
        {
            SetSqlInt64(0, log56Track.CustomerID);
            SetSqlString(1, log56Track.CustomerName);
            SetSqlInt64(2, log56Track.PodID);
            SetSqlString(3, log56Track.SystemNumber);
            SetSqlString(4, log56Track.CustomerOrderNumber);
            SetSqlString(5, log56Track.Phone);
            SetSqlString(6, log56Track.CurrentLocation);
            SetSqlDateTime(7, log56Track.TrackTime);
            SetSqlString(8, log56Track.Trackor);
            SetSqlDateTime(9, log56Track.CreateTime);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("PodID", SqlDbType.BigInt),
            new SqlMetaData("SystemNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("CustomerOrderNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("Phone",SqlDbType.NVarChar,50),
            new SqlMetaData("CurrentLocation", SqlDbType.NVarChar, 200),
            new SqlMetaData("TrackTime", SqlDbType.DateTime),
            new SqlMetaData("Trackor", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime)
        };
    }
}
