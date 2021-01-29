using System.Data;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity
{
    public class UdtUpdateNikePodToDb : SqlDataRecord
    {
        public UdtUpdateNikePodToDb(UpdateNikePod pod)
            : base(s_metadata)
        {
            SetSqlInt64(0, pod.CustomerID);
            SetSqlString(1, pod.CustomerOrderNumber);
            SetSqlDateTime(2, pod.ActualDeliveryDate);
            SetSqlDouble(3, pod.Weight);
            SetSqlString(4, pod.TS.ToString());
            SetSqlString(5, pod.SS.ToString());
            SetSqlString(6, pod.EndCityName);
            SetSqlString(7, pod.StartCityName);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("CustomerID",SqlDbType.BigInt),
            new SqlMetaData("CustomerOrderNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("ActualDeliveryDate",SqlDbType.DateTime),
            new SqlMetaData("Weight",SqlDbType.Float),
            new SqlMetaData("TS",SqlDbType.NVarChar,50),
            new SqlMetaData("SS",SqlDbType.NVarChar,50),
            new SqlMetaData("EndCityName",SqlDbType.NVarChar,50),
            new SqlMetaData("StartCityName",SqlDbType.NVarChar,50)

        };
    }
}


