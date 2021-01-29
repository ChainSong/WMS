using System.Data;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity
{
    public class UdtUpdateAkzoModelsPodToDb : SqlDataRecord
    {
        public UdtUpdateAkzoModelsPodToDb(UpdateAKZOModelsPod pod)
            : base(s_metadata)
        {
            SetSqlInt64(0, pod.CustomerID);
            SetSqlString(1, pod.CustomerOrderNumber);
            SetSqlString(2, pod.Models);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("CustomerID",SqlDbType.BigInt),
            new SqlMetaData("CustomerOrderNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("Models",SqlDbType.NVarChar,50)

        };
    }
}


